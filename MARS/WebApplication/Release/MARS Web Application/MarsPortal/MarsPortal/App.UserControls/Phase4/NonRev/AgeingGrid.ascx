<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AgeingGrid.ascx.cs" Inherits="Mars.App.UserControls.Phase4.NonRev.AgeingGrid" %>

<%@ Register Src="~/App.UserControls/Phase4/HelpIcons/ExportToExcel.ascx" TagName="ExportToExcel" TagPrefix="uc" %>

<table style="height: 355px;">
    <tr style="vertical-align: top;">
        <td >
            <asp:GridView runat="server" ID="gvAgeing" CssClass="StandardBorder"
                AutoGenerateColumns="False" Width="800px">
                <HeaderStyle CssClass="GridHeaderStyle" />
                <RowStyle CssClass="GridRowStyle" />
                <AlternatingRowStyle CssClass="GridAlterenatingRowStyle" />
                <FooterStyle CssClass="GridAlterenatingRowStyle" />

                <Columns>
                    <asp:BoundField DataField="Key" HeaderText="Key" >
                        <ItemStyle HorizontalAlign="Left" CssClass="PadLeft" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FleetCount" HeaderText="Fleet" DataFormatString="{0:#,0}" Visible="False" >
                        <ItemStyle HorizontalAlign="Right" CssClass="PadRight" />
                    </asp:BoundField>
                    <asp:BoundField DataField="NonRevCount" HeaderText="Non Rev" DataFormatString="{0:#,0}" >
                        <ItemStyle HorizontalAlign="Right" CssClass="PadRight" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PercentOfTotalFleet" HeaderText="% of Total Fleet" DataFormatString="{0:p}" Visible="False" >
                        <ItemStyle HorizontalAlign="Right" CssClass="PadRight" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PercentNonRevOfTotalNonRev" HeaderText="% of Non Rev Total" DataFormatString="{0:p}" >
                        <ItemStyle HorizontalAlign="Right" CssClass="PadRight" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Group1" HeaderText="0-2 Days" DataFormatString="{0:#,0}" >
                        <ItemStyle HorizontalAlign="Right" CssClass="PadRight" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Group2" HeaderText="3 Days" DataFormatString="{0:#,0}" >
                        <ItemStyle HorizontalAlign="Right" CssClass="PadRight" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Group3" HeaderText="4-6 Days" DataFormatString="{0:#,0}" >
                        <ItemStyle HorizontalAlign="Right" CssClass="PadRight" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Group4" HeaderText="7 Days" DataFormatString="{0:#,0}" >
                        <ItemStyle HorizontalAlign="Right" CssClass="PadRight" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Group5" HeaderText="8-10 Days" DataFormatString="{0:#,0}" >
                        <ItemStyle HorizontalAlign="Right" CssClass="PadRight" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Group6" HeaderText="11-15 Days" DataFormatString="{0:#,0}" >
                        <ItemStyle HorizontalAlign="Right" CssClass="PadRight" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Group7" HeaderText="16-30 Days" DataFormatString="{0:#,0}" >
                        <ItemStyle HorizontalAlign="Right" CssClass="PadRight" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Group8" HeaderText="31-60 Days" DataFormatString="{0:#,0}" >
                        <ItemStyle HorizontalAlign="Right" CssClass="PadRight" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Group9" HeaderText="60+ Days" DataFormatString="{0:#,0}" >
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






