using System;
using System.ComponentModel;

namespace DBExporter.Models;

public class ClaimStatusReport
{
    [DisplayName("Dist GCC ID")]
    public string DistributorId { get; set; } = string.Empty;

    [DisplayName("Customer Name")]
    public string DistributorName { get; set; } = string.Empty;

    [DisplayName("Claim No")]
    public string ClaimNo { get; set; } = string.Empty;

    [DisplayName("Status")]
    public string Status { get; set; } = string.Empty;

    [DisplayName("Claim Year")]
    public string ClaimYear { get; set; } = string.Empty;

    [DisplayName("Top Sheet Status")]
    public string TopSheetStatus { get; set; } = string.Empty;

    [DisplayName("Finance Confirmation Date")]
    public string FinanceConfirmationDate { get; set; } = string.Empty;

    [DisplayName("Dist Signoff Date")]
    public string DistSignoffDate { get; set; } = string.Empty;

    [DisplayName("Sales Signoff Date")]
    public string SalesSignoffDate { get; set; } = string.Empty;

    [DisplayName("Finance Signoff Date")]
    public string FinanceSignoffDate { get; set; } = string.Empty;

    [DisplayName("dsd")]
    public string Dsd { get; set; } = string.Empty;

    [DisplayName("ssd")]
    public string Ssd { get; set; } = string.Empty;

    [DisplayName("fsd")]
    public string Fsd { get; set; } = string.Empty;

    [DisplayName("eClaim Amt")]
    public decimal EClaimAmt { get; set; }

    [DisplayName("Fin Confirmed By")]
    public string FinConfirmedBy { get; set; } = string.Empty;

    [DisplayName("Sales Signoff By")]
    public string SalesSignoffBy { get; set; } = string.Empty;

    [DisplayName("Fin Signoff By")]
    public string FinSignoffBy { get; set; } = string.Empty;
}
