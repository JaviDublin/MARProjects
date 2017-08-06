<%@ Page Title="" Language="C#" MasterPageFile="~/App.MasterPages/Fao.Master" AutoEventWireup="true" 
    CodeBehind="VehicleDistribution.aspx.cs" Inherits="Mars.FleetAllocation.Webpages.VehicleDistribution" %>

<%@ Register Src="~/FleetAllocation/UserControls/VehicleDistribution.ascx" TagPrefix="uc" TagName="VehicleDistribution" %>





<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceholderHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceholderMainContent" runat="server">
    <uc:VehicleDistribution runat="server" ID="ucVehicleDistribution" />    
</asp:Content>
