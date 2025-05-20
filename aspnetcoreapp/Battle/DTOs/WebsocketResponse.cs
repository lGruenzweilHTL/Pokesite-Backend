using System.Text.Json;
using System.Text.Json.Serialization;

public struct WebsocketResponse : IResponseDto
{
    [JsonPropertyName("messages")]
    public string[] Messages { get; set; }
    
    [JsonPropertyName("gameState")]
    public string GameState { get; set; }
    
    [JsonPropertyName("player1_hp")]
    public int Player1Hp { get; set; }
    
    [JsonPropertyName("player2_hp")]
    public int Player2Hp { get; set; }
    
    public string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }
}