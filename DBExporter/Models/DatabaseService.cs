using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace DBExporter.Models;

public class DatabaseService : IDatabaseService
{
    // private string _connectionString = "Server=118.179.215.5\\MSSQLSERVER,1433;Database=master;User Id=dev;Password=J@ntrik007#&;Connection Timeout=30;TrustServerCertificate=True;";

    public async Task<string> GetConnectionStringAsync()
    {
        var config = await DatabaseConfig.GetInstanceAsync();
        return config.ConnectionString;
    }

    public async Task<bool> TestConnectionAsync()
    {
        var connectionString = await GetConnectionStringAsync();
        try
        {
            using (var connection = new SqlConnection(connectionString))
            {
                // connection.Open();
                await connection.OpenAsync();
                return true;
            }
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<DataTable> ExecuteQueryAsync(string query)
    {
        var dataTable = new DataTable();
        string connectionString = await GetConnectionStringAsync();
        try
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(query, connection))
                {
                    using (var adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }
        }
        catch (Exception)
        {
            // Consider logging the exception
            throw;
        }

        return dataTable;
    }

    public async Task<int> ExecuteNonQueryAsync(string query)
    {
        string connectionString = await GetConnectionStringAsync();
        try
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(query, connection))
                {
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }
        catch (Exception)
        {
            // Consider logging the exception
            throw;
        }
    }

    public async Task<string[]> GetDatabasesAsync()
    {
        var dataTable = await ExecuteQueryAsync("SELECT name FROM sys.databases WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb') ORDER BY name");
        string[] databases = new string[dataTable.Rows.Count];

        for (int i = 0; i < dataTable.Rows.Count; i++)
        {
            databases[i] = dataTable.Rows[i]["name"].ToString() ?? string.Empty;
        }

        return databases;
    }
}
