public class PokemonStats
{
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Speed { get; set; }
    public int SpecialAttack { get; set; }
    public int SpecialDefense { get; set; }
    public int Hp { get; set; }
    
    public int GetStat(PokemonStat stat) => stat switch {
        PokemonStat.Attack => Attack,
        PokemonStat.Defense => Defense,
        PokemonStat.Speed => Speed,
        PokemonStat.SpecialAttack => SpecialAttack,
        PokemonStat.SpecialDefense => SpecialDefense,
        PokemonStat.Hp => Hp
    };
}