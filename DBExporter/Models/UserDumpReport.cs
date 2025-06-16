using System;
using System.ComponentModel;

namespace DBExporter.Models;

public class UserDumpReport
{
    [DisplayName("Customer ID")]
    public string Customer_ID { get; set; } = string.Empty;

    [DisplayName("SAP Customer ID")]
    public string SAP_Customer_ID { get; set; } = string.Empty;

    [DisplayName("Customer Name")]
    public string Customer_Name { get; set; } = string.Empty;

    [DisplayName("Customer Category ID")]
    public string Customer_Category_ID { get; set; } = string.Empty;

    [DisplayName("Customer Category Desc")]
    public string Customer_Category_Desc { get; set; } = string.Empty;

    [DisplayName("Customer Class ID")]
    public string Customer_Class_ID { get; set; } = string.Empty;

    [DisplayName("Customer Class Desc")]
    public string Customer_Class_Desc { get; set; } = string.Empty;

    [DisplayName("Entity ID")]
    public string Entity_ID { get; set; } = string.Empty;

    [DisplayName("Entity Name")]
    public string Entity_Name { get; set; } = string.Empty;

    [DisplayName("Admin Entity ID")]
    public string Admin_Entity_ID { get; set; } = string.Empty;

    [DisplayName("Admin Entity Name")]
    public string Admin_Entity_Name { get; set; } = string.Empty;

    [DisplayName("Region ID")]
    public string Region_ID { get; set; } = string.Empty;

    [DisplayName("Region Name")]
    public string Region_Name { get; set; } = string.Empty;

    [DisplayName("Site ID")]
    public string Site_ID { get; set; } = string.Empty;

    [DisplayName("Site Name")]
    public string Site_Name { get; set; } = string.Empty;

    [DisplayName("Market Area")]
    public string Market_Area { get; set; } = string.Empty;

    [DisplayName("MarketArea Territory")]
    public string MarketArea_Territory { get; set; } = string.Empty;

    [DisplayName("MD Name")]
    public string MD_Name { get; set; } = string.Empty;

    [DisplayName("Active")]
    public string Active { get; set; } = string.Empty;

    [DisplayName("Order Block")]
    public string Order_Block { get; set; } = string.Empty;

    [DisplayName("Customer Activated Date")]
    public string Customer_Activated_Date { get; set; } = string.Empty;

    [DisplayName("Address 1")]
    public string Address_1 { get; set; } = string.Empty;

    [DisplayName("Address 2")]
    public string Address_2 { get; set; } = string.Empty;

    [DisplayName("Address 3")]
    public string Address_3 { get; set; } = string.Empty;

    [DisplayName("Pin Code")]
    public string Pin_Code { get; set; } = string.Empty;

    [DisplayName("Town")]
    public string Town { get; set; } = string.Empty;

    [DisplayName("District")]
    public string District { get; set; } = string.Empty;

    [DisplayName("State Name")]
    public string State_Name { get; set; } = string.Empty;

    [DisplayName("PIN Code")]
    public string PIN_Code { get; set; } = string.Empty;

    [DisplayName("Home Address 1")]
    public string Home_Address_1 { get; set; } = string.Empty;

    [DisplayName("Home Address 2")]
    public string Home_Address_2 { get; set; } = string.Empty;

    [DisplayName("Home Address 3")]
    public string Home_Address_3 { get; set; } = string.Empty;

    [DisplayName("Pin Code")]
    public string Home_Pin_Code { get; set; } = string.Empty;

    [DisplayName("Town")]
    public string Home_Town { get; set; } = string.Empty;

    [DisplayName("District")]
    public string Home_District { get; set; } = string.Empty;

    [DisplayName("Pincode")]
    public string Pincode { get; set; } = string.Empty;

    [DisplayName("Contact Person")]
    public string Contact_Person { get; set; } = string.Empty;

    [DisplayName("Telephone No")]
    public string Telephone_No { get; set; } = string.Empty;

    [DisplayName("Mobile No")]
    public string Mobile_No { get; set; } = string.Empty;

    [DisplayName("Email ID")]
    public string Email_ID { get; set; } = string.Empty;

    [DisplayName("Distributor Route ID")]
    public string Distributor_Route_ID { get; set; } = string.Empty;

    [DisplayName("Parent code")]
    public string Parent_Code { get; set; } = string.Empty;

    [DisplayName("Customer Name")]
    public string Customer_Name_2 { get; set; } = string.Empty;

    [DisplayName("SE ID")]
    public string SE_ID { get; set; } = string.Empty;

    [DisplayName("SE Territory")]
    public string SE_Territory { get; set; } = string.Empty;

    [DisplayName("SE Name")]
    public string SE_Name { get; set; } = string.Empty;

    [DisplayName("ASM ID")]
    public string ASM_ID { get; set; } = string.Empty;

    [DisplayName("ASM Territory")]
    public string ASM_Territory { get; set; } = string.Empty;

    [DisplayName("ASM Name")]
    public string ASM_Name { get; set; } = string.Empty;

    [DisplayName("SM_ID")]
    public string SM_ID { get; set; } = string.Empty;

    [DisplayName("SM Territory")]
    public string SM_Territory { get; set; } = string.Empty;

    [DisplayName("SM Name")]
    public string SM_Name { get; set; } = string.Empty;

    [DisplayName("Channel ID")]
    public string Channel_ID { get; set; } = string.Empty;

    [DisplayName("Channel Desc")]
    public string Channel_Desc { get; set; } = string.Empty;

    [DisplayName("Sub Channel ID")]
    public string Sub_Channel_ID { get; set; } = string.Empty;

    [DisplayName("Sub Channel Desc")]
    public string Sub_Channel_Desc { get; set; } = string.Empty;

    [DisplayName("CSS Cluster Desc")]
    public string CSS_Cluster_Desc { get; set; } = string.Empty;

    [DisplayName("RSA Cluster Desc")]
    public string RSA_Cluster_Desc { get; set; } = string.Empty;

    [DisplayName("VPO Class Desc")]
    public string VPO_Class_Desc { get; set; } = string.Empty;

    [DisplayName("NKA Customer ID")]
    public string NKA_Customer_ID { get; set; } = string.Empty;

    [DisplayName("NKA Customer Name")]
    public string NKA_Customer_Name { get; set; } = string.Empty;

    [DisplayName("Pricelist Code")]
    public string Pricelist_Code { get; set; } = string.Empty;

    [DisplayName("Cheque Allowed")]
    public string Cheque_Allowed { get; set; } = string.Empty;

    [DisplayName("Open Market Discount")]
    public string Open_Market_Discount { get; set; } = string.Empty;

    [DisplayName("Payment Mode")]
    public string Payment_Mode { get; set; } = string.Empty;

    [DisplayName("V2020 Status")]
    public string V2020_Status { get; set; } = string.Empty;

    [DisplayName("V2020 Activation Date")]
    public string V2020_Activation_Date { get; set; } = string.Empty;

    [DisplayName("V2020 Deactivation Date")]
    public string V2020_Deactivation_Date { get; set; } = string.Empty;

    [DisplayName("HDO Status")]
    public string HDO_Status { get; set; } = string.Empty;

    [DisplayName("HDO Activation Date")]
    public string HDO_Activation_Date { get; set; } = string.Empty;

    [DisplayName("HDO Deactivation Date")]
    public string HDO_Deactivation_Date { get; set; } = string.Empty;

    [DisplayName("Cola Customer Code")]
    public string Cola_Customer_Code { get; set; } = string.Empty;

    [DisplayName("Vehicle Type ID")]
    public string Vehicle_Type_ID { get; set; } = string.Empty;

    [DisplayName("Latitude")]
    public decimal Latitude { get; set; }

    [DisplayName("Longitude")]
    public decimal Longitude { get; set; }

    [DisplayName("FSSAI No")]
    public string FSSAI_No { get; set; } = string.Empty;

    [DisplayName("FSSAI DATE")]
    public string FSSAI_Date { get; set; } = string.Empty;

    [DisplayName("Agreement Exp Date")]
    public string Agreement_Exp_Date { get; set; } = string.Empty;

    [DisplayName("LST_Number")]
    public string LST_Number { get; set; } = string.Empty;

    [DisplayName("CST_No")]
    public string CST_No { get; set; } = string.Empty;

    [DisplayName("Lead_Time")]
    public string Lead_Time { get; set; } = string.Empty;

    [DisplayName("Distance")]
    public decimal Distance { get; set; }

    [DisplayName("AR_Credit_Limit")]
    public decimal AR_Credit_Limit { get; set; }

    [DisplayName("Col_Limit")]
    public decimal Col_Limit { get; set; }

    [DisplayName("Credit_Days")]
    public string Credit_Days { get; set; } = string.Empty;

    [DisplayName("Cust_Cond_GRP1")]
    public string Cust_Cond_GRP1 { get; set; } = string.Empty;

    [DisplayName("AutoKnockingOff")]
    public string AutoKnockingOff { get; set; } = string.Empty;

    [DisplayName("Recon_Accoount")]
    public string Recon_Account { get; set; } = string.Empty;

    [DisplayName("Payment_Terms")]
    public string Payment_Terms { get; set; } = string.Empty;

    [DisplayName("Block_for_Payment")]
    public string Block_for_Payment { get; set; } = string.Empty;

    [DisplayName("House_Bank")]
    public string House_Bank { get; set; } = string.Empty;

    [DisplayName("BANK_KEY")]
    public string BANK_KEY { get; set; } = string.Empty;

    [DisplayName("Reference_Details_Virtual_Code")]
    public string Reference_Details_Virtual_Code { get; set; } = string.Empty;

    [DisplayName("Pricing_Procedure")]
    public string Pricing_Procedure { get; set; } = string.Empty;

    [DisplayName("Sales_District")]
    public string Sales_District { get; set; } = string.Empty;

    [DisplayName("Accnt_Assign_Grp")]
    public string Accnt_Assign_Grp { get; set; } = string.Empty;

    [DisplayName("Max_Partial_Delivery_Allowed")]
    public string Max_Partial_Delivery_Allowed { get; set; } = string.Empty;

    [DisplayName("PrintOption")]
    public string PrintOption { get; set; } = string.Empty;
}
