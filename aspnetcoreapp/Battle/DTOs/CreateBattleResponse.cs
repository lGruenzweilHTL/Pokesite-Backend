using System.Text.Json;
using System.Text.Json.Serialization;

public struct CreateBattleResponse : IResponseDto {
    [JsonPropertyName("battle_guid")]
    public string BattleGuid { get; set; }
    
    public string ToJson() {
        return JsonSerializer.Serialize(this);
    }
}