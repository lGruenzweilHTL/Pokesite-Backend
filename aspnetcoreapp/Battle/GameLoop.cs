using System.Text.Json;
using aspnetcoreapp.Battle.DTOs;

public class GameLoop {
    public string BattleGuid { get; }
    public GameState GameState;
    public int MaxPlayers { get; }
    
    public List<(Guid guid, Player p)> ConnectedPlayers { get; }
    public List<(Player p, ITrainerBotBehaviour behaviour)> ConnectedBots { get; }
    
    // TEMP
    private List<Player> AllPlayers => ConnectedPlayers.Select(p => p.p)
        .Concat(ConnectedBots.Select(b => b.p)).ToList();
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

    public void StartGame()
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
        if (ConnectedPlayers.Any(p => p.p.Defeated)
            || ConnectedBots.Any(p => p.p.Defeated)) 
        {
            GameState = GameState.Over;
            return true;
        }

        return false;
    }

    // Executed, when actions for every player have been collected.
    private void ExecuteTurn() {
        // Process all collected actions in correct order
        List<GameAction> orderedActions = _currentActions.Order().ToList();
        
        foreach (GameAction action in orderedActions) {
            ProcessGameAction(action);
        }
        
        // Apply status effects
        foreach (var (_, player) in ConnectedPlayers) {
            player.CurrentPokemon.ApplyStatusEffects();
        }
        foreach (var (player, _) in ConnectedBots) {
            player.CurrentPokemon.ApplyStatusEffects();
        }
        
        if (IsGameOver()) {
            _clientMessages.Add("Game ended with status: " + GameState);
        }

        Player player1 = AllPlayers[0],
            player2 = AllPlayers[1];
        SocketResponse response = new()
        {
            GameState = GameState.ToString(),
            Messages = _clientMessages.Where(m => !string.IsNullOrWhiteSpace(m)).ToArray(),
            Player1 = new BattlePlayerData
            {
                Items = player1.Items
                    .Select(i => i.Name)
                    .ToArray(),
                Switches = player1.AlivePokemons
                    .Where(p => p != player1.CurrentPokemon)
                    .Select(p => p.Name)
                    .ToArray(),
                Pokemon = new BattlePokemonData
                {
                    Name = player1.CurrentPokemon.Name,
                    Id = player1.CurrentPokemon.Id,
                    Level = player1.CurrentPokemon.Level,
                    CurrentHp = player1.CurrentPokemon.CurrentHp,
                    MaxHp = player1.CurrentPokemon.MaxHp,
                    StatusEffects = player1.CurrentPokemon.StatusEffects
                        .Select(e => e.Name)
                        .ToArray(),
                    Moves = player1.CurrentPokemon.Moves
                        .Select(m => m.Name)
                        .ToArray(),
                }
            },
            Player2 = new BattlePlayerData
            {
                Items = player2.Items
                    .Select(i => i.Name)
                    .ToArray(),
                Switches = player2.AlivePokemons
                    .Where(p => p != player2.CurrentPokemon)
                    .Select(p => p.Name)
                    .ToArray(),
                Pokemon = new BattlePokemonData
                {
                    Name = player2.CurrentPokemon.Name,
                    Id = player2.CurrentPokemon.Id,
                    Level = player2.CurrentPokemon.Level,
                    CurrentHp = player2.CurrentPokemon.CurrentHp,
                    MaxHp = player2.CurrentPokemon.MaxHp,
                    StatusEffects = player2.CurrentPokemon.StatusEffects
                        .Select(e => e.Name)
                        .ToArray(),
                    Moves = player2.CurrentPokemon.Moves
                        .Select(m => m.Name)
                        .ToArray(),
                }
            }
        };
        _webSocketHandler.BroadcastMessageAsync(response.ToJson()).GetAwaiter().GetResult();
        
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
        if (!action.Pokemon.CanAttack(out string message)
            || action.Pokemon.Fainted)
        {
            _clientMessages.Add(message);
        }
        
        Pokemon defender = action.Target.CurrentPokemon;
        int hitChance = action.Move.Accuracy * action.Pokemon.Accuracy / defender.Evasion;
        if (!RandomUtils.Chance(hitChance)) {
            _clientMessages.Add($"{action.Pokemon.Name}'s {action.Move} missed.");
        }
        
        action.Move.AddEffects(action.Pokemon, defender);
        
        _clientMessages.Add($"{action.Pokemon.Name} used {action.Move.Name}");
        
        if (action.Move.Status) return; // Status moves don't deal any damage
        
        _clientMessages.Add(action.Move.EffectivenessMessage(defender));
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
    
    private void ProcessClientMessage(Guid clientId, JsonDocument message)
    {
        // Handle the message (e.g., player actions)
        ActionRequest request = message.Deserialize<ActionRequest>();
        Console.WriteLine($"Processing message: {JsonSerializer.Serialize(request)}");
        
        if (BattleGuid != request.BattleGuid
            || ConnectedPlayers.All(p => p.guid != clientId)
            || GameState != GameState.InProgress) { 
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
        return AllPlayers
            .Single(p => p != player);
    } 
}