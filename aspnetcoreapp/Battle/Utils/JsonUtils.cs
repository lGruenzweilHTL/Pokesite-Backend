using System.Text.Json.Nodes;

namespace aspnetcoreapp.Battle.Utils;

public static class JsonUtils
{
    public static T ParseFromJson<T>(JsonNode json, IFormatProvider? provider = null) 
        where T : IParsable<T>
    {
        return T.Parse(json.ToString(), provider);
    }
    
    public static bool TryParseFromJson<T>(JsonNode? json, out T result, IFormatProvider? provider = null) 
        where T : IParsable<T>
    {
        if (json != null) return T.TryParse(json.ToString(), provider, out result);
        
        result = default!;
        return false;
    }

    public static bool TryParseBattleRequest(object request, out Player player1, out Player player2)
    {
        // Implement the logic to parse the battle request and create Player objects
        // For now, just return false to indicate failure
        player1 = null!;
        player2 = null!;
        
        JsonNode? json = JsonNode.Parse(request.ToString()!);
        
        if (json is not null)
        {
            return false;
        }
        
        return TryParsePlayer(json!["player1"]!, out player1) &&
               TryParsePlayer(json["player2"]!, out player2);
    }

    private static bool TryParsePlayer(JsonNode json, out Player player)
    {
        player = null;
        return false;
    }

    private static bool TryParsePokemon(JsonNode json, out Pokemon pokemon)
    {
        pokemon = null;
        return false;
    }
}