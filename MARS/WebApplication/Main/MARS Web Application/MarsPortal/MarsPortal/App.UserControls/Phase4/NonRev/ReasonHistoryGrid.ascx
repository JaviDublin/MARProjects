<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReasonHistoryGrid.ascx.cs"
    Inherits="Mars.App.UserControls.Phase4.NonRev.ReasonHistoryGrid" %>

<table style="height: 410px;">
    <tr style="vertical-align: top;">
        <td>
            <asp:GridView runat="server" ID="gvOverview" CssClass="StandardBorder" ShowHeaderWhenEmpty="True"
                AutoGenerateColumns="False" Width="800px">
                <HeaderStyle CssClass="GridHeaderStyle" />
                <RowStyle CssClass="GridRowStyle" />
                <AlternatingRowStyle CssClass="GridAlterenatingRowStyle" />
                <FooterStyle CssClass="GridAlterenatingRowStyle" />

                <Columns>
                    <asp:BoundField DataField="Key" HeaderText="Date" >
                        <ItemStyle HorizontalAlign="Right" CssClass="PadRight" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FleetCount" HeaderText="Total Fleet" DataFormatString="{0:#,0}" >
                        <ItemStyle HorizontalAlign="Right" CssClass="PadRight" />
                    </asp:BoundField>
                    <asp:BoundField DataField="NonRevCount" HeaderText="Total Non Rev" DataFormatString="{0:#,0}" >
                        <ItemStyle HorizontalAlign="Right" CssClass="PadRight" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PercentNonRev" HeaderText="Non Rev Total %" DataFormatString="{0:p}" >
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
            <asp:ImageButton runat="server" ID="ibExportToExcel" ImageUrl="~/App.Images/Excel.jpg" AlternateText="Export to Excel" CssClass="PlainDataExportButton"
                OnClick="ibExportToExcel_Click" />
        </td>
    </tr>
</table>
