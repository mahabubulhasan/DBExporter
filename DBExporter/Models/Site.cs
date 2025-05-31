namespace DBExporter.Models;

public class Site
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;

    public override string ToString() => $"{Name} - {Code}";
}