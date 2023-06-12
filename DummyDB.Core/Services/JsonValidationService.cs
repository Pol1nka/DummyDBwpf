using DummyDB.Core.Models;

namespace DummyDB.Core.Services;

public static class JsonValidationService
{
    public static bool CheckBySchema(Schema schema, string[] data, string dbName)
    {
        var areColumnsNamesOk = CheckColumnsNames(schema, data[0].Split(';'));
        for (var i = 1; i < data.Length; i++)
        {
            var flag = CheckColumnsTypes(schema, data[i].Split(';'));
            if (!flag)
            {
                return false;
            }
        }

        return areColumnsNamesOk && CheckPrimaryKeys(schema, data);
    }

    private static bool CheckColumnsNames(Schema schema, string[] names)
    {
        if (names.Where((t, i) => t != schema.Columns[i].Name).Any())
        {
            throw new FormatException("Названия столбцов в таблице не совпадают со схемой.");
        }

        return true;
    }

    private static bool CheckColumnsTypes(Schema schema, string[] row)
    {
        var result = false;
        for (var i = 0; i < row.Length; i++)
        {
            if (row[i] == "" && schema.Columns[i].Type == "float")
            {
                result = true;
                continue;
            }
            
            result = schema.Columns[i].Type switch
            {
                "int" => int.TryParse(row[i], out _),
                "bool" => bool.TryParse(row[i], out _),
                "float" => float.TryParse(row[i], out _),
                "dateTime" => DateTime.TryParse(row[i], out _),
                "string" => true,
                _ => throw new ArgumentException($"В схеме неверно указан тип данных столбца под номером {i + 1}.")
            };
        }

        return result;
    }

    private static bool CheckPrimaryKeys(Schema schema, string[] data)
    {
        for (var i = 0; i < schema.Columns.Count; i++)
        {
            if (!schema.Columns[i].IsPrimary)
            {
                continue;
            }

            var flag = CompareKeys(i, data);
            if (!flag)
            {
                return false;
            }
        }

        return true;
    }

    private static bool CompareKeys(int index, string[] data)
    {
        var notDistinct = data.Select(row => row.Split(';')[index]).ToList();
        var distinct = notDistinct.Distinct().ToList();
        return notDistinct.Count == distinct.Count 
               && notDistinct
                   .Select((t, i) => Equals(t, distinct[i]))
                   .All(flag => flag);
    }
}