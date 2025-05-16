using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/test/[controller]")]
public class ItemsController(IItemService itemService) : ControllerBase {
    [HttpGet]
    public async Task<IActionResult> GetAllItems() {
        var items = await itemService.GetAllItemsAsync();
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetItemById(int id) {
        var item = await itemService.GetItemByIdAsync(id);
        return item == null ? NotFound($"Item with id: {id} not found.") : Ok(item);
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> GetItemByName(string name) {
        var item = await itemService.GetItemByNameAsync(name);
        return item == null ? NotFound($"Item with name: {name} not found.") : Ok(item);
    }
}