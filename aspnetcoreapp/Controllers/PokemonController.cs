using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/test/[controller]")]
public class PokemonController(IPokemonService pokemonService) : ControllerBase {
    [HttpGet]
    public async Task<IActionResult> GetPokemon() {
        var pokemon = await pokemonService.GetAllPokemonAsync();
        return Ok(pokemon);
    }
    [HttpGet("full")]
    public async Task<IActionResult> GetFullPokemon() {
        var pokemon = await pokemonService.GetAllFullPokemonAsync();
        return Ok(pokemon);
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> GetPokemonByName(string name) {
        var pokemon = await pokemonService.GetPokemonByNameAsync(name);
        if (pokemon == null) {
            return NotFound($"Pokemon with name '{name}' not found.");
        }
        return Ok(pokemon);
    }

    [HttpGet("full/{name}")]
    public async Task<IActionResult> GetFullPokemonByName(string name) {
        var pokemon = await pokemonService.GetFullPokemonByNameAsync(name);
        if (pokemon == null) {
            return NotFound($"Pokemon with name '{name}' not found.");
        }

        return Ok(pokemon);
    }
}