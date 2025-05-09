public class Effect
{
    public string Name { get; set; }
    public int Duration { get; set; }
    public int Chance { get; set; }
    public string Target { get; set; }
    public object Options { get; set; }

    public Effect(dynamic json)
    {
        Name = json.name;
        Duration = json.duration;
        Chance = json.chance;
        Target = json.target;
        Options = null;
    }
}