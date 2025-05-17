public class Player
{
    public Player(string name, bool isBot, Pokemon[] team, List<Item> items) {
        Name = name;
        IsBot = isBot;
        Team = team;
        Items = items;
        CurrentPokemonIndex = 0;
    }
    
    public string Name { get; set; }
    public bool IsBot { get; set; }
    public Pokemon[] Team { get; set; }
    public List<Item> Items { get; set; }
    public int CurrentPokemonIndex { get; set; }
    public Pokemon CurrentPokemon => Team[CurrentPokemonIndex];
    public Pokemon[] AlivePokemons => Team.Where(p => !p.Fainted).ToArray();
    public bool Defeated => AlivePokemons.Length <= 0;

    public void InitializeTeam()
    {
        foreach (var pokemon in Team)
        {
            pokemon.MaxHp = Pokemon.CalculateStartingHp(pokemon);
            pokemon.CurrentHp = pokemon.MaxHp;
        }
    }

    public static List<Item> GetDefaultItems() {
        return [];
    }
}