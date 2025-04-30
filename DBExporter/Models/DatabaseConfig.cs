using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace DBExporter.Models
{
    public class DatabaseConfig
    {
        public string Host { get; set; } = string.Empty;
        public string Port { get; set; } = "1433";
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Report1Sp { get; set; } = string.Empty;

        // Cache the connection string to avoid rebuilding it frequently
        private string? _connectionString;

        // Lazily build the connection string when needed
        public string ConnectionString
        {
            get
            {
                if (_connectionString == null)
                {
                    _connectionString = BuildConnectionString();
                }
                return _connectionString;
            }
        }

        // Create the path once instead of for each operation
        private static readonly Lazy<string> ConfigFilePathLazy = new Lazy<string>(() =>
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "DBExporter",
                "config.json"));

        private static string ConfigFilePath => ConfigFilePathLazy.Value;

        // Store a singleton instance for efficient access
        private static DatabaseConfig? _instance;

        // Thread-safe singleton accessor
        public static async Task<DatabaseConfig> GetInstanceAsync()
        {
            if (_instance == null)
            {
                _instance = await LoadAsync();
            }
            return _instance;
        }

        public static async Task<DatabaseConfig> LoadAsync()
        {
            try
            {
                var directory = Path.GetDirectoryName(ConfigFilePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                if (!File.Exists(ConfigFilePath))
                {
                    return new DatabaseConfig();
                }

                var json = await File.ReadAllTextAsync(ConfigFilePath);
                var config = JsonSerializer.Deserialize<DatabaseConfig>(json);
                return config ?? new DatabaseConfig();
            }
            catch (Exception ex)
            {
                // Log the exception in production code
                Console.WriteLine($"Error loading configuration: {ex.Message}");
                return new DatabaseConfig();
            }
        }

        public async Task SaveAsync()
        {
            try
            {
                var directory = Path.GetDirectoryName(ConfigFilePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(this, options);
                await File.WriteAllTextAsync(ConfigFilePath, json);

                // Update the singleton instance after save
                _instance = this;
            }
            catch (Exception ex)
            {
                // Log the exception in production code
                Console.WriteLine($"Error saving configuration: {ex.Message}");
            }
        }

        private string BuildConnectionString()
        {
            return $"Server={Host},{Port};Database=master;User Id={Username};Password={Password};TrustServerCertificate=True;";
        }
    }
}