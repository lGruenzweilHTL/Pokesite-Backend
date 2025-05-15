using Microsoft.EntityFrameworkCore;

public class DatabaseMoveService(PokesiteDbContext ctx) : IMoveService {
    public async Task<IEnumerable<MoveEntity>> GetAllMovesAsync() {
        return await ctx.Moves.ToListAsync();
    }
    public async Task<MoveEntity?> GetMoveByNameAsync(string name) {
        return (await ctx.Moves.ToListAsync())
            .Find(m => m.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
    }
    public async Task<IEnumerable<Move>> GetAllFullMovesAsync() {
        return await ctx.BuildFullMoves().ToListAsync();
    }
    public async Task<Move?> GetFullMoveByNameAsync(string name) {
        return await ctx.BuildFullMoves()
            .FirstOrDefaultAsync(m => m.Name.ToLower() == name.ToLower());
    }
}