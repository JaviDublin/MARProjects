<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ComparisonGrid.ascx.cs" 
    Inherits="Mars.App.UserControls.Phase4.Availability.ComparisonGrid" %>
<%@ Register TagPrefix="uc" TagName="ExportToExcel" Src="~/App.UserControls/Phase4/HelpIcons/ExportToExcel.ascx" %>

<asp:HiddenField runat="server" ID="hfPercentageCalculation" Value="False" />

<div style="width: 1070px; overflow: scroll">


    <table style="height: 355px; text-align: center;" >
        <tr style="vertical-align: top;">
            <td>
                <table style="text-align: center; margin-left: auto; margin-right: auto;">
                    <tr>
                        <td>
                            <uc:ExportToExcel ID="ucExportToExcel" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView runat="server" ID="gvComparison" CssClass="StandardBorder" 
                                OnRowDataBound="gvComparison_RowDataBound"
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