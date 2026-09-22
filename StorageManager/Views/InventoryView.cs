using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using StorageManager.ManagmentService;
using StorageManager.Storage;
using StorageManager.Views;


namespace StorageManager.Views;

public partial class InventoryView : UserControl
{
    private Services? _services;
    public bool _isLowStockFilterActive = false;

    public InventoryView()
    {
        InitializeComponent();
    }

    public void Initialize(Services services)
    {
        _services = services;
        RefreshList();
    }

    public void RefreshList()
    {
        if (_services != null)
        {
            ItemListBox.ItemsSource = _services.GetItems();
            UpdateTotalPrice();
        }
    }

    private void RemoveButton_Click(object? sender, RoutedEventArgs e)
    {
        var selectedItem = ItemListBox.SelectedItem as Item;
        if (selectedItem != null && _services != null)
        {
            _services.RemoveItem(selectedItem);
            RefreshList();
        }
    }

    private void UpdateTotalPrice()
    {
        if (_services != null)
        {
            decimal total = 0;
            foreach (var item in _services.GetItems())
            {
                total += item.Price * item.Stock;
            }

            TotalValueText.Text = $"{total} $";
        }
    }

    private void SearchTextBox_TextChanged(object? sender, TextChangedEventArgs e)
    {
        ApplyFilterAndDisplay();
    }

    private void LowStockFilterButton_Click(object? sender, RoutedEventArgs e)
    {
        _isLowStockFilterActive = !_isLowStockFilterActive;

        if (_isLowStockFilterActive)
        {
            LowStockFilterButton.Background = Avalonia.Media.Brush.Parse("#FACC15");
            LowStockFilterButton.Foreground = Avalonia.Media.Brush.Parse("#18181B");
            LowStockFilterButton.BorderBrush = Avalonia.Media.Brush.Parse("#FACC15");
        }
        else
        {
            LowStockFilterButton.Background = Avalonia.Media.Brush.Parse("#27272A");
            LowStockFilterButton.Foreground = Avalonia.Media.Brush.Parse("#FACC15");
            LowStockFilterButton.BorderBrush = Avalonia.Media.Brush.Parse("#3F3F46");
        }

        ApplyFilterAndDisplay();
    }

    private void ApplyFilterAndDisplay()
    {
        if (_services == null) return;

        var allItems = _services.GetItems();
        string query = SearchTextBox?.Text?.ToLower()?.Trim() ?? string.Empty;

        var filtered = allItems.Where(item => 
            (string.IsNullOrEmpty(query) || item.Name.ToLower().Contains(query) || item.ID.ToString().ToLower().Contains(query)) &&
            (!_isLowStockFilterActive || item.Stock < 5)
        ).ToList();

        ItemListBox.ItemsSource = filtered;
    }

    private async void EditMenuItem_Click(object? sender, RoutedEventArgs e)
    {
        var selectedItem = ItemListBox.SelectedItem as Item;
        if (selectedItem != null)
        {
            var editWindow = new EditItemWindow(selectedItem);
            var mainWindow = Avalonia.Application.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop ? desktop.MainWindow : null;
            if (mainWindow != null)
            {
                var result = await editWindow.ShowDialog<bool?>(mainWindow);
                if (result == true)
                {
                    RefreshList();
                }
            }
        }
    }
}