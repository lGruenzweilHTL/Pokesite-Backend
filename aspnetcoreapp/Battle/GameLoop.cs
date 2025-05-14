public class GameLoop
{
    public GameLoop(Player player1, Player player2) {
        _gameState = GameState.NotStarted;

        _player1 = player1;
        _player2 = player2;
    }

    private Player _player1, _player2;
    
    private GameState _gameState;

    public void Start() {
        _gameState = GameState.InProgress;
    }

    public void ReceiveAction(string type, string move, string item, string switchTo, int playerId) {
        
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
}