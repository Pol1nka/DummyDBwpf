using DummyDB.Desktop.ViewModels;
using DummyDB.Desktop.ViewModels.FormsViewModels;

namespace DummyDB.Desktop.Views.Forms;

public partial class DatabaseCreatingForm
{
    public DatabaseCreatingForm(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = new DatabaseCreatingViewModel(this, viewModel);
    }
}