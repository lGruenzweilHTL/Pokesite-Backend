public class Move
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
    public bool Special { get; set; }
    public bool Priority { get; set; }
    public bool Status { get; set; }
    public int Power { get; set; }
    public int Accuracy { get; set; }
    public Effect? Effect { get; set; }

    public Move(string key, Dictionary<string, dynamic> movesData)
    {
        var data = movesData[key];
        Name = data.display_name;
        Description = data.description;
        Type = data.type;
        Special = data.special;
        Priority = data.priority;
        Status = data.status;
        Power = data.power;
        Accuracy = data.accuracy;
        Effect = data.effect ?? new Effect(data.effect);
    }
}