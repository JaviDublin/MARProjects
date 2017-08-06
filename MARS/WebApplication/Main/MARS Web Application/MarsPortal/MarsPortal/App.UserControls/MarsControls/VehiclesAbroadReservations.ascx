<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VehiclesAbroadReservations.ascx.cs" Inherits="App.UserControls.VehiclesAbroadReservations" %>

<asp:UpdatePanel ID="UpdatePanelReservations" runat="server" Visible="false">
    <ContentTemplate>

        <div id="screenCover" runat="server"
            style="background-color:Black;position:fixed;top:0px;left:0px;width:100%;height:100%;z-index:998;opacity:0.4;filter:alpha(opacity=40);"></div>
            <div id="vehicleDetailsModal" style="position:fixed; z-index:999; top:80px; left:25%;" runat="server">
                <table class='tableReservations' style='width:720px;background-color:white;padding:5px;border:1px solid grey;'>
                    <tr>
                        <th colspan='7' style='background-image:url(../App.Styles/ImagesAvailability/header_reservations.gif);'>
                            <img style='float:left' src='../../../App.Images/hertz-logo.jpg' alt='Hertz Logo' />
                            <span id="modalVin"></span>
                            <img style='float:right' src='../../../App.Images/application-logo.gif' alt='Application Logo' />
                        </th>
                    </tr>
                    <tr>
                        <td colspan="7">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="4" style="font-size:12px;font-weight:bold">
                            RES-ID
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td colspan="4" id="tdResNumber" style="font-size:12px;" runat="server">
                            ResNumber
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td colspan="7"><hr /></td>
                    </tr>
                    <tr>
                        <td style='width:120px;font-size:12px;font-weight:bold'>
                            Pick-Up Location
                        </td>
                        <td style='width:120px;font-size:12px;font-weight:bold'>
                            Pick-Up Date
                        </td>
                        <td style='width:120px;font-size:12px;font-weight:bold'>
                            Oneway                  
                        </td>
                        <td style='width:120px'>
                        </td>
                        <td style='width:120px;font-size:12px;font-weight:bold'>
                            Drop-Off Location
                        </td>
                        <td style='width:120px;font-size:12px;font-weight:bold'>
                            Drop-Off Date    
                        </td>
                        <td style='width:120px;font-size:12px;font-weight:bold'>
                            Rental Length
                        </td>
                    </tr>
                    <tr>
                        <td id="tdRentLoc" runat="server">
                            rentLoc
                        </td>
                        <td id="tdPickUpLoc" runat="server">
                            pickUpLoc
                        </td>
                        <td id="tdOneWay" runat="server">
                            oneway                  
                        </td>
                        <td>
                        </td>
                        <td id="tdRtrnLoc" runat="server">
                            rtrnLoc
                        </td>
                        <td id="tdRtrnTime" runat="server">
                            rtrnTime
                        </td>
                        <td id="tdResDays" runat="server">
                            resDays
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7"><hr /></td>
                    </tr>
                    <tr>
                        <td style='font-size:12px;font-weight:bold'>
                            Group Reserved
                        </td>
                        <td>
                        </td>
                        <td style='font-size:12px;font-weight:bold'>
                            Group to be rented
                        </td>
                        <td>
                        </td>
                        <td style='font-size:12px;font-weight:bold'>
                            Customer Name
                        </td>
                        <td>
                        </td>
                        <td style='font-size:12px;font-weight:bold'>
                            Customer Phone
                        </td>
                    </tr>
                    <tr>
                        <td id="tdResVehClass" runat="server">
                            resVehClass
                        </td>
                        <td>
                        </td>
                        <td id="tdGrInclGoldUpr" runat="server">
                            GrIncleGoldUpr
                        </td>
                        <td>
                        </td>
                        <td id="tdCustName" runat="server">
                            custName
                        </td>
                        <td>
                        </td>
                        <td id="tdPhone" runat="server">
                            phone
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7"><hr /></td>
                    </tr>
                    <tr>
                        <td style='font-size:12px;font-weight:bold'>
                            #1 Gold
                        </td>
                        <td style='font-size:12px;font-weight:bold'>
                            Gold Type
                        </td>
                        <td style='font-size:12px;font-weight:bold'>
                            CDP
                        </td>
                        <td>
                        </td>
                        <td style='font-size:12px;font-weight:bold'>
                            Taco
                        </td>
                        <td style='font-size:12px;font-weight:bold'>
                            Rate
                        </td>
                        <td style='font-size:12px;font-weight:bold'>
                            Flight Number
                        </td>
                    </tr>
                    <tr>
                        <td id="tdNo1ClubGold" runat="server">
                            no1ClubGold
                        </td>
                        <td id="tdGoldType" runat="server">
                            goldType
                        </td>
                        <td id="tdCpidNbr" runat="server">
                            cpidNbr
                        </td>
                        <td>
                        </td>
                        <td id="tdTaco" runat="server">
                            taco 
                        </td>
                        <td id="tdRate" runat="server">
                            rate
                        </td>
                        <td id="tdFlightNbr" runat="server">
                            flightNbr
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7"><hr /></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td colspan="5">
                            <textarea rows='5' cols='64' id="remarksText" runat="server"></textarea>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td colspan="7"><hr /></td>
                    </tr>
                    <tr>
                        <td colspan="6">
                        </td>
                        <td>
                            <asp:Button ID="buttonClose" runat="server" Text="Close" 
                                ToolTip="Click to close this form" onclick="buttonClose_Click" />
                        </td>
                    </tr>
                </table>
            </div>

    </ContentTemplate>
</asp:UpdatePanel>
