using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class BattleController(IPokemonService pokemonService) : ControllerBase
{
    [HttpPost("start")]
    public async Task<IActionResult> StartBattle([FromBody] object battleRequest) {
        JsonNode jsonNode = JsonNode.Parse(battleRequest.ToString()!);
        JsonNode player1Json = jsonNode["player1"];
        JsonNode player2Json = jsonNode["player2"];

        Pokemon[] player1Team = await Task.WhenAll(player1Json["pokemon"]
                .AsArray()
                .Select(async p => await pokemonService.GetPokemonWithMovesByNameAsync(p["name"].ToString(),
                    p["moves"].AsArray().Select(m => m.ToString()).ToArray())));
        Player player1 = new Player(
            player1Json["name"].GetValue<string>(),
            !player1Json["human"]!.GetValue<bool>(),
            player1Team);
        
        Pokemon[] player2Team = await Task.WhenAll(player2Json["pokemon"]
            .AsArray()
            .Select(async p => await pokemonService.GetPokemonWithMovesByNameAsync(p["name"].ToString(),
                p["moves"].AsArray().Select(m => m.ToString()).ToArray())));
        Player player2 = new Player(
            player2Json["name"].GetValue<string>(),
            !player2Json["human"]!.GetValue<bool>(),
            player2Team);
        
        // Validate the players
        if (player1.Team.Length <= 0 || player2.Team.Length <= 0
            || player1.Team.Any(p => p == null) || player2.Team.Any(p => p == null)) {
            return BadRequest("Both players must have at least one valid Pokémon.");
        }

        // Create and start the GameLoop
        GameLoop game = new(player1, player2);
        int websocketPort = game.StartWithWebSocket(); // Start the WebSocket server and get the port

        // Return the WebSocket URL to the client
        JsonNode response = new JsonObject
        {
            ["websocket_url"] = $"ws://127.0.0.1:{websocketPort}",
            ["player1"] = new JsonObject {
                ["pokemon"] = player1.CurrentPokemon.Name,
                ["hp"] = player1.CurrentPokemon.CurrentHp
            },
            ["player2"] = new JsonObject {
                ["pokemon"] = player2.CurrentPokemon.Name,
                ["hp"] = player2.CurrentPokemon.CurrentHp
            }
        };
        return Ok(response.ToJsonString());
    }
}