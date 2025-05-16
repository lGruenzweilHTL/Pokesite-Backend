using Microsoft.EntityFrameworkCore;

public class DatabaseItemService(PokesiteDbContext ctx) : IItemService {
    public async Task<IEnumerable<ItemEntity>> GetAllItemsAsync() {
        return await ctx.Items.ToListAsync();
    }
    public async Task<ItemEntity?> GetItemByIdAsync(int id) {
        return await ctx.Items.FindAsync(id);
    }
    public async Task<ItemEntity?> GetItemByNameAsync(string name) {
        return (await ctx.Items.ToListAsync())
            .FirstOrDefault(i => i.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
    }
}