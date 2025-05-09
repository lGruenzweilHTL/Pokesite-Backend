using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/test/[controller]")]
public class PokemonController : ControllerBase {
    private readonly IPokemonService _pokemonService;
    public PokemonController(IPokemonService pokemonService) {
        _pokemonService = pokemonService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPokemon() {
        var pokemon = await _pokemonService.GetAllPokemonAsync();
        return Ok(pokemon);
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> GetPokemonByName(string name) {
        var pokemon = await _pokemonService.GetPokemonByNameAsync(name);
        return Ok(pokemon);
    }
}