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
    
    public IQueryable<Move> BuildFullMove()
    {
        return Moves
            .GroupJoin(MoveEffects, m => m.Id, me => me.MoveId, (m, moveEffects) => new { m, moveEffects })
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