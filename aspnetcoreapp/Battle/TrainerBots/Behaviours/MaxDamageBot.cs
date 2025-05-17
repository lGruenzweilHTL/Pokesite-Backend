[TrainerBotBehaviour("max")]
public class MaxDamageBot : ITrainerBotBehaviour {
    public GameAction ChooseAction(Player bot, Player opponent) {
        Pokemon currentPokemon = bot.CurrentPokemon;
        Move move = currentPokemon.Moves.MaxBy(m => DamageUtils.CalculateDamage(currentPokemon, opponent.CurrentPokemon, m))
            ?? currentPokemon.Moves.RandomElement(); // if no move found, fallback
        return new AttackAction(bot, opponent, move);
    }
}