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
        // Required for systems with case-sensitive SQL like Linux
        modelBuilder.Entity<PokemonEntity>().ToTable("pokemon");
        modelBuilder.Entity<StatsEntity>().ToTable("stats");
        modelBuilder.Entity<TypesEntity>().ToTable("types");
        modelBuilder.Entity<EffectEntity>().ToTable("effects");
        modelBuilder.Entity<MoveEntity>().ToTable("moves");
        modelBuilder.Entity<MoveEffectsEntity>().ToTable("move_effects");

        modelBuilder.Entity<StatsEntity>()
            .Property(s => s.SpecialAttack)
            .HasColumnName("spAttack");

        modelBuilder.Entity<StatsEntity>()
            .Property(s => s.SpecialDefense)
            .HasColumnName("spDefense");

        modelBuilder.Entity<StatsEntity>()
            .HasKey(s => s.PokemonId);

        modelBuilder.Entity<MoveEffectsEntity>()
            .HasKey(m => new { m.MoveId, m.EffectId });

        // Map effectName column to Name property
        modelBuilder.Entity<EffectEntity>()
            .Property(e => e.Name)
            .HasColumnName("effectName");
    }

    public IQueryable<Pokemon> BuildFullPokemon() {
        return Pokemon
            .Join(Stats, p => p.Id, s => s.PokemonId, (p, s) => new { p, s })
            .GroupJoin(Types, ps => ps.p.Id, t => t.PokemonId, (ps, types) => new { ps.p, ps.s, Types = types.Select(t => t.TypeName).ToList() })
            .Select(result => new Pokemon {
                Id = result.p.Id,
                Name = result.p.Name,
                Description = result.p.Description,
                BaseStats = new PokemonStats {
                    Attack = result.s.Attack,
                    Defense = result.s.Defense,
                    Speed = result.s.Speed,
                    SpecialAttack = result.s.SpecialAttack,
                    SpecialDefense = result.s.SpecialDefense,
                    Hp = result.s.Hp
                },
                Types = result.Types
            });
    }
    
    public IQueryable<Move> BuildFullMoves()
    {
        return (from move in Moves
                join moveEffect in MoveEffects on move.Id equals moveEffect.MoveId into moveEffects
                from moveEffect in moveEffects.DefaultIfEmpty()
                join effect in Effects on moveEffect.EffectId equals effect.Id into effects
                from effect in effects.DefaultIfEmpty()
                group effect by move)
            .Select(grouping => new Move
            {
                Name = grouping.Key.Name,
                Description = grouping.Key.Description,
                Type = grouping.Key.Type,
                Power = grouping.Key.Power,
                Accuracy = grouping.Key.Accuracy,
                Special = grouping.Key.Special,
                Priority = grouping.Key.Priority,
                Status = grouping.Key.Status,
                Effects = grouping
                    .Where(e => e != null)
                    .Select(e => new Effect
                    {
                        Name = e.Name,
                        Duration = e.Duration,
                        Chance = e.Chance,
                        Target = e.Target
                    })
                    .ToArray()
            });
    }
}