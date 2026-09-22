using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using StorageManager.Storage;
using StorageManager.ManagmentService;

namespace StorageManager.Views;

public partial class AddItemView : UserControl
{
    private Services? _services;
    private InventoryView? _inventoryView;

    public AddItemView()
    {
        InitializeComponent();
    }

   
    public void Initialize(Services services, InventoryView inventoryView)
    {
        _services = services;
        _inventoryView = inventoryView;
    }

    private void OnAddClick(object? sender, RoutedEventArgs e)
    {
        string name = NameTextBox.Text ?? string.Empty;

        int.TryParse(IdTextBox.Text, out int id);
        decimal.TryParse(PriceTextBox.Text, out decimal price);
        int.TryParse(StockTextBox.Text, out int stock);

        if (string.IsNullOrEmpty(NameTextBox.Text) || string.IsNullOrEmpty(PriceTextBox.Text) || string.IsNullOrEmpty(StockTextBox.Text) || string.IsNullOrEmpty(IdTextBox.Text))
        {
            MessageTextBlock.Foreground = Avalonia.Media.Brushes.DarkRed;
            MessageTextBlock.Text = "Please fill in all required fields!";
            return;
        }

        MessageTextBlock.Text = string.Empty;

        // KROK 1: Pokud právě upravujeme existující položku
        if (_editingItem != null)
        {
            _editingItem.ID = id;
            _editingItem.Name = name;
            _editingItem.Price = price;
            _editingItem.Stock = stock;

            MessageTextBlock.Foreground = Avalonia.Media.Brushes.ForestGreen;
            MessageTextBlock.Text = "Item successfully updated!";
            
            _editingItem = null;
        }
        else
        {
            
            Item newItem = new Item
            {
                ID = id,
                Name = name,
                Price = price,
                Stock = stock
            };
    
            bool success = _services?.AddItem(newItem) ?? false;

            if (!success)
            {
                MessageTextBlock.Foreground = Avalonia.Media.Brushes.DarkRed;
                MessageTextBlock.Text = "Error: Item with this ID already exists!";
                return;
            }
    
            MessageTextBlock.Foreground = Avalonia.Media.Brushes.ForestGreen;
            MessageTextBlock.Text = "Item successfully added!";
        }
    
        _inventoryView?.RefreshList(); 
        ClearInput();
    }

    public void ClearInput()
    {
        IdTextBox.Text = string.Empty;
        NameTextBox.Text = string.Empty;
        PriceTextBox.Text = string.Empty;
        StockTextBox.Text = string.Empty;
    }


    private Item? _editingItem;

    public void LoadItem(Item item)
    {
        _editingItem = item;

        IdTextBox.Text = item.ID.ToString();
        NameTextBox.Text = item.Name;
        PriceTextBox.Text = item.Price.ToString();
        StockTextBox.Text = item.Stock.ToString();
    }



}