[TrainerBotBehaviour("random")]
public class RandomTrainerBot : ITrainerBotBehaviour {
    public GameAction ChooseAction(Pokemon[] team, Pokemon[] opponentTeam, int teamCurrentActive, int opponentCurrentActive) {
        if (team[teamCurrentActive].Moves.Count == 0) {
            throw new InvalidOperationException("The active Pokémon has no moves to choose from.");
        }

        Move move = RandomElement(team[teamCurrentActive].Moves);
        return new GameAction(ActionType.Attack, move, team[teamCurrentActive], opponentTeam[opponentCurrentActive]);
    }

    private static T RandomElement<T>(List<T> collection) => collection[Random.Shared.Next(collection.Count)];
}