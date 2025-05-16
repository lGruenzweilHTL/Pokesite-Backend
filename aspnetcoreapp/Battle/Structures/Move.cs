public class Move
{
    public string Name { get; set; }
    public string Description { get; set; }
    public PokemonTypeFlags Type { get; set; }
    public bool Special { get; set; }
    public int Priority { get; set; }
    public bool Status { get; set; }
    public int Power { get; set; }
    public int Accuracy { get; set; }
    public Effect[] Effects { get; set; }
}