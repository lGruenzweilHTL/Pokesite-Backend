public class AttackAction(Player player, Player target, Move move) : GameAction(player) {
    public Player Target { get; set; } = target;
    public Move Move { get; set; } = move;
}