using System.Text.Json.Nodes;

namespace aspnetcoreapp.Battle.Utils;

public static class JsonUtils
{
    public static bool TryParseBattleRequest(object request, out Player player1, out Player player2)
    {
        player1 = null!;
        player2 = null!;

        try
        {
            // Ensure the request is not null
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Request object cannot be null.");
            }

            // Parse the JSON from the request
            JsonNode? json = JsonNode.Parse(request.ToString()!);
            if (json == null)
            {
                throw new ArgumentException("Failed to parse JSON from the request.");
            }

            // Ensure "player1" and "player2" fields exist
            if (json["player1"] == null)
            {
                throw new ArgumentException("Missing 'player1' field in the JSON.");
            }

            if (json["player2"] == null)
            {
                throw new ArgumentException("Missing 'player2' field in the JSON.");
            }

            // Try parsing both players
            if (!TryParsePlayer(json["player1"]!, out player1))
            {
                throw new InvalidOperationException("Failed to parse 'player1' from the JSON.");
            }

            if (!TryParsePlayer(json["player2"]!, out player2))
            {
                throw new InvalidOperationException("Failed to parse 'player2' from the JSON.");
            }

            return true;
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            Console.WriteLine($"Error parsing battle request: {ex.Message}");
            return false;
        }
    }

    private static bool TryParsePlayer(JsonNode json, out Player player)
    {
        player = null!;

        try
        {
            // Extract and validate the player's name
            string? name = json["name"]?.GetValue<string>();
            if (string.IsNullOrEmpty(name)) return false;

            // Extract and validate the "human" field
            bool? isHuman = json["human"]?.GetValue<bool>();
            if (isHuman == null) return false;

            // Extract and validate the "pokemon" array
            JsonArray? pokemonArray = json["pokemon"]?.AsArray();
            if (pokemonArray == null || pokemonArray.Count == 0) return false;

            // Parse each Pokemon in the array
            var pokemons = new List<Pokemon>();
            foreach (var pokemonJson in pokemonArray)
            {
                if (pokemonJson == null) return false;

                if (!TryParsePokemon(pokemonJson, out Pokemon pokemon)) return false;
                pokemons.Add(pokemon);
            }

            // Create the Player object
            player = new Player(name, !isHuman.Value, pokemons.ToArray());
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static bool TryParsePokemon(JsonNode json, out Pokemon pokemon)
    {
        pokemon = null!;

        try
        {
            // Extract and validate the Pokemon's name
            string? name = json["name"]?.GetValue<string>();
            if (string.IsNullOrEmpty(name)) return false;

            // Extract and validate the Pokemon's level
            int? level = json["level"]?.GetValue<int>();
            if (level == null || level <= 0) return false;

            // Extract and validate the moves array
            JsonArray? movesArray = json["moves"]?.AsArray();
            if (movesArray == null || movesArray.Count == 0) return false;

            var moves = new List<string>();
            foreach (var moveJson in movesArray)
            {
                string? move = moveJson?.GetValue<string>();
                if (string.IsNullOrEmpty(move)) return false;
                moves.Add(move);
            }

            // Create the Pokemon object
            pokemon = new Pokemon
            {
                Name = name,
                Level = level.Value,
                Moves = Pokemon.MovesFromNames(moves)
            };

            return true;
        }
        catch
        {
            return false;
        }
    }
}