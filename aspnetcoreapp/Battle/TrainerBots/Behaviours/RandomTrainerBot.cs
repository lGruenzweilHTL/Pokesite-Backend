[TrainerBotBehaviour("random")]
public class RandomTrainerBot : ITrainerBotBehaviour {
    public GameAction ChooseAction(Player bot, Player opponent) {
        Pokemon currentPokemon = bot.CurrentPokemon;
        if (currentPokemon.Moves.Count == 0) {
            throw new InvalidOperationException("The active Pokémon has no moves to choose from.");
        }

        Move move = RandomElement(bot.CurrentPokemon.Moves);
        return new GameAction(ActionType.Attack, move, currentPokemon, opponent.CurrentPokemon);
    }

    private static T RandomElement<T>(List<T> collection) => collection[Random.Shared.Next(collection.Count)];
}