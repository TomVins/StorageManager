using Avalonia.Controls;
using Avalonia.Interactivity;
using StorageManager.ManagmentService;
using StorageManager.Views;

namespace StorageManager;

public partial class MainWindow : Window
{
    private readonly Services _services = new();
    private readonly InventoryView _inventoryView = new();
    private readonly AddItemView _addItemView = new();

    public MainWindow()
    {
        InitializeComponent();

      
        _inventoryView.Initialize(_services);
        _addItemView.Initialize(_services, _inventoryView);

       
        MainContentControl.Content = _inventoryView;
    }

    private void ShowInventoryClick(object? sender, RoutedEventArgs e)
    {
        MainContentControl.Content = _inventoryView;
        _inventoryView.RefreshList(); 
    }

    private void ShowAddClick(object? sender, RoutedEventArgs e)
    {
        MainContentControl.Content = _addItemView;
    }
}