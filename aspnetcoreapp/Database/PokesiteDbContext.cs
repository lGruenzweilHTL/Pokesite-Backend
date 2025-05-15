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

    public IQueryable<Move> Test()
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
    public IQueryable<Move> BuildFullMoves()
    {
        return Moves
            .GroupJoin(MoveEffects, m => m.Id, me => me.MoveId, (m, moveEffects) => new { m = m, moveEffects })
            .SelectMany(
                temp => temp.moveEffects.DefaultIfEmpty(),
                (temp, moveEffect) => new { temp.m, moveEffect }
            )
            .GroupJoin(Effects, temp => temp.moveEffect.EffectId, e => e.Id, (temp, effects) => new { temp.m, temp.moveEffect, effects })
            .SelectMany(
                temp => temp.effects.DefaultIfEmpty(),
                (temp, effect) => new { temp.m, Effect = effect }
            )
            .GroupBy(
                result => result.m,
                result => result.Effect,
                (key, effects) => new Move
                {
                    Name = key.Name,
                    Description = key.Description,
                    Type = key.Type,
                    Power = key.Power,
                    Accuracy = key.Accuracy,
                    Special = key.Special,
                    Priority = key.Priority,
                    Status = key.Status,
                    Effects = effects
                        .Where(e => e != null)
                        .Select(e => new Effect
                        {
                            Name = e.Name,
                            Duration = e.Duration,
                            Chance = e.Chance,
                            Target = e.Target
                        })
                        .ToArray()
                }
            );
    }
}