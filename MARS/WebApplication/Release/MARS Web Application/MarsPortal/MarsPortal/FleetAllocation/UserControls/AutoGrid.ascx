<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AutoGrid.ascx.cs" Inherits="Mars.FleetAllocation.UserControls.AutoGrid" %>

<asp:UpdatePanel runat="server" ID="upnlGrid" UpdateMode="Conditional">
    <ContentTemplate>
        <div style="text-align: left;"> 
    <asp:GridView runat="server" Width="950px" ID="gvAutoGrid" HorizontalAlign="Center" AllowPaging="True"
        AutoGenerateColumns="True" AllowSorting="True" OnSorting="Gridview_Sorting" 
        OnRowDataBound="gvAutoGrid_DataRowBound"  >
            <HeaderStyle CssClass="StandardDataGridHeaderStyle" />
            <RowStyle CssClass="StandardDataGrid"  Height="8px"  />
            <PagerTemplate>
                    <table style="width: 100%; background-color: white;" >
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
                                <asp:Label runat="server" ID="lblPageAt" Text="Page 1 of 2" />
                                <asp:ImageButton runat="server" ID="lbgvNext" ImageUrl="~/App.Images/pager-next.png" 
                                        OnClick="gvNextButton_Click" />
                                <asp:ImageButton runat="server" ID="lbgvLast" ImageUrl="~/App.Images/pager-last.png" 
                                    OnClick="gvLastButton_Click" />
                            </td>
                        </tr>
                    </table>
            </PagerTemplate>
    </asp:GridView>
            </div>
    </ContentTemplate>
</asp:UpdatePanel>
    
    

