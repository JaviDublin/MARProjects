<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FleetAdmin.ascx.cs" Inherits="Mars.App.UserControls.Phase4.Administration.Fleet.FleetAdmin" %>


<asp:HiddenField runat="server" ID="hfCurrentGvPage" Value="1" />

<table style="height: 500px; width: 800px;" >

    <tr style="vertical-align: top; ">
        <td>
            <table>
                <tr style="text-align: left;">
                    <td style="text-align: right;">Fleet:
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="tbParameterFleetName" Width="150px" OnTextChanged="ResetCurrentPage"
                            AutoPostBack="False" onkeyup="RefreshUpdatePanel();" />
                    </td>
                    <td style="text-align: right;">Country:
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlParameterCountry" Width="150px" AutoPostBack="True" 
                            OnSelectedIndexChanged="CountryChanged" />
                    </td>

                    <td style="text-align: right;">Company
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlParameterCompany" Width="220" AutoPostBack="True" />
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
                        <asp:GridView runat="server" ID="gvFleetGrid" Width="950px" AutoGenerateColumns="False"
                            AllowSorting="True" OnSorting="FleetGrid_Sorting"
                            AllowPaging="True" BorderStyle="None" ShowHeaderWhenEmpty="True" PageSize="20"
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
                                <asp:BoundField DataField="CountryName" HeaderText="Country" SortExpression="CountryId">
                                    <ItemStyle HorizontalAlign="Left" Width="40px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CompanyName" HeaderText="Company" SortExpression="CompanyName">
                                    <ItemStyle HorizontalAlign="Left" Width="40px" />
                                </asp:BoundField>
                                <asp:TemplateField>
                                    <ItemStyle Width="50px" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButtonEdit" runat="server" Text="Edit" CommandName='<%# Eval("EditCommand") %>'
                                            CommandArgument='<%# Eval("FleetOwnerId") %>' />
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
                                <asp:DropDownList runat="server" ID="ddlPageSize" AutoPostBack="True" 
                                    OnSelectedIndexChanged="ddlPageSize_SizeChange">
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
                CssClass="StandardButton" OnClick="btnLoadFleets_Click" />
        </td>
    </tr>
</table>


<asp:Panel ID="pnlFleetEdit" runat="server" CssClass="Phase4ModalPopup" Width="470px">
    <asp:HiddenField runat="server" ID="hfSelectedFleetId" />

    <table style="width: 100%; text-align: center;">
        <tr>
            <td>
                <div style="float: right;">
                    <asp:ImageButton runat="server" ID="ibClose" ImageUrl="~/App.Images/Icons/close.png" />
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>Country:
                        </td>
                        <td style="text-align: left;">
                            <asp:Label runat="server" ID="lblFleetCountry" Width="120"  />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 110px;">Fleet Code:
                        </td>
                        <td style="text-align: left;">
                            <asp:Label runat="server" ID="lblFleetCode" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 110px;">Fleet Name:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="tbFleetName" Width="220" />
                        </td>
                    </tr>
                    <tr>
                        <td>Company:
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlCompany" Width="220"  />
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
                            <asp:Button runat="server" ID="btnSaveFleet" Text="Save" OnClick="btnSaveFleet_Click"
                                CssClass="StandardButton" />
                        </td>
                    </tr>
                </table>
            </td>

        </tr>
    </table>
</asp:Panel>
<asp:Button ID="btnDummy" runat="server" CssClass="hidden" />
<asp:ModalPopupExtender
    ID="mpeFleetUpdate"
    runat="server"
    PopupControlID="pnlFleetEdit"
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
        __doPostBack('<%= tbParameterFleetName.ClientID %>', '');
    }

</script>