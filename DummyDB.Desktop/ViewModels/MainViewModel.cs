using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using DummyDB.Core.Models;
using DummyDB.Desktop.Views;
using DummyDB.Desktop.Views.Forms;
using DummyDB.Desktop.Views.Pages;

namespace DummyDB.Desktop.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private const string DatabasesFolderPath = "../../../../DummyDB.Core/Databases";
    private readonly MainWindow _window;
    private Database _selectedDatabase = null!;

    public Database SelectedDatabase
    {
        get => _selectedDatabase;
        set
        {
            if (_selectedDatabase == value) return;
            _selectedDatabase = value;
            _window.TableChoosingBox.ItemsSource = new ObservableCollection<Table>(_selectedDatabase.Tables);
            if (PropertyChanged is not null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedDatabase)));
            }
        }
    }
    public ObservableCollection<Database> Databases { get; set; } = new (InitDatabases());
    public ICommand ShowTablePageCommand { get; init; }
    public ICommand ShowMetaDataCommand { get; init; }
    public ICommand ShowDbsMetaDataCommand { get; init; }
    public ICommand CreateDatabaseCommand { get; init; }
    public ICommand CreateTableCommand { get; init; }
    public ICommand DbRedactingCommand { get; init; }
    public ICommand RefreshCommand { get; init; }
    public ICommand OpenChangingMenuCommand { get; init; }

    public MainViewModel(MainWindow window)
    {
        _window = window;
        ShowTablePageCommand = new RelayCommand(ShowTablePage);
        ShowMetaDataCommand = new RelayCommand(ShowMetaData);
        ShowDbsMetaDataCommand = new RelayCommand(ShowDbsMetaData);
        CreateDatabaseCommand = new RelayCommand(CreateDatabase);
        CreateTableCommand = new RelayCommand(CreateTable);
        DbRedactingCommand = new RelayCommand(RedactDb);
        RefreshCommand = new RelayCommand(Refresh);
        OpenChangingMenuCommand = new RelayCommand(OpenChangingMenu);
    }

    private static IEnumerable<Database> InitDatabases()
    {
        var databasesFolder = new DirectoryInfo(DatabasesFolderPath);
        var databases = new List<Database>();
        foreach (var database in databasesFolder.GetDirectories())
        {
            var db = Database.GetFromDirectoryInfo(database);
            databases.Add(db);
        }

        return databases;
    }
    
    private void ShowTablePage(object? o)
    {
        if (_window.TableChoosingBox.SelectedItem is Table table)
        {
            _window.MainFrame.Content = new TablePage(_selectedDatabase, table);
        }
        else
        {
            MessageBox.Show("Пожалуйста, выберите таблицу.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void ShowMetaData(object? o)
    {
        if (_window.TableChoosingBox.SelectedItem is not null)
        {
            var chosenTable = _window.TableChoosingBox.SelectedItem as Table;
            var metaDataWindow = new TableMetaDataWindow(chosenTable!.Schema);
            metaDataWindow.Show();
        }
        else
        {
            MessageBox.Show("No-таблица = no-метаданные", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private static void ShowDbsMetaData(object? o)
    {
        var metaDataWindow = new DbMetaDataWindow();
        metaDataWindow.Show();
    }

    private void CreateDatabase(object? o)
    {
        var window = new DatabaseCreatingForm(this);
        window.Show();
    }

    private void CreateTable(object? o)
    {
        var window = new TableCreatingForm(Databases);
        window.Show();
    }

    private void RedactDb(object? o)
    {
        if (_window.DatabaseChoosingBox.SelectedItem is not null)
        {
            var window = new DbRedactingWindow(_selectedDatabase);
            window.Show();
        }
        else
        {
            MessageBox.Show("Пожалуйста, выберите Базу данных", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Refresh(object? o)
    {
        _window.DatabaseChoosingBox.ItemsSource = new ObservableCollection<Database>(InitDatabases());
        _window.DatabaseChoosingBox.SelectedItem = null;
        _window.TableChoosingBox.SelectedItem = null;
        _window.MainFrame.Content = null;
    }

    private void OpenChangingMenu(object? o)
    {
        if (_window.TableChoosingBox.SelectedItem is not null)
        {
            var window =
                new TableRedactingWindow((_window.TableChoosingBox.SelectedItem as Table)!,
                    _selectedDatabase);
            window.Show();
        }
        else
        {
            MessageBox.Show("Пожалуйста, выберите таблицу.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
}