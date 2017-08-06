<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EntityGrid.ascx.cs" Inherits="Mars.App.UserControls.Phase4.Administration.Mapping.EntityGrid" %>

<%@ Register Src="~/App.UserControls/Phase4/HelpIcons/ExportToExcel.ascx" TagName="ExportToExcel" TagPrefix="uc" %>

<asp:HiddenField runat="server" ID="hfEntityType" />
<asp:HiddenField runat="server" ID="hfCurrentGvPage" Value="1" />
<asp:HiddenField runat="server" ID="hfColumnNames" Value="" />

<table>
    <tr>
        <td>
            <asp:Panel ID="Panel1" runat="server" ScrollBars="Vertical" Width="970px">
                <asp:GridView runat="server" ID="gvEntityGrid" Width="950px" AutoGenerateColumns="False"
                    AllowSorting="True"  OnSorting="Gridview_Sorting"
                    AllowPaging="True" BorderStyle="None"  ShowHeaderWhenEmpty="True" PageSize="20"
                    CssClass="StandardBorder">
                    <HeaderStyle CssClass="StandardDataGridHeaderStyle" />
                    <RowStyle CssClass="StandardDataGrid" Height="8px" />
                    <PagerSettings Position="Bottom" />
                    <PagerTemplate>
                    </PagerTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <Columns></Columns>
                </asp:GridView>
            </asp:Panel>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Panel runat="server" ID="pnlPager">
                <table style="width: 100%; background-color: white;">
                    <tr>
                        <td style="text-align: left; margin-top: 0;">
                            <asp:Label runat="server" ID="lblRowCount" />
                        </td>

                        <td style="float: right;">Page Size:
                                <asp:DropDownList runat="server" ID="ddlPageSize" AutoPostBack="True" OnSelectedIndexChanged="ddlPageSize_SizeChange">
                                    <asp:ListItem Text="10" Value="10" />
                                    <asp:ListItem Text="20" Value="20" Selected="True" />
                                    <asp:ListItem Text="30" Value="30" />
                                    <asp:ListItem Text="40" Value="40" />
                                    <asp:ListItem Text="50" Value="50" />
                                    <asp:ListItem Text="100" Value="100" />
                                    <asp:ListItem Text="200" Value="200" />
                                    <asp:ListItem Text="500" Value="500" />
                                </asp:DropDownList>
                            <asp:ImageButton runat="server" ID="lbgvFirst" ImageUrl="~/App.Images/pager-first.png" OnClick="gvFirstButton_Click" />
                            <asp:ImageButton runat="server" ID="lbgvPrevious" ImageUrl="~/App.Images/pager-previous.png" OnClick="gvPreviousButton_Click" />
                            <asp:Label runat="server" ID="lblPageAt" Text="Page 1 of 2" />
                            <asp:ImageButton runat="server" ID="lbgvNext" ImageUrl="~/App.Images/pager-next.png" OnClick="gvNextButton_Click" />
                            <asp:ImageButton runat="server" ID="lbgvLast" ImageUrl="~/App.Images/pager-last.png" OnClick="gvLastButton_Click" />
                        </td>
                    </tr>
                </table>
                <table style="width: 100%">
                    <tr style="text-align: left; margin-top: 0;">
                        <td style="float: right; text-align: center;">
                            <uc:ExportToExcel ID="ucExportToExcel" runat="server" Visible="False"  />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </td>
    </tr>
</table>





