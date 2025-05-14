using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("battle")]
public class BattleController(IPokemonService pokemonService) : ControllerBase
{
    [HttpPost("start")]
    public async Task<IActionResult> StartBattle([FromBody] object battleRequest) {
        JsonNode jsonNode = JsonNode.Parse(battleRequest.ToString()!);
        JsonNode player1Json = jsonNode["player1"];
        JsonNode player2Json = jsonNode["player2"];

        Pokemon[] player1Team = await Task.WhenAll(player1Json["pokemon"]
                .AsArray()
                .Select(async p => await pokemonService.GetFullPokemonByNameAsync(p["name"].ToString()))
            );
        Player player1 = new Player(
            player1Json["name"].GetValue<string>(),
            !player1Json["human"]!.GetValue<bool>(),
            player1Team);
        
        Pokemon[] player2Team = await Task.WhenAll(player2Json["pokemon"]
            .AsArray()
            .Select(async p => await pokemonService.GetFullPokemonByNameAsync(p["name"].ToString()))
        );
        Player player2 = new Player(
            player2Json["name"].GetValue<string>(),
            !player2Json["human"]!.GetValue<bool>(),
            player2Team);

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
        return Ok(response);
    }

    [HttpPost("action")]
    public IActionResult BattleAction([FromBody] object action) {
        return NotFound("Not implemented yet.");
    }
}