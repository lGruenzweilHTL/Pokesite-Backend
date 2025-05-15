using System.ComponentModel.DataAnnotations.Schema;

public class EffectEntity {
    public int Id { get; set; }
    public string Name { get; set; }
    public int Duration { get; set; }
    
    // TODO: map to effects table
    [NotMapped]
    public int Chance { get; set; }
    [NotMapped]
    public string Target { get; set; }
}