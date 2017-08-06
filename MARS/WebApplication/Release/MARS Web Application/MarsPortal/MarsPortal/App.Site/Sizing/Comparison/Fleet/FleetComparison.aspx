<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/App.Masterpages/Application.Master"
    CodeBehind="FleetComparison.aspx.cs" Inherits="App.Reports.Sizing.FleetComparison" %>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="ContentPlaceholderMainContent">
    <asp:UpdatePanel ID="upChart" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <table width="950px">
                <tr>
                    <td>
                        <asp:Menu ID="menuFleetComparisonQuickNav" runat="server" Orientation="Horizontal"
                            CssClass="QuickNavMenu" OnMenuItemClick="SiteComparisonMenu_MenuItemClick" />
                    </td>
                </tr>
            </table>
            <uc:ChartControl ID="ccSiteComparison" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upnlGeneralParameters" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <uc:GeneralReportParameters ID="GeneralParams" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress runat="server" ID="upChartProgress"  DisplayAfter="1000">
        <ProgressTemplate>
            <uc:LoadingScreen ID="clsLoadingScreen" runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>

</asp:Content>
