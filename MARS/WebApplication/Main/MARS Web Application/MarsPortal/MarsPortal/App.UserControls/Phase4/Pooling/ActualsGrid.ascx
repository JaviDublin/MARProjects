<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ActualsGrid.ascx.cs" Inherits="Mars.App.UserControls.Phase4.Pooling.ActualsGrid" %>

<asp:Label ID="lblTest" runat="server" />
<table>
    <tr style="vertical-align: top;">
        <td>
            <table class="ActualsMainTable">
                <tr>
                    <td>Time Zone: GMT
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                </tr>
                <asp:Repeater runat="server" ID="rptTopics">
                    <ItemTemplate>
                        <tr>
                            <td><%# Container.DataItem.ToString() %> </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>

            </table>
        </td>

        <td>
            <asp:Panel runat="server" ScrollBars="Horizontal" Width="900px">
                <table class="ActualsMainTable">
                    <tr>

                        <asp:Repeater runat="server" ID="rptHeaderDaysRow" ItemType="Mars.App.Classes.Phase4Dal.Pooling.Entities.DayActualDayHeaderCell" >
                            <ItemTemplate>
                                <th colspan='<%# Item.ColSpan %>'> <%# Item.CellName %> </th>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tr>
                    <tr>
                        <asp:Repeater runat="server" ID="rptHeaderHoursRow">
                            <ItemTemplate>
                                <th><%# Container.DataItem.ToString() %> </th>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tr>
                    <asp:ListView ID="rptActualsGrid" runat="server" ItemType="Mars.App.Classes.Phase4Dal.Pooling.Entities.DayActualsRow">
                        <ItemTemplate>
                            <tr>
                                <asp:Repeater runat="server" ItemType="Mars.App.Classes.Phase4Dal.Pooling.Entities.DayActualCell" DataSource='<%# Item.CellValues  %>'>
                                    <ItemTemplate>
                                        <td>
                                            <asp:LinkButton runat="server" Text='<%# Item.CellValue.ToString("#,0") %>'
                                                Enabled='<%# Item.LinkButton %>' />
                                        </td>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                </table>

            </asp:Panel>
        </td>
    </tr>
</table>


