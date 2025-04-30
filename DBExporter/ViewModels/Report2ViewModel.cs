using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DBExporter.Models;

namespace DBExporter.ViewModels;

public partial class Report2ViewModel: ObservableObject
{
    [ObservableProperty]
    private List<Entity> _entities = new();

    [ObservableProperty]
    private Entity _selectedEntity;

    [ObservableProperty]
    private List<Distributor> _distributors = new();

    [ObservableProperty]
    private Distributor _selectedDistributor;

    [ObservableProperty]
    private DateTime? _dateFrom;

    public string FormattedDateFrom => DateFrom.HasValue ? DateFrom.Value.ToString("MM/dd/yyyy") : string.Empty;

    public Report2ViewModel()
    {
        Entities = new List<Entity>
        {
            new Entity { Name = "Entity1", Code = "1" },
            new Entity { Name = "Entity2", Code = "2" },
            new Entity { Name = "Entity3", Code = "3" }
        };

        SelectedEntity = Entities[0];

        Distributors = new List<Distributor>
        {
            new Distributor { Name = "Distributor1", Code = "1" },
            new Distributor { Name = "Distributor2", Code = "2" },
            new Distributor { Name = "Distributor3", Code = "3" }
        };

        SelectedDistributor = Distributors[0];
    }

    partial void OnDateFromChanged(DateTime? value)
    {
        OnPropertyChanged(nameof(FormattedDateFrom));
    }

    [RelayCommand]
    private void Load()
    {
        Console.WriteLine(SelectedEntity);
        Console.WriteLine(SelectedDistributor);
        Console.WriteLine(FormattedDateFrom);
    }
}