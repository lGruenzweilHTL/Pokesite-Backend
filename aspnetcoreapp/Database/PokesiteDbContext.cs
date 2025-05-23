using Microsoft.EntityFrameworkCore;

public class PokesiteDbContext : DbContext
{
    public PokesiteDbContext(DbContextOptions ctx) : base(ctx) { }

    public DbSet<PokemonEntity> Pokemon { get; set; }
    public DbSet<StatsEntity> Stats { get; set; }
    public DbSet<ItemEntity> Items { get; set; }
    public DbSet<EffectEntity> Effects { get; set; }
    public DbSet<MoveEntity> Moves { get; set; }
    public DbSet<MoveEffectsEntity> MoveEffects { get; set; }
    public DbSet<LearnableMovesEntity> LearnableMoves { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        // Required for systems with case-sensitive SQL like Linux
        modelBuilder.Entity<PokemonEntity>().ToTable("pokemon");
        modelBuilder.Entity<StatsEntity>().ToTable("stats");
        modelBuilder.Entity<EffectEntity>().ToTable("effects");
        modelBuilder.Entity<MoveEntity>().ToTable("moves");
        modelBuilder.Entity<MoveEffectsEntity>().ToTable("move_effects");
        modelBuilder.Entity<ItemEntity>().ToTable("items");
        modelBuilder.Entity<LearnableMovesEntity>().ToTable("learnable_moves");

        modelBuilder.Entity<StatsEntity>()
            .Property(s => s.SpecialAttack)
            .HasColumnName("spAttack");

        modelBuilder.Entity<StatsEntity>()
            .Property(s => s.SpecialDefense)
            .HasColumnName("spDefense");
        
        modelBuilder.Entity<LearnableMovesEntity>()
            .Property(m => m.Level)
            .HasColumnName("levelLearned");

        modelBuilder.Entity<StatsEntity>()
            .HasKey(s => s.PokemonId);

        modelBuilder.Entity<MoveEffectsEntity>()
            .HasKey(m => new { m.MoveId, m.EffectCode });

        modelBuilder.Entity<EffectEntity>()
            .HasKey(e => e.EffectCode);
        
        modelBuilder.Entity<LearnableMovesEntity>()
            .HasKey(m => new { m.PokemonId, m.MoveId });
    }

    public IQueryable<Pokemon> BuildFullPokemon() {
        return Pokemon
            .Join(Stats, p => p.Id, s => s.PokemonId, (p, s) => new { p, s })
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
                Types = (PokemonTypeFlags)result.p.TypeFlags
            });
    }
    
    public IQueryable<Move> BuildFullMoves()
    {
        return (from move in Moves
                join moveEffect in MoveEffects on move.Id equals moveEffect.MoveId into moveEffects
                from moveEffect in moveEffects.DefaultIfEmpty()
                join effect in Effects on moveEffect.EffectCode equals effect.EffectCode into effects
                from effect in effects.DefaultIfEmpty()
                group new { moveEffect, effect } by move)
            .Select(grouping => new Move
            {
                Name = grouping.Key.Name,
                Description = grouping.Key.Description,
                Type = (PokemonTypeFlags)grouping.Key.TypeFlags,
                Power = grouping.Key.Power,
                Accuracy = grouping.Key.Accuracy,
                Special = grouping.Key.Special,
                Priority = grouping.Key.Priority,
                Status = grouping.Key.Status,
                Effects = grouping
                    .Where(e => e.effect != null && e.moveEffect != null)
                    .Select(e => new Effect
                    {
                        Name = e.effect.EffectName,
                        Code = e.effect.EffectCode,
                        Duration = e.moveEffect.Duration,
                        Chance = e.moveEffect.Chance,
                        TargetsSelf = e.moveEffect.TargetsSelf
                    })
                    .ToArray()
            });
    }
    
    public IQueryable<MoveEntity> GetLearnableMoves(string pokemon, int level)
    {
        return from move in Moves
                join learnableMove in LearnableMoves on move.Id equals learnableMove.MoveId
                join pokemonEntity in Pokemon on learnableMove.PokemonId equals pokemonEntity.Id
                where pokemonEntity.Name.ToLower() == pokemon.ToLower() && learnableMove.Level <= level
                select new MoveEntity(
                    move.Id,
                    move.Name,
                    move.Description,
                    move.TypeFlags,
                    move.Power,
                    move.Accuracy,
                    move.Special,
                    move.Priority,
                    move.Status
                );
    }

    public IQueryable<Item> GetItems() {
        return Items.Select(i => new Item {
            Name = i.Name,
            Description = i.Description,
            Amount = i.Amount,
            Type = i.Type
        });
    }
}
