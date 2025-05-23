public class Effect
{
    public string Name { get; set; }
    public string Code { get; set; }
    public int Duration { get; set; }
    public int Chance { get; set; }
    public bool TargetsSelf { get; set; }

    public void Add(Pokemon attacker, Pokemon target)
    {
        if (!RandomUtils.Chance(Chance)) return;
        
        if (TargetsSelf)
        {
            attacker.StatusEffects.Add(this);
        }
        else
        {
            target.StatusEffects.Add(this);
        }
    }
}