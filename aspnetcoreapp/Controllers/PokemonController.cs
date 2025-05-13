using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/test/[controller]")]
public class PokemonController(IPokemonService pokemonService) : ControllerBase {
    [HttpGet]
    public async Task<IActionResult> GetPokemon() {
        var pokemon = await pokemonService.GetAllPokemonAsync();
        return Ok(pokemon);
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> GetPokemonByName(string name) {
        var pokemon = await pokemonService.GetPokemonByNameAsync(name);
        return Ok(pokemon);
    }
}