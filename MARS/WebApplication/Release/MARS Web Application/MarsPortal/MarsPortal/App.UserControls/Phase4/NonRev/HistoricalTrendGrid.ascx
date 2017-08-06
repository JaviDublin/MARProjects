<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HistoricalTrendGrid.ascx.cs" Inherits="Mars.App.UserControls.Phase4.NonRev.HistoricalTrendGrid" %>

<%@ Register Src="~/App.UserControls/Phase4/HelpIcons/ExportToExcel.ascx" TagName="ExportToExcel" TagPrefix="uc" %>

<asp:Panel runat="server" Height="355px" ScrollBars="Both">
    <table >
        <tr style="vertical-align: top;">
            <td>
                <asp:GridView runat="server" ID="gvHistoricalTrend" CssClass="StandardBorder" ShowHeaderWhenEmpty="True"
                    AutoGenerateColumns="True" Width="800px">
                    <HeaderStyle CssClass="GridHeaderStyle" />
                    <RowStyle CssClass="GridRowStyle" HorizontalAlign="Right"></RowStyle>
                    <AlternatingRowStyle CssClass="GridAlterenatingRowStyle" />
                    <FooterStyle CssClass="GridAlterenatingRowStyle" />
                </asp:GridView>
            </td>
            <td style="vertical-align: top;">
                <uc:ExportToExcel ID="ucExportToExcel" runat="server" />

            </td>
        </tr>
    </table>

</asp:Panel>
