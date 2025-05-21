using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DBExporter.Models;
using DBExporter.Services;

namespace DBExporter.ViewModels;

public partial class Report3ViewModel : ObservableObject
{
    private readonly IDatabaseService _databaseService;

    [ObservableProperty] private List<Entity> _entities = new();

    [ObservableProperty] private Entity? _selectedEntity;

    [ObservableProperty] private List<Distributor> _distributors = new();

    [ObservableProperty] private Distributor? _selectedDistributor;

    [ObservableProperty] private string? _claimNo;

    [ObservableProperty] private string? _year;

    [ObservableProperty] private DataTable _reportData = new();

    [ObservableProperty] private ObservableCollection<ClaimStatusReport> _reportItems = new();

    [ObservableProperty] private bool _isLoading;

    public Report3ViewModel(IDatabaseService databaseService)
    {
        _databaseService = databaseService;

        Task.Run(LoadEntities);
        Task.Run(LoadDistributors);
    }

    private async Task LoadEntities()
    {
        const string sql = """
                           Select distinct B.Entity_Id, C.Entity_Name + '('+b.Entity_Id+')' Entity_Name
                           from BDC1_Masters.dbo.iDAS_Activation_Setup A (Nolock)  Inner join BDC1_Masters.dbo.GCC_Master B (Nolock)
                           on a.Dist_GCC_ID=b.GCC_ID And a.Dist_GCC_ID_Active='y' and a.Dist_Go_Live_Flag='y'
                           inner join BDC1_Masters.dbo.Entity_Master C on b.Entity_Id=c.Entity_ID inner join BDC1_Masters.dbo.Admin_Entity_Master AEM
                           on AEM.Admin_Entity_Id=c.Admin_Entity_Id And B.Entity_ID IN ('1364BDB053','1364BDB001','1364BDB055','1364BDB054','1364BDB052','1364BDB051','1364BDB050')
                           and B.Entity_ID in (Select Distinct Entity_ID From BDC1_Masters.DBO.Site_Master Where  Active = 'Y' )  Order By Entity_Name
                           """;

        try
        {
            var data = await _databaseService.ExecuteQueryAsync(sql);
            var entities = new List<Entity>();
            foreach (DataRow row in data.Rows)
            {
                entities.Add(new Entity
                {
                    Name = row["Entity_Name"]?.ToString() ?? "",
                    Code = row["Entity_Id"]?.ToString() ?? ""
                });
            }

            Entities = entities;
            SelectedEntity = entities.FirstOrDefault();
        }
        catch (Exception)
        {
            Entities =
            [
                new Entity { Name = "Entity1", Code = "1" },
                new Entity { Name = "Entity2", Code = "2" },
                new Entity { Name = "Entity3", Code = "3" }
            ];
            SelectedEntity = Entities[0];
        }
    }

    private async Task LoadDistributors()
    {
        const string sql = """
                           SELECT  distinct   IAS.Customer_ID Distributor_id,IAS.Dist_GCC_ID, IAS.Distributor_Name + ' (' + IAS.Dist_GCC_ID + ')' + ' - ' + case when GCC.Customer_Class_ID='4' then 'DIST' when GCC.Customer_Class_ID='26' then 'AMC' when GCC.Customer_Class_ID='23' then 'IWHOL' end AS Distributor_Name
                           FROM         BDC1_Masters.DBO.iDAS_Activation_Setup IAS (NOLOCK)
                           INNER JOIN BDC1_Masters.DBO.GCC_Master GCC (NOLOCK) ON IAS.Dist_GCC_ID = GCC.GCC_ID
                           INNER JOIN BDC1_Masters.DBO.GCC_Master_Sales_Area GCSA (NOLOCK) ON IAS.Dist_GCC_ID = GCSA.GCC_ID
                           INNER JOIN  BDC1_Masters.DBO.Entity_Master EM (NOLOCK) ON GCC.Entity_Id = EM.Entity_ID
                           INNER JOIN BDC1_Masters.DBO.SITE_MASTER S (NOLOCK) ON EM.ENTITY_ID=S.ENTITY_ID AND S.Active='Y'
                           and S.Site_ID in ('B051','B053','B055','B054','B052','B050')  And EM.Entity_ID IN ('1364BDB053')
                           Where Dist_GCC_ID_Active='Y' and GCC.Active='Y'    Order by Distributor_Name
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

    [RelayCommand]
    private async Task Load()
    {
        IsLoading = false;
        var condition = new StringBuilder("1=1");
        if (SelectedEntity != null)
        {
            condition.Append($" AND B.entity_id = '{SelectedEntity.Code}'");
        }

        if (SelectedDistributor != null && SelectedDistributor.Code != "Al")
        {
            condition.Append($" AND A.dist_gcc_id = '{SelectedDistributor.Code}'");
        }

        if (!string.IsNullOrEmpty(ClaimNo))
        {
            condition.Append($" AND A.claim_no = '{ClaimNo}'");
        }

        if (!string.IsNullOrEmpty(Year))
        {
            condition.Append($" AND A.claim_year = '{Year}'");
        }

        IsLoading = true;
        string sql = $"""
                      SELECT AA.dist_gcc_id,
                             AA.customer_name,
                             AA.claimno,
                             AA.status,
                             AA.claim_no,
                             AA.claim_year,
                             AA.top_sheet_status,
                             AA.finance_confirmation_date,
                             AA.dist_signoff_date,
                             AA.sales_signoff_date,
                             AA.finance_signoff_date,
                             AA.dsd,
                             AA.ssd,
                             AA.fsd,
                             Sum(eclaim_amt) AS eClaim_Amt,
                             AA.fin_confirmed_by,
                             AA.sales_signoff_by,
                             AA.fin_signoff_by
                      FROM   (SELECT A.dist_gcc_id,
                                     B.customer_name,
                                     CONVERT(VARCHAR(2), A.claim_no) + ' - '
                                     + CONVERT(VARCHAR(4), A.claim_year) + ' '         AS ClaimNo,
                                     CASE
                                       WHEN A.top_sheet_status = 0 THEN 'Finance Confirmation Pending'
                                       WHEN A.top_sheet_status = 1 THEN 'Distributor SignOff Pending'
                                       WHEN A.top_sheet_status = 2 THEN 'Sales SignOff Pending'
                                       WHEN A.top_sheet_status = 3 THEN
                                       'Finance Manager SignOff Pending'
                                       WHEN A.top_sheet_status IN( 4, 5 ) THEN
                                       'eClaim has been processed'
                                       ELSE ''
                                     END                                               AS Status,
                                     A.claim_no,
                                     A.claim_year,
                                     A.top_sheet_status,
                                     A.finance_confirmation_date,
                                     A.dist_signoff_date,
                                     A.sales_signoff_date,
                                     A.finance_signoff_date,
                                     CONVERT(VARCHAR(12), A.dist_signoff_date, 103)    AS dsd,
                                     CONVERT(VARCHAR(12), A.sales_signoff_date, 103)   AS ssd,
                                     CONVERT(VARCHAR(12), A.finance_signoff_date, 103) AS fsd,
                                     Round(Sum(A.eclaim_amt), 2)                       AS eClaim_Amt,
                                     U1.user_name                                      AS
                                     Fin_Confirmed_By,
                                     U2.user_name                                      AS
                                     Sales_Signoff_By,
                                     U3.user_name                                      AS
                                     Fin_Signoff_By
                              FROM   bdc1_idas_hqdb.dbo.claim_top_sheet AS A
                                     INNER JOIN bdc1_masters.dbo.gcc_master AS B
                                             ON A.dist_gcc_id = B.gcc_id
                                     LEFT OUTER JOIN bdc1_net.dbo.user_master U1
                                                  ON A.finance_confirmation_done_by = U1.user_id
                                     LEFT OUTER JOIN bdc1_net.dbo.user_master U2
                                                  ON A.sales_signoff_done_by = U2.user_id
                                     LEFT OUTER JOIN bdc1_net.dbo.user_master U3
                                                  ON A.finance_signoff_done_by = U3.user_id
                              WHERE  {condition}
                              GROUP  BY A.dist_gcc_id,
                                        B.customer_name,
                                        A.claim_no,
                                        A.claim_year,
                                        A.top_sheet_status,
                                        A.finance_confirmation_date,
                                        A.dist_signoff_date,
                                        A.sales_signoff_date,
                                        A.finance_signoff_date,
                                        U1.user_name,
                                        U2.user_name,
                                        U3.user_name
                              UNION ALL
                              SELECT A.dist_gcc_id,
                                     B.customer_name,
                                     CONVERT(VARCHAR(2), A.claim_no) + ' - '
                                     + CONVERT(VARCHAR(4), A.claim_year) + ' '         AS ClaimNo,
                                     CASE
                                       WHEN A.top_sheet_status = 0 THEN 'Finance Confirmation Pending'
                                       WHEN A.top_sheet_status = 1 THEN 'Distributor SignOff Pending'
                                       WHEN A.top_sheet_status = 2 THEN 'Sales SignOff Pending'
                                       WHEN A.top_sheet_status = 3 THEN
                                       'Finance Manager SignOff Pending'
                                       WHEN A.top_sheet_status IN( 4, 5 ) THEN
                                       'eClaim has been processed'
                                       ELSE ''
                                     END                                               AS Status,
                                     A.claim_no,
                                     A.claim_year,
                                     A.top_sheet_status,
                                     A.finance_confirmation_date,
                                     A.dist_signoff_date,
                                     A.sales_signoff_date,
                                     A.finance_signoff_date,
                                     CONVERT(VARCHAR(12), A.dist_signoff_date, 103)    AS dsd,
                                     CONVERT(VARCHAR(12), A.sales_signoff_date, 103)   AS ssd,
                                     CONVERT(VARCHAR(12), A.finance_signoff_date, 103) AS fsd,
                                     Round(Sum(A.eclaim_amt), 2)                       AS eClaim_Amt,
                                     U1.user_name                                      AS
                                     Fin_Confirmed_By,
                                     U2.user_name                                      AS
                                     Sales_Signoff_By,
                                     U3.user_name                                      AS
                                     Fin_Signoff_By
                              FROM   bdc1_idas_hqdb.dbo.claim_top_sheet_redistribution_commision AS A
                                     INNER JOIN bdc1_masters.dbo.gcc_master AS B
                                             ON A.dist_gcc_id = B.gcc_id
                                     LEFT OUTER JOIN bdc1_net.dbo.user_master U1
                                                  ON A.finance_confirmation_done_by = U1.user_id
                                     LEFT OUTER JOIN bdc1_net.dbo.user_master U2
                                                  ON A.sales_signoff_done_by = U2.user_id
                                     LEFT OUTER JOIN bdc1_net.dbo.user_master U3
                                                  ON A.finance_signoff_done_by = U3.user_id
                              WHERE  {condition}
                              GROUP  BY A.dist_gcc_id,
                                        B.customer_name,
                                        A.claim_no,
                                        A.claim_year,
                                        A.top_sheet_status,
                                        A.finance_confirmation_date,
                                        A.dist_signoff_date,
                                        A.sales_signoff_date,
                                        A.finance_signoff_date,
                                        U1.user_name,
                                        U2.user_name,
                                        U3.user_name) AA
                      GROUP  BY AA.dist_gcc_id,
                                AA.customer_name,
                                AA.claimno,
                                AA.status,
                                AA.claim_no,
                                AA.claim_year,
                                AA.top_sheet_status,
                                AA.finance_confirmation_date,
                                AA.dist_signoff_date,
                                AA.sales_signoff_date,
                                AA.finance_signoff_date,
                                AA.dsd,
                                AA.ssd,
                                AA.fsd,
                                AA.fin_confirmed_by,
                                AA.sales_signoff_by,
                                AA.fin_signoff_by
                      HAVING Sum(eclaim_amt) > 0
                      ORDER  BY AA.customer_name
                      """;

        try
        {
            var data = await _databaseService.ExecuteQueryAsync(sql);
            ReportData = data;


            var items = new ObservableCollection<ClaimStatusReport>();

            foreach (DataRow row in data.Rows)
            {
                items.Add(new()
                {
                    DistributorId = row["dist_gcc_id"]?.ToString() ?? "",
                    DistributorName = row["customer_name"]?.ToString() ?? "",
                    ClaimNo = row["ClaimNo"]?.ToString() ?? "",
                    Status = row["Status"]?.ToString() ?? "",
                    ClaimYear = row["claim_year"]?.ToString() ?? "",
                    TopSheetStatus = row["top_sheet_status"]?.ToString() ?? "",
                    FinanceConfirmationDate = row["finance_confirmation_date"]?.ToString() ?? "",
                    DistSignoffDate = row["dist_signoff_date"]?.ToString() ?? "",
                    SalesSignoffDate = row["sales_signoff_date"]?.ToString() ?? "",
                    FinanceSignoffDate = row["finance_signoff_date"]?.ToString() ?? "",
                    Dsd = row["dsd"]?.ToString() ?? "",
                    Ssd = row["ssd"]?.ToString() ?? "",
                    Fsd = row["fsd"]?.ToString() ?? "",
                    EClaimAmt = Convert.ToDecimal(row["eClaim_Amt"]),
                    FinConfirmedBy = row["Fin_Confirmed_By"]?.ToString() ?? "",
                    SalesSignoffBy = row["Sales_Signoff_By"]?.ToString() ?? "",
                    FinSignoffBy = row["Fin_Signoff_By"]?.ToString() ?? ""
                });
            }

            ReportItems = items;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
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