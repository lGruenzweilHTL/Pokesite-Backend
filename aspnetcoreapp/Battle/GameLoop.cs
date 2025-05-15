using System.Text.Json;

public class GameLoop
{
    private readonly Player _player1;
    private readonly Player _player2;
    private GameState _gameState;
    private readonly WebSocketHandler _webSocketHandler;

    private Dictionary<Player, GameAction> _currentActions; // the collected actions of the current turn.

    public GameLoop(Player player1, Player player2)
    {
        _player1 = player1;
        _player2 = player2;
        _gameState = GameState.NotStarted;
        _currentActions = new Dictionary<Player, GameAction>();
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

    private void ReceiveAction(string type, string move, string item, string switchTo, int playerId) {
        Player player = playerId == 1 ? _player1 : _player2;
        Pokemon playerPokemon = player.CurrentPokemon;

        ActionType actionType = Enum.Parse<ActionType>(type, true);

        // TODO: different classes for game action
        GameAction action;
        if (actionType == ActionType.Attack) {
            Move attack = new Move(); // TODO: move service
            action = new GameAction(ActionType.Attack, attack, playerPokemon, null!);
        }
        else if (actionType == ActionType.Item) {
            // TODO: parse item
            action = new GameAction(ActionType.Item, null!, playerPokemon, null!);
        }
        else if (actionType == ActionType.Switch) {
            // TODO: represent with custom SwitchAction
            action = new GameAction(ActionType.Switch, null!, playerPokemon, null!);
        }
        else if (actionType == ActionType.Run) {
            action = new GameAction(ActionType.Run, null!, null!, null!);
        }
        else return; // if the type is invalid, ignore the action
        
        CollectAction(player, action);
    }

    private void CollectAction(Player player, GameAction action) {
        _currentActions.Add(player, action);

        // if every player has submitted an action
        if (_currentActions.Count >= 2) {
            Console.WriteLine("All actions collected. Executing turn...");
            ExecuteTurn();
        }
    }

    // Executed, when actions for every player have been collected.
    private void ExecuteTurn() {
        // Process all collected actions in correct order
        var orderedActions = _currentActions.OrderBy(a => a.Value,
            Comparer<GameAction>.Create((a1, a2) => a1.GoesBefore(a2) ? 1 : -1));
        
        foreach ((Player? player, GameAction? action) in orderedActions) {
            ProcessPlayerAction(player, action);
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
            
            Console.WriteLine($"Bot (id: 1) has chosen the {action.Type} action");
            CollectAction(_player1, action);
        }
        
        if (_player2.IsBot) {
            var botBehaviour = TrainerBotHandler.GetDefault();
            var action = botBehaviour.ChooseAction(_player2, _player1);
            Console.WriteLine($"Bot (id: 2) has chosen the {action.Type} action");
            
            CollectAction(_player2, action);
        }
    }

    private void ProcessPlayerAction(Player player, GameAction action)
    {
        // Process the player's action
        switch (action.Type)
        {
            case ActionType.Attack:
                // TODO: Handle attack action
                break;
            case ActionType.Switch:
                // TODO: Handle switch action
                break;
            case ActionType.Item:
                // TODO: Handle item action
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ProcessClientMessage(JsonDocument message)
    {
        // Handle the message (e.g., player actions)
        Console.WriteLine($"Processing message: {message}");
        
        ReceiveAction(
            message.RootElement.GetProperty("type").GetString()!,
            message.RootElement.GetProperty("move").GetString()!,
            message.RootElement.GetProperty("item").GetString()!,
            message.RootElement.GetProperty("switch_to").GetString()!,
            message.RootElement.GetProperty("player_id").GetInt32());
    }

    public void SendMessage(JsonDocument message)
    {
        _webSocketHandler.SendMessage(message);
    }
}