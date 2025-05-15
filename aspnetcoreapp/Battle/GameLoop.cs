using System.Text.Json;

public class GameLoop
{
    private readonly Player _player1;
    private readonly Player _player2;
    private GameState _gameState;
    private readonly WebSocketHandler _webSocketHandler;

    public GameLoop(Player player1, Player player2)
    {
        _player1 = player1;
        _player2 = player2;
        _gameState = GameState.NotStarted;
        _webSocketHandler = new WebSocketHandler();
        _webSocketHandler.OnMessageReceived += ProcessClientMessage;
    }

    public int StartWithWebSocket()
    {
        _gameState = GameState.InProgress;
        
        // Initialize players
        _player1.InitializeTeam();
        _player2.InitializeTeam();
        
        return _webSocketHandler.StartServer();
    }

    private void ReceiveAction(string type, string move, string item, string switchTo, int playerId) {
        
    }
    
    private void ProcessPlayerAction(Player player, Player opponent, GameAction action)
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
        
        // TODO: uncomment when battle system is done
        /*ReceiveAction(
            message.RootElement.GetProperty("type").GetString()!,
            message.RootElement.GetProperty("move").GetString()!,
            message.RootElement.GetProperty("item").GetString()!,
            message.RootElement.GetProperty("switch_to").GetString()!,
            message.RootElement.GetProperty("player_id").GetInt32());*/
    }

    public void SendMessage(string message)
    {
        _webSocketHandler.SendMessage(message);
    }
}