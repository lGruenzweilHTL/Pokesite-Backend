public record PokemonEntity(int Id, string Name, string Description, int TypeFlags);
public record StatsEntity(int PokemonId, int Attack, int Defense, int Speed, int SpecialAttack, int SpecialDefense, int Hp);
public record ItemEntity(int Id, string Name, string Description, int Amount, string Type);
public record MoveEntity(int Id, string Name, string Description, int TypeFlags, int Power, int Accuracy, bool Special, int Priority, bool Status);
public record EffectEntity(int Id, string Name, int Duration, int Chance);
public record MoveEffectsEntity(int MoveId, int EffectId);