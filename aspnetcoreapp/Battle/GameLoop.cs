using System.Text.Json;

public class GameLoop {
    public string BattleGuid { get; }
    public GameState GameState;
    public int MaxPlayers { get; }
    
    public List<(Guid guid, Player p)> ConnectedPlayers { get; }
    public List<(Player p, ITrainerBotBehaviour behaviour)> ConnectedBots { get; }
    public int NumPlayers => ConnectedPlayers.Count + ConnectedBots.Count;
    
    private readonly WebSocketHandler _webSocketHandler;
    private readonly List<GameAction> _currentActions; // the collected actions of the current turn.
    private readonly List<string> _clientMessages; // Messages to send to the client for the current turn

    public GameLoop(string battleGuid, int maxPlayers, WebSocketHandler socketHandler) {
        BattleGuid = battleGuid;
        MaxPlayers = maxPlayers;
        GameState = GameState.NotStarted;
        _currentActions = [];
        _clientMessages = [];
        ConnectedBots = [];
        ConnectedPlayers = [];
        _webSocketHandler = socketHandler;
        _webSocketHandler.OnMessageReceived += ProcessClientMessage;
        
        Console.WriteLine("Created new game with guid: " + battleGuid);
    }

    public bool ConnectPlayer(Player player, out Guid guid) {
        guid = Guid.NewGuid();
        ConnectedPlayers.Add((guid, player));
        return NumPlayers < MaxPlayers
               && GameState == GameState.NotStarted;
    }

    public bool ConnectBot(Player player, string? preferredBehaviour = null) {
        if (NumPlayers >= MaxPlayers
            || GameState != GameState.NotStarted) return false;

        ITrainerBotBehaviour behaviour = preferredBehaviour == null
            ? TrainerBotHandler.GetDefault()
            : TrainerBotHandler.FindByName(preferredBehaviour);
        ConnectedBots.Add((player, behaviour));
        return true;
    }

    public void StartWithWebSocket()
    {
        GameState = GameState.InProgress;
        
        // Initialize Players
        foreach (var (_, player) in ConnectedPlayers) {
            player.InitializeTeam();
        }
        foreach ((Player p, ITrainerBotBehaviour? _) in ConnectedBots) {
            p.InitializeTeam();
        }

        StartNewTurn();
    }
    
    private void ReceiveAction(Guid playerGuid, ActionRequest request)
    {
        Player player = ConnectedPlayers
            .Find(p => p.guid == playerGuid).p
            ?? throw new ArgumentException($"Player with guid {playerGuid} not found.");

        GameAction action;

        switch (request.ActionType) {
            case "attack":
                Move move = player.CurrentPokemon.Moves
                                .Find(m => string.Equals(m.Name, request.Object, StringComparison.CurrentCultureIgnoreCase))
                    ?? throw new ArgumentException($"{player.CurrentPokemon.Name} has no move with the name {request.Object}.");
                action = new AttackAction(player, FindOpponentIn2PlayerGame(player), move);
                break;
            
            case "item":
                var item = player.Items
                    .Find(i => i.Name.Equals(request.Object, StringComparison.CurrentCultureIgnoreCase));
                if (item.Quantity <= 0)
                    throw new ArgumentException($"{player.Name} doesn't have any items of type {request.Object}.");

                action = new ItemAction(player, item);
                break;
            
            case "switch":
                if (!int.TryParse(request.Object, out int newPokemonIdx)
                    || newPokemonIdx is < 0 or > 3)
                    throw new ArgumentException($"{request.Object} is not a valid pokemon index.");
                action = new SwitchAction(player, newPokemonIdx);
                break;
            
            default:
                throw new ArgumentException($"{request.ActionType} is not a valid action type");
        }
        
        CollectAction(action);
    }

    private void CollectAction(GameAction action) {
        _currentActions.Add(action);

        // if every player has submitted an action
        if (_currentActions.Count >= NumPlayers) {
            Console.WriteLine("All actions collected. Executing turn...");
            ExecuteTurn();
        }
    }

    private bool IsGameOver() {
        return false;
    }

    // Executed, when actions for every player have been collected.
    private void ExecuteTurn() {
        // Process all collected actions in correct order
        List<GameAction> orderedActions = _currentActions.Order().ToList();
        
        foreach (GameAction action in orderedActions) {
            ProcessGameAction(action);
        }
        
        if (IsGameOver()) {
            _clientMessages.Add("Game ended with status: " + GameState);
        }
        
        _webSocketHandler.BroadcastMessageAsync("done").GetAwaiter().GetResult();
        
        StartNewTurn();
    }

    // Executed, when a turn is finished. Clears all collected actions and gets bot actions
    private void StartNewTurn() {
        _currentActions.Clear();
        _clientMessages.Clear();

        foreach ((Player player, ITrainerBotBehaviour behaviour) in ConnectedBots) {
            var action = behaviour.ChooseAction(player, FindOpponentIn2PlayerGame(player));
            
            Console.WriteLine($"Bot {player.Name} has chosen the \"{action}\" action");
            CollectAction(action);
        }
    }

    private void ProcessGameAction(GameAction action)
    {
        if (action is AttackAction attack) {
            HandleAttack(attack);
        }
        else if (action is ItemAction item) {
            HandleItem(item);
        }
        else if (action is SwitchAction @switch) {
            HandleSwitch(@switch);
        }
    }

    private void HandleAttack(AttackAction action) {
        Pokemon defender = action.Target.CurrentPokemon;
        int hitChance = action.Move.Accuracy * action.Pokemon.Accuracy / defender.Evasion;
        if (!RandomUtils.Chance(hitChance)) {
            _clientMessages.Add($"{action.Pokemon.Name}'s {action.Move} missed.");
        }
        
        _clientMessages.Add($"{action.Pokemon.Name} used {action.Move.Name}");
        _clientMessages.Add(action.Move.EffectivenessMessage(defender));
        
        if (action.Move.Status) return; // Status moves don't deal any damage

        int damage = DamageUtils.CalculateDamage(action);
        defender.CurrentHp -= damage;
    }

    private void HandleItem(ItemAction action) {
        
    }

    private void HandleSwitch(SwitchAction action) {
        Pokemon newPokemon = action.Player.Team[action.NewPokemonIndex];
        if (newPokemon.Fainted) throw new ArgumentException("The selected pokemon from switch action has already fainted");
        
        _clientMessages.Add($"{action.Player.Name} switched to {newPokemon.Name}");
        action.Player.CurrentPokemonIndex = action.NewPokemonIndex;
    }


    /*
     * 2 Solutions to link player guid to client guid:
     *
     * 1. Assign client guid only when connecting to the websocket
     *      - possible race conditions for clients (connect before game starts)
     *          - not necessarily a problem, but may be annoying
     *      - annoying setup but may be worth it
     *      - more secure
     *
     * 2. Save both guids in the GameLoop
     *      - annoying to implement
     *      - don't really want to do this
     *      - more responsibility for client (sending guid with each message)
     */
    private void ProcessClientMessage(Guid clientId, JsonDocument message)
    {
        // Handle the message (e.g., player actions)
        ActionRequest request = message.Deserialize<ActionRequest>();
        Console.WriteLine($"Processing message: {JsonSerializer.Serialize(request)}");
        
        if (BattleGuid != request.BattleGuid
            || ConnectedPlayers.All(p => p.guid != clientId)){
            Console.WriteLine("Discarding message...");
            return;
        }
        
        ReceiveAction(clientId, request);
    }

    private async Task SendMessage(JsonDocument message)
    {
        // TODO: only send to the correct client
        await _webSocketHandler.BroadcastMessageAsync(message);
    }

    // Finds the opponent of a player in a 2 player game; Asserts that there are only 2 players
    private Player FindOpponentIn2PlayerGame(Player player) {
        return ConnectedPlayers.Select(pair => pair.p)
            .Concat(ConnectedBots.Select(p => p.p))
            .Single(p => p != player);
    } 
}