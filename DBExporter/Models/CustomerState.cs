using System.Collections.Generic;

namespace DBExporter.Models;

public class CustomerState
{
    public string Name { get; set; }
    public string Code { get; set; }

    public static List<CustomerState> GetAll()
    {
        return
        [
            new CustomerState { Name = "All", Code = "" },
            new CustomerState { Name = "Only Active Customers", Code = "Y" },
            new CustomerState { Name = "Only Inactive Customers", Code = "N" },
        ];
    }

    public override string ToString()
    {
        return $"{Name} - {Code}";
    }
}