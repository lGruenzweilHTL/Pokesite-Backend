using System.Text.Json.Serialization;

public struct ActionRequest {
    [JsonPropertyName("type")]
    public string ActionType { get; set; }
    
    [JsonPropertyName("object")]
    public string Object { get; set; }
    
    [JsonPropertyName("battle_guid")]
    public string BattleGuid { get; set; }
}