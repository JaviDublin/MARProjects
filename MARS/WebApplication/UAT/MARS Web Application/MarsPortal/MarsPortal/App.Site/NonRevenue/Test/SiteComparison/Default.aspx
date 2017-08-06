<%@ Page Title="" Language="C#" MasterPageFile="~/App.MasterPages/Application.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="App.Site.NonRevenue.Comparison.Site.Test.Default" %>
<asp:Content ID="ContentHead" ContentPlaceHolderID="ContentPlaceholderHead" runat="server">
</asp:Content>
<asp:Content ID="ContentMain" ContentPlaceHolderID="ContentPlaceholderMainContent" runat="server">
<asp:UpdatePanel ID="upChart" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                 <table width="950px">
                <tr>
                    <td>
                        <asp:Menu ID="menuSiteComparisonQuickNav" runat="server" Orientation="Horizontal"
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

      <asp:UpdateProgress ID="UpdateProgress1" runat="server" ClientIDMode="Static" DisplayAfter="1000">
        <ProgressTemplate>
            <asp:Panel ID="PanelBackgroundCover" runat="server" CssClass="backgroundCover"></asp:Panel>
            <asp:Panel ID="PanelLoadData" runat="server" CssClass="loadData">
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;
                Loading Data.....
                <br />
                <asp:Image ID="Image1" runat="server" CssClass="loadDataImage" ImageUrl="~/App.Images/ajax-loader.gif" AlternateText="Please wait..." />
            </asp:Panel>
        </ProgressTemplate>        
    </asp:UpdateProgress>
</asp:Content>
