using DummyDB.Core.Models;
using DummyDB.Desktop.ViewModels;

namespace DummyDB.Desktop.Views;

public partial class TableMetaDataWindow
{
    public TableMetaDataWindow(Schema schema)
    {
        InitializeComponent();
        DataContext = new TableMetaDataWindowViewModel(this, schema);
    }
}