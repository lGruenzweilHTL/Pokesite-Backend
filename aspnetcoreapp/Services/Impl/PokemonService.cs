using Microsoft.EntityFrameworkCore;

public class PokemonService : IPokemonService {
    private readonly PokesiteDbContext _ctx;

    public PokemonService(PokesiteDbContext ctx) {
        _ctx = ctx;
    }
    
    public async Task<IEnumerable<DbPokemon>> GetAllPokemonAsync() {
        return await _ctx.Pokemon.ToListAsync();
    }
    public async Task<DbPokemon> GetPokemonByNameAsync(string name) {
        Console.WriteLine($"Looking for {name} in {string.Join(", ", _ctx.Pokemon.Select(p => p.Name))}");
        return (await _ctx.Pokemon.ToListAsync()).Find(p => p.Name.ToLower() == name.ToLower());
    }
}