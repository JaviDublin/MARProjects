<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ComparisonGrid.ascx.cs" Inherits="Mars.App.UserControls.Phase4.NonRev.ComparisonGrid" %>

<%@ Register Src="~/App.UserControls/Phase4/HelpIcons/ExportToExcel.ascx" TagName="ExportToExcel" TagPrefix="uc" %>

<table style="height: 355px;" >
    <tr style="vertical-align: top;">
        <td>
            <asp:GridView runat="server" ID="gvOverview" CssClass="insetBorders"
                AutoGenerateColumns="False" Width="800px">
                <HeaderStyle CssClass="GridHeaderStyle" />
                <RowStyle CssClass="GridRowStyle" />
                <AlternatingRowStyle CssClass="GridAlterenatingRowStyle" />
                <FooterStyle CssClass="GridAlterenatingRowStyle" />

                <Columns>
                    <asp:BoundField DataField="Key" HeaderText="Key" >
                        <ItemStyle HorizontalAlign="Left" CssClass="PadLeft" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FleetCount" HeaderText="Total Fleet" DataFormatString="{0:#,0}" >
                        <ItemStyle HorizontalAlign="Right" CssClass="PadRight" />
                    </asp:BoundField>
                    <asp:BoundField DataField="NonRevCount" HeaderText="Total Non Rev" DataFormatString="{0:#,0}" >
                        <ItemStyle HorizontalAlign="Right" CssClass="PadRight" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PercentNonRev" HeaderText="% Non Rev" DataFormatString="{0:p}" >
                        <ItemStyle HorizontalAlign="Right" CssClass="PadRight" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PercentNonRevOfTotalNonRev" HeaderText="% Non Rev Total" DataFormatString="{0:p}" >
                        <ItemStyle HorizontalAlign="Right" CssClass="PadRight" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ReasonsEntered" HeaderText="Reasons Entered" DataFormatString="{0:#,0}" >
                        <ItemStyle HorizontalAlign="Right" CssClass="PadRight" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PercentNonRevReasonsEntered" HeaderText="% Reasons Entered" DataFormatString="{0:p}" >
                        <ItemStyle HorizontalAlign="Right" CssClass="PadRight" />
                    </asp:BoundField>
                </Columns>

            </asp:GridView>
        </td>
        <td style="vertical-align: top;">
            <uc:ExportToExcel ID="ucExportToExcel" runat="server" />
            
            
        </td>
    </tr>
</table>
