using System.Collections.Generic;
using DummyDB.Core.Models;
using DummyDB.Desktop.ViewModels;
using DummyDB.Desktop.ViewModels.FormsViewModels;

namespace DummyDB.Desktop.Views.Forms;

public partial class TableCreatingForm
{
    public TableCreatingForm(IEnumerable<Database> databases)
    {
        InitializeComponent();
        DataContext = new TableCreatingWindowViewModel(this, databases);
    }
}