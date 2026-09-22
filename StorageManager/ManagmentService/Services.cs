using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using StorageManager.Storage;

namespace StorageManager.ManagmentService;

public class Services
{
    private ObservableCollection<Item> _items = new();
    private readonly string _filePath = "inventory.json";

    public Services()
    {
        LoadData();
    }

    public ObservableCollection<Item> GetItems()
    {
        return _items;
    }

    public bool AddItem(Item item)
    {
        bool exists = _items.Any(i => i.ID == item.ID);

        if (exists)
        {
            return false;   
        }
        
        _items.Add(item);
        SaveData();
        return true;
    }

    public void RemoveItem(Item item)
    {
        _items.Remove(item);
        SaveData();
    }

    private void SaveData()
    {
        string jsonString = JsonSerializer.Serialize(_items, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, jsonString);
    }

    private void LoadData()
    {
        if (File.Exists(_filePath))
        {
            string jsonString = File.ReadAllText(_filePath);
            var loadedList = JsonSerializer.Deserialize<List<Item>>(jsonString) ?? new();
            _items = new ObservableCollection<Item>(loadedList);
        }
    }
}