using Microsoft.EntityFrameworkCore;

public class DatabasePokemonService(PokesiteDbContext ctx) : IPokemonService {
    public async Task<IEnumerable<PokemonEntity>> GetAllPokemonAsync() {
        return await ctx.Pokemon.ToListAsync();
    }
    public async Task<PokemonEntity?> GetPokemonByNameAsync(string name) {
        return (await ctx.Pokemon.ToListAsync()).Find(p => p.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
    }
}