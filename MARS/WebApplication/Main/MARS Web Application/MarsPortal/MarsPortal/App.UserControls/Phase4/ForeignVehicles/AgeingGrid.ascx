<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AgeingGrid.ascx.cs" Inherits="Mars.App.UserControls.Phase4.ForeignVehicles.AgeingGrid" %>



<%@ Register Src="~/App.UserControls/Phase4/HelpIcons/ExportToExcel.ascx" TagName="ExportToExcel" TagPrefix="uc" %>
<%@ Register Src="~/App.UserControls/Phase4/Common/NoDataScreen.ascx" TagName="NoDataScreen" TagPrefix="uc" %>


<table style="height: 355px; width: 800px; margin-right: auto; margin-left: auto;">
    <tr>
        <td colspan="2">
            <uc:NoDataScreen ID="ucNoData" runat="server" Visible="False" />
        </td>
    </tr>
    <tr style="vertical-align: top;">
        <td>
            <asp:Panel ID="pnlGrid" ScrollBars="Vertical" runat="server" Height="355px" Visible="False">
                <asp:GridView runat="server" ID="gvAgeing" CssClass="StandardBorder" AllowSorting="True"
                    AutoGenerateColumns="False" Width="800px" OnSorting="gvAgeing_Sorting">
                    <HeaderStyle CssClass="GridHeaderStyle" />
                    <RowStyle CssClass="GridRowStyle" />
                    <AlternatingRowStyle CssClass="GridAlterenatingRowStyle" />
                    <FooterStyle CssClass="GridAlterenatingRowStyle" />

                    <Columns>
                        <asp:BoundField DataField="Key" HeaderText="Key">
                            <ItemStyle HorizontalAlign="Left" CssClass="PadLeft" />
                        </asp:BoundField>
                        <asp:BoundField DataField="FleetCount" HeaderText="Fleet" SortExpression="FleetCount">
                            <ItemStyle HorizontalAlign="Right" CssClass="PadRight" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Group1" HeaderText="0-1 Days" DataFormatString="{0:#,0}"  SortExpression="Group1">
                            <ItemStyle HorizontalAlign="Right" CssClass="PadRight" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Group2" HeaderText="2-3 Days" DataFormatString="{0:#,0}" SortExpression="Group2">
                            <ItemStyle HorizontalAlign="Right" CssClass="PadRight" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Group3" HeaderText="4-5 Days" DataFormatString="{0:#,0}" SortExpression="Group3">
                            <ItemStyle HorizontalAlign="Right" CssClass="PadRight" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Group4" HeaderText="6-7 Days" DataFormatString="{0:#,0}" SortExpression="Group4">
                            <ItemStyle HorizontalAlign="Right" CssClass="PadRight" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Group5" HeaderText="8-10 Days" DataFormatString="{0:#,0}" SortExpression="Group5">
                            <ItemStyle HorizontalAlign="Right" CssClass="PadRight" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Group6" HeaderText="10-15 Days" DataFormatString="{0:#,0}" SortExpression="Group6">
                            <ItemStyle HorizontalAlign="Right" CssClass="PadRight" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Group7" HeaderText="16-30 Days" DataFormatString="{0:#,0}" SortExpression="Group7">
                            <ItemStyle HorizontalAlign="Right" CssClass="PadRight" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Group8" HeaderText="31+ Days" DataFormatString="{0:#,0}" SortExpression="Group8">
                            <ItemStyle HorizontalAlign="Right" CssClass="PadRight" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>

        </td>

    </tr>
    <tr>
        <td style="vertical-align: top; float: right;">
            <uc:ExportToExcel ID="ucExportToExcel" runat="server" Visible="False" />
        </td>
    </tr>
</table>
