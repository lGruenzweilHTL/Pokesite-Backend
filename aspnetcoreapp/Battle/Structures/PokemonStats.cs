public struct PokemonStats
{
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Speed { get; set; }
    public int SpecialAttack { get; set; }
    public int SpecialDefense { get; set; }
    public int Hp { get; set; }

    // TODO: make an enum for this
    public int GetStat(string name) => name.ToLower() switch {
        "attack" => Attack,
        "defense" => Defense,
        "speed" => Speed,
        "specialAttack" or "spAttack" or "spAtk" => SpecialAttack,
        "specialDefense" or "spDefense" or "spDef" => SpecialDefense,
        "hp" or "health" => Hp
    };
}