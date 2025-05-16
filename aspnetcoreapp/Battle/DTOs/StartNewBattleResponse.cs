using System.Text.Json;
using System.Text.Json.Serialization;

public struct StartNewBattleResponse : IResponseDto {
    [JsonPropertyName("battle_guid")]
    public string BattleGuid { get; set; }
    
    [JsonPropertyName("websocket_url")]
    public string WebsocketUrl { get; set; }
    
    [JsonPropertyName("player1")]
    public ResponsePlayerData Player1 { get; set; }
    
    [JsonPropertyName("player2")]
    public ResponsePlayerData Player2 { get; set; }
    
    public string ToJson() {
        return JsonSerializer.Serialize(this);
    }
}