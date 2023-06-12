namespace DummyDB.Core.Models;

public class Table
{
    public string Name { get; set; }
    public List<Row> Rows { get; }
    public Schema Schema { get; }
    public List<string> ReferencedTables { get; }

    public Table(string name, Schema schema)
    {
        Name = name;
        Rows = new List<Row>();
        Schema = schema;
        ReferencedTables = InitReferences();
    }

    private List<string> InitReferences()
    {
        var result = new List<string>();
        foreach (var column in Schema.Columns)
        {
            if (column.ReferencedTable != "")
            {
                result.Add(column.ReferencedTable);
            }
        }

        return result;
    }
    
    public override string ToString()
    {
        return Name;
    }
}