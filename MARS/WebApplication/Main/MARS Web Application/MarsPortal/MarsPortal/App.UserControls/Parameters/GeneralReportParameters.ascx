<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GeneralReportParameters.ascx.cs"
    Inherits="App.UserControls.Parameters.GeneralReportParameters" %>
<%@ Register Src="~/App.UserControls/Parameters/NonRevenueParameters.ascx" TagPrefix="uc"
    TagName="NonRevenueParams" %>
<div style="width: 100%;">
    <center>
        <table id="tblGeneralParams" runat="server" border="0" width="935px" cellpadding="0"
            cellspacing="0">
            <tr>
                <td>
                    <asp:Menu runat="server" ID="menuReportsSelection" Orientation="Horizontal" OnMenuItemClick="menuReportsSelection_MenuItemClick"
                        StaticMenuItemStyle-Font-Size="14px" StaticMenuItemStyle-ForeColor="Black" StaticMenuItemStyle-BorderStyle="Outset"
                        StaticMenuItemStyle-BorderColor="AliceBlue" StaticMenuItemStyle-HorizontalPadding="10"
                        StaticSelectedStyle-BackColor="#FFF985" StaticSelectedStyle-BorderStyle="Inset"
                        BackColor="White">
                        <Items>
                            <asp:MenuItem Text="<%$ Resources:LocalizedParameterControl, MenuItemChartParameters %>"
                                Value="0" Selected="true" />
                            <asp:MenuItem Text="<%$ Resources:LocalizedParameterControl, MenuItemExportData %>"
                                Value="1" />
                        </Items>
                    </asp:Menu>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:MultiView runat="server" ID="mvReportsSelection">
                        <asp:View ID="ChartTab" runat="server">
                            <asp:Panel ID="pnlChart" runat="server" CssClass="option-container" BackColor="White">
                                <asp:Table runat="server" HorizontalAlign="Left">
                                    <asp:TableRow>
                                        <asp:TableCell ID="tcSelectionReport" VerticalAlign="Top">
                                                <uc:ReportType ID="rtSelectionReport" runat="server" />
                                                <uc:NonRevenueParams ID="ucNonRevParams" runat="server" />
                                        </asp:TableCell>
                                        <asp:TableCell runat="server" ID="tcDynamParams">
                                            <uc:DynamicParameters ID="DynamicParameters" runat="server" />
                                        </asp:TableCell>
                                        <asp:TableCell runat="server" ID="tcExcelParams">
                                            <uc:ExcelExport ID="eeExcelExport" runat="server" />
                                        </asp:TableCell>

                                    </asp:TableRow>
                                </asp:Table>
                            </asp:Panel>
                        </asp:View>
                    </asp:MultiView>
                </td>
            </tr>
        </table>
    </center>
</div>
