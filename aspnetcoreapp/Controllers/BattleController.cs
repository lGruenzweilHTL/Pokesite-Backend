using aspnetcoreapp.Battle;
using aspnetcoreapp.Battle.Utils;
using Microsoft.AspNetCore.Mvc;

namespace aspnetcoreapp.Controllers;

[ApiController]
[Route("battle")]
public class BattleController : ControllerBase
{
    [HttpPost("start")]
    public IActionResult StartBattle([FromBody] object battleRequest)
    {
        if (!JsonUtils.TryParseBattleRequest(battleRequest, out var player1, out var player2))
        {
            return BadRequest("Could not parse battle request");
        }

        GameLoop game = new();
        game.Init(player1, player2);
        return Ok("Battle started!");
    }
}