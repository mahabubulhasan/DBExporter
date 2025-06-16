using System.Collections.Generic;

namespace DBExporter.Models;

public class CustomerType
{
    public string Value { get; set; }
    public string Text { get; set; }
    public string Parameter { get; set; }

    public static List<CustomerType> GetAll()
    {
        return
        [
            new CustomerType { Value = "01", Text = "Direct", Parameter = "D" },
            new CustomerType { Value = "07", Text = "MEP's Outlet's", Parameter = "M" },
            new CustomerType { Value = "11", Text = "DSD/MEP's Outlet's", Parameter = "B" },
            new CustomerType { Value = "03", Text = "FOBO", Parameter = "F" },
            new CustomerType { Value = "05", Text = "Inter Unit", Parameter = "U" },
            new CustomerType { Value = "06", Text = "Export", Parameter = "E" }
        ];
    }

    public override string ToString()
    {
        return $"{Text} - {Value} - {Parameter}";
    }
}