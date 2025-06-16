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

public partial class Report5ViewModel : ObservableObject
{
    private readonly IDatabaseService _databaseService;

    [ObservableProperty]
    private List<Entity> _entities = new();

    [ObservableProperty]
    private Entity? _selectedEntity;

    [ObservableProperty]
    private List<Site> _sites = new();

    [ObservableProperty]
    private Site? _selectedSite;

    [ObservableProperty] private List<CustomerClass> _customerClasses = new();

    [ObservableProperty]
    private CustomerClass? _selectedCustomerClass;

    [ObservableProperty]
    private List<CustomerType> _customerTypes = CustomerType.GetAll();

    [ObservableProperty]
    private CustomerType? _selectedCustomerType;

    [ObservableProperty]
    private List<CustomerState> _customerStates = CustomerState.GetAll();

    [ObservableProperty]
    private CustomerState? _selectedCustomerState;

    [ObservableProperty]
    private List<Stl> _stls = new();

    [ObservableProperty]
    private Stl? _selectedStl;

    [ObservableProperty]
    private string _sapId = string.Empty;

    [ObservableProperty]
    private bool _isLoading = true;

    [ObservableProperty]
    private DataTable _reportData = new();

    [ObservableProperty]
    private ObservableCollection<UserDumpReport> _reportItems = new();

    public Report5ViewModel(IDatabaseService databaseService)
    {
        _databaseService = databaseService;

        Task.Run(LoadEntitiesAsync);
        Task.Run(LoadSitesAsync);
        Task.Run(LoadStlAsync);
        Task.Run(LoadCustomerClassAsync);
    }

    private async Task LoadEntitiesAsync()
    {
        const string sql = """
               SELECT DISTINCT B.entity_id,
                               C.entity_name + '(' + b.entity_id + ')' Entity_Name
               FROM   bdc1_masters.dbo.idas_activation_setup A (nolock)
                      INNER JOIN bdc1_masters.dbo.gcc_master B (nolock)
                              ON a.dist_gcc_id = b.gcc_id
                                 AND a.dist_gcc_id_active = 'y'
                                 AND a.dist_go_live_flag = 'y'
                      INNER JOIN bdc1_masters.dbo.entity_master C
                              ON b.entity_id = c.entity_id
                      INNER JOIN bdc1_masters.dbo.admin_entity_master AEM
                              ON AEM.admin_entity_id = c.admin_entity_id
                                 AND B.entity_id IN ( '1364BDB053', '1364BDB001', '1364BDB055',
                                                      '1364BDB054',
                                                      '1364BDB052', '1364BDB051', '1364BDB050'
                                                    )
                                 AND B.entity_id IN (SELECT DISTINCT entity_id
                                                     FROM   bdc1_masters.dbo.site_master
                                                     WHERE  active = 'Y')
               ORDER  BY entity_name
               """;

        try
        {
            var data = await _databaseService.ExecuteQueryAsync(sql);
            var entities = new List<Entity>();

            foreach (DataRow row in data.Rows)
            {
                entities.Add(new Entity
                {
                    Name = row["Entity_Name"].ToString() ?? string.Empty,
                    Code = row["entity_id"].ToString() ?? string.Empty
                });
            }

            Entities = entities;
            SelectedEntity = Entities.FirstOrDefault();
        }
        catch (Exception)
        {
            Entities = new List<Entity>
            {
                new Entity { Name = "Entity1", Code = "E1" },
                new Entity { Name = "Entity2", Code = "E2" },
                new Entity { Name = "Entity3", Code = "E3" }
            };
            SelectedEntity = Entities.FirstOrDefault();
        }
    }

    private async Task LoadSitesAsync()
    {
        string sql = $"""
              SELECT site_id,
                     site_name + '-' + site_id AS Site_Name
              FROM   bdc1_masters.dbo.site_master
              WHERE  entity_id = '1364BDB053'
                     AND active = 'Y'
              """;

        try
        {
            var data = await _databaseService.ExecuteQueryAsync(sql);
            var sites = new List<Site>();

            foreach (DataRow row in data.Rows)
            {
                sites.Add(new Site
                {
                    Name = row["site_name"].ToString() ?? string.Empty,
                    Code = row["site_id"].ToString() ?? string.Empty
                });
            }

            Sites = sites;
            SelectedSite = Sites.FirstOrDefault();
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

    public async Task LoadStlAsync()
    {
        string sql = $"""
              SELECT DISTINCT A.se_id,
                              se_name + '(' + A.se_id + ')' SE_Name
              FROM   bdc1_masters.dbo.se_master A
                     INNER JOIN bdc1_masters.dbo.se_master_matrix C
                             ON A.se_id = C.se_id
                     INNER JOIN bdc1_masters.dbo.entity_master D
                             ON D.admin_entity_id = C.admin_entity_id
              WHERE  A.active_flag = 'Y'
                     AND D.entity_id = '1364BDB053'
              ORDER  BY se_name
              """;

        try
        {
            var data = await _databaseService.ExecuteQueryAsync(sql);
            var stls = new List<Stl>();

            foreach (DataRow row in data.Rows)
            {
                stls.Add(new Stl
                {
                    Name = row["SE_Name"].ToString() ?? string.Empty,
                    Code = row["se_id"].ToString() ?? string.Empty
                });
            }

            Stls = stls;
            SelectedStl = Stls.FirstOrDefault();
        }
        catch (Exception)
        {
            Stls = new List<Stl>
            {
                new Stl { Name = "STL1", Code = "STL1" },
                new Stl { Name = "STL2", Code = "STL2" },
                new Stl { Name = "STL3", Code = "STL3" }
            };
            SelectedStl = Stls.FirstOrDefault();
        }
    }

    public async Task LoadCustomerClassAsync()
    {
        string sql = $"""
              SELECT DISTINCT customer_class_id,
                              customer_class_desc
              FROM   bdc1_masters.dbo.customer_class_master
              WHERE  customer_category_id = '01'
                     AND active = 'Y'
              """;

        try
        {
            var data = await _databaseService.ExecuteQueryAsync(sql);
            var customerClasses = new List<CustomerClass>();

            foreach (DataRow row in data.Rows)
            {
                customerClasses.Add(new CustomerClass
                {
                    Name = row["customer_class_desc"].ToString() ?? string.Empty,
                    Code = row["customer_class_id"].ToString() ?? string.Empty
                });
            }

            CustomerClasses = customerClasses;
            SelectedCustomerClass = CustomerClasses.FirstOrDefault();
        }
        catch (Exception)
        {
            CustomerClasses = new List<CustomerClass>
            {
                new CustomerClass { Name = "Retail", Code = "00" },
                new CustomerClass { Name = "Wholesale", Code = "00" },
                new CustomerClass { Name = "Distributor", Code = "00" }
            };
            SelectedCustomerClass = CustomerClasses.FirstOrDefault();
        }
    }

    [RelayCommand]
    private async Task Load()
    {
        IsLoading = false;
        string sql = $"EXEC BDC1_Masters.DBO.RPT_Customer_Data_Dump '1364BDB053','B053','D','21','88000741','ff','','Y','1','','','','','','','','','','','','' ";

        try
        {
            IsLoading = true;
            var data = await _databaseService.ExecuteQueryAsync(sql);
            ReportData = data;

            var items = new ObservableCollection<UserDumpReport>();
            foreach (DataRow row in data.Rows)
            {
                items.Add(new UserDumpReport
                {
                    Customer_ID = row["Customer_ID"]?.ToString() ?? string.Empty,
                    SAP_Customer_ID = row["SAP_Customer_ID"]?.ToString() ?? string.Empty,
                    Customer_Name = row["Customer_Name"]?.ToString() ?? string.Empty,
                    Customer_Category_ID = row["Customer_Category_ID"]?.ToString() ?? string.Empty,
                    Customer_Category_Desc = row["Customer_Category_Desc"]?.ToString() ?? string.Empty,
                    Customer_Class_ID = row["Customer_Class_ID"]?.ToString() ?? string.Empty,
                    Customer_Class_Desc = row["Customer_Class_Desc"]?.ToString() ?? string.Empty,
                    Entity_ID = row["Entity_ID"]?.ToString() ?? string.Empty,
                    Entity_Name = row["Entity_Name"]?.ToString() ?? string.Empty,
                    Admin_Entity_ID = row["Admin_Entity_ID"]?.ToString() ?? string.Empty,
                    Admin_Entity_Name = row["Admin_Entity_Name"]?.ToString() ?? string.Empty,
                    Region_ID = row["Region_ID"]?.ToString() ?? string.Empty,
                    Region_Name = row["Region_Name"]?.ToString() ?? string.Empty,
                    Site_ID = row["Site_ID"]?.ToString() ?? string.Empty,
                    Site_Name = row["Site_Name"]?.ToString() ?? string.Empty,
                    Market_Area = row["Market_Area"]?.ToString() ?? string.Empty,
                    MarketArea_Territory = row["MarketArea_Territory"]?.ToString() ?? string.Empty,
                    MD_Name = row["MD_Name"]?.ToString() ?? string.Empty,
                    Active = row["Active"]?.ToString() ?? string.Empty,
                    Order_Block = row["Order_Block"]?.ToString() ?? string.Empty,
                    Customer_Activated_Date = row["Customer_Activated_Date"]?.ToString() ?? string.Empty,
                    Address_1 = row["Address_1"]?.ToString() ?? string.Empty,
                    Address_2 = row["Address_2"]?.ToString() ?? string.Empty,
                    Address_3 = row["Address_3"]?.ToString() ?? string.Empty,
                    Pin_Code = row["Pin_Code"]?.ToString() ?? string.Empty,
                    Town = row["Town"]?.ToString() ?? string.Empty,
                    District = row["District"]?.ToString() ?? string.Empty,
                    State_Name = row["State_Name"]?.ToString() ?? string.Empty,
                    PIN_Code = row["PIN_Code"]?.ToString() ?? string.Empty,
                    Home_Address_1 = row["Home_Address_1"]?.ToString() ?? string.Empty,
                    Home_Address_2 = row["Home_Address_2"]?.ToString() ?? string.Empty,
                    Home_Address_3 = row["Home_Address_3"]?.ToString() ?? string.Empty,
                    Home_Pin_Code = row["Home_Pin_Code"]?.ToString() ?? string.Empty,
                    Home_Town = row["Home_Town"]?.ToString() ?? string.Empty,
                    Home_District = row["Home_District"]?.ToString() ?? string.Empty,
                    Pincode = row["Pincode"]?.ToString() ?? string.Empty,
                    Contact_Person = row["Contact_Person"]?.ToString() ?? string.Empty,
                    Telephone_No = row["Telephone_No"]?.ToString() ?? string.Empty,
                    Mobile_No = row["Mobile_No"]?.ToString() ?? string.Empty,
                    Email_ID = row["Email_ID"]?.ToString() ?? string.Empty,
                    Distributor_Route_ID = row["Distributor_Route_ID"]?.ToString() ?? string.Empty,
                    Parent_Code = row["Parent_Code"]?.ToString() ?? string.Empty,
                    Customer_Name_2 = row["Customer_Name_2"]?.ToString() ?? string.Empty,
                    SE_ID = row["SE_ID"]?.ToString() ?? string.Empty,
                    SE_Territory = row["SE_Territory"]?.ToString() ?? string.Empty,
                    SE_Name = row["SE_Name"]?.ToString() ?? string.Empty,
                    ASM_ID = row["ASM_ID"]?.ToString() ?? string.Empty,
                    ASM_Territory = row["ASM_Territory"]?.ToString() ?? string.Empty,
                    ASM_Name = row["ASM_Name"]?.ToString() ?? string.Empty,
                    SM_ID = row["SM_ID"]?.ToString() ?? string.Empty,
                    SM_Territory = row["SM_Territory"]?.ToString() ?? string.Empty,
                    SM_Name = row["SM_Name"]?.ToString() ?? string.Empty,
                    Channel_ID = row["Channel_ID"]?.ToString() ?? string.Empty,
                    Channel_Desc = row["Channel_Desc"]?.ToString() ?? string.Empty,
                    Sub_Channel_ID = row["Sub_Channel_ID"]?.ToString() ?? string.Empty,
                    Sub_Channel_Desc = row["Sub_Channel_Desc"]?.ToString() ?? string.Empty,
                    CSS_Cluster_Desc = row["CSS_Cluster_Desc"]?.ToString() ?? string.Empty,
                    RSA_Cluster_Desc = row["RSA_Cluster_Desc"]?.ToString() ?? string.Empty,
                    VPO_Class_Desc = row["VPO_Class_Desc"]?.ToString() ?? string.Empty,
                    NKA_Customer_ID = row["NKA_Customer_ID"]?.ToString() ?? string.Empty,
                    NKA_Customer_Name = row["NKA_Customer_Name"]?.ToString() ?? string.Empty,
                    Pricelist_Code = row["Pricelist_Code"]?.ToString() ?? string.Empty,
                    Cheque_Allowed = row["Cheque_Allowed"]?.ToString() ?? string.Empty,
                    Open_Market_Discount = row["Open_Market_Discount"]?.ToString() ?? string.Empty,
                    Payment_Mode = row["Payment_Mode"]?.ToString() ?? string.Empty,
                    V2020_Status = row["V2020_Status"]?.ToString() ?? string.Empty,
                    V2020_Activation_Date = row["V2020_Activation_Date"]?.ToString() ?? string.Empty,
                    V2020_Deactivation_Date = row["V2020_Deactivation_Date"]?.ToString() ?? string.Empty,
                    HDO_Status = row["HDO_Status"]?.ToString() ?? string.Empty,
                    HDO_Activation_Date = row["HDO_Activation_Date"]?.ToString() ?? string.Empty,
                    HDO_Deactivation_Date = row["HDO_Deactivation_Date"]?.ToString() ?? string.Empty,
                    Cola_Customer_Code = row["Cola_Customer_Code"]?.ToString() ?? string.Empty,
                    Vehicle_Type_ID = row["Vehicle_Type_ID"]?.ToString() ?? string.Empty,
                    Latitude = row.Table.Columns.Contains("Latitude") && decimal.TryParse(row["Latitude"]?.ToString(), out var lat) ? lat : 0,
                    Longitude = row.Table.Columns.Contains("Longitude") && decimal.TryParse(row["Longitude"]?.ToString(), out var lng) ? lng : 0,
                    FSSAI_No = row["FSSAI_No"]?.ToString() ?? string.Empty,
                    FSSAI_Date = row["FSSAI_Date"]?.ToString() ?? string.Empty,
                    Agreement_Exp_Date = row["Agreement_Exp_Date"]?.ToString() ?? string.Empty,
                    LST_Number = row["LST_Number"]?.ToString() ?? string.Empty,
                    CST_No = row["CST_No"]?.ToString() ?? string.Empty,
                    Lead_Time = row["Lead_Time"]?.ToString() ?? string.Empty,
                    Distance = row.Table.Columns.Contains("Distance") && decimal.TryParse(row["Distance"]?.ToString(), out var dist) ? dist : 0,
                    AR_Credit_Limit = row.Table.Columns.Contains("AR_Credit_Limit") && decimal.TryParse(row["AR_Credit_Limit"]?.ToString(), out var arcl) ? arcl : 0,
                    Col_Limit = row.Table.Columns.Contains("Col_Limit") && decimal.TryParse(row["Col_Limit"]?.ToString(), out var coll) ? coll : 0,
                    Credit_Days = row["Credit_Days"]?.ToString() ?? string.Empty,
                    Cust_Cond_GRP1 = row["Cust_Cond_GRP1"]?.ToString() ?? string.Empty,
                    AutoKnockingOff = row["AutoKnockingOff"]?.ToString() ?? string.Empty,
                    Recon_Account = row["Recon_Account"]?.ToString() ?? string.Empty,
                    Payment_Terms = row["Payment_Terms"]?.ToString() ?? string.Empty,
                    Block_for_Payment = row["Block_for_Payment"]?.ToString() ?? string.Empty,
                    House_Bank = row["House_Bank"]?.ToString() ?? string.Empty,
                    BANK_KEY = row["BANK_KEY"]?.ToString() ?? string.Empty,
                    Reference_Details_Virtual_Code = row["Reference_Details_Virtual_Code"]?.ToString() ?? string.Empty,
                    Pricing_Procedure = row["Pricing_Procedure"]?.ToString() ?? string.Empty,
                    Sales_District = row["Sales_District"]?.ToString() ?? string.Empty,
                    Accnt_Assign_Grp = row["Accnt_Assign_Grp"]?.ToString() ?? string.Empty,
                    Max_Partial_Delivery_Allowed = row["Max_Partial_Delivery_Allowed"]?.ToString() ?? string.Empty,
                    PrintOption = row["PrintOption"]?.ToString() ?? string.Empty,
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