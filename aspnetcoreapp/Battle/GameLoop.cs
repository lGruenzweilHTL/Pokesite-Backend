using aspnetcoreapp.Battle.Enums;

namespace aspnetcoreapp.Battle;

public class GameLoop
{
    private GameState _gameState = GameState.NotStarted;

    public void Init()
    {
        // Initialize the game loop
        _gameState = GameState.Initialized;
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