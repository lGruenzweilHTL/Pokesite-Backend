using System.Text.Json.Serialization;

public struct ResponsePlayerData {
    [JsonPropertyName("guid")]
    public string Guid { get; set; }
    
    [JsonPropertyName("pokemon")]
    public string Pokemon { get; set; }
    
    [JsonPropertyName("hp")]
    public int Hp { get; set; }
}