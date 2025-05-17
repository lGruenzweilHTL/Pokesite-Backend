using System.Text.Json.Serialization;

public struct ResponsePlayerData {
    [JsonPropertyName("bot")]
    public bool IsBot { get; set; }
    
    [JsonPropertyName("guid")]
    public string Guid { get; set; }
    
    [JsonPropertyName("pokemon")]
    public string Pokemon { get; set; }
    
    [JsonPropertyName("hp")]
    public int Hp { get; set; }
}