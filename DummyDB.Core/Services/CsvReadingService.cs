using DummyDB.Core.Models;

namespace DummyDB.Core.Services;

public static class CsvReadingService
{
    public static string[] ReadFromCsv(string dataFilePath, Schema schema, string dbName)
    {
        var dataFromCsv = File.ReadAllLines(dataFilePath);
        return JsonValidationService.CheckBySchema(schema, dataFromCsv, dbName) ? dataFromCsv[1..] : throw new FormatException("Невозможно считать данные из файла, так как они не соответствуют схеме таблицы.");
    }
}