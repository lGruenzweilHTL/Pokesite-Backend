public interface ITrainerBotBehaviour {
    GameAction ChooseAction(Player bot, Player opponent);
    SwitchAction OnPokemonFainted(Player bot);
}