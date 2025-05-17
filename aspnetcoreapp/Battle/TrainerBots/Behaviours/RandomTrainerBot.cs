[TrainerBotBehaviour("random")]
public class RandomTrainerBot : ITrainerBotBehaviour {
    public GameAction ChooseAction(Player bot, Player opponent) {
        Pokemon currentPokemon = bot.CurrentPokemon;
        if (currentPokemon.Moves.Count == 0) {
            throw new InvalidOperationException("The active Pokémon has no moves to choose from.");
        }

        Move move = currentPokemon.Moves.RandomElement();
        return new AttackAction(bot, opponent, move);
    }
}