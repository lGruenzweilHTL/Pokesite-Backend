public static class TypeUtils {
    public static readonly Dictionary<PokemonType, Dictionary<PokemonType, double>> TypeWeaknesses = new()
{
    { PokemonType.Normal, new() { { PokemonType.Fighting, 2 }, { PokemonType.Ghost, 0 } } },
    { PokemonType.Fire, new() { { PokemonType.Fire, 0.5 }, { PokemonType.Water, 2 }, { PokemonType.Grass, 0.5 }, { PokemonType.Ice, 0.5 }, { PokemonType.Ground, 2 }, { PokemonType.Bug, 0.5 }, { PokemonType.Rock, 2 }, { PokemonType.Steel, 0.5 }, { PokemonType.Fairy, 0.5 } } },
    { PokemonType.Water, new() { { PokemonType.Fire, 0.5 }, { PokemonType.Water, 0.5 }, { PokemonType.Electric, 2 }, { PokemonType.Grass, 2 }, { PokemonType.Ice, 0.5 }, { PokemonType.Steel, 0.5 } } },
    { PokemonType.Electric, new() { { PokemonType.Electric, 0.5 }, { PokemonType.Ground, 2 }, { PokemonType.Flying, 0.5 }, { PokemonType.Steel, 0.5 } } },
    { PokemonType.Grass, new() { { PokemonType.Fire, 2 }, { PokemonType.Water, 0.5 }, { PokemonType.Electric, 0.5 }, { PokemonType.Grass, 0.5 }, { PokemonType.Ice, 2 }, { PokemonType.Poison, 2 }, { PokemonType.Ground, 0.5 }, { PokemonType.Flying, 2 }, { PokemonType.Bug, 2 } } },
    { PokemonType.Ice, new() { { PokemonType.Fire, 2 }, { PokemonType.Ice, 0.5 }, { PokemonType.Fighting, 2 }, { PokemonType.Rock, 2 }, { PokemonType.Steel, 2 } } },
    { PokemonType.Fighting, new() { { PokemonType.Flying, 2 }, { PokemonType.Psychic, 2 }, { PokemonType.Bug, 0.5 }, { PokemonType.Rock, 0.5 }, { PokemonType.Dark, 0.5 }, { PokemonType.Fairy, 2 } } },
    { PokemonType.Poison, new() { { PokemonType.Grass, 0.5 }, { PokemonType.Fighting, 0.5 }, { PokemonType.Poison, 0.5 }, { PokemonType.Ground, 2 }, { PokemonType.Psychic, 2 }, { PokemonType.Bug, 0.5 }, { PokemonType.Fairy, 0.5 } } },
    { PokemonType.Ground, new() { { PokemonType.Water, 2 }, { PokemonType.Grass, 2 }, { PokemonType.Ice, 2 }, { PokemonType.Poison, 0.5 }, { PokemonType.Rock, 0.5 }, { PokemonType.Electric, 0 } } },
    { PokemonType.Flying, new() { { PokemonType.Electric, 2 }, { PokemonType.Grass, 0.5 }, { PokemonType.Fighting, 0.5 }, { PokemonType.Bug, 0.5 }, { PokemonType.Rock, 2 }, { PokemonType.Ice, 2 }, { PokemonType.Ground, 0 } } },
    { PokemonType.Psychic, new() { { PokemonType.Fighting, 0.5 }, { PokemonType.Psychic, 0.5 }, { PokemonType.Bug, 2 }, { PokemonType.Ghost, 2 }, { PokemonType.Dark, 2 } } },
    { PokemonType.Bug, new() { { PokemonType.Fire, 2 }, { PokemonType.Grass, 0.5 }, { PokemonType.Fighting, 0.5 }, { PokemonType.Ground, 0.5 }, { PokemonType.Flying, 2 }, { PokemonType.Rock, 2 } } },
    { PokemonType.Rock, new() { { PokemonType.Normal, 0.5 }, { PokemonType.Fire, 0.5 }, { PokemonType.Water, 2 }, { PokemonType.Grass, 2 }, { PokemonType.Fighting, 2 }, { PokemonType.Poison, 0.5 }, { PokemonType.Ground, 2 }, { PokemonType.Flying, 0.5 }, { PokemonType.Steel, 2 } } },
    { PokemonType.Ghost, new() { { PokemonType.Normal, 0 }, { PokemonType.Fighting, 0 }, { PokemonType.Poison, 0.5 }, { PokemonType.Bug, 0.5 }, { PokemonType.Ghost, 2 }, { PokemonType.Dark, 2 } } },
    { PokemonType.Dragon, new() { { PokemonType.Fire, 0.5 }, { PokemonType.Water, 0.5 }, { PokemonType.Grass, 0.5 }, { PokemonType.Electric, 0.5 }, { PokemonType.Ice, 2 }, { PokemonType.Dragon, 2 }, { PokemonType.Fairy, 2 } } },
    { PokemonType.Dark, new() { { PokemonType.Fighting, 2 }, { PokemonType.Psychic, 0 }, { PokemonType.Bug, 2 }, { PokemonType.Ghost, 0.5 }, { PokemonType.Dark, 0.5 }, { PokemonType.Fairy, 2 } } },
    { PokemonType.Steel, new() { { PokemonType.Normal, 0.5 }, { PokemonType.Fire, 2 }, { PokemonType.Grass, 0.5 }, { PokemonType.Ice, 0.5 }, { PokemonType.Fighting, 2 }, { PokemonType.Poison, 0 }, { PokemonType.Ground, 2 }, { PokemonType.Flying, 0.5 }, { PokemonType.Psychic, 0.5 }, { PokemonType.Bug, 0.5 }, { PokemonType.Rock, 0.5 }, { PokemonType.Dragon, 0.5 }, { PokemonType.Steel, 0.5 }, { PokemonType.Fairy, 0.5 } } },
    { PokemonType.Fairy, new() { { PokemonType.Fighting, 0.5 }, { PokemonType.Poison, 2 }, { PokemonType.Bug, 0.5 }, { PokemonType.Dragon, 0 }, { PokemonType.Dark, 0.5 }, { PokemonType.Steel, 2 } } }
};
    
    public static double MoveEffectiveness(Move move, Pokemon defender) {
        return MoveEffectiveness(move.Type, defender.Types.ToArray());
    }
    public static double MoveEffectiveness(string moveType, string[] defenderTypes)
    {
        return PartialMoveEffectiveness(moveType, defenderTypes[0])
            * defenderTypes.Length > 1
                ? PartialMoveEffectiveness(moveType, defenderTypes[1])
                : 1;
    }

    private static double PartialMoveEffectiveness(string moveType, string defenderType) {
        // TODO: convert all types to enum, parsing logic in pokemon builder
        if (!Enum.TryParse<PokemonType>(moveType, out var move) || !Enum.TryParse<PokemonType>(defenderType, out var def)) {
            return 0;
        }

        return TypeWeaknesses[def][move];
    }
}