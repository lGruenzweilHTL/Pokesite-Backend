using System.Text.Json;
using System.Text.Json.Serialization;

public struct StartBattleResponse : IResponseDto {
    [JsonPropertyName("websocket_url")]
    public string WebsocketUrl { get; set; }

    [JsonPropertyName("players")] 
    public ResponsePlayerData[] Players { get; set; }
    
    public string ToJson() {
        return JsonSerializer.Serialize(this);
    }
}