using System.Windows;
using DummyDB.Desktop.ViewModels;

namespace DummyDB.Desktop.Views;

public partial class DbMetaDataWindow : Window
{
    public DbMetaDataWindow()
    {
        InitializeComponent();
        DataContext = new DbMetaDataViewModel(this);
    }
}