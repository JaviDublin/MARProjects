<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VehicleOverview.ascx.cs" Inherits="Mars.App.UserControls.Phase4.ForeignVehicles.VehicleOverview" %>

<%@ Register Src="~/App.UserControls/Phase4/HelpIcons/ExportToExcel.ascx" TagName="ExportToExcel" TagPrefix="uc" %>

<asp:HiddenField runat="server" ID="hfColumnOneName"/>
<asp:HiddenField runat="server" ID="hfOtherColumnsName" />

<asp:HiddenField runat="server" ID="hfReservationsGrid" Value="False" />

<table>
    <tr>
        <td>
            <asp:GridView runat="server" ID="gvOverview" AutoGenerateColumns="False" 
                AllowSorting="True" ShowHeaderWhenEmpty="True"
                AllowPaging="False" Width="1000px" BorderStyle="None" 
                CssClass="StandardBorder">
                <HeaderStyle CssClass="StandardDataGridHeaderStyle" />
                <RowStyle CssClass="StandardDataGrid" Height="8px" />
                <PagerSettings Position="Bottom" />
                <PagerTemplate>
                </PagerTemplate>
                <HeaderStyle HorizontalAlign="Center" />
                <Columns></Columns>

            </asp:GridView>
        </td>
    </tr>
<tr>
    <td>
        <table style="width: 100%">
            <tr style="text-align: left; margin-top: 0;">
                <td style="float: right; text-align: center;">
                    <uc:ExportToExcel ID="ucExportToExcel" runat="server" />
                </td>
            </tr>
        </table>
    </td>
</tr>
</table>

