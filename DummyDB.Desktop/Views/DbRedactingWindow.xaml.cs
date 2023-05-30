using System.Windows;
using DummyDB.Desktop.ViewModels;

namespace DummyDB.Desktop.Views;

public partial class DbRedactingWindow : Window
{
    public DbRedactingWindow(string databases)
    {
        InitializeComponent();
        DataContext = new DbRedactingWindowViewModel(this, databases);
    }
}