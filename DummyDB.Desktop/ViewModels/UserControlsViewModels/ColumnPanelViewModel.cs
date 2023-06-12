using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using DummyDB.Core.Models;
using DummyDB.Desktop.UserControls;
using DummyDB.Desktop.Views;

namespace DummyDB.Desktop.ViewModels.UserControlsViewModels;

public class ColumnPanelViewModel : INotifyPropertyChanged
{
    private readonly Column _column;
    private readonly Schema _schema;
    private readonly TableRedactingWindow _window;
    private readonly ColumnPanel _panel;
    private readonly Table _table;
    private bool _isChecked;
    private string _selectedTable = null!;

    public bool IsChecked
    {
        get => _isChecked;
        set
        {
            if (value)
            {
                _panel.TablesInDb.Visibility = Visibility.Visible;
            }
            else
            {
                SelectedTable = null!;
                _panel.TablesInDb.Visibility = Visibility.Collapsed;
            }

            _isChecked = value;
            if (PropertyChanged is not null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsChecked)));
            }
        }
    }

    public string SelectedTable
    {
        get => _selectedTable;
        set
        {
            _selectedTable = value;
            if (PropertyChanged is not null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedTable)));
            }
        }
    }

    public string[] TablesInCurrentDb { get; set; }
    public string? ColumnName { get; set; }
    public ObservableCollection<string> Types { get; } = new() { "int", "float", "string", "dateTime", "bool" };
    public string? SelectedType { get; set; }
    public bool IsPrimary { get; set; }
    public ICommand DeleteCommand { get; set; }
    public ICommand SaveCommand { get; set; }
    public ColumnPanelViewModel(Column column, Schema schema, TableRedactingWindow window, ColumnPanel panel, 
        string dbName, Table table)
    {
        _column = column;
        _schema = schema;
        _window = window;
        _panel = panel;
        _table = table;
        TablesInCurrentDb = InitTablesInDb(dbName).ToArray();
        ColumnName = column.Name;
        SelectedType = column.Type;
        IsPrimary = column.IsPrimary;
        DeleteCommand = new RelayCommand(Delete);
        SaveCommand = new RelayCommand(Save);
        CheckIfFk();
    }

    private void Delete(object? o)
    {
        var index = _schema.Columns.IndexOf(_column);
        
        _window.Columns.Children.Remove(_panel);
        _schema.Columns.RemoveAt(index);
        _window.TableGrid.Columns.RemoveAt(index);
        
        var viewModel = _window.DataContext as TableRedactingWindowViewModel;
        foreach (var row in viewModel!.Rows)
        {
            row.Elements.RemoveAt(index);
        }
    }

    private void Save(object? o)
    {
        var index = _schema.Columns.IndexOf(_column);
        var tempType = _schema.Columns[index].Type;
        if (_window.TableGrid.Columns.Count < index + 1)
        {
            _schema.Columns[index].Name = _panel.ColumnName.Text;
            _schema.Columns[index].Type = (_panel.ColumnType.SelectedItem as string)!;
            _schema.Columns[index].IsPrimary = _panel.ColumnIsPrimary.IsChecked ?? false;
            _schema.Columns[index].ReferencedTable = _selectedTable;
            if (!_table.ReferencedTables.Contains(_selectedTable))
            {
                _table.ReferencedTables.Add(_selectedTable);
            }
            var viewModel = _window.DataContext as TableRedactingWindowViewModel;
            foreach (var row in viewModel!.Rows)
            {
                row.Elements.Add("");
            }
            AddColumnInDataGrid(index);
        }
        else
        {
            _window.TableGrid.Columns.RemoveAt(index);
            AddColumnInDataGrid(index);
        }

        if (tempType != SelectedType)
        {
            ClearColumnInDataGrid(index);
        }
    }

    private void AddColumnInDataGrid(int index)
    {
        DataGridColumn dataGridColumn;
        if (_column.Type.Equals("bool"))
        {
            dataGridColumn = new DataGridCheckBoxColumn
            {
                Binding = new Binding($"Elements[{index}]")
            };
        }
        else
        {
            dataGridColumn = new DataGridTextColumn
            {
                Binding = new Binding($"Elements[{index}]")
            };
        }

        dataGridColumn.Header = _panel.ColumnName.Text;
        _window.TableGrid.Columns.Insert(index, dataGridColumn);
    }

    private void ClearColumnInDataGrid(int index)
    {
        var viewModel = _window.DataContext as TableRedactingWindowViewModel;
        foreach (var row in viewModel!.Rows)
        {
            row.Elements.RemoveAt(index);
        }
    }

    private List<string> InitTablesInDb(string dbName)
    {
        var folder = new DirectoryInfo($"../../../../DummyDB.Core/Databases/{dbName}");
        var tables = folder
            .GetDirectories()
            .Select(d => d.Name)
            .Where(name => name != _table.Name).ToList();
        tables.Remove(_table.Name);
        return tables;
    }

    private void CheckIfFk()
    {
        if (_column.ReferencedTable != "")
        {
            IsChecked = true;
            SelectedTable = _column.ReferencedTable;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}