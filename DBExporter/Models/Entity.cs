namespace DBExporter.Models;

public class Entity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"{Name} - {Code}";
    }
}