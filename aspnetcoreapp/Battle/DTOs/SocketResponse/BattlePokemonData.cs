using System.Text.Json.Serialization;

namespace aspnetcoreapp.Battle.DTOs;

public struct BattlePokemonData
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("level")]
    public int Level { get; set; }
    
    [JsonPropertyName("current_hp")]
    public int CurrentHp { get; set; }
    
    [JsonPropertyName("max_hp")]
    public int MaxHp { get; set; }
    
    [JsonPropertyName("effects")]
    public string[] StatusEffects { get; set; }
    
    [JsonPropertyName("moves")]
    public string[] Moves { get; set; }
}