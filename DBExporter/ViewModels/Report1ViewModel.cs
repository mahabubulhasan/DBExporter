﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DBExporter.Models;
using DBExporter.Services;

namespace DBExporter.ViewModels;

public enum ReportType
{
    SalesOrgStructure,
    SalesOrgWithIBPLBilledCustomer,
    SalesOrgWithMEPCustomersBilled
}

public partial class Report1ViewModel : ObservableObject
{
    private readonly IDatabaseService _databaseService;

    [ObservableProperty]
    private List<SalesUser> _salesUsers = new();

    [ObservableProperty]
    private SalesUser? _selectedSalesUser;

    [ObservableProperty]
    private ReportType _selectedReportType = ReportType.SalesOrgStructure;

    [ObservableProperty]
    private DataTable _reportData = new();

    [ObservableProperty]
    private ObservableCollection<SalesReportItem> _reportItems = new();

    [ObservableProperty]
    private bool _isLoading;

    public Report1ViewModel(IDatabaseService databaseService)
    {
        _databaseService = databaseService;

        Task.Run(LoadSalesUsers);
    }

    [RelayCommand]
    private async Task Refresh()
    {
        IsLoading = false;
        if (SelectedSalesUser == null)
            return;

        string reportFlag = "S"; // Default for SalesOrgStructure

        switch (SelectedReportType)
        {
            case ReportType.SalesOrgWithIBPLBilledCustomer:
                reportFlag = "C";
                break;
            case ReportType.SalesOrgWithMEPCustomersBilled:
                reportFlag = "M";
                break;
        }

        var config = await DatabaseConfig.GetInstanceAsync();
        if (config == null)
            return;

        IsLoading = true;
        
        string query = $"EXEC {config.Report1Sp} '{SelectedSalesUser.Code}','{reportFlag}'";
        var data = await _databaseService.ExecuteQueryAsync(query);

        ReportData = data;
        var items = new ObservableCollection<SalesReportItem>();
        foreach (DataRow row in data.Rows)
        {
            items.Add(new()
            {
                ASM_ID = row["ASM ID"]?.ToString() ?? "",
                ASM_Name = row["ASM Name"]?.ToString() ?? "",
                ASM_Territory = row["ASM Territory"]?.ToString() ?? "",
                ASM_Mobile_No = row["ASM Mobile No"]?.ToString() ?? "",
                ASM_KO_ID = row["ASM KO ID"]?.ToString() ?? "",
                SE_ID = row["SE ID"]?.ToString() ?? "",
                SE_Name = row["SE Name"]?.ToString() ?? "",
                SE_Territory = row["SE Territory"]?.ToString() ?? "",
                SE_Mobile_No = row["SE Mobile No"]?.ToString() ?? "",
                SE_KO_ID = row["SE KO ID"]?.ToString() ?? ""
            });
        }

        ReportItems = items;
        IsLoading = false;
    }

    private async Task LoadSalesUsers()
    {
        try
        {
            var data = await _databaseService.ExecuteQueryAsync("select distinct SM_ID, SM_Name from BDC1_Masters.dbo.SM_Master where  Active_Flag = 'Y' and SM_ID not like '%ZZ%' order by SM_Name");
            var users = new List<SalesUser>();

            foreach (DataRow row in data.Rows)
            {
                users.Add(new SalesUser
                {
                    Name = row["SM_Name"].ToString() ?? "",
                    Code = row["SM_ID"].ToString() ?? ""
                });
            }

            SalesUsers = users;
            SelectedSalesUser = users.FirstOrDefault();
        }
        catch (Exception)
        {
            SalesUsers = new List<SalesUser>
            {
                new SalesUser { Name = "Tapash Kumar", Code = "88000013" },
                new SalesUser { Name = "Another User", Code = "88000014" }
            };

            SelectedSalesUser = SalesUsers[0];
        }
    }

    [RelayCommand]
    private async Task Export()
    {
        if (ReportData.Rows.Count == 0)
            return;

        await ExportService.ExportToCsv(ReportData);
    }
}