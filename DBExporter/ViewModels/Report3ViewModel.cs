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

namespace DBExporter.ViewModels;

public partial class Report3ViewModel : ObservableObject
{
    private readonly IDatabaseService _databaseService;

    [ObservableProperty]
    private List<Entity> _entities = new();

    [ObservableProperty]
    private Entity? _selectedEntity;

    [ObservableProperty]
    private List<Distributor> _distributors = new();

    [ObservableProperty]
    private Distributor? _selectedDistributor;

    [ObservableProperty]
    private string? _claimNo;

    [ObservableProperty]
    private string? _year;

    [ObservableProperty]
    private DataTable _reportData = new();

    [ObservableProperty]
    private ObservableCollection<ClaimStatusReport> _reportItem = new();

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
                new(){Name = "All", Code = "Al"}
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
        if (SelectedEntity == null || SelectedDistributor == null || Year == null || ClaimNo == null)
            return;

        string sql = $"""

            """;
    }
}
