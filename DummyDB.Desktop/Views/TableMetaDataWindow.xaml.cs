using System.Windows;
using DummyDB.Desktop.ViewModels;

namespace DummyDB.Desktop.Views;

public partial class TableMetaDataWindow : Window
{
    public TableMetaDataWindow(string schemaName, string dbName)
    {
        InitializeComponent();
        DataContext = new TableMetaDataWindowViewModel(this, schemaName, dbName);
    }
}