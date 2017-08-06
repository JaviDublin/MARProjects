<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReservationsGrid.ascx.cs" Inherits="Mars.App.UserControls.Phase4.Reservations.ReservationsGrid" %>

<%@ Register Src="~/App.UserControls/Phase4/HelpIcons/ExportToExcel.ascx" TagName="ExportToExcel" TagPrefix="uc" %>


<asp:HiddenField runat="server" ID="hfEmptyGrid" Value="1" />
<asp:HiddenField runat="server" ID="hfCurrentGvPage" Value="1" />

<table>
    <tr>
        <td>
            <asp:GridView runat="server" ID="gvReservations" AutoGenerateColumns="False" AllowSorting="True" ShowHeaderWhenEmpty="True"
                OnSorting="GridviewOverview_Sorting" AllowPaging="True" Width="1000px" BorderStyle="None">
                <HeaderStyle CssClass="StandardDataGridHeaderStyle" />
                <RowStyle CssClass="StandardDataGrid" />
                <PagerSettings Position="Bottom" />
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
                    <table style="width: 100%">
                        <tr style="text-align: left; margin-top: 0;">
                            <td style="float: right; text-align: center;">
                                <uc:ExportToExcel ID="ucExportToExcel" runat="server" />
                            </td>
                        </tr>
                    </table>
                </PagerTemplate>
                <EditRowStyle CssClass="GridEditRowStyle" />
                <Columns>
                    <asp:BoundField DataField="ExternalId" HeaderText="Res ID" SortExpression="ExternalId">
                        <HeaderStyle VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" Width="30px"  />
                    </asp:BoundField>
                    <asp:BoundField DataField="Country" HeaderText="Check Out Country" SortExpression="Country">
                        <ItemStyle HorizontalAlign="Left" Width="40px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CarGroupReserved" HeaderText="Group Reserved" SortExpression="CarGroupReserved">
                        <ItemStyle HorizontalAlign="Left" Width="40px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CarGroupUpgraded" HeaderText="Group Upgraded" SortExpression="CarGroupUpgraded">
                        <ItemStyle HorizontalAlign="Left" Width="40px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PickupLocation" HeaderText="Check Out Location" SortExpression="PickupLocation">
                        <ItemStyle HorizontalAlign="Left" Width="50px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PickupDate" HeaderText="Check Out Date" SortExpression="PickupDate" DataFormatString="{0:dd/MM/yyyy HH:mm}">
                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ReturnLocation" HeaderText="Check In Location" SortExpression="ReturnLocation">
                        <ItemStyle HorizontalAlign="Left" Width="50px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ReturnDate" HeaderText="Check In Date" SortExpression="ReturnDate" DataFormatString="{0:dd/MM/yyyy HH:ss}">
                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="DaysReserved" HeaderText="Length" SortExpression="DaysReserved" >
                        <ItemStyle HorizontalAlign="Right" Width="50px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CustomerName" HeaderText="Customer" SortExpression="CustomerName">
                        <ItemStyle HorizontalAlign="Left" Width="120px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FlightNumber" HeaderText="Flight Number" SortExpression="FlightNumber">
                        <HeaderStyle VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" Width="50px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="N1Type" HeaderText="N1Type" SortExpression="FlightNumber">
                        <HeaderStyle VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Left" Width="50px" />
                    </asp:BoundField>
                    <asp:TemplateField>
                        <ItemStyle Width="50px" HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButtonEdit" runat="server" Text="Select" CommandName="ShowReservation"
                                CommandArgument='<%# Eval("ReservationId") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle HorizontalAlign="Center" />
            </asp:GridView>
        </td>
    </tr>
</table>
