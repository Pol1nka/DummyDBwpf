using DummyDB.Core.Models;
using DummyDB.Desktop.ViewModels;

namespace DummyDB.Desktop.Views;

public partial class DbRedactingWindow
{
    public DbRedactingWindow(Database database)
    {
        InitializeComponent();
        DataContext = new DbRedactingWindowViewModel(this, database);
    }
}