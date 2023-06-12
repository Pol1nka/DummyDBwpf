using DummyDB.Core.Models;

namespace DummyDB.Core.Services;

public static class FkCheckingService
{
    public static bool CheckForForeignKeys(Database db, Table table, List<List<object>> rows)
    {
        foreach (var column in table.Schema.Columns)
        {
            if (column.ReferencedTable == "")
            {
                continue;
            }

            var objects = new List<object>();
            foreach (var row in rows)
            {
                objects.Add(row[table.Schema.Columns.IndexOf(column)]);
            }

            var referencedTable = db.Tables.First(t => t.Name == column.ReferencedTable);
                
            foreach (var element in objects)
            {
                var flag = CheckForeignKey(element.ToString()!, referencedTable);
                if (!flag)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private static bool CheckForeignKey(string value, Table table)
    {
        var column = table.Schema.Columns.First(c => c.IsPrimary);
        var result = false;
        foreach (var row in table.Rows)
        {
            if (row.Elements[column].ToString() != value)
            {
                result = false;
                continue;
            }

            result = true;
            break;
        }

        return result;
    }

    public static Table? CheckIfReferenceOnThisTable(Table table, Database db)
    {
        foreach (var dbTable in db.Tables)
        {
            foreach (var column in dbTable.Schema.Columns)
            {
                if (column.ReferencedTable == table.Name)
                {
                    return dbTable;
                }
            }
        }

        return null;
    }
}