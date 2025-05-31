using System;
using System.ComponentModel;

namespace DBExporter.Models;

public class SalesDataReport
{
    [DisplayName("Distributor ID")]
    public string Distributor_ID { get; set; } = string.Empty;

    [DisplayName("Distributor Name")]
    public string Distributor_Name { get; set; } = string.Empty;

    [DisplayName("Invoice No")]
    public string Invoice_No { get; set; } = string.Empty;

    [DisplayName("Invoice Date")]
    public string Invoice_Date { get; set; } = string.Empty;

    [DisplayName("Outlet ID")]
    public string Outlet_ID { get; set; } = string.Empty;

    [DisplayName("Outlet Name")]
    public string Outlet_Name { get; set; } = string.Empty;

    [DisplayName("SM Name")]
    public string SM_Name { get; set; } = string.Empty;

    [DisplayName("ASM Name")]
    public string ASM_Name { get; set; } = string.Empty;

    [DisplayName("SE Name")]
    public string SE_Name { get; set; } = string.Empty;

    [DisplayName("Inv Line No")]
    public string Inv_Line_No { get; set; } = string.Empty;

    [DisplayName("Item ID")]
    public string Item_ID { get; set; } = string.Empty;

    [DisplayName("Item Name")]
    public string Item_Name { get; set; } = string.Empty;

    [DisplayName("Item Category")]
    public string Item_Category { get; set; } = string.Empty;

    [DisplayName("Pack Size")]
    public string Pack_Size { get; set; } = string.Empty;

    [DisplayName("Pack Type")]
    public string Pack_Type { get; set; } = string.Empty;

    [DisplayName("Brand")]
    public string Brand { get; set; } = string.Empty;

    [DisplayName("BPC")]
    public string BPC { get; set; } = string.Empty;

    [DisplayName("MRP")]
    public decimal MRP { get; set; }

    [DisplayName("Qty (EA)")]
    public decimal Qty_EA { get; set; }

    [DisplayName("Qty (CS)")]
    public decimal Qty_CS { get; set; }

    [DisplayName("Basic_Price")]
    public decimal Basic_Price { get; set; }

    [DisplayName("Line Amount")]
    public decimal Line_Amount { get; set; }

    [DisplayName("Discount Amount")]
    public decimal Discount_Amount { get; set; }

    [DisplayName("MER No")]
    public string MER_No { get; set; } = string.Empty;

    [DisplayName("MER Type")]
    public string MER_Type { get; set; } = string.Empty;

    [DisplayName("Disbursement Method")]
    public string Disbursement_Method { get; set; } = string.Empty;

    [DisplayName("Tax_Value_1")]
    public decimal Tax_Value_1 { get; set; }

    [DisplayName("Tax_Value_2")]
    public decimal Tax_Value_2 { get; set; }

    [DisplayName("Tax on Discount")]
    public decimal Tax_on_Discount { get; set; }

    [DisplayName("Total_Tax")]
    public decimal Total_Tax { get; set; }

    [DisplayName("Route Description")]
    public string Route_Description { get; set; } = string.Empty;

    [DisplayName("Sales Man Name")]
    public string Sales_Man_Name { get; set; } = string.Empty;

    [DisplayName("Vehicle Registration No")]
    public string Vehicle_Registration_No { get; set; } = string.Empty;

    [DisplayName("LoadOut No")]
    public string LoadOut_No { get; set; } = string.Empty;

    [DisplayName("Order No")]
    public string Order_No { get; set; } = string.Empty;

    [DisplayName("Cancelled Inv No")]
    public string Cancelled_Inv_No { get; set; } = string.Empty;

    [DisplayName("Customer Type")]
    public string Customer_Type { get; set; } = string.Empty;

    [DisplayName("eClaim Rate")]
    public decimal eClaim_Rate { get; set; }
}
