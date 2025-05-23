public record PokemonEntity(int Id, string Name, string Description, int TypeFlags);
public record StatsEntity(int PokemonId, int Attack, int Defense, int Speed, int SpecialAttack, int SpecialDefense, int Hp);
public record ItemEntity(int Id, string Name, string Description, int Amount, int Type);
public record MoveEntity(int Id, string Name, string Description, int TypeFlags, int Power, int Accuracy, bool Special, int Priority, bool Status);
public record EffectEntity(string EffectCode, string EffectName);
public record MoveEffectsEntity(int MoveId, string EffectCode, int Duration, int Chance, bool TargetsSelf);
