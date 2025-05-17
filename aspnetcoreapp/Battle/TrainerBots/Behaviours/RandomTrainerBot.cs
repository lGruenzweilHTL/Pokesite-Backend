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

    public SwitchAction OnPokemonFainted(Player bot) {
        int newPokemon = RandomUtils.FromRange(0, bot.AlivePokemons.Length);
        return new SwitchAction(bot, newPokemon);
    }
}