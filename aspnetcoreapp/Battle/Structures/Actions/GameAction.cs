public abstract class GameAction : IComparable<GameAction> {
    public Player Player { get; set; }
    public Pokemon Pokemon => Player.CurrentPokemon;

    public GameAction(Player player)
    {
        Player = player;
    }

    public bool GoesBefore(GameAction otherAction) => CompareTo(otherAction) < 0;
    
    public int CompareTo(GameAction? other) {
        if (this is SwitchAction) return -1;
        if (other is SwitchAction) return 1;

        if (this is ItemAction) return -1;
        if (other is ItemAction) return 1;

        if (this is AttackAction thisAttack && other is AttackAction otherAttack)
        {
            if (thisAttack.Move.Priority != otherAttack.Move.Priority)
            {
                return otherAttack.Move.Priority - thisAttack.Move.Priority;
            }
            return other.Pokemon.GetModifiedStat(PokemonStat.Speed) - Pokemon.GetModifiedStat(PokemonStat.Speed);
        }

        return 0;
    }

    public override string ToString() {
        switch (this) {
            case AttackAction:
                return "Attack";
            case SwitchAction:
                return "Switch Pokémon";
            case ItemAction:
                return "Use Item";
        }

        return "Unknown";
    }
}