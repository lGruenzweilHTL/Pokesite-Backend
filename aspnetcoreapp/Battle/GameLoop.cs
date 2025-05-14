using System.Text.Json.Nodes;

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

    private void ProcessClientMessage(string message)
    {
        // Handle the message (e.g., player actions)
        Console.WriteLine($"Processing message: {message}");
        
        JsonNode node = JsonNode.Parse(message);
        ReceiveAction(
            node["type"].GetValue<string>(),
            node["move"].GetValue<string>(), 
            node["item"].GetValue<string>(),
            node["switch_to"].GetValue<string>(),
            node["player_id"].GetValue<int>());
    }

    public void SendMessage(string message)
    {
        _webSocketHandler.SendMessage(message);
    }
}