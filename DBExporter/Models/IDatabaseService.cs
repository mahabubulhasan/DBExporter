using System;
using System.Data;
using System.Threading.Tasks;

namespace DBExporter.Models;

public interface IDatabaseService
{
    Task<string> GetConnectionStringAsync();
    Task<bool> TestConnectionAsync();
    Task<DataTable> ExecuteQueryAsync(string query);
    Task<int> ExecuteNonQueryAsync(string query);
    Task<string[]> GetDatabasesAsync();
}
