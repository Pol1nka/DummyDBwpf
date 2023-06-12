using DummyDB.Core.Models;
using DummyDB.Desktop.ViewModels.UserControlsViewModels;
using DummyDB.Desktop.Views;

namespace DummyDB.Desktop.UserControls;

public partial class ColumnPanel
{
    public ColumnPanel(Column column, Schema schema, TableRedactingWindow window, string dbName, Table table)
    {
        InitializeComponent();
        DataContext = new ColumnPanelViewModel(column, schema, window, this, dbName, table);
    }
}