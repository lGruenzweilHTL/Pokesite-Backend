using System.Text.Json;
using System.Text.Json.Serialization;

public class StartBotMatchResponse : IResponseDto
{
    [JsonPropertyName("websocket_url")]
    public string WebsocketUrl { get; set; }
    
    [JsonPropertyName("battle_guid")]
    public string BattleGuid { get; set; }

    public string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }
}