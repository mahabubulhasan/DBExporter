using Avalonia;
using DBExporter.Models;
using DBExporter.ViewModels;
using DBExporter.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace DBExporter;

sealed class Program
{
    public static IServiceProvider? ServiceProvider { get; private set; }
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        // Setup dependency injection
        var host = CreateHostBuilder().Build();
        ServiceProvider = host.Services;

        BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();

    private static IHostBuilder CreateHostBuilder() =>
        Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                // Register ViewModels
                services.AddTransient<MainWindowViewModel>();
                services.AddTransient<Report1ViewModel>();

                // Register Views
                services.AddTransient<MainWindow>();
                services.AddTransient<Report1>();
                services.AddTransient<Report2>();
                services.AddTransient<Report3>();
                services.AddTransient<Report4>();
                services.AddTransient<Settings>();
                services.AddTransient<Report1ViewModel>();
                services.AddTransient<Report2ViewModel>();
                services.AddTransient<Report3ViewModel>();
                services.AddTransient<Report4ViewModel>();
                services.AddTransient<SettingsViewModel>();

                // In Program.cs, add to ConfigureServices:
                services.AddSingleton<IDatabaseService, DatabaseService>();
            });
}