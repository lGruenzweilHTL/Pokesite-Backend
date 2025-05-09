using aspnetcoreapp.Battle.Enums;

namespace aspnetcoreapp.Battle;

public class GameLoop
{
    public GameLoop(Player player1, Player player2) {
        _gameState = GameState.Initialized;
        InitPlayers(player1, player2);
    }
    
    private GameState _gameState = GameState.NotStarted;

    private void InitPlayers(Player player1, Player player2)
    {
        // TODO
    }

    public void Start() {
        
    }
    
    public void ProcessPlayerAction(Player player, Player opponent, GameAction action)
    {
        // Process the player's action
        switch (action.Type)
        {
            case ActionType.Attack:
                // Handle attack action
                break;
            case ActionType.Switch:
                // Handle switch action
                break;
            case ActionType.Item:
                // Handle item action
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}