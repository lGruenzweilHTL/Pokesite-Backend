public class Item
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int HealingAmount { get; set; }
    public int Quantity { get; set; }
    public string Type { get; set; }

    public Item(string type, string name, Dictionary<string, Dictionary<string, dynamic>> itemsData)
    {
        var data = itemsData[type][name];
        Name = data.name;
        HealingAmount = data.healing_amount;
        Description = data.description;
        Quantity = data.quantity;
        Type = data.type;
    }
}