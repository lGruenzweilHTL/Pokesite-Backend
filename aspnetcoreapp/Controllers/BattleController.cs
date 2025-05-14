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

        GameLoop game = new(player1, player2);
        game.Start();
        return Ok("Battle created");
    }

    [HttpPost("action")]
    public IActionResult BattleAction([FromBody] object action) {
        return NotFound("Not implemented yet.");
    }
}