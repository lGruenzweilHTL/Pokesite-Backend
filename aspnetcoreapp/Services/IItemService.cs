public interface IItemService {
    Task<IEnumerable<ItemEntity>> GetAllItemsAsync();
    Task<ItemEntity?> GetItemByIdAsync(int id);
    Task<ItemEntity?> GetItemByNameAsync(string name);
}