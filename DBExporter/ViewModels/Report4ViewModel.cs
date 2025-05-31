using System;
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

partial class Report4ViewModel: ObservableObject
{
    private readonly IDatabaseService _databaseService;

    [ObservableProperty]
    private List<Site> _sites = new();

    [ObservableProperty]
    private Site? _selectedSite;

    [ObservableProperty]
    private List<Distributor> _distributors = new();

    [ObservableProperty]
    private Distributor? _selectedDistributor;

    [ObservableProperty]
    private string _fromDate = string.Empty;
    
    public string FormattedFromDate => string.IsNullOrEmpty(FromDate) ? "From Date" : FromDate;
    public string FormattedToDate => string.IsNullOrEmpty(ToDate) ? "To Date" : ToDate;

    [ObservableProperty]
    private string _toDate = string.Empty;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private DataTable _reportData = new();

    [ObservableProperty]
    private ObservableCollection<SalesDataReport> _reportItems = new();

    public Report4ViewModel(IDatabaseService databaseService)
    {
        _databaseService = databaseService;

        Task.Run(LoadSites);
        Task.Run(LoadDistributors);
    }

    private async Task LoadDistributors()
    {
        string condition = string.Empty;
        
        if(SelectedSite?.Code is not null)
        {
            condition = $"AND GCC.site_id = '{SelectedSite.Code}'";
        }
        const string sql = """
               SELECT DISTINCT IAS.customer_id Distributor_id,
                               IAS.dist_gcc_id,
                               IAS.distributor_name + ' (' + IAS.dist_gcc_id
                               + ')' + ' - ' + CASE WHEN GCC.customer_class_id='4' THEN 'DIST'
                               WHEN
                               GCC.customer_class_id='23' THEN 'IWHOL' WHEN
                               GCC.customer_class_id='26' THEN
                               'AMC' END       AS Distributor_Name
               FROM   bdc1_masters.dbo.idas_activation_setup IAS (nolock)
                      INNER JOIN bdc1_masters.dbo.gcc_master GCC (nolock)
                              ON IAS.dist_gcc_id = GCC.gcc_id
                      INNER JOIN bdc1_masters.dbo.gcc_master_sales_area GCSA (nolock)
                              ON GCC.gcc_id = GCSA.gcc_id
                      INNER JOIN bdc1_masters.dbo.entity_master EM (nolock)
                              ON GCC.entity_id = EM.entity_id
                      INNER JOIN bdc1_masters.dbo.site_master S (nolock)
                              ON EM.entity_id = S.entity_id
                                 AND S.active = 'Y'
                                 AND S.site_id IN ( 'B051', 'B053', 'B055', 'B054',
                                                    'B052', 'B050' )
                                 AND EM.entity_id IN ( '1364BDB053', '1364BDB001', '1364BDB055'
                                                       ,
                                                       '1364BDB054',
                                                       '1364BDB052', '1364BDB051', '1364BDB050'
                                                     )
               WHERE  dist_gcc_id_active = 'Y'
                      AND GCC.active = 'Y'
                      {condition}
               ORDER  BY distributor_name
               """;

        try
        {
            var data = await _databaseService.ExecuteQueryAsync(sql);
            var distributors = new List<Distributor>
            {
                new() { Name = "All", Code = "Al" }
            };

            foreach (DataRow row in data.Rows)
            {
                distributors.Add(new Distributor
                {
                    Name = row["Distributor_Name"]?.ToString() ?? "",
                    Code = row["Dist_GCC_ID"]?.ToString() ?? ""
                });
            }

            Distributors = distributors;
            SelectedDistributor = distributors.FirstOrDefault();
        }
        catch (Exception)
        {
            Distributors =
            [
                new Distributor { Name = "Distributor1", Code = "1" },
                new Distributor { Name = "Distributor2", Code = "2" },
                new Distributor { Name = "Distributor3", Code = "3" }
            ];
            SelectedDistributor = Distributors[0];
        }
    }

    private async Task LoadSites()
    {
        const string sql = """
               SELECT DISTINCT SM.site_id,
                               SM.site_name + ' (' + SM.site_id + ')' AS SITE_NAME
               FROM   bdc1_masters.dbo.idas_activation_setup IAS (nolock)
                      INNER JOIN bdc1_masters.dbo.gcc_master GCC (nolock)
                              ON IAS.dist_gcc_id = GCC.gcc_id
                      INNER JOIN bdc1_masters.dbo.gcc_master_sales_area GCSA (nolock)
                              ON GCC.gcc_id = GCSA.gcc_id
                      INNER JOIN bdc1_masters.dbo.entity_master EM (nolock)
                              ON GCC.entity_id = EM.entity_id
                      INNER JOIN bdc1_masters.dbo.site_master SM (nolock)
                              ON SM.site_id = GCC.site_id
                      INNER JOIN bdc1_masters.dbo.site_master S (nolock)
                              ON EM.entity_id = S.entity_id
                                 AND S.active = 'Y'
                                 AND SM.site_id IN ( 'B051', 'B053', 'B055', 'B054',
                                                     'B052', 'B050' )
                                 AND EM.entity_id IN ( '1364BDB053', '1364BDB001', '1364BDB055'
                                                       ,
                                                       '1364BDB054',
                                                       '1364BDB052', '1364BDB051', '1364BDB050'
                                                     )
                                 AND SM.active = 'Y'
               ORDER  BY 2
               """;

        try
        {
            var data = await _databaseService.ExecuteQueryAsync(sql);
            var sites = new List<Site>();
            foreach (DataRow row in data.Rows)
            {
                sites.Add(new Site
                {
                    Name = row["site_name"]?.ToString() ?? "",
                    Code = row["site_id"]?.ToString() ?? ""
                });
            }

            Sites = sites;
            SelectedSite = sites.FirstOrDefault();
        }
        catch (Exception)
        {
            Sites = new List<Site>
            {
                new Site { Name = "Site1", Code = "S1" },
                new Site { Name = "Site2", Code = "S2" },
                new Site { Name = "Site3", Code = "S3" }
            };
            SelectedSite = Sites.FirstOrDefault();
        }
    }

    [RelayCommand]
    private async Task Load()
    {
        IsLoading = false;
        string sql = $"Exec BDC1_iDAS_HQDB.dbo.RPT_NET_Sales_Data_Dump '{FormattedFromDate}', '{FormattedToDate}',  '{SelectedSite?.Code}',  '{SelectedDistributor?.Code}'";

        try
        {
            IsLoading = true;
            var data = await _databaseService.ExecuteQueryAsync(sql);
            ReportData = data;

            var items = new ObservableCollection<SalesDataReport>();
            foreach (DataRow row in data.Rows)
            {
                items.Add(new SalesDataReport
                {
                    Distributor_ID = row["Distributor_ID"]?.ToString() ?? "",
                    Distributor_Name = row["Distributor_Name"]?.ToString() ?? "",
                    Invoice_No = row["Invoice_No"]?.ToString() ?? "",
                    Invoice_Date = row["Invoice_Date"]?.ToString() ?? "",
                    Outlet_ID = row["Outlet_ID"]?.ToString() ?? "",
                    Outlet_Name = row["Outlet_Name"]?.ToString() ?? "",
                    SM_Name = row["SM_Name"]?.ToString() ?? "",
                    ASM_Name = row["ASM_Name"]?.ToString() ?? "",
                    SE_Name = row["SE_Name"]?.ToString() ?? "",
                    Inv_Line_No = row["Inv_Line_No"]?.ToString() ?? "",
                    Item_ID = row["Item_ID"]?.ToString() ?? "",
                    Item_Name = row["Item_Name"]?.ToString() ?? "",
                    Item_Category = row["Item_Category"]?.ToString() ?? "",
                    Pack_Size = row["Pack_Size"]?.ToString() ?? "",
                    Pack_Type = row["Pack_Type"]?.ToString() ?? "",
                    Brand = row["Brand"]?.ToString() ?? "",
                    BPC = row["BPC"]?.ToString() ?? "",
                    MRP = row.Table.Columns.Contains("MRP") && row["MRP"] != DBNull.Value ? Convert.ToDecimal(row["MRP"]) : 0,
                    Qty_EA = row.Table.Columns.Contains("Qty_EA") && row["Qty_EA"] != DBNull.Value ? Convert.ToDecimal(row["Qty_EA"]) : 0,
                    Qty_CS = row.Table.Columns.Contains("Qty_CS") && row["Qty_CS"] != DBNull.Value ? Convert.ToDecimal(row["Qty_CS"]) : 0,
                    Basic_Price = row.Table.Columns.Contains("Basic_Price") && row["Basic_Price"] != DBNull.Value ? Convert.ToDecimal(row["Basic_Price"]) : 0,
                    Line_Amount = row.Table.Columns.Contains("Line_Amount") && row["Line_Amount"] != DBNull.Value ? Convert.ToDecimal(row["Line_Amount"]) : 0,
                    Discount_Amount = row.Table.Columns.Contains("Discount_Amount") && row["Discount_Amount"] != DBNull.Value ? Convert.ToDecimal(row["Discount_Amount"]) : 0,
                    MER_No = row["MER_No"]?.ToString() ?? "",
                    MER_Type = row["MER_Type"]?.ToString() ?? "",
                    Disbursement_Method = row["Disbursement_Method"]?.ToString() ?? "",
                    Tax_Value_1 = row.Table.Columns.Contains("Tax_Value_1") && row["Tax_Value_1"] != DBNull.Value ? Convert.ToDecimal(row["Tax_Value_1"]) : 0,
                    Tax_Value_2 = row.Table.Columns.Contains("Tax_Value_2") && row["Tax_Value_2"] != DBNull.Value ? Convert.ToDecimal(row["Tax_Value_2"]) : 0,
                    Tax_on_Discount = row.Table.Columns.Contains("Tax_on_Discount") && row["Tax_on_Discount"] != DBNull.Value ? Convert.ToDecimal(row["Tax_on_Discount"]) : 0,
                    Total_Tax = row.Table.Columns.Contains("Total_Tax") && row["Total_Tax"] != DBNull.Value ? Convert.ToDecimal(row["Total_Tax"]) : 0,
                    Route_Description = row["Route_Description"]?.ToString() ?? "",
                    Sales_Man_Name = row["Sales_Man_Name"]?.ToString() ?? "",
                    Vehicle_Registration_No = row["Vehicle_Registration_No"]?.ToString() ?? "",
                    LoadOut_No = row["LoadOut_No"]?.ToString() ?? "",
                    Order_No = row["Order_No"]?.ToString() ?? "",
                    Cancelled_Inv_No = row["Cancelled_Inv_No"]?.ToString() ?? "",
                    Customer_Type = row["Customer_Type"]?.ToString() ?? "",
                    eClaim_Rate = row.Table.Columns.Contains("eClaim_Rate") && row["eClaim_Rate"] != DBNull.Value ? Convert.ToDecimal(row["eClaim_Rate"]) : 0
                });
            }
            
            ReportItems = items;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        IsLoading = false;
    }

    [RelayCommand]
    private async Task Export()
    {
        if (ReportData.Rows.Count == 0)
            return;

        await ExportService.ExportToCsv(ReportData);
    }
}