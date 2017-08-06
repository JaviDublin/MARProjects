<%@ Page Title="" Language="C#" MasterPageFile="~/App.MasterPages/Fao.Master" AutoEventWireup="true" 
    CodeBehind="DemandGapDisplay.aspx.cs" Inherits="Mars.FleetAllocation.Webpages.DemandGapDisplay" %>

<%@ Register Src="~/FleetAllocation/UserControls/DemandGapDisplay.ascx" TagPrefix="uc" TagName="DemandGapDisplay" %>





<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceholderHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceholderMainContent" runat="server">
    
    <uc:DemandGapDisplay runat="server" id="ucDemandGapDisplay" />
        
</asp:Content>
