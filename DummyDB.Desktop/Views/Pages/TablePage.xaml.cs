using DummyDB.Core.Models;
using DummyDB.Desktop.ViewModels;
using DummyDB.Desktop.ViewModels.PagesViewModels;

namespace DummyDB.Desktop.Views.Pages;

public partial class TablePage
{
    public TablePage(Database db, Table table)
    {
        InitializeComponent();
        DataContext = new TablePageViewModel(this, table, db);
    }
}