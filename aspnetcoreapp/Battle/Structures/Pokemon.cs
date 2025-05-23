public class Pokemon
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Level { get; set; }
    public PokemonStats BaseStats { get; init; }
    public PokemonTypeFlags Types { get; init; }
    public int CurrentHp { get; set; }
    public int MaxHp { get; set; }
    public List<Move> Moves { get; set; } = new List<Move>();
    public List<Effect> StatusEffects { get; private set; } = [];

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

    public bool CanAttack(out string message)
    {
        message = "";
        
        var effectNames = StatusEffects.Select(e => e.Name).ToArray();

        if (effectNames.Contains("flinch"))
        {
            message = $"{Name} flinched and couldn't move!";
            return false;
        }
        
        if (effectNames.Contains("paralysis") && RandomUtils.Chance(25))
        {
            message = $"{Name} is paralyzed and cannot move!";
            return false;
        }

        if (effectNames.Contains("freeze"))
        {
            message = $"{Name} is frozen solid!";
            return false;
        }

        if (effectNames.Contains("sleep"))
        {
            message = $"{Name} is asleep!";
            return false;
        }

        if (effectNames.Contains("confusion"))
        {
            if (RandomUtils.Chance(50))
            {
                return true; // 50% chance for the attack to hit
            }

            // Confusion damage is calculated as a typeless move with 40 power
            var confusionMove = new Move
            {
                Accuracy = 100,
                Description = "Confusion damage",
                Name = "Confusion",
                Power = 40,
                Type = PokemonTypeFlags.None,
                Special = false,
                Status = false,
                Effects = [],
                Priority = 0,
            };
            var confusionDamage = DamageUtils.CalculateDamage(this, this, confusionMove);
            CurrentHp -= confusionDamage;
            message = $"{Name} hurt itself in confusion!";
            return false;
        }

        return true;
    }

    public void ApplyStatusEffects()
    {
        foreach (var statusEffect in StatusEffects)
        {
            ApplyEffect(statusEffect);
        }

        StatusEffects = StatusEffects
            .Where(effect => effect.Duration > 0)
            .ToList();
    }

    private void ApplyEffect(Effect effect)
    {
        switch (effect.Code)
        {
            case "atk_up":
                UpdateStatModifier(PokemonStat.Attack, 1);
                break;
            case "atk_down":
                UpdateStatModifier(PokemonStat.Attack, -1);
                break;
            case "def_up":
                UpdateStatModifier(PokemonStat.Defense, 1);
                break;
            case "def_down":
                UpdateStatModifier(PokemonStat.Defense, -1);
                break;
            case "sp_atk_up":
                UpdateStatModifier(PokemonStat.SpecialAttack, 1);
                break;
            case "sp_atk_down":
                UpdateStatModifier(PokemonStat.SpecialAttack, -1);
                break;
            case "sp_def_up":
                UpdateStatModifier(PokemonStat.SpecialDefense, 1);
                break;
            case "sp_def_down":
                UpdateStatModifier(PokemonStat.SpecialDefense, -1);
                break;
            case "speed_up":
                UpdateStatModifier(PokemonStat.Speed, 1);
                break;
            case "speed_down":
                UpdateStatModifier(PokemonStat.Speed, -1);
                break;

            case "heal":
                CurrentHp = Math.Min(CurrentHp + 100, MaxHp);
                break;
            
            case "burn":
                var burnDamage = (int)(MaxHp * 0.0625);
                CurrentHp -= burnDamage;
                break;
            case "poison":
                var poisonDamage = (int)(MaxHp * 0.0625);
                CurrentHp -= poisonDamage;
                break;
            case "faint":
                CurrentHp = 0;
                break;
        }

        effect.Duration--;
    }
}