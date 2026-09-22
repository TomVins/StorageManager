namespace StorageManager.Storage;

public class Item
{
    public int ID { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }

    public override string ToString()
    {
        return $"{ID} - {Name} ({Price}$ {Stock} pcs )";
    }
}

