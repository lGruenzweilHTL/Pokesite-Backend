public class Pokemon
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Level { get; set; }
    public PokemonStats BaseStats { get; set; }
    public PokemonTypeFlags Types { get; set; }
    public int CurrentHp { get; set; }
    public int MaxHp { get; set; }
    public List<Move> Moves { get; set; } = new List<Move>();
    public List<Effect> StatusEffects { get; set; } = new List<Effect>();
    public List<Effect> ConditionalEffects { get; set; } = new List<Effect>();
    
    public bool Fainted => CurrentHp <= 0;
    public int Accuracy => GetModifiedStat(PokemonStat.Accuracy);
    public int Evasion => GetModifiedStat(PokemonStat.Evasion);

    private readonly Dictionary<PokemonStat, int> _statModifiers = new()
    {
        { PokemonStat.Attack, 0 },
        { PokemonStat.Defense, 0 },
        { PokemonStat.SpecialAttack, 0 },
        { PokemonStat.SpecialDefense, 0 },
        { PokemonStat.Speed, 0 },
        { PokemonStat.Accuracy, 0 },
        { PokemonStat.Evasion, 0 }
    };
    
    public int GetModifiedStat(PokemonStat stat)
    {
        int stage = Math.Clamp(_statModifiers[stat], -6, 6);

        double multiplier = Math.Pow((2 + Math.Abs(stage)) / 2d, stage > 0 ? 1 : -1);
        return BaseStats.GetStat(stat) * (int)multiplier;
    }
    
    public void UpdateStatModifier(PokemonStat stat, int change)
    {
        if (change == 0) return;

        _statModifiers[stat] = Math.Clamp(_statModifiers[stat] + change, -6, 6);
    }

    public static int CalculateStartingHp(Pokemon pokemon)
    {
        return 2 * pokemon.Level + 10 + pokemon.BaseStats.Hp;
    }
}