public class ItemAction(Player player, Item item) : GameAction(player) {
    public Item Item { get; set; } = item;
}