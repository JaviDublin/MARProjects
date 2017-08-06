<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CompanyFleetOwnerMapping.ascx.cs"
    Inherits="Mars.App.UserControls.Phase4.Administration.Users.CompanyFleetOwnerMapping" %>

<asp:HiddenField runat="server" ID="hfCurrentGvPage" Value="1" />

<table style="height: 500px; width: 800px;">
    <tr style="width: 100%; text-align: right;">
        <td>
            <asp:Button runat="server" ID="btnNewCompany" Text="New Company" Width="120"
                CssClass="StandardButton" OnClick="btnNewCompany_Click" />
        </td>
    </tr>
    <tr style="vertical-align: top;">
        <td>
            <table>
                <tr style="text-align: left;">
                    <td style="text-align: right;">Company:
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="tbParameterCompanyName" Width="150px"
                            AutoPostBack="False" onkeyup="RefreshUpdatePanel();" OnTextChanged="ResetCurrentPage" />

                    </td>
                    <td style="text-align: right;">Type:
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlParameterCompanyType" Width="150px"
                            AutoPostBack="True" OnSelectedIndexChanged="ResetCurrentPage" />
                    </td>
                    <td style="text-align: right;">Country:
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlParameterCountry" Width="150px" AutoPostBack="True"
                            OnSelectedIndexChanged="ResetCurrentPage" />
                    </td>
                    
                </tr>
            </table>
        </td>

    </tr>
    <tr style="vertical-align: top;">
        <td style="height: 400px;">
            <table>
                <tr>
                    <td>
                        <asp:GridView runat="server" ID="gvCompanyGrid" Width="950px" AutoGenerateColumns="False"
                            AllowSorting="True" OnSorting="CompanyGrid_Sorting"
                            AllowPaging="True" BorderStyle="None" ShowHeaderWhenEmpty="True" PageSize="20"
                            CssClass="StandardBorder">
                            <HeaderStyle CssClass="StandardDataGridHeaderStyle" />
                            <RowStyle CssClass="StandardDataGrid" Height="8px" />
                            <PagerSettings Position="Bottom" />
                            <PagerTemplate>
                            </PagerTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <Columns>
                                <asp:BoundField DataField="CompanyName" HeaderText="Company" SortExpression="CompanyName">
                                    <ItemStyle HorizontalAlign="Left" Width="40px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CountryName" HeaderText="Country" SortExpression="CountryName">
                                    <ItemStyle HorizontalAlign="Left" Width="40px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CompanyTypeName" HeaderText="Type" SortExpression="CompanyTypeName">
                                    <ItemStyle HorizontalAlign="Left" Width="40px" />
                                </asp:BoundField>

                                <asp:TemplateField>
                                    <ItemStyle Width="50px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButtonEdit" runat="server" Text="Edit" CommandName='<%# Eval("EditCommand") %>'
                                            CommandArgument='<%# Eval("CompanyId") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Width="50px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButtonView" runat="server" Text="View" CommandName='<%# Eval("ViewCommand") %>'
                                            CommandArgument='<%# Eval("CompanyId") %>' />
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
                                        <asp:HiddenField runat="server" ID="hfRowCount" />
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
    <tr style="text-align: center;">
        <td>
            <asp:Button runat="server" ID="btnLoadCompanies" Text="Load"
                CssClass="StandardButton" OnClick="btnLoadCompanies_Click" />
        </td>
    </tr>
</table>

<asp:HiddenField runat="server" ID="hfSelectedCompanyId" />

<asp:Panel ID="pnlCompanyEdit" runat="server" CssClass="Phase4ModalPopup" Width="470px">
    <table style="width: 100%; text-align: center;">
        <tr>
            <td>
                <div style="float: right;">
                    <asp:ImageButton runat="server" ID="ibClose2" ImageUrl="~/App.Images/Icons/close.png" />
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <table style="text-align: left;">
                    <tr>
                        <td style="width: 110px;">Company Name:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="tbCompanyName" Width="220" />
                        </td>
                    </tr>
                    <tr>
                        <td>Company Type:
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlCompanyType" Width="120" />
                        </td>
                    </tr>
                    <tr>
                        <td>Country:
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlCompanyCountry" Width="120" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: center;">
                            <asp:Label runat="server" ID="lblPopupMessage" ForeColor="Red" />
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
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table style="width: 100%; text-align: center;">
                    <tr>
                        <td>
                            <asp:Button runat="server" ID="btnDelete" Text="Delete" ForeColor="Red" OnClick="bnDeletePopup_Click"
                                ValidationGroup="CheckControls" CssClass="StandardButton" />
                            <asp:ConfirmButtonExtender runat="server" TargetControlID="btnDelete" ConfirmText="Delete this Company?" />
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btnSaveCompany" Text="Save" OnClick="btnSaveCompany_Click"
                                CssClass="StandardButton" />
                        </td>
                    </tr>
                </table>
            </td>

        </tr>
    </table>

</asp:Panel>

<asp:Panel ID="pnlCompanyView" runat="server" CssClass="Phase4ModalPopup" Width="800px">

    <table style="width: 100%; text-align: center;">
        <tr>
            <td colspan="3">
                <div style="float: right;">
                    <asp:ImageButton runat="server" ID="ibClose" ImageUrl="~/App.Images/Icons/close.png" />
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="3" >
                <h1>
                <asp:Label runat="server" ID="lblCompanyName" />
                </h1>
                <br/>
            </td>
        </tr>
        <tr style="vertical-align: top;">
            <td>
                <fieldset style="height: 300px; overflow-y: scroll;">
                    <legend>Users</legend>
                    <asp:GridView runat="server" ID="gvUserGrid" Width="200px" AutoGenerateColumns="False"
                        AllowSorting="False"
                        AllowPaging="True" BorderStyle="None" ShowHeaderWhenEmpty="True"
                        CssClass="StandardBorder">
                        <HeaderStyle CssClass="StandardDataGridHeaderStyle" />
                        <RowStyle CssClass="StandardDataGrid" Height="8px" />
                        <PagerSettings Position="Bottom" />
                        <PagerTemplate>
                        </PagerTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <Columns>
                            <asp:BoundField DataField="RacfId" HeaderText="RACF ID" SortExpression="RacfId">
                                <ItemStyle HorizontalAlign="Left" Width="40px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="UserName" HeaderText="Name" SortExpression="UserName">
                                <ItemStyle HorizontalAlign="Left" Width="40px" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </fieldset>
            </td>
            <td>
                <fieldset style="height: 300px; overflow-y: scroll;">
                    <legend>Fleet</legend>
                    <asp:GridView runat="server" ID="gvFleetGrid" Width="200px" AutoGenerateColumns="False"
                        AllowSorting="False"
                        AllowPaging="True" BorderStyle="None" ShowHeaderWhenEmpty="True"
                        CssClass="StandardBorder">
                        <HeaderStyle CssClass="StandardDataGridHeaderStyle" />
                        <RowStyle CssClass="StandardDataGrid" Height="8px" />
                        <PagerSettings Position="Bottom" />
                        <PagerTemplate>
                        </PagerTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <Columns>
                            <asp:BoundField DataField="FleetOwnerCode" HeaderText="Code" SortExpression="FleetOwnerCode">
                                <ItemStyle HorizontalAlign="Left" Width="40px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FleetOwnerName" HeaderText="Name" SortExpression="FleetOwnerName">
                                <ItemStyle HorizontalAlign="Left" Width="40px" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </fieldset>
            </td>
            <td>
                <fieldset style="height: 300px; overflow-y: scroll;">
                    <legend>Locations</legend>

                    <asp:GridView runat="server" ID="gvLocations" Width="300px" AutoGenerateColumns="False"
                        AllowSorting="False"
                        AllowPaging="True" BorderStyle="None" ShowHeaderWhenEmpty="True"
                        CssClass="StandardBorder">
                        <HeaderStyle CssClass="StandardDataGridHeaderStyle" />
                        <RowStyle CssClass="StandardDataGrid" Height="8px" />
                        <PagerSettings Position="Bottom" />
                        <PagerTemplate>
                        </PagerTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <Columns>
                            <asp:BoundField DataField="LocationCode" HeaderText="Code" SortExpression="LocationCode">
                                <ItemStyle HorizontalAlign="Left" Width="40px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="LocationFullName" HeaderText="Name" SortExpression="LocationFullName">
                                <ItemStyle HorizontalAlign="Left" Width="40px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ServedBy" HeaderText="Served By" SortExpression="ServedBy">
                                <ItemStyle HorizontalAlign="Left" Width="40px" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </fieldset>

            </td>
        </tr>
    </table>

</asp:Panel>
<asp:Button ID="btnDummy" runat="server" CssClass="hidden" />
<asp:ModalPopupExtender
    ID="mpeCompanyView"
    runat="server"
    PopupControlID="pnlCompanyView"
    TargetControlID="btnDummy"
    DropShadow="True"
    BackgroundCssClass="modalBackgroundGray" />

<asp:Button ID="btnDummy2" runat="server" CssClass="hidden" />
<asp:ModalPopupExtender
    ID="mpeCompanyEdit"
    runat="server"
    PopupControlID="pnlCompanyEdit"
    TargetControlID="btnDummy2"
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

    <%--    var yPos;
    $(document).ready(function () {
        getScrollPosition();
        prm.add_beginRequest(getScrollPosition);
        prm.add_endRequest(setScrollPosition);
    });

    function pageLoad() {
        var popup = $find('<%=mpeUserUpdate.ClientID%>');
        popup.add_shown(setScrollPosition);
    }


    function userTyped() {
        __doPostBack('<%= tbParameterCompanyName.ClientID %>', '');
    }


    var prm = Sys.WebForms.PageRequestManager.getInstance();

    function getScrollPosition() {
        yPos = $("#<%=pnlOwnership.ClientID %>").scrollTop();
    }
    function setScrollPosition() {
        $("#<%=pnlOwnership.ClientID %>").scrollTop(yPos);
    }--%>



</script>
