using System.Text;
using DummyDB.Core.Models;

namespace DummyDB.Core.Services;

public static class CsvWritingService
{
    private const string DatabasesFolderPath = "../../../../DummyDb.Core/Databases/";
    
    public static void CreateCsv(Table table, string dbName)
    {
        var stream = File.Create($"{DatabasesFolderPath}/{dbName}/{table.Name}/{table.Name}.csv");
        stream.Close();
        WriteInCsv(table, dbName);
    }

    public static void WriteInCsv(Table table, string dbName)
    {
        var sb = new StringBuilder();
        var columnsNames = table.Schema.Columns.Select(column => column.Name!).ToList();
        sb.AppendJoin(';', columnsNames);
        sb.AppendLine();
        for (var i = 0; i < table.Rows.Count; i++)
        {
            var rowElements = table.Rows[i].Elements.Select(pair => pair.Value);
            sb.AppendJoin(';', rowElements);
            sb.AppendLine();
        }

        try
        {
            File.WriteAllText($"{DatabasesFolderPath}/{dbName}/{table.Name}/{table.Name}.csv", sb.ToString());
        }
        catch
        {
            //Ignore
        }
    }
}