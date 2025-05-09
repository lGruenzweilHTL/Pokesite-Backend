public interface IPokemonService {
    Task<IEnumerable<DbPokemon>> GetAllPokemonAsync();
    Task<DbPokemon> GetPokemonByNameAsync(string name);
}