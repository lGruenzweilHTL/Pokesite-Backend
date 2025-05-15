[TrainerBotBehaviour("random")]
public class RandomTrainerBot : ITrainerBotBehaviour {
    public GameAction ChooseAction(Player bot, Player opponent) {
        Pokemon currentPokemon = bot.CurrentPokemon;
        if (currentPokemon.Moves.Count == 0) {
            throw new InvalidOperationException("The active Pokémon has no moves to choose from.");
        }

        Move move = RandomElement(currentPokemon.Moves);
        return new AttackAction(bot, opponent, move);
    }

    private static T RandomElement<T>(List<T> collection) => collection[Random.Shared.Next(collection.Count)];
}