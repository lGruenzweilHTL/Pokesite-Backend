using System.Text.Json;
using System.Text.Json.Serialization;

public struct JoinBattleResponse : IResponseDto {
    [JsonPropertyName("player_guid")]
    public string PlayerGuid { get; set; }
    
    public string ToJson() {
        return JsonSerializer.Serialize(this);
    }
}