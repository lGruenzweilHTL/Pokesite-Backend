using System.Text.Json;
using System.Text.Json.Serialization;

namespace aspnetcoreapp.Battle.DTOs;

public struct SocketResponse : IResponseDto
{
    [JsonPropertyName("messages")]
    public string[] Messages { get; set; }
    
    [JsonPropertyName("player1")]
    public BattlePlayerData Player1 { get; set; }
    
    [JsonPropertyName("player2")]
    public BattlePlayerData Player2 { get; set; }
    
    [JsonPropertyName("game_state")]
    public string GameState { get; set; }

    public string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }
}