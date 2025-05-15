public interface IPokemonService {
    Task<IEnumerable<PokemonEntity>> GetAllPokemonAsync();
    Task<PokemonEntity?> GetPokemonByNameAsync(string name);
    Task<PokemonEntity?> GetPokemonByIdAsync(int id);

    Task<IEnumerable<Pokemon>> GetAllFullPokemonAsync();
    Task<Pokemon?> GetFullPokemonByNameAsync(string name);
    Task<Pokemon?> GetFullPokemonByIdAsync(int id);
    Task<Pokemon?> GetPokemonWithMovesByNameAsync(string name, string[] moves);
}