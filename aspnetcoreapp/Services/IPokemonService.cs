public interface IPokemonService {
    Task<IEnumerable<PokemonEntity>> GetAllPokemonAsync();
    Task<PokemonEntity?> GetPokemonByNameAsync(string name);
}