using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("battle")]
public class BattleController(IPokemonService pokemonService) : ControllerBase
{
    [HttpPost("start")]
    public IActionResult StartBattle([FromBody] object battleRequest)
    {
        return NotFound("Not implemented yet.");
    }

    [HttpPost("action")]
    public IActionResult BattleAction([FromBody] object action) {
        return NotFound("Not implemented yet.");
    }
}