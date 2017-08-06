<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReservationDetails.ascx.cs" Inherits="Mars.App.UserControls.Phase4.Reservations.ReservationDetails" %>

<asp:HiddenField runat="server" id="hfReservationId" Value="0" />

<table class="table-form-vehicledetails" style="width: 880px">
    <tr class="table-rowHeader">
        <td class="table-cellHeader">Res ID</td>
        <td class="table-cellHeader">Customer</td>
        <td class="table-cellHeader">Tariff</td>
        <td class="table-cellHeader">Flight Number</td>
        <td class="table-cellHeader">Booking Date</td>
    </tr>
    <tr class="table-rowData">
        <td class="table-cellData">
            <asp:Label ID="lblExternalId" runat="server" />
        </td>
        <td class="table-cellData">
            <asp:Label ID="lblCustomer" runat="server" />
        </td>
        
        <td class="table-cellData">
            <asp:Label ID="lblTariff" runat="server" />
        </td>
        <td class="table-cellData">
            <asp:Label ID="lblFlightNumber" runat="server" />
        </td>
        <td class="table-cellData">
            <asp:Label ID="lblBookingDate" runat="server" />
        </td>
    </tr>
    <tr class="table-rowHeader">
        <td class="table-cellHeader">Check Out Location</td>
        <td class="table-cellHeader">Check Out Date</td>
        <td class="table-cellHeader">Check In Location</td>
        <td class="table-cellHeader">Check In Date</td>
        <td class="table-cellHeader">Days</td>
    </tr>
    <tr class="table-rowData">
        <td class="table-cellData">
            <asp:Label ID="lblCheckoutLocation" runat="server" />
        </td>
        <td class="table-cellData">
            <asp:Label ID="lblCheckoutDate" runat="server" />
        </td>
        <td class="table-cellData">
            <asp:Label ID="lblCheckinLocation" runat="server" />
        </td>
        <td class="table-cellData">
            <asp:Label ID="lblCheckInDate" runat="server" />
        </td>
        <td class="table-cellData">
            <asp:Label ID="lblReservationLengthInDays" runat="server" />
        </td>
    </tr>
    <tr class="table-rowHeader">
        <td class="table-cellHeader">Group Reserved</td>
        <td class="table-cellHeader">Upgraded Group</td>
        <td class="table-cellHeader">Gold Service</td>
        <td class="table-cellHeader">N1 Type</td>
        <td class="table-cellHeader">Neverlost</td>
        
    </tr>
    <tr class="table-rowData">
        <td class="table-cellData">
            <asp:Label ID="lblCarGroupReserved" runat="server" />
        </td>
        <td class="table-cellData">
            <asp:Label ID="lblCarGroupUpgraded" runat="server" />
        </td>
        <td class="table-cellData">
            <asp:Label ID="lblGoldService" runat="server" />
        </td>
        <td class="table-cellData">
            <asp:Label ID="lblN1Type" runat="server" />
        </td>
        <td class="table-cellData">
            <asp:Label ID="lblNeverlost" runat="server" />
        </td>

    </tr>
        <tr class="table-rowHeader" style="text-align: center;">
        <td class="table-cellHeader" colspan="5">Remark</td>
    </tr>
    <tr class="table-rowData">
        <td class="table-cellData" colspan="5">
            <asp:Label ID="lblRemark" runat="server" />
        </td>
    </tr>
    <tr class="table-rowHeader" style="text-align: center;">
        <td class="table-cellHeader" colspan="5">Comment</td>
    </tr>
    <tr class="table-rowData">
        <td class="table-cellData" colspan="5">
            <asp:TextBox ID="lblComment" TextMode="MultiLine" Width="800" Height="70" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="table-cellData" colspan="5">
            <asp:Button runat="server" ID="btnSaveComment" Text="Save" CssClass="StandardButton" OnClick="btnSaveComment_Click" />
        </td>
    </tr>
</table>
