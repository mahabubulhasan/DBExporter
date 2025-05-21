using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace DBExporter.Services;

public class ExportService
{
    public static async Task ExportToCsv(DataTable reportData)
    {
        try
        {
            var csv = new StringBuilder();

            // Add header row
            var headerLine = string.Join(",", reportData.Columns.Cast<DataColumn>().Select(column => QuoteCsvField(column.ColumnName)));
            csv.AppendLine(headerLine);

            // Add data rows
            foreach (DataRow row in reportData.Rows)
            {
                var fields = row.ItemArray.Select(field => QuoteCsvField(field?.ToString() ?? string.Empty));
                var line = string.Join(",", fields);
                csv.AppendLine(line);
            }

            // Get the current date/time for the filename
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string defaultFilename = $"Report_{timestamp}.csv";

            // Get the top-level window
            var topLevel = TopLevel.GetTopLevel(App.MainWindow);

            if (topLevel != null)
            {
                // Set up the file save options
                var fileOptions = new FilePickerSaveOptions
                {
                    Title = "Save Report",
                    SuggestedFileName = defaultFilename,
                    FileTypeChoices = new[]
                    {
                        new FilePickerFileType("CSV files")
                        {
                            Patterns = new[] { "*.csv" },
                            MimeTypes = new[] { "text/csv" }
                        },
                        new FilePickerFileType("All files")
                        {
                            Patterns = new[] { "*.*" }
                        }
                    }
                };

                // Show the file picker
                var file = await topLevel.StorageProvider.SaveFilePickerAsync(fileOptions);

                if (file != null)
                {
                    // Write the CSV content to the selected file
                    await using var stream = await file.OpenWriteAsync();
                    using var writer = new StreamWriter(stream);
                    await writer.WriteAsync(csv.ToString());
                }
            }
        }
        catch (Exception ex)
        {
            // Handle exception
            Console.WriteLine($"Error exporting CSV: {ex.Message}");
        }
    }

    // Helper method for CSV formatting
    private static string QuoteCsvField(string field)
    {
        // If the field contains a comma, newline, or quote, wrap it in quotes and escape any quotes
        if (field.Contains(",") || field.Contains("\"") || field.Contains("\n"))
        {
            return $"\"{field.Replace("\"", "\"\"")}\"";
        }
        return field;
    }
}