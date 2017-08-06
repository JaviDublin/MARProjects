<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserRoleMapping.ascx.cs"
    Inherits="Mars.App.UserControls.Phase4.Administration.Users.UserRoleMapping" %>
<%@ Import Namespace="System.Drawing" %>

<asp:HiddenField runat="server" ID="hfCurrentGvPage" Value="1" />

<table style="width: 880px;">

    <tr style="width: 100%; text-align: right;">
        <td colspan="4">
            <asp:Button runat="server" ID="btnNewUser" Text="New User" CssClass="StandardButton" Visible="False"
                OnClick="btnNewUser_Clicked" />
        </td>
    </tr>
    <tr style="vertical-align: top;">
        <td>
            <table>
                <tr>
                    <td style="text-align: right;">User:
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="tbUserSearch" Width="150px" AutoPostBack="False" OnTextChanged="ResetCurrentPage"
                            onkeyup="RefreshUpdatePanel();" />
                    </td>
                    <td style="text-align: right;">Type:
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlParameterUserType" Width="150px" OnSelectedIndexChanged="ResetCurrentPage"
                            AutoPostBack="True" />
                    </td>
                    <td style="text-align: right;">Role:
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlRole" Width="150px" OnSelectedIndexChanged="ResetCurrentPage"
                            AutoPostBack="True" />
                    </td>
                    <td style="text-align: right;">Company Country:
                    </td>
                    <td>
                        <asp:DropdownList runat="server" ID="ddlParameterCompanyCountry" Width="150px" AutoPostBack="True" 
                             OnSelectedIndexChanged="ddlParameterCompanyCountry_SelectionChanged" />
                    </td>
                    <td style="text-align: right;">Company:
                    </td>
                    <td>
                        <asp:DropdownList runat="server" ID="ddlParameterCompany" Width="150px" AutoPostBack="True" 
                            OnSelectedIndexChanged="ResetCurrentPage" />
                    </td>
                </tr>

            </table>
        </td>

    </tr>
    <tr style="vertical-align: top;">
        <td colspan="4" style="height: 400px;">
            <table>
                <tr>
                    <td>
                        <asp:GridView runat="server" ID="gvUserGrid" Width="950px" AutoGenerateColumns="False"
                            AllowSorting="True" OnSorting="UserGrid_Sorting"
                            AllowPaging="True" BorderStyle="None" ShowHeaderWhenEmpty="True" PageSize="20"
                            CssClass="StandardBorder">
                            <HeaderStyle CssClass="StandardDataGridHeaderStyle" />
                            <RowStyle CssClass="StandardDataGrid" Height="8px" />
                            <PagerSettings Position="Bottom" />
                            <PagerTemplate>
                            </PagerTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <Columns>
                                <asp:BoundField DataField="EmployeeId" HeaderText="Employee ID" SortExpression="EmployeeId">
                                    <ItemStyle HorizontalAlign="Left" Width="40px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="RacfId" HeaderText="RACF ID" SortExpression="RacfId">
                                    <ItemStyle HorizontalAlign="Left" Width="40px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="UserName" HeaderText="Name" SortExpression="UserName">
                                    <ItemStyle HorizontalAlign="Left" Width="40px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CompanyType" HeaderText="Type" SortExpression="CompanyType">
                                    <ItemStyle HorizontalAlign="Left" Width="40px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CompanyCountryName" HeaderText="Company Country" SortExpression="CompanyCountryName">
                                    <ItemStyle HorizontalAlign="Left" Width="40px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CompanyName" HeaderText="Company" SortExpression="CompanyName">
                                    <ItemStyle HorizontalAlign="Left" Width="40px" />
                                </asp:BoundField>
                                <asp:TemplateField>
                                    <ItemStyle Width="50px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButtonEdit" runat="server" Text="Edit" CommandName='<%# Eval("EditCommand") %>'
                                            CommandArgument='<%# Eval("MarsUserId") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel runat="server" ID="pnlPager">
                            <table style="width: 100%; background-color: white;">
                                <tr>
                                    <td style="text-align: left; margin-top: 0;">
                                        <asp:Label runat="server" ID="lblRowCount" />
                                        <asp:HiddenField runat="server" ID="hfRowCount"/>
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

                        </asp:Panel>
                    </td>
                </tr>
            </table>

        </td>
    </tr>
    <tr>
        <td colspan="2" style="text-align: center;">
            <asp:Button runat="server" ID="btnRefreshUsersList" Text="Load" CssClass="StandardButton"
                OnClick="btnRefreshUsersList_Click" />
        </td>
    </tr>

    <tr style="vertical-align: top;">

        <td>
            <asp:Panel ID="pnlUserAssignment" runat="server">
            </asp:Panel>
        </td>

    </tr>

</table>


<asp:Panel ID="pnlUserEdit" runat="server" CssClass="Phase4ModalPopup" Width="580px">
    <asp:HiddenField runat="server" ID="hfUserId" Value="0" />
    <table style="width: 100%;">
        <tr>
            <td colspan="2">
                <div style="float: right;">
                    <asp:ImageButton runat="server" ID="ibClose" OnClick="ibClose_Click" ImageUrl="~/App.Images/Icons/close.png" />
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Panel runat="server" ID="pnlSearchForUser">
                <table>
                    <tr>
                        <td>
                            Search by RACF / Name:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="tbSearchBox" Width="220px" OnTextChanged="tbSearchBoxItemSelected" />
                            <asp:AutoCompleteExtender ID="acUserName" runat="server" ServiceMethod="SearchMembershipDbForRacf"
                                TargetControlID="tbSearchBox" ServicePath="~/AutoComplete.asmx" MinimumPrefixLength="3"
                                CompletionInterval="500" EnableCaching="true" CompletionSetCount="20" 
                                    OnClientItemSelected="EmployeeSelected"
                                UseContextKey="True" />    
                            <script type="text/javascript">
                                function EmployeeSelected() {
                                    var evt = window.event;
                                    if (evt.type == "mousedown")
                                        __doPostBack("<%= tbSearchBox.ClientID %>", "");
                                }
                            </script>
                        </td>            
                    </tr>
                </table>
                    </asp:Panel>
            </td>
            
        </tr>
        <tr style="vertical-align: top;">
            <td>
                <asp:Panel ID="pnlUserDetails" runat="server">
                    <fieldset style="height: 200px;">
                        <legend>User Details</legend>

                        <table style="text-align: left;">

                            <tr>
                                <td style="width: 110px;">RACF:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="tbRacfId" Width="150" ReadOnly="True"  /> 
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 110px;">Employee ID:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="tbEmployeeId" Width="150" ReadOnly="True" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 110px;">Name:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="tbEmployeeName" Width="150" ReadOnly="True" />
                                </td>
                            </tr>
                            <tr>
                                <td>User Type:
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlUserType" Width="150"  AutoPostBack="True" 
                                        OnSelectedIndexChanged="ddlUserType_SelectionChanged" Enabled="False" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblPopupCompanyCountry" Text="Company Country:" />
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlCompanyCountry"  Width="150"  AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlCompanyCountry_SelectionChanged" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblPopupCompanyName" Text="Company Name:" />
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlCompany" Width="150"  />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div style="height: 20px;">
                                        &nbsp;
                                    </div>
                                    <asp:Label runat="server" ID="lblMessage" ForeColor="Red" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="text-align: center;">
                                    <asp:Button runat="server" ID="btnSaveUser" Text="Save" OnClick="btnSaveUser_Click"
                                        CssClass="StandardButton" />
                                </td>
                            </tr>

                        </table>
                    </fieldset>
                </asp:Panel>
            </td>
            <td>
                <asp:Panel runat="server" ID="pnlPriviliges">
                    <fieldset style="height: 200px; width: 200px;">
                        <legend>Page Access</legend>
                        <asp:GridView ID="rptRolesForUser" runat="server" Width="200px" ShowHeader="False" CssClass="StandardBorder"
                             AutoGenerateColumns="False" OnRowCommand="ChangeRoleCommand">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "RoleName") %>'
                                            ForeColor='<%# ( (bool) Eval("Granted")) ? Color.Green : Color.Red %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                       <asp:LinkButton runat="server" ID="lbGrantRevoke" Text='<%# DataBinder.Eval(Container.DataItem, "ButtonText") %>'
                                            CommandName='<%# ( (bool) Eval("Granted")) ? RevokeCommand : GrantCommand %>' 
                                            Enabled='<%# bool.Parse(DataBinder.Eval(Container.DataItem, "Enabled").ToString()) %>'
                                            CommandArgument='<%#DataBinder.Eval(Container.DataItem, "UserRoleId") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                    </fieldset>
                </asp:Panel>
            </td>
        </tr>
    </table>

</asp:Panel>

<asp:Button ID="btnDummy" runat="server" CssClass="hidden" />
<asp:ModalPopupExtender
    ID="mpeUserUpdate"
    runat="server"
    PopupControlID="pnlUserEdit"
    TargetControlID="btnDummy"
    DropShadow="True"
    BackgroundCssClass="modalBackgroundGray" />


<script type="text/javascript">

    $(document).keypress(function (e) {
        if (e.keyCode === 13) {
            e.preventDefault();
            return false;
        }
        else return true;
    });

    var t;

    function RefreshUpdatePanel() {
        if (t) {
            clearTimeout(t);
            t = setTimeout(userTyped, 1500);
        }
        else {
            t = setTimeout(userTyped, 1500);
        }

    };

    function userTyped() {
        __doPostBack('<%= tbUserSearch.ClientID %>', '');
    }
</script>
