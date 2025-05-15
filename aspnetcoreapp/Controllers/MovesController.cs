using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/test/[controller]")]
public class MovesController(IMoveService moveService) : ControllerBase {
    [HttpGet]
    public async Task<IActionResult> GetAllMoves() {
        var moves = await moveService.GetAllMovesAsync();
        return Ok(moves);
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> GetMoveByName(string name) {
        var move = await moveService.GetMoveByNameAsync(name);
        if (move == null) {
            return NotFound($"Move with name '{name}' not found.");
        }
        return Ok(move);
    }
    
    [HttpGet("full")]
    public async Task<IActionResult> GetAllFullMoves() {
        var moves = await moveService.GetAllFullMovesAsync();
        return Ok(moves);
    }

    [HttpGet("full/{name}")]
    public async Task<IActionResult> GetFullMoveByName(string name) {
        var move = await moveService.GetFullMoveByNameAsync(name);
        if (move == null) {
            return NotFound($"Move with name '{name}' not found.");
        }
        return Ok(move);
    }
}