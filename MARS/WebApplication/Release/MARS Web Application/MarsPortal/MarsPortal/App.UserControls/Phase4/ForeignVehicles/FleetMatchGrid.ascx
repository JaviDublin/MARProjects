<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FleetMatchGrid.ascx.cs" Inherits="Mars.App.UserControls.Phase4.ForeignVehicles.FleetMatchGrid" %>

<asp:HiddenField runat="server" ID="hfCurrentGvPage" Value="1" />
<asp:HiddenField runat="server" ID="hfShowNonRevFields" Value="1" />

<asp:HiddenField runat="server" ID="hfShowSelectColumn" Value="True" />
<asp:HiddenField runat="server" ID="hfHighlightedVehicleId" Value="0" />

<table>
    <tr>
        <td>
            <div style="width: 494px; height: 15px; font-weight: bold; font-size: larger;" class="StandardDataGridHeaderStyle">
                <asp:Label runat="server" ID="lblHeader"/> 
            </div>
            <asp:GridView runat="server" ID="gvOverview" AutoGenerateColumns="False" AllowSorting="True" ShowHeaderWhenEmpty="True"
                OnSorting="GridviewOverview_Sorting" OnRowCommand="Overview_RowCommand" AllowPaging="True" Width="500px" BorderStyle="None" >
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
                
                
                <Columns>
                    <asp:BoundField DataField="OwningCountry" HeaderText="Owning Country" SortExpression="OwningCountry" >
                        <ItemStyle HorizontalAlign="Left" Width="50px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="LastLocation" HeaderText="Check Out Location" SortExpression="LastLocation">
                        <ItemStyle HorizontalAlign="Left" Width="60px"/>
                    </asp:BoundField>
                    <asp:BoundField DataField="CarGroup" HeaderText="Group" SortExpression="CarGroup">
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="LicensePlate" HeaderText="License" SortExpression="LicensePlate">
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="UnitNumber" HeaderText="Unit" SortExpression="UnitNumber">
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ModelDescription" HeaderText="Desc" SortExpression="ModelDescription">
                        <ItemStyle HorizontalAlign="Left" Width="120px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="NonRevDays" HeaderText="Days NR" DataFormatString="{0:#,0}" SortExpression="NonRevDays">
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OperationalStatusCode" HeaderText="Stat" SortExpression="OperationalStatusCode" Visible="False">
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ReservationsMatched" HeaderText="Matches" SortExpression="ReservationsMatched">
                        <HeaderStyle VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:TemplateField>
                        <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButtonEdit" runat="server" Text="Select" CommandName="ShowReservations" 
                                CommandArgument='<%# Eval("VehicleId") %>'  />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="VehicleId" HeaderText="VehicleId" Visible="True" />

                </Columns>
                <HeaderStyle HorizontalAlign="Center" />
            </asp:GridView>
        </td>
    </tr>

</table>





