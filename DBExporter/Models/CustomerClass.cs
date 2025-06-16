﻿namespace DBExporter.Models;

public class CustomerClass
{
    public string Name { get; set; }
    public string Code { get; set; }

    public override string ToString()
    {
        return $"{Name} - {Code}";
    }
}