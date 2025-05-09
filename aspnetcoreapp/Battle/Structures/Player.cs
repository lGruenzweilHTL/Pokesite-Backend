namespace aspnetcoreapp.Battle;

public class Player
{
    public string Name { get; set; }
    public bool IsBot { get; set; }
    public Pokemon[] Team { get; set; }
    public int CurrentPokemonIndex { get; set; }
    public Pokemon CurrentPokemon => Team[CurrentPokemonIndex];
    public Pokemon[] AlivePokemons => Team.Where(p => !p.Fainted).ToArray();
}