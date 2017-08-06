<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/App.Masterpages/Application.Master"
    CodeBehind="SupplyAnalysis.aspx.cs" Inherits="App.Reports.Sizing.SupplyAnalysis" %>

<%@ Register Src="~/App.UserControls/Parameters/GeneralReportParameters.ascx" TagPrefix="uc"
    TagName="GeneralReportParameters" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="ContentPlaceholderMainContent">
        <asp:UpdatePanel ID="upChart" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <uc:ChartControl ID="ccSuplyAnalysis" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="upnlGeneralParameters" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <uc:GeneralReportParameters ID="GeneralParams" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdateProgress runat="server" ID="upChartProgress" DisplayAfter="1000">
            <ProgressTemplate>
                <uc:LoadingScreen ID="clsLoadingScreen" runat="server" />
            </ProgressTemplate>
        </asp:UpdateProgress>


</asp:Content>
