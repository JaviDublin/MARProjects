<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReservationMatchGrid.ascx.cs"
    Inherits="Mars.App.UserControls.Phase4.ForeignVehicles.ReservationMatchGrid" %>

<asp:HiddenField runat="server" ID="hfCurrentGvPage" Value="1" />
<asp:HiddenField runat="server" ID="hfShowNonRevFields" Value="1" />
<asp:HiddenField runat="server" ID="hfShowSelectColumn" Value="True" />

<asp:HiddenField runat="server" ID="hfHighlightedReservationId" Value="0" />

<table>
    <tr>
        <td>
            <div style="width: 494px; height: 15px; font-weight: bold; font-size: larger;" class="StandardDataGridHeaderStyle">
                <asp:Label runat="server" ID="lblHeader"/> 
            </div>
            <asp:GridView runat="server" ID="gvOverview" AutoGenerateColumns="False" AllowSorting="True" ShowHeaderWhenEmpty="True"
                OnSorting="GridviewOverview_Sorting" OnRowCommand="Overview_RowCommand" AllowPaging="True" Width="500px" BorderStyle="None" CssClass="StandardBorder">
                <HeaderStyle CssClass="StandardDataGridHeaderStyle" />
                <RowStyle CssClass="StandardDataGrid" />
                <PagerSettings Position="Bottom" Visible="True" />
                <PagerTemplate>
                    <table style="width: 100%; background-color: white;">
                        <tr>
                            <td style="text-align: left; margin-top: 0;">
                                <asp:Label runat="server" ID="lblRowCount" />
                            </td>
                            <td style="float: right;">Page Size:
                                        <asp:DropDownList runat="server" ID="ddlPageSize" AutoPostBack="True" OnSelectedIndexChanged="ddlPageSize_SizeChange">
                                            <asp:ListItem Text="10" Value="10" />
                                            <asp:ListItem Text="20" Value="20" />
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
                </PagerTemplate>
                <EditRowStyle CssClass="GridEditRowStyle" />
                <Columns>
                    <asp:BoundField DataField="DaysToPickup" HeaderText="Days Until Checkout" SortExpression="PickupDate">
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PickupDate" HeaderText="Check Out Date" SortExpression="PickupDate"
                        DataFormatString="{0:dd/MM/yyyy HH:mm}">
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PickupLocation" HeaderText="Check Out Location" SortExpression="PickupLocation">
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CarGroup" HeaderText="Group" SortExpression="CarGroup">
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ExternalId" HeaderText="Reservation ID" SortExpression="ExternalId">
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CustomerName" HeaderText="Customer" SortExpression="CustomerName">
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ReservationDuration" HeaderText="Length" SortExpression="ReservationDuration">
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="VehiclesMatched" HeaderText="Matches" SortExpression="VehiclesMatched">
                        <HeaderStyle VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:TemplateField>
                        <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButtonEdit" runat="server" Text="Select" CommandName="ShowVehicles"
                                CommandArgument='<%# Eval("ReservationId") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ReservationId" HeaderText="VehicleId" Visible="True" />
                </Columns>
                <HeaderStyle HorizontalAlign="Center" />
            </asp:GridView>
        </td>
    </tr>

</table>


