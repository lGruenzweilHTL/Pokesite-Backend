using System.Text.Json;

public class GameLoop
{
    private readonly Player _player1;
    private readonly Player _player2;
    private GameState _gameState;
    private readonly WebSocketHandler _webSocketHandler;

    private readonly List<GameAction> _currentActions; // the collected actions of the current turn.

    public GameLoop(Player player1, Player player2)
    {
        _player1 = player1;
        _player2 = player2;
        _gameState = GameState.NotStarted;
        _currentActions = [];
        _webSocketHandler = new WebSocketHandler();
        _webSocketHandler.OnMessageReceived += ProcessClientMessage;
    }

    public int StartWithWebSocket()
    {
        _gameState = GameState.InProgress;
        
        // Initialize players
        _player1.InitializeTeam();
        _player2.InitializeTeam();
        
        StartNewTurn();
        
        return _webSocketHandler.StartServer();
    }

    private void ReceiveAction(string actionType, string obj, int playerId) {
        Player player = playerId == 1 ? _player1 : _player2;
        Player opponent = playerId == 1 ? _player2 : _player1;

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
        List<GameAction> orderedActions = _currentActions.OrderBy(a1 => a1, Comparer<GameAction>.Create((a1, a2) => 
            a1.GoesBefore(a2) ? -1 : a2.GoesBefore(a1) ? 1 : 0)).ToList();
        
        foreach (GameAction action in orderedActions) {
            ProcessGameAction(action);
        }
        
        SendMessage(JsonDocument.Parse("{\"message\": \"turn executed\"}"));
        
        StartNewTurn();
    }

    // Executed, when a turn is finished. Clears all collected actions and gets bot actions
    private void StartNewTurn() {
        _currentActions.Clear();
        
        if (_player1.IsBot) {
            var botBehaviour = TrainerBotHandler.GetDefault();
            var action = botBehaviour.ChooseAction(_player1, _player2);
            
            Console.WriteLine($"Bot (id: 1) has chosen the \"{action}\" action");
            CollectAction(action);
        }
        
        if (_player2.IsBot) {
            var botBehaviour = TrainerBotHandler.GetDefault();
            var action = botBehaviour.ChooseAction(_player2, _player1);
            Console.WriteLine($"Bot (id: 2) has chosen the \"{action}\" action");
            
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

    private void ProcessClientMessage(JsonDocument message)
    {
        // Handle the message (e.g., player actions)
        Console.WriteLine($"Processing message: {message}");
        
        ReceiveAction(
            message.RootElement.GetProperty("type").GetString()!,
            message.RootElement.GetProperty("object").GetString()!,
            message.RootElement.GetProperty("player_id").GetInt32());
    }

    public void SendMessage(JsonDocument message)
    {
        _webSocketHandler.SendMessage(message);
    }
}