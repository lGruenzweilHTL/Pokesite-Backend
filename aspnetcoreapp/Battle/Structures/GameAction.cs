using aspnetcoreapp.Battle.Enums;

public class GameAction
{
    public ActionType Type { get; set; }
    public Move Move { get; set; }
    public Pokemon Pokemon { get; set; }
    public string Target { get; set; }

    public GameAction(ActionType type, Move move, Pokemon pokemon, string target)
    {
        Type = type;
        Move = move;
        Pokemon = pokemon;
        Target = target;
    }

    public bool GoesBefore(GameAction otherAction)
    {
        var actionPriorities = new Dictionary<ActionType, int>
        {
            { ActionType.Attack, 0 },
            { ActionType.Item, 1 },
            { ActionType.Switch, 2 },
            { ActionType.Run, 3 }
        };

        int actionPriority = actionPriorities[Type];
        int otherPriority = actionPriorities[otherAction.Type];

        if (actionPriority != otherPriority)
        {
            return actionPriority > otherPriority;
        }

        if (Move.Priority != otherAction.Move.Priority)
        {
            return Move.Priority;
        }

        return Pokemon.GetModifiedStat("speed") > otherAction.Pokemon.GetModifiedStat("speed");
    }
}