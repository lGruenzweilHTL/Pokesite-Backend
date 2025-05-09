using Microsoft.EntityFrameworkCore;

public class PokesiteDbContext : DbContext {
    public PokesiteDbContext(DbContextOptions ctx) : base(ctx) {
        
    }
    
    public DbSet<DbPokemon> Pokemon { get; set; }
}