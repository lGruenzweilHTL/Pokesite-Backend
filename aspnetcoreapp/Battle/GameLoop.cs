using System.Text.Json;

public class GameLoop {
    public string Guid { get; }
    public GameState GameState;
    public int MaxPlayers { get; }
    
    public Dictionary<string, Player> ConnectedPlayers { get; }
    
    // Probably temp
    public Player Player1 => ConnectedPlayers.Values.ElementAt(0);
    public Player Player2 => ConnectedPlayers.Values.ElementAt(1);
    
    private readonly WebSocketHandler _webSocketHandler;
    private readonly List<GameAction> _currentActions; // the collected actions of the current turn.

    public GameLoop(string guid, int maxPlayers, WebSocketHandler socketHandler) {
        Guid = guid;
        MaxPlayers = maxPlayers;
        GameState = GameState.NotStarted;
        _currentActions = [];
        ConnectedPlayers = new Dictionary<string, Player>();
        _webSocketHandler = socketHandler;
        _webSocketHandler.OnMessageReceived += ProcessClientMessage;
        
        Console.WriteLine("Created new game with guid: " + guid);
    }

    public bool ConnectPlayer(string guid, Player player) {
        return ConnectedPlayers.Count < MaxPlayers
               && GameState == GameState.NotStarted
               && ConnectedPlayers.TryAdd(guid, player);
    }

    public int StartWithWebSocket()
    {
        GameState = GameState.InProgress;
        
        // Initialize Players
        foreach (Player player in ConnectedPlayers.Values) {
            player.InitializeTeam();
        }
        
        StartNewTurn();
        
        return _webSocketHandler.StartServer();
    }
    
    private void ReceiveAction(string actionType, string obj, string guid) {
        Player player = ConnectedPlayers[guid];

        GameAction action;
        
        return; // TODO: construct correct action for the type
        
        CollectAction(action);
    }

    private void CollectAction(GameAction action) {
        _currentActions.Add(action);

        // if every player has submitted an action
        if (_currentActions.Count >= 2) {
            Console.WriteLine("All actions collected. Executing turn...");
            ExecuteTurn();
        }
    }

    // Executed, when actions for every player have been collected.
    private void ExecuteTurn() {
        // Process all collected actions in correct order
        List<GameAction> orderedActions = _currentActions.Order().ToList();
        
        foreach (GameAction action in orderedActions) {
            ProcessGameAction(action);
        }
        
        SendMessage(JsonDocument.Parse("{\"message\": \"turn executed\"}"));
        
        StartNewTurn();
    }

    // Executed, when a turn is finished. Clears all collected actions and gets bot actions
    private void StartNewTurn() {
        _currentActions.Clear();

        foreach (Player player in ConnectedPlayers.Values.Where(p => p.IsBot)) {
            var botBehaviour = TrainerBotHandler.GetDefault();
            var action = botBehaviour.ChooseAction(player, player); // TODO: find opponent
            
            Console.WriteLine($"Bot (id: 1) has chosen the \"{action}\" action");
            CollectAction(action);
        }
    }

    private void ProcessGameAction(GameAction action)
    {
        if (action is AttackAction attack) {
            // handle attack
        }
        else if (action is ItemAction item) {
            // handle item
        }
        else if (action is SwitchAction @switch) {
            // handle switch
        }
    }

    private void ProcessClientMessage(Guid clientId, JsonDocument message)
    {
        // Handle the message (e.g., player actions)
        Console.WriteLine($"Processing message: {message}");

        string? guid = message.RootElement.GetProperty("battle_guid").GetString();
        if (Guid != (guid ?? "")) {
            Console.WriteLine("Discarding message...");
            return;
        }
        
        ReceiveAction(
            message.RootElement.GetProperty("type").GetString()!,
            message.RootElement.GetProperty("object").GetString()!,
            message.RootElement.GetProperty("player_guid").GetString()!);
    }

    public void SendMessage(JsonDocument message)
    {
        _webSocketHandler.SendMessageAsync(System.Guid.Empty, message);
    }
}