using System.Text.Json.Serialization;

public struct JoinBattleResponse
{
    [JsonPropertyName("websocket_url")]
    public string WebsocketUrl { get; set; }
}