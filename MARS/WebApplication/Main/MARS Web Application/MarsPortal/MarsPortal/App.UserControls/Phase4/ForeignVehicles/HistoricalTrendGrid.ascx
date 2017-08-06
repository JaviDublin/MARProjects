<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HistoricalTrendGrid.ascx.cs" Inherits="Mars.App.UserControls.Phase4.ForeignVehicles.HistoricalTrendGrid" %>


<%@ Register Src="~/App.UserControls/Phase4/HelpIcons/ExportToExcel.ascx" TagName="ExportToExcel" TagPrefix="uc" %>
<%@ Register Src="~/App.UserControls/Phase4/Common/NoDataScreen.ascx" TagName="NoDataScreen" TagPrefix="uc" %>

<table style="height: 355px; width: 1000px;" >
    <tr>
        <td >
            <uc:NoDataScreen ID="ucNoData" runat="server" Visible="False" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:Panel ID="pnlGrid" ScrollBars="Both" runat="server" Height="355px" Width="950px" Visible="False">

                <asp:GridView runat="server" ID="gvHistoricalTrend" CssClass="StandardBorder" HorizontalAlign="Center"
                    Width="100%" OnRowDataBound="gvHistoricalTrend_DataBound"
                    AutoGenerateColumns="True">
                    <HeaderStyle CssClass="GridHeaderStyle SpacedHeader" />
                    <RowStyle CssClass="GridRowStyle" />
                </asp:GridView>

            </asp:Panel>

        </td>

    </tr>
    <tr >
        <td style="float: right;">
            
            <uc:ExportToExcel ID="ucExportToExcel" runat="server" Visible="False" />

        </td>
    </tr>
</table>
