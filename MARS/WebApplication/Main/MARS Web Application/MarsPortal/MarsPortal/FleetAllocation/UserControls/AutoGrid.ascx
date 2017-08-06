<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AutoGrid.ascx.cs" Inherits="Mars.FleetAllocation.UserControls.AutoGrid" %>
<%@ Register Src="~/App.UserControls/Phase4/HelpIcons/ExportToExcel.ascx" TagPrefix="uc" TagName="ExportToExcel" %>




<asp:UpdatePanel runat="server" ID="upnlGrid" UpdateMode="Conditional">
    <ContentTemplate>
        
        <asp:HiddenField runat="server" id="hfHideLastColumn" Value="False"/>
        <asp:HiddenField runat="server" id="hfExportDataFileName" Value=""/>
        
        <asp:HiddenField runat="server" id="hfHighlightedLineId" Value=""/>

        <asp:Panel runat="server" HorizontalAlign="Justify" ID="pnlGrid" Width="100%">
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:GridView runat="server" Width="950px" ID="gvAutoGrid" HorizontalAlign="Right" AllowPaging="True"
                            AutoGenerateColumns="True" AllowSorting="True" OnSorting="Gridview_Sorting" 
                            OnRowDataBound="gvAutoGrid_DataRowBound">
                            <HeaderStyle CssClass="StandardDataGridHeaderStyle" />
                            <RowStyle CssClass="StandardDataGrid" Height="8px" />
                            <PagerTemplate>
                            </PagerTemplate>
                        </asp:GridView>
                    </td>
                    <td style="vertical-align: bottom; text-align: center;" rowspan="2">
                        <uc:ExportToExcel runat="server" ID="ucSideExportToExcel" Visible="False" OnExportData="ExportToExcel" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlPager" runat="server">

                        <table style="width: 100%; background-color: white;">
                            <tr>
                                <td style="text-align: left; margin-top: 0;">
                                    <asp:Label runat="server" ID="lblRowCount" />
                                </td>
                                <td style="float: right;">Page Size:
                                <asp:DropDownList runat="server" ID="ddlPageSize" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlPageSize_SizeChange">
                                    <asp:ListItem Text="10" Value="10" />
                                    <asp:ListItem Text="20" Value="20" />
                                    <asp:ListItem Text="30" Value="30" />
                                    <asp:ListItem Text="40" Value="40" />
                                    <asp:ListItem Text="50" Value="50" />
                                    <asp:ListItem Text="100" Value="100" />
                                    <asp:ListItem Text="200" Value="200" />
                                    <asp:ListItem Text="500" Value="500" />
                                </asp:DropDownList>
                                    <asp:ImageButton runat="server" ID="lbgvFirst" ImageUrl="~/App.Images/pager-first.png"
                                        OnClick="gvFirstButton_Click" />
                                    <asp:ImageButton runat="server" ID="lbgvPrevious" ImageUrl="~/App.Images/pager-previous.png"
                                        OnClick="gvPreviousButton_Click" />
                                    <asp:Label runat="server" ID="lblPageAt" Text="Page 1 of 1" />
                                    <asp:ImageButton runat="server" ID="lbgvNext" ImageUrl="~/App.Images/pager-next.png"
                                        OnClick="gvNextButton_Click" />
                                    <asp:ImageButton runat="server" ID="lbgvLast" ImageUrl="~/App.Images/pager-last.png"
                                        OnClick="gvLastButton_Click" />
                                </td>
                            </tr>
                        </table>
                         </asp:Panel>
                    </td>

                </tr>
                <tr>
                    <td style="float:right; text-align: center;">
                        <uc:ExportToExcel runat="server" ID="ucBottomExportToExcel" Visible="False" OnExportData="ExportToExcel" />
                    </td>
                </tr>
            </table>


        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>



