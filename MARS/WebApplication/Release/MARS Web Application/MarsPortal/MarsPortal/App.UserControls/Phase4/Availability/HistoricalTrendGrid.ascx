<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HistoricalTrendGrid.ascx.cs" Inherits="Mars.App.UserControls.Phase4.Availability.HistoricalTrendGrid" %>

<%@ Register Src="~/App.UserControls/Phase4/HelpIcons/ExportToExcel.ascx" TagName="ExportToExcel" TagPrefix="uc" %>

<asp:HiddenField runat="server" ID="hfPercentageCalculation" Value="False" />

<asp:HiddenField runat="server" ID="hfHourlySeries" Value="False" />
<div style="width: 950px; height: 355px; overflow: scroll">
    <table style="height: 355px; text-align: center;" width="100%">
        <tr style="vertical-align: top;">
            <td>
                <table style="text-align: center; margin-left: auto; margin-right: auto;">
                    <tr>
                        <td style="vertical-align: top;">
                            <uc:ExportToExcel ID="ucExportToExcel" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView runat="server" ID="gvHistoricalTrend" CssClass="StandardBorder" OnRowDataBound="gvHistoricalTrend_RowDataBound"
                                AutoGenerateColumns="True" Width="1200px" HorizontalAlign="Center">
                                <HeaderStyle CssClass="GridHeaderStyle" />
                                <RowStyle CssClass="GridRowStyle" HorizontalAlign="Right" />
                                <AlternatingRowStyle CssClass="GridAlterenatingRowStyle" />
                                <FooterStyle CssClass="GridAlterenatingRowStyle" />
                            </asp:GridView>
                        </td>

                    </tr>
                </table>

            </td>
        </tr>
    </table>
</div>
