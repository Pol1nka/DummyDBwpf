using DummyDB.Core.Models;
using DummyDB.Desktop.ViewModels;

namespace DummyDB.Desktop.Views;

public partial class TableRedactingWindow
{
    public TableRedactingWindow(Table table, Database db)
    {
        InitializeComponent();
        DataContext = new TableRedactingWindowViewModel(this, table, db);
    }
}