using System;
using System.ComponentModel;

namespace DBExporter.Models;

public class SalesReportItem
{
    [DisplayName("ASM ID")]
    public string ASM_ID { get; set; } = string.Empty;

    [DisplayName("ASM Name")]
    public string ASM_Name { get; set; } = string.Empty;

    [DisplayName("ASM Territory")]
    public string ASM_Territory { get; set; } = string.Empty;

    [DisplayName("ASM Mobile No")]
    public string ASM_Mobile_No { get; set; } = string.Empty;

    [DisplayName("ASM KO ID")]
    public string ASM_KO_ID { get; set; } = string.Empty;

    [DisplayName("SE ID")]
    public string SE_ID { get; set; } = string.Empty;

    [DisplayName("SE Name")]
    public string SE_Name { get; set; } = string.Empty;

    [DisplayName("SE Territory")]
    public string SE_Territory { get; set; } = string.Empty;

    [DisplayName("SE Mobile No")]
    public string SE_Mobile_No { get; set; } = string.Empty;

    [DisplayName("SE KO ID")]
    public string SE_KO_ID { get; set; } = string.Empty;
}
