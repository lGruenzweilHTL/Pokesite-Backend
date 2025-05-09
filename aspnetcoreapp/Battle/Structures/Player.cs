namespace aspnetcoreapp.Battle;

public class Player
{
    public Player(string name, bool isBot, Pokemon[] team) {
        Name = name;
        IsBot = isBot;
        Team = team;
        CurrentPokemonIndex = 0;
    }
    
    public string Name { get; set; }
    public bool IsBot { get; set; }
    public Pokemon[] Team { get; set; }
    public int CurrentPokemonIndex { get; set; }
    public Pokemon CurrentPokemon => Team[CurrentPokemonIndex];
    public Pokemon[] AlivePokemons => Team.Where(p => !p.Fainted).ToArray();
}