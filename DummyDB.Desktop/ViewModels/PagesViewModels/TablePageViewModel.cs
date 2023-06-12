using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using DummyDB.Core.Models;
using DummyDB.Desktop.Views.Pages;

namespace DummyDB.Desktop.ViewModels.PagesViewModels;

public class TablePageViewModel
{
    private readonly TablePage _page;
    private readonly Table _table;
    private readonly Database _db;

    public TablePageViewModel(TablePage page, Table table, Database db)
    {
        _page = page;
        _table = table;
        _db = db;
        _page.Table.MouseDoubleClick += UploadColumn;
        InitRows();
        InitColumns();
    }
    
    private void InitColumns()
    {
        for (var i = 0; i < _table.Schema.Columns.Count; i++)
        {
            DataGridColumn gridColumn;
            var binding = new Binding($"Elements[{i}]");
            if (_table.Schema.Columns[i].Type.Equals("bool"))
            {
                gridColumn = new DataGridCheckBoxColumn
                {
                    Header = _table.Schema.Columns[i].Name,
                    Binding = binding
                };
            }
            else if (_table.Schema.Columns[i].ReferencedTable != "")
            {
                gridColumn = new DataGridTextColumn
                {
                    Header = _table.Schema.Columns[i].Name,
                    Binding = binding,
                    CellStyle = new Style
                    {
                        TargetType = typeof(DataGridCell),
                        Setters =
                        {
                            new Setter(FrameworkElement.ToolTipProperty, null)
                        }
                    }
                };
            }
            else
            {
                gridColumn = new DataGridTextColumn
                {
                    Header = _table.Schema.Columns[i].Name,
                    Binding = binding
                };
            }

            gridColumn.HeaderStyle = new Style
            {
                TargetType = typeof(DataGridColumnHeader),
                Setters =
                {
                    new Setter(FrameworkElement.ToolTipProperty,
                        _table.Schema.Columns[i].ReferencedTable)
                }
            };
            _page.Table.Columns.Add(gridColumn);
        }
    }

    private void InitRows()
    {
        var data = new ObservableCollection<RowViewModel>();
        foreach (var row in _table.Rows)
        {
            data.Add(new RowViewModel { Elements = row.Elements.Values.ToList() });
        }

        _page.Table.ItemsSource = data;
    }

    private string GetReference(object value, string referencedTableName)
    {
        var referencedTable = _db.Tables.First(table => table.Name.Equals(referencedTableName));
        var column = referencedTable.Schema.Columns.First(c => c.IsPrimary);
        var index = 0;
        foreach (var row in referencedTable.Rows)
        {
            if (row.Elements[column].Equals(value))
            {
                index = referencedTable.Rows.IndexOf(row);
                break;
            }
        }

        var strings = new List<string>();
        foreach (var element in referencedTable.Rows[index].Elements.Values)
        {
            strings.Add(element.ToString()!);
        }
        
        return string.Join(' ', strings);
    }

    private void UploadColumn(object sender, MouseButtonEventArgs e)
    {
        var index = _page.Table.Columns.IndexOf(_page.Table.CurrentColumn);
        var rowViewModel = _page.Table.SelectedCells[0].Item! as RowViewModel;
        var value = rowViewModel!.Elements[index];
        var referencedTable = _table.Schema.Columns[index].ReferencedTable;
        var binding = new Binding($"Elements[{index}]");
        var gridColumn = new DataGridTextColumn
        {
            Header = _table.Schema.Columns[index].Name,
            Binding = binding,
            CellStyle = new Style
            {
                TargetType = typeof(DataGridCell),
                Setters =
                {
                    new Setter(FrameworkElement.ToolTipProperty, GetReference(value, referencedTable))
                }
            },
            HeaderStyle = new Style
            {
                TargetType = typeof(DataGridColumnHeader),
                Setters =
                {
                    new Setter(FrameworkElement.ToolTipProperty,
                        _table.Schema.Columns[index].ReferencedTable)
                }
            }
        };
        _page.Table.Columns[index] = gridColumn;
    }
}