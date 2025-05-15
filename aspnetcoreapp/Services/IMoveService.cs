public interface IMoveService {
    Task<IEnumerable<MoveEntity>> GetAllMovesAsync();
    Task<MoveEntity?> GetMoveByNameAsync(string name);

    Task<IEnumerable<Move>> GetAllFullMovesAsync();
    Task<Move?> GetFullMoveByNameAsync(string name);
}