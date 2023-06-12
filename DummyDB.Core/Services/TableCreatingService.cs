using DummyDB.Core.Models;

namespace DummyDB.Core.Services;

public static class TableCreatingService
{
    public static Table CreateTable(string tableName, Schema schema, string[] data)
    {
        var result = new Table(tableName, schema);
        foreach (var str in data)
        {
            var row = new Row();
            var rowElements = str.Split(';');
            for (var j = 0; j < rowElements.Length; j++)
            {
                row.Elements.Add(schema.Columns[j], GetData(rowElements[j], schema.Columns[j]));
            }
            result.Rows.Add(row);
        }

        return result;
    }

    private static object GetData(string value, Column column)
    {
        if (column.Type == "dateTime" && value == "")
        {
            return "";
        }

        if (column.Type == "float" && value == "")
        {
            return "";
        }

        return column.Type switch
        {
            "int" => int.Parse(value),
            "float" => float.Parse(value),
            "bool" => bool.Parse(value),
            "dateTime" => DateTime.Parse(value),
            _ => value
        };
    }
}