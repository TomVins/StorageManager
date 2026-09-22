using Avalonia.Controls;
using Avalonia.Interactivity;
using StorageManager.Storage;

namespace StorageManager.Views;

public partial class EditItemWindow : Window
{
    private Item _itemToEdit;

    public EditItemWindow()
    {
        InitializeComponent();
    }

    public EditItemWindow(Item item) : this()
    {
        _itemToEdit = item;

        IdTextBox.Text = item.ID.ToString();
        NameTextBox.Text = item.Name;
        PriceTextBox.Text = item.Price.ToString();
        StockTextBox.Text = item.Stock.ToString();
    }

    private void SaveButton_Click(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(NameTextBox.Text) || string.IsNullOrEmpty(PriceTextBox.Text) || string.IsNullOrEmpty(StockTextBox.Text) || string.IsNullOrEmpty(IdTextBox.Text))
        {
            MessageTextBlock.Foreground = Avalonia.Media.Brushes.DarkRed;
            MessageTextBlock.Text = "Please fill in all fields!";
            return;
        }

        int.TryParse(IdTextBox.Text, out int id);
        decimal.TryParse(PriceTextBox.Text, out decimal price);
        int.TryParse(StockTextBox.Text, out int stock);

        _itemToEdit.ID = id;
        _itemToEdit.Name = NameTextBox.Text;
        _itemToEdit.Price = price;
        _itemToEdit.Stock = stock;

        Close(true);
    }
}