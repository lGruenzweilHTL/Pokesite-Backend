using Microsoft.EntityFrameworkCore;

public class PokesiteDbContext : DbContext
{
    public PokesiteDbContext(DbContextOptions ctx) : base(ctx) { }

    public DbSet<PokemonEntity> Pokemon { get; set; }
    public DbSet<StatsEntity> Stats { get; set; }
    public DbSet<TypesEntity> Types { get; set; }
    public DbSet<EffectEntity> Effects { get; set; }
    public DbSet<MoveEntity> Moves { get; set; }
    public DbSet<MoveEffectsEntity> MoveEffects { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<StatsEntity>()
            .HasKey(s => s.PokemonId);

        modelBuilder.Entity<MoveEffectsEntity>()
            .HasKey(m => new { m.MoveId, m.EffectId });
    }
}