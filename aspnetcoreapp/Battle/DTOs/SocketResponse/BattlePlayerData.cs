using System.Text.Json.Serialization;

namespace aspnetcoreapp.Battle.DTOs;

public struct BattlePlayerData
{
    [JsonPropertyName("pokemon")]
    public BattlePokemonData Pokemon { get; set; }
    
    [JsonPropertyName("items")]
    public string[] Items { get; set; }
    
    [JsonPropertyName("switches")]
    public string[] Switches { get; set; }
}