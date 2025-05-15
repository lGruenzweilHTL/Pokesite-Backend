using Microsoft.EntityFrameworkCore;

public class DatabasePokemonService(PokesiteDbContext ctx) : IPokemonService {
    public async Task<IEnumerable<PokemonEntity>> GetAllPokemonAsync() {
        return await ctx.Pokemon.ToListAsync();
    }
    public async Task<PokemonEntity?> GetPokemonByNameAsync(string name) {
        return (await ctx.Pokemon
                .ToListAsync())
            .Find(p => p.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
    }

    public async Task<PokemonEntity?> GetPokemonByIdAsync(int id) {
        return await ctx.Pokemon
            .FindAsync(id); // Find by primary key
    }
    public async Task<IEnumerable<Pokemon>> GetAllFullPokemonAsync() {
        return await ctx.BuildFullPokemon()
            .ToListAsync();
    }
    public async Task<Pokemon?> GetFullPokemonByNameAsync(string name) {
        return await ctx.BuildFullPokemon()
            .FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower());
    }
    public async Task<Pokemon?> GetFullPokemonByIdAsync(int id) {
        return await ctx.BuildFullPokemon()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Pokemon?> GetPokemonWithMovesByNameAsync(string name, string[] moves)
    {
        var pokemon = await GetFullPokemonByNameAsync(name);
        if (pokemon == null) return null;
        
        var availableMoves = ctx.BuildFullMoves();
        pokemon.Moves = availableMoves.Where(p => moves.Contains(p.Name)).ToList();
        
        return pokemon;
    }
}