using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using DummyDB.Core.Models;
using DummyDB.Desktop.Views;

namespace DummyDB.Desktop.ViewModels;

public class DbRedactingWindowViewModel
{
    private const string DatabasesFolderPath = "../../../../DummyDb.Core/Databases";
    private readonly DbRedactingWindow _window;
    private readonly Database _db;
    public ICommand RenameDbCommand { get; init; }
    public ICommand SaveCommand { get; init; }

    public DbRedactingWindowViewModel(DbRedactingWindow window, Database db)
    {
        _window = window;
        _db = db;
        window.DatabaseName.Text = db.Name;
        InitTables();
        RenameDbCommand = new RelayCommand(RenameDb);
        SaveCommand = new RelayCommand(Save);
    }

    private void InitTables()
    {
        foreach (var table in _db.Tables)
        {
            var tablePanel = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };
            tablePanel.Children.Add(new TextBox { Text = table.Name });
            tablePanel.Children.Add(new Button { Content = "Переименовать", Command = new RelayCommand(_ =>
            {
                var tableDirectoryInfo = new DirectoryInfo($"{DatabasesFolderPath}/{_db.Name}/{table.Name}");
                tableDirectoryInfo.MoveTo($"{DatabasesFolderPath}/{_db.Name}/{(tablePanel.Children[0] as TextBox)!.Text}");
            })});
            tablePanel.Children.Add(new Button { Content = "Удалить", Command = new RelayCommand(_ =>
            {
                var tableDirectoryInfo = new DirectoryInfo($"{DatabasesFolderPath}/{_db.Name}/{table.Name}");
                DeleteDirectory(tableDirectoryInfo);
                _window.TablesNames.Children.Remove(tablePanel);
            })});
            _window.TablesNames.Children.Add(tablePanel);
        }
    }

    private static void DeleteDirectory(DirectoryInfo info)
    {
        var files = info.GetFiles();
        if (files.Length > 0)
        {
            foreach (var fileInfo in files)
            {
                fileInfo.Delete();
            }
        }

        info.Delete();
    }
    
    private void RenameDb(object? o)
    {
        Directory.Move($"{DatabasesFolderPath}/{_db.Name}",
            $"{DatabasesFolderPath}/{_window.DatabaseName.Text}");
        _db.Name = _window.DatabaseName.Text;
    }

    private void Save(object? o)
    {
        _window.Close();
    }
}