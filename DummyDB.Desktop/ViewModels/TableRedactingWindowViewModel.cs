using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using DummyDB.Core.Models;
using DummyDB.Core.Services;
using DummyDB.Desktop.UserControls;
using DummyDB.Desktop.Views;

namespace DummyDB.Desktop.ViewModels;

public class TableRedactingWindowViewModel
{
    private const string DatabasesFolderPath = "../../../../DummyDB.Core/Databases";
    private readonly TableRedactingWindow _window;
    private readonly string _oldTableName;
    private readonly Database _db;
    private readonly Table _table;
    private bool _isCorrect;

    public ObservableCollection<RowViewModel> Rows { get; private set; } = new();
    public RowViewModel? SelectedRow { get; set; } = null!;
    public ICommand AddColumnCommand { get; init; }
    public ICommand AddRowCommand { get; init; }
    public ICommand CheckChangesCommand { get; init; }
    public ICommand SaveAllChangesCommand { get; init; }
    public ICommand DeleteRowCommand { get; init; }
    public ICommand RenameTableCommand { get; init; }

    public TableRedactingWindowViewModel(TableRedactingWindow window, Table table, Database db)
    {
        _window = window;
        _oldTableName = table.Name;
        _db = db;
        _table = table;
        AddColumnCommand = new RelayCommand(AddColumn);
        AddRowCommand = new RelayCommand(AddRow);
        CheckChangesCommand = new RelayCommand(CheckChanges);
        SaveAllChangesCommand = new RelayCommand(SaveAllChanges);
        DeleteRowCommand = new RelayCommand(DeleteRow);
        RenameTableCommand = new RelayCommand(RenameTable);
        InitColumns();
        InitRows();
    }
    
    private void InitColumns()
    {
        _window.TableGrid.Columns.Clear();
        _window.Columns.Children.Clear();
        
        for (var i = 0; i < _table.Schema.Columns.Count; i++)
        {
            DataGridColumn? gridColumn;
            if (_table.Schema.Columns[i].Type.Equals("bool"))
            {
                gridColumn = new DataGridCheckBoxColumn()
                {
                    Header = _table.Schema.Columns[i].Name,
                    Binding = new Binding($"Elements[{i}]")
                };
            }
            else
            {
                gridColumn = new DataGridTextColumn
                {
                    Header = _table.Schema.Columns[i].Name,
                    Binding = new Binding($"Elements[{i}]")
                };
            }

            _window.TableGrid.Columns.Add(gridColumn);
            
            var columnPanel = new ColumnPanel(_table.Schema.Columns[i], _table.Schema, _window, _db.Name, _table);
            _window.Columns.Children.Add(columnPanel);
        }
    }

    private void InitRows()
    {
        if (Rows.Count != 0) return;
        var data = _table.Rows
            .Select(row => new RowViewModel { Elements = row.Elements.Values.ToList() })
            .ToList();
        Rows = new ObservableCollection<RowViewModel>(data);
    }

    private void AddColumn(object? o)
    {
        foreach (var row in Rows)
        {
            row.Elements.Add("");
        }
        var column = new Column { Name = "", Type = "" };
        _table.Schema.Columns.Add(column);
        var columnPanel = new ColumnPanel(column, _table.Schema, _window, _db.Name, _table);
        _window.Columns.Children.Add(columnPanel);
    }

    private void AddRow(object? o)
    {
        var adapter = new RowViewModel(_table.Schema.Columns.Count);
        Rows.Add(adapter);
    }
    
    private void CheckChanges(object? o)
    {
        var strings = new List<string>();
        var columns = _table.Schema.Columns
            .Select(column => column.Name)
            .ToList();
        strings.Add(string.Join(';', columns));
        strings.AddRange(Rows
            .Select(row => row.Elements
                .Select(rowElement => rowElement.ToString())
                .ToList())
            .Select(rowElementsAsStrings => string.Join(';', rowElementsAsStrings)));

        var tempSchema = _table.Schema;
        _isCorrect = JsonValidationService.CheckBySchema(tempSchema, strings.ToArray(), _db.Name) 
                     && FkCheckingService.CheckForForeignKeys(_db, _table, Rows.Select(r => r.Elements)
                         .ToList());
        ShowResponse();
    }

    private void ShowResponse()
    {
        string response;
        if (_isCorrect)
        {
            InitColumns();
            response = "Данные корректны.";
            MessageBox.Show(response, "Success", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
        else
        {
            response = "Данные не корректны.";
            MessageBox.Show(response, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    
    private void RenameTable(object? o)
    {
        if (_window.TableName.Text != "")
        {
            _table.Name = _window.TableName.Text;
        }
        else
        {
            MessageBox.Show("Введите имя таблицы.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private bool CheckIfRowCanBeDeleted(int index)
    {
        var referencedTable = FkCheckingService.CheckIfReferenceOnThisTable(_table, _db);
        if (referencedTable is null)
        {
            return true;
        }

        var column = referencedTable.Schema.Columns.First(column => column.ReferencedTable == _table.Name);
        var pkIndex = _table.Schema.Columns.IndexOf(_table.Schema.Columns.First(c => c.IsPrimary));
        foreach (var row in referencedTable.Rows)
        {
            if (row.Elements[column].Equals(Rows[index - 1].Elements[pkIndex]))
            {
                return false;
            }
        }

        return true;
    }

    private void DeleteRow(object? o)
    {
        if (_window.IndexForDelete.Visibility == Visibility.Collapsed)
        {
            _window.IndexForDelete.Visibility = Visibility.Visible;
        }
        else
        {
            try
            {
                _ = int.TryParse(_window.IndexForDelete.Text, out var index);
                if (!CheckIfRowCanBeDeleted(index))
                {
                    MessageBox.Show("Эту строку нельзя удалить, на неё ссылается другая таблица.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    _window.IndexForDelete.Visibility = Visibility.Collapsed;
                    return;
                }
                Rows.RemoveAt(index - 1);
                _window.IndexForDelete.Visibility = Visibility.Collapsed;
            }
            catch
            {
                MessageBox.Show("Пожалуйста, введите число.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
    }

    private void SaveTable()
    {
        _table.Schema.Name = _table.Name;
        _table.Rows.Clear();
        foreach (var rowAdapter in Rows)
        {
            var row = new Dictionary<Column, object>();
            for (var j = 0; j < _table.Schema.Columns.Count; j++)
            {
                row[_table.Schema.Columns[j]] = rowAdapter.Elements[j];
            }

            _table.Rows.Add(new Row { Elements = row });
        }
    }

    private void SaveAllChanges(object? o)
    {
        if (!_isCorrect)
        {
            MessageBox.Show("Где-то в таблице есть ошибка", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        try
        {
            SaveTable();
            if (_table.Name != _oldTableName)
            {
                var tableFolder = new DirectoryInfo($"{DatabasesFolderPath}/{_db.Name}/{_oldTableName}");
                foreach (var file in tableFolder.GetFiles())
                {
                    file.MoveTo($"{DatabasesFolderPath}/{_db.Name}/{_oldTableName}/{string.Concat(_table.Name, file.Extension)}");
                }
                tableFolder.MoveTo($"{DatabasesFolderPath}/{_db.Name}/{_table.Name}");
            }
            CsvWritingService.WriteInCsv(_table, _db.Name);
            _table.Schema.ToJsonFile(_db.Name);
            _window.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}