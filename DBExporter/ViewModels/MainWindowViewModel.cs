using System;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DBExporter.Models;
using DBExporter.Views;
using Microsoft.Extensions.DependencyInjection;

namespace DBExporter.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public string Greeting { get; } = "Welcome to Avalonia!";
    private readonly IDatabaseService _databaseService;

    [ObservableProperty]
    private object? _currentView;

    [ObservableProperty]
    private bool _isInitialized;

    public MainWindowViewModel(IDatabaseService databaseService)
    {
        _databaseService = databaseService;

        // Set initial view
        CurrentView = Program.ServiceProvider!.GetRequiredService<Report1>();

        Task.Run(InitializeAsync);
    }

    private async Task InitializeAsync()
    {
        try
        {
            var connected = await _databaseService.TestConnectionAsync();

            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                IsInitialized = true;

                if (connected)
                {
                    CurrentView = Program.ServiceProvider!.GetRequiredService<Report1>();
                }
                else
                {
                    CurrentView = Program.ServiceProvider!.GetRequiredService<Settings>();
                }
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Initialization error: {ex.Message}");

            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                IsInitialized = true;
                CurrentView = Program.ServiceProvider!.GetRequiredService<Settings>();
            });
        }
    }

    [RelayCommand]
    private void Settings()
    {
        Dispatcher.UIThread.Post(() =>
        {
            CurrentView = Program.ServiceProvider!.GetRequiredService<Settings>();
        });
    }

    [RelayCommand]
    private void Exit()
    {
        // Close the application
        Environment.Exit(0);
    }

    [RelayCommand]
    private void Report1()
    {
        Dispatcher.UIThread.Post(() =>
        {
            CurrentView = Program.ServiceProvider!.GetRequiredService<Report1>();
        });
    }

    [RelayCommand]
    private void Report2()
    {
        Dispatcher.UIThread.Post(() =>
        {
            CurrentView = Program.ServiceProvider!.GetRequiredService<Report2>();
        });
    }

    [RelayCommand]
    private void Report3()
    {
        Dispatcher.UIThread.Post(() =>
        {
            CurrentView = Program.ServiceProvider!.GetRequiredService<Report3>();
        });
    }
    
    [RelayCommand]
    private void Report4()
    {
        Dispatcher.UIThread.Post(() =>
        {
            CurrentView = Program.ServiceProvider!.GetRequiredService<Report4>();
        });
    }

    [RelayCommand]
    private void Report5()
    {
        Dispatcher.UIThread.Post(() =>
        {
            CurrentView = Program.ServiceProvider!.GetRequiredService<Report5>();
        });
    }
}