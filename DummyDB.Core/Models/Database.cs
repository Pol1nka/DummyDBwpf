using DummyDB.Core.Services;

namespace DummyDB.Core.Models;

public class Database
{
    public string Name { get; set; }
    public List<Table> Tables { get; }

    private Database(string name)
    {
        Name = name;
        Tables = new List<Table>();
    }

    public override string ToString()
    {
        return Name;
    }

    public static Database GetFromDirectoryInfo(DirectoryInfo info)
    {
        var name = info.Name;
        var result = new Database(name);
        foreach (var directory in info.GetDirectories())
        {
            var schema = Schema.GetFromJsonFile($"../../../../DummyDB.Core/Databases/{name}/{directory.Name}/{directory.Name}.json");
            var dataFilePath = $"../../../../DummyDB.Core/Databases/{name}/{directory.Name}/{directory.Name}.csv";
            var data = CsvReadingService.ReadFromCsv(dataFilePath, schema!, name);
            var table = TableCreatingService.CreateTable(directory.Name, schema!, data);
            result.Tables.Add(table);
        }

        return result;
    }
}