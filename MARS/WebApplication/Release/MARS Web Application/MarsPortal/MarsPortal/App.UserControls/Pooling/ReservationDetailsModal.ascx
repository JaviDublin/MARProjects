<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReservationDetailsModal.ascx.cs"
    Inherits="App.UserControls.Pooling.ReservationDetailsModal" %>
<div id="screenCover" runat="server" clientidmode="Static" style="background-color: Black;
    position: fixed; top: 0px; left: 0px; width: 100%; height: 100%; z-index: 998;
    opacity: 0.4; filter: alpha(opacity=40);">
</div>
<div id="vehicleDetailsModal" style="position: fixed; z-index: 999; top: 80px; left: 25%;"
    runat="server" clientidmode="Static">
    <div class="div-form-vehicledetails">
        <table class="table-form-vehicledetails">
            <tr class="table-rowData">
                <td colspan='5'>
                    <asp:Label ID="Label1" runat="server" Text="Reservation Details" Font-Size="Medium"
                        Font-Bold="True"></asp:Label>
                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App.Images/Icons/modal-close.gif"
                        Style='border: 1px solid White; float: right; height: 14px;' onmouseover='this.style.borderColor="#808080";'
                        onmouseout='this.style.borderColor="White";' OnClick="ImageButtonClose_Click" />
                </td>
            </tr>
            <tr class="table-rowHeader">
                <td class="table-cellHeader">
                    <asp:Label ID="lblResID" runat="server" Text="RES-ID"></asp:Label>
                </td>
                <td class="table-cellHeader">
                    <asp:Label ID="lblResLoc" runat="server" Text="RES LOC"></asp:Label>
                </td>
                <td class="table-cellHeader">
                    <asp:Label ID="lblCustomer" runat="server" Text="CUSTOMER"></asp:Label>
                </td>
                <td class="table-cellHeader">
                    <asp:Label ID="lblPhone" runat="server" Visible="False" Text="PHONE"></asp:Label>
                </td>
                <td class="table-cellHeader">
                    <asp:Label ID="lblTariff" runat="server" Text="TARIFF"></asp:Label>
                </td>
            </tr>
            <tr class="table-rowData">
                <td class="table-cellData">
                    <asp:Label ID="lblResIDValue" runat="server"></asp:Label>
                </td>
                <td class="table-cellData">
                    <asp:Label ID="lblResLocValue" runat="server"></asp:Label>
                </td>
                <td class="table-cellData">
                    <asp:Label ID="lblCustomerValue" runat="server"></asp:Label>
                </td>
                <td class="table-cellData">
                    <asp:Label ID="lblPhoneValue" Visible="False" runat="server"></asp:Label>
                </td>
                <td class="table-cellData">
                    <asp:Label ID="lblTariffValue" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="table-rowHeader">
                <td class="table-cellHeader">
                    <asp:Label ID="lblCheckOut" runat="server" Text="CHECK OUT LOCATION"></asp:Label>
                </td>
                <td class="table-cellHeader">
                    <asp:Label ID="lblCheckOutDate" runat="server" Text="CHECK OUT DATE"></asp:Label>
                </td>
                <td class="table-cellHeader">
                    <asp:Label ID="lblCheckIn" runat="server" Text="CHECK IN LOCATION"></asp:Label>
                </td>
                <td class="table-cellHeader">
                    <asp:Label ID="lblCheckInDate" runat="server" Text="CHECK IN DATE"></asp:Label>
                </td>
                <td class="table-cellHeader">
                    <asp:Label ID="lblDays" runat="server" Text="DAYS"></asp:Label>
                </td>
            </tr>
            <tr class="table-rowData">
                <td class="table-cellData">
                    <asp:Label ID="lblCheckOutValue" runat="server"></asp:Label>
                </td>
                <td class="table-cellData">
                    <asp:Label ID="lblCheckOutDateValue" runat="server"></asp:Label>
                </td>
                <td class="table-cellData">
                    <asp:Label ID="lblCheckInValue" runat="server"></asp:Label>
                </td>
                <td class="table-cellData">
                    <asp:Label ID="lblCheckInDateValue" runat="server"></asp:Label>
                </td>
                <td class="table-cellData">
                    <asp:Label ID="lblDaysValue" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="table-rowHeader">
                <td class="table-cellHeader">
                    <asp:Label ID="lblGroup" runat="server" Text="GROUP RESERVED"></asp:Label>
                </td>
                <td class="table-cellHeader">
                    <asp:Label ID="lblGroupGold" runat="server" Text="GROUP AFTER GOLD UPGRADE"></asp:Label>
                </td>
                <td class="table-cellHeader">
                    <asp:Label ID="lblCDP" runat="server" Text="CDP"></asp:Label>
                </td>
                <td class="table-cellHeader">
                    <asp:Label ID="lblGoldNumber" Visible="False" runat="server" Text="#1-GOLD-NBR"></asp:Label>
                </td>
                <td class="table-cellHeader">
                    <asp:Label ID="lblGoldStatus" runat="server" Text="GOLD STATUS"></asp:Label>
                </td>
            </tr>
            <tr class="table-rowData">
                <td class="table-cellData">
                    <asp:Label ID="lblGroupValue" runat="server"></asp:Label>
                </td>
                <td class="table-cellData">
                    <asp:Label ID="lblGroupGoldValue" runat="server"></asp:Label>
                </td>
                <td class="table-cellData">
                    <asp:Label ID="lblCDPValue" runat="server"></asp:Label>
                </td>
                <td class="table-cellData">
                    <asp:Label ID="lblGoldNumberValue" Visible="False" runat="server"></asp:Label>
                </td>
                <td class="table-cellData">
                    <asp:Label ID="lblGoldStatusValue" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="table-rowHeader">
                <td class="table-cellHeader">
                    <asp:Label ID="lblFlightNumber" runat="server" Text="FLT-NBR"></asp:Label>
                </td>
                <td class="table-cellHeader">
                    <asp:Label ID="lblTaco" runat="server" Text="TACO"></asp:Label>
                </td>
                <td class="table-cellHeader">
                    <asp:Label ID="lblNeverLost" runat="server" Text="NEVERLOST"></asp:Label>
                </td>
                <td class="table-cellHeader">
                    <asp:Label ID="lblPrepaid" runat="server" Text="PREPAID"></asp:Label>
                </td>
                <td class="table-cellHeader">
                    <asp:Label ID="lblResDate" runat="server" Text="RES-DATE"></asp:Label>
                </td>
            </tr>
            <tr class="table-rowData">
                <td class="table-cellData">
                    <asp:Label ID="lblFlightNumberValue" runat="server"></asp:Label>
                </td>
                <td class="table-cellData">
                    <asp:Label ID="lblTacoValue" runat="server"></asp:Label>
                </td>
                <td class="table-cellData">
                    <asp:Label ID="lblNeverLostValue" runat="server"></asp:Label>
                </td>
                <td class="table-cellData">
                    <asp:Label ID="lblPrepaidValue" runat="server"></asp:Label>
                </td>
                <td class="table-cellData">
                    <asp:Label ID="lblResDateValue" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="table-rowHeader">
                <td class="table-cellHeader" colspan="5" style="text-align: center; width: 100%">
                    <asp:Label ID="lblRemarks" runat="server" Text="REMARKS"/>
                </td>
            </tr>
            <tr class="table-rowData">
                <td class="table-cellData" colspan="5" style="width: 100%; text-align: left;">
                    <asp:Label ID="lblRemarksValue" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
 <%--   <table style='width: 720px; background-color: white; padding: 5px; border: 1px solid grey;'>
        <tr>
            <td colspan='5'>
            </td>
            <td>
                <asp:ImageButton ID="ImageButtonClose" runat="server" ImageUrl="~/App.Images/Icons/modal-close.gif"
                    Style='border: 1px solid White; float: right; height: 14px;' onmouseover='this.style.borderColor="#808080";'
                    onmouseout='this.style.borderColor="White";' OnClick="ImageButtonClose_Click" />
            </td>
        </tr>
        <tr>
            <th colspan='6' style='background-image: url(../../App.Styles/ImagesAvailability/header_reservations.gif);'>
                <img style='float: left' src='../../../App.Images/hertz-logo.jpg' alt='Hertz Logo' />
                <span id="spanResId" runat="server" style="font-size: 2em; font-weight: bold; color: #CC6600;">
                    XXXXXXXX</span>
                <img style='float: right' src='../../../App.Images/application-logo.gif' alt='Application Logo' />
            </th>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style='font-size: 12px; font-weight: bold; text-align: left;'>
                RES-ID:
            </td>
            <td style='text-align: left;' id="tdResId" runat="server">
                XXXXXXXXXX
            </td>
            <td colspan="4">
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style='font-size: 12px; font-weight: bold; text-align: left;'>
                CHECK OUT:
            </td>
            <td style='text-align: left;' id="tdCheckOutLoc" runat="server">
                XXXXXX
            </td>
            <td style='font-size: 12px; font-weight: bold; text-align: left;'>
                DATE:
            </td>
            <td style='text-align: left;' id="tdCheckOutDate" runat="server">
                XX/XX/XX
            </td>
            <td style='font-size: 12px; font-weight: bold; text-align: left;'>
                RES LOC:
            </td>
            <td style='text-align: left;' id="tdResLoc" runat="server">
                XXXXXX
            </td>
        </tr>
        <tr>
            <td style='font-size: 12px; font-weight: bold; text-align: left;'>
                CHECK IN:
            </td>
            <td style='text-align: left;' id="tdCheckInLoc" runat="server">
                XXXXXX
            </td>
            <td style='font-size: 12px; font-weight: bold; text-align: left;'>
                DATE:
            </td>
            <td style='text-align: left;' id="tdCheckInDate" runat="server">
                XX/XX/XX
            </td>
            <td style='font-size: 12px; font-weight: bold; text-align: left;'>
                DAYS:
            </td>
            <td style='text-align: left;' id="tdDays" runat="server">
                XX
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style='font-size: 12px; font-weight: bold; text-align: left;'>
                GROUP:
            </td>
            <td style='text-align: left;' id="tdGroup" runat="server">
                XXXXXX
            </td>
            <td style='font-size: 12px; font-weight: bold; text-align: left;'>
                GROUP(GOLD):
            </td>
            <td style='text-align: left;' id="tdGroupGold" runat="server">
                XXXXXX
            </td>
            <td>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td style='font-size: 12px; font-weight: bold; text-align: left;'>
                TARIFF:
            </td>
            <td style='text-align: left;' id="tdTariff" runat="server">
                XXXXXX
            </td>
            <td colspan='4'>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style='font-size: 12px; font-weight: bold; text-align: left;'>
                CUSTOMER:
            </td>
            <td style='text-align: left;' id="tdCustomer" runat="server">
                XXXXXX
            </td>
            <td colspan="4">
            </td>
        </tr>
        <tr>
            <td style='font-size: 12px; font-weight: bold; text-align: left;'>
                PHONE:
            </td>
            <td style='text-align: left;' id="tdPhone" runat="server">
                XXXXXX
            </td>
            <td colspan="4">
            </td>
        </tr>
        <tr>
            <td style='font-size: 12px; font-weight: bold; text-align: left;'>
                CDP:
            </td>
            <td style='text-align: left;' id="tdCdp" runat="server">
                XXXXXX
            </td>
            <td colspan="4">
            </td>
        </tr>
        <tr>
            <td style='font-size: 12px; font-weight: bold; text-align: left;'>
                #1-GOLD-NBR:
            </td>
            <td style='text-align: left;' id="tdGoldNbr" runat="server">
                XXXXXX
            </td>
            <td colspan="4">
            </td>
        </tr>
        <tr>
            <td style='font-size: 12px; font-weight: bold; text-align: left;'>
                GOLD STATUS:
            </td>
            <td style='text-align: left;' id="tdGoldStatus" runat="server">
                XXXXXX
            </td>
            <td colspan="4">
            </td>
        </tr>
        <tr>
            <td style='font-size: 12px; font-weight: bold; text-align: left;'>
                FLT-NBR:
            </td>
            <td style='text-align: left;' id="tdFltNbr" runat="server">
                XXXXXX
            </td>
            <td colspan="4">
            </td>
        </tr>
        <tr>
            <td style='font-size: 12px; font-weight: bold; text-align: left;'>
                TACO:
            </td>
            <td style='text-align: left;' id="tdTaco" runat="server">
                XXXXXX
            </td>
            <td colspan="4">
            </td>
        </tr>
        <tr>
            <td style='font-size: 12px; font-weight: bold; text-align: left;'>
                NEVERLOST:
            </td>
            <td style='text-align: left;' id="tdNeverlost" runat="server">
                XXXXXX
            </td>
            <td colspan="4">
            </td>
        </tr>
        <tr>
            <td style='font-size: 12px; font-weight: bold; text-align: left;'>
                PREPAID:
            </td>
            <td style='text-align: left;' id="tdPrepaid" runat="server">
                XXXXXX
            </td>
            <td colspan="4">
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style='font-size: 12px; font-weight: bold; text-align: left;'>
                RES-DATE:
            </td>
            <td style='text-align: left;' id="tdResDate" runat="server">
                XXXXXX
            </td>
            <td colspan="4">
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style='font-size: 12px; font-weight: bold; text-align: left;'>
                REMARKS:
            </td>
            <td style='text-align: left;' id="tdRemarks" runat="server">
                XXXXXX
            </td>
            <td colspan="4">
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>--%>
</div>
