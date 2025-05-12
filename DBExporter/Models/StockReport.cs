using System;
using System.ComponentModel;

namespace DBExporter.Models;

public class StockReport
{
    [DisplayName("Distributor Name")]
    public string Distributor_Name { get; set; } = string.Empty;

    [DisplayName("Category Group")]
    public string Category_Group { get; set; } = string.Empty;

    [DisplayName("Item Category ID")]
    public string Item_Category_ID { get; set; } = string.Empty;

    [DisplayName("Pack Size ID")]
    public string Pack_Size_ID { get; set; } = string.Empty;

    [DisplayName("Item ID")]
    public string Item_ID { get; set; } = string.Empty;

    [DisplayName("Item Name")]
    public string Item_Name { get; set; } = string.Empty;

    [DisplayName("MRP")]
    public decimal MRP { get; set; }

    [DisplayName("In-hand Qty PC")]
    public decimal In_hand_Qty_PC { get; set; }

    [DisplayName("In-hand Qty PC Landed")]
    public decimal In_hand_Qty_PC_landed { get; set; }

    [DisplayName("Open Settlements")]
    public decimal OpenSettlements { get; set; }

    [DisplayName("Open Settlements Landed")]
    public decimal OpenSettlementsLanded { get; set; }
}
