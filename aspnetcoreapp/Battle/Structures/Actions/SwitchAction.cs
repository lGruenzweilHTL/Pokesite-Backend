public class SwitchAction(Player player, int newPokemonIndex) : GameAction(player) {
    public int NewPokemonIndex { get; set; } = newPokemonIndex;
}