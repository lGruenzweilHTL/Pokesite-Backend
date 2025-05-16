public static class TypeUtils {
    public static readonly Dictionary<PokemonTypeFlags, Dictionary<PokemonTypeFlags, double>> TypeWeaknesses = new()
{
    { PokemonTypeFlags.Normal, new() { { PokemonTypeFlags.Fighting, 2 }, { PokemonTypeFlags.Ghost, 0 } } },
    { PokemonTypeFlags.Fire, new() { { PokemonTypeFlags.Fire, 0.5 }, { PokemonTypeFlags.Water, 2 }, { PokemonTypeFlags.Grass, 0.5 }, { PokemonTypeFlags.Ice, 0.5 }, { PokemonTypeFlags.Ground, 2 }, { PokemonTypeFlags.Bug, 0.5 }, { PokemonTypeFlags.Rock, 2 }, { PokemonTypeFlags.Steel, 0.5 }, { PokemonTypeFlags.Fairy, 0.5 } } },
    { PokemonTypeFlags.Water, new() { { PokemonTypeFlags.Fire, 0.5 }, { PokemonTypeFlags.Water, 0.5 }, { PokemonTypeFlags.Electric, 2 }, { PokemonTypeFlags.Grass, 2 }, { PokemonTypeFlags.Ice, 0.5 }, { PokemonTypeFlags.Steel, 0.5 } } },
    { PokemonTypeFlags.Electric, new() { { PokemonTypeFlags.Electric, 0.5 }, { PokemonTypeFlags.Ground, 2 }, { PokemonTypeFlags.Flying, 0.5 }, { PokemonTypeFlags.Steel, 0.5 } } },
    { PokemonTypeFlags.Grass, new() { { PokemonTypeFlags.Fire, 2 }, { PokemonTypeFlags.Water, 0.5 }, { PokemonTypeFlags.Electric, 0.5 }, { PokemonTypeFlags.Grass, 0.5 }, { PokemonTypeFlags.Ice, 2 }, { PokemonTypeFlags.Poison, 2 }, { PokemonTypeFlags.Ground, 0.5 }, { PokemonTypeFlags.Flying, 2 }, { PokemonTypeFlags.Bug, 2 } } },
    { PokemonTypeFlags.Ice, new() { { PokemonTypeFlags.Fire, 2 }, { PokemonTypeFlags.Ice, 0.5 }, { PokemonTypeFlags.Fighting, 2 }, { PokemonTypeFlags.Rock, 2 }, { PokemonTypeFlags.Steel, 2 } } },
    { PokemonTypeFlags.Fighting, new() { { PokemonTypeFlags.Flying, 2 }, { PokemonTypeFlags.Psychic, 2 }, { PokemonTypeFlags.Bug, 0.5 }, { PokemonTypeFlags.Rock, 0.5 }, { PokemonTypeFlags.Dark, 0.5 }, { PokemonTypeFlags.Fairy, 2 } } },
    { PokemonTypeFlags.Poison, new() { { PokemonTypeFlags.Grass, 0.5 }, { PokemonTypeFlags.Fighting, 0.5 }, { PokemonTypeFlags.Poison, 0.5 }, { PokemonTypeFlags.Ground, 2 }, { PokemonTypeFlags.Psychic, 2 }, { PokemonTypeFlags.Bug, 0.5 }, { PokemonTypeFlags.Fairy, 0.5 } } },
    { PokemonTypeFlags.Ground, new() { { PokemonTypeFlags.Water, 2 }, { PokemonTypeFlags.Grass, 2 }, { PokemonTypeFlags.Ice, 2 }, { PokemonTypeFlags.Poison, 0.5 }, { PokemonTypeFlags.Rock, 0.5 }, { PokemonTypeFlags.Electric, 0 } } },
    { PokemonTypeFlags.Flying, new() { { PokemonTypeFlags.Electric, 2 }, { PokemonTypeFlags.Grass, 0.5 }, { PokemonTypeFlags.Fighting, 0.5 }, { PokemonTypeFlags.Bug, 0.5 }, { PokemonTypeFlags.Rock, 2 }, { PokemonTypeFlags.Ice, 2 }, { PokemonTypeFlags.Ground, 0 } } },
    { PokemonTypeFlags.Psychic, new() { { PokemonTypeFlags.Fighting, 0.5 }, { PokemonTypeFlags.Psychic, 0.5 }, { PokemonTypeFlags.Bug, 2 }, { PokemonTypeFlags.Ghost, 2 }, { PokemonTypeFlags.Dark, 2 } } },
    { PokemonTypeFlags.Bug, new() { { PokemonTypeFlags.Fire, 2 }, { PokemonTypeFlags.Grass, 0.5 }, { PokemonTypeFlags.Fighting, 0.5 }, { PokemonTypeFlags.Ground, 0.5 }, { PokemonTypeFlags.Flying, 2 }, { PokemonTypeFlags.Rock, 2 } } },
    { PokemonTypeFlags.Rock, new() { { PokemonTypeFlags.Normal, 0.5 }, { PokemonTypeFlags.Fire, 0.5 }, { PokemonTypeFlags.Water, 2 }, { PokemonTypeFlags.Grass, 2 }, { PokemonTypeFlags.Fighting, 2 }, { PokemonTypeFlags.Poison, 0.5 }, { PokemonTypeFlags.Ground, 2 }, { PokemonTypeFlags.Flying, 0.5 }, { PokemonTypeFlags.Steel, 2 } } },
    { PokemonTypeFlags.Ghost, new() { { PokemonTypeFlags.Normal, 0 }, { PokemonTypeFlags.Fighting, 0 }, { PokemonTypeFlags.Poison, 0.5 }, { PokemonTypeFlags.Bug, 0.5 }, { PokemonTypeFlags.Ghost, 2 }, { PokemonTypeFlags.Dark, 2 } } },
    { PokemonTypeFlags.Dragon, new() { { PokemonTypeFlags.Fire, 0.5 }, { PokemonTypeFlags.Water, 0.5 }, { PokemonTypeFlags.Grass, 0.5 }, { PokemonTypeFlags.Electric, 0.5 }, { PokemonTypeFlags.Ice, 2 }, { PokemonTypeFlags.Dragon, 2 }, { PokemonTypeFlags.Fairy, 2 } } },
    { PokemonTypeFlags.Dark, new() { { PokemonTypeFlags.Fighting, 2 }, { PokemonTypeFlags.Psychic, 0 }, { PokemonTypeFlags.Bug, 2 }, { PokemonTypeFlags.Ghost, 0.5 }, { PokemonTypeFlags.Dark, 0.5 }, { PokemonTypeFlags.Fairy, 2 } } },
    { PokemonTypeFlags.Steel, new() { { PokemonTypeFlags.Normal, 0.5 }, { PokemonTypeFlags.Fire, 2 }, { PokemonTypeFlags.Grass, 0.5 }, { PokemonTypeFlags.Ice, 0.5 }, { PokemonTypeFlags.Fighting, 2 }, { PokemonTypeFlags.Poison, 0 }, { PokemonTypeFlags.Ground, 2 }, { PokemonTypeFlags.Flying, 0.5 }, { PokemonTypeFlags.Psychic, 0.5 }, { PokemonTypeFlags.Bug, 0.5 }, { PokemonTypeFlags.Rock, 0.5 }, { PokemonTypeFlags.Dragon, 0.5 }, { PokemonTypeFlags.Steel, 0.5 }, { PokemonTypeFlags.Fairy, 0.5 } } },
    { PokemonTypeFlags.Fairy, new() { { PokemonTypeFlags.Fighting, 0.5 }, { PokemonTypeFlags.Poison, 2 }, { PokemonTypeFlags.Bug, 0.5 }, { PokemonTypeFlags.Dragon, 0 }, { PokemonTypeFlags.Dark, 0.5 }, { PokemonTypeFlags.Steel, 2 } } }
};
    
    public static double EffectivenessAgainst(this Move move, Pokemon defender) {
        return MoveEffectiveness(move.Type, defender.Types);
    }
    public static double MoveEffectiveness(PokemonTypeFlags moveType, PokemonTypeFlags defenderTypes) {
        var def = defenderTypes.GetAllTypes().Select(Convert.ToDouble);
        return def.Aggregate((a, b) => 
            PartialMoveEffectiveness(moveType, (PokemonTypeFlags)a) 
            * PartialMoveEffectiveness(moveType, (PokemonTypeFlags)b));
    }

    public static int[] GetAllTypes(this PokemonTypeFlags types) {
        return (from flag in Enum.GetValues<PokemonTypeFlags>() 
            where flag != PokemonTypeFlags.None && types.HasFlag(flag) 
            select (int)flag).ToArray();
    }

    private static double PartialMoveEffectiveness(PokemonTypeFlags moveType, PokemonTypeFlags defenderType) {
        return TypeWeaknesses[defenderType][moveType];
    }
}