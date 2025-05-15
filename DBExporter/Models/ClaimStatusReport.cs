using System;
using System.ComponentModel;

namespace DBExporter.Models;

public class ClaimStatusReport
{
    [DisplayName("Dist GCC ID")]
    public string Dist_GCC_ID { get; set; } = string.Empty;

    [DisplayName("Customer Name")]
    public string Customer_Name { get; set; } = string.Empty;

    [DisplayName("Claim No")]
    public string ClaimNo { get; set; } = string.Empty;

    [DisplayName("Status")]
    public string Status { get; set; } = string.Empty;

    [DisplayName("Claim_No")]
    public string Claim_No { get; set; } = string.Empty;

    [DisplayName("Claim Year")]
    public int Claim_Year { get; set; }

    [DisplayName("Top Sheet Status")]
    public string Top_Sheet_Status { get; set; } = string.Empty;

    [DisplayName("Finance Confirmation Date")]
    public DateTime? Finance_Confirmation_Date { get; set; }

    [DisplayName("Dist Signoff Date")]
    public DateTime? Dist_Signoff_Date { get; set; }

    [DisplayName("Sales Signoff Date")]
    public DateTime? Sales_Signoff_Date { get; set; }

    [DisplayName("Finance Signoff Date")]
    public DateTime? Finance_Signoff_Date { get; set; }

    [DisplayName("dsd")]
    public DateTime? dsd { get; set; }

    [DisplayName("ssd")]
    public DateTime? ssd { get; set; }

    [DisplayName("fsd")]
    public DateTime? fsd { get; set; }

    [DisplayName("eClaim Amt")]
    public decimal eClaim_Amt { get; set; }

    [DisplayName("Fin Confirmed By")]
    public string Fin_Confirmed_By { get; set; } = string.Empty;

    [DisplayName("Sales Signoff By")]
    public string Sales_Signoff_By { get; set; } = string.Empty;

    [DisplayName("Fin Signoff By")]
    public string Fin_Signoff_By { get; set; } = string.Empty;
}
