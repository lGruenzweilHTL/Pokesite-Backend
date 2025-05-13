public class Pokemon
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Level { get; set; }
    public PokemonStats BaseStats { get; set; }
    public List<string> Types { get; set; }
    public int CurrentHp { get; set; }
    public int MaxHp { get; set; }
    public List<Move> Moves { get; set; } = new List<Move>();
    public List<Effect> StatusEffects { get; set; } = new List<Effect>();
    public List<Effect> ConditionalEffects { get; set; } = new List<Effect>();
    public bool Fainted => CurrentHp <= 0;

    private Dictionary<string, int> StatModifiers { get; set; } = new Dictionary<string, int>
    {
        { "attack", 0 },
        { "defense", 0 },
        { "spAttack", 0 },
        { "spDefense", 0 },
        { "speed", 0 },
        { "accuracy", 0 },
        { "evasion", 0 }
    };

    // TODO: use this method whenever stats are needed
    public int GetModifiedStat(string statName)
    {
        int stage = Math.Clamp(StatModifiers[statName], -6, 6);

        double multiplier = Math.Pow((2 + Math.Abs(stage)) / 2d, stage > 0 ? 1 : -1);
        return BaseStats.GetStat(statName) * (int)multiplier;
    }

    public int GetAccuracyModifier() => GetModifiedStat("accuracy");
    public int GetEvasionModifier() => GetModifiedStat("evasion");

    public static int CalculateStartingHp(Pokemon pokemon)
    {
        return 2 * pokemon.Level + 10 + pokemon.BaseStats.Hp / 100;
    }
}