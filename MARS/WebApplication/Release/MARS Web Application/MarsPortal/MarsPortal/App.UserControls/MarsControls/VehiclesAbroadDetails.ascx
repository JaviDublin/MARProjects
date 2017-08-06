<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VehiclesAbroadDetails.ascx.cs" Inherits="App.UserControls.VehiclesAbroadDetails" %>

<asp:UpdatePanel ID="UpdatePanelVehicleDetailsModal" runat="server" Visible="false">
    <ContentTemplate>

        <div id="screenCover" runat="server"
            style="background-color:Black;position:fixed;top:0px;left:0px;width:100%;height:100%;z-index:998;opacity:0.4;filter:alpha(opacity=40);"></div>
            <div id="vehicleDetailsModal" style="position:fixed; z-index:999; top:80px; left:25%;" runat="server">
                <table class='tableVehicleDetails' style='width:720px;background-color:white;padding:5px;border:1px solid grey;'>
                    <tr>
                        <th colspan='6' style='background-image:url(../App.Styles/ImagesAvailability/header_reservations.gif);'>
                            <img style='float:left' src='../../../App.Images/hertz-logo.jpg' alt='Hertz Logo' />
                            <span style='font-size:16px;font-weight:bold;' id="modalVin"></span>
                            <img style='float:right' src="../../../App.Images/application-logo.gif" alt='Application Logo' />
                        </th>
                    </tr>
                    <tr>
                        <td style='font-size:12px;font-weight:bold;width:120px'>Group</td>
                        <td style='font-size:12px;font-weight:bold;width:120px'>ModelCode</td>
                        <td style='font-size:12px;font-weight:bold;width:120px'>Model</td>
                        <td style='font-size:12px;font-weight:bold;width:120px'>Unit</td>
                        <td style='font-size:12px;font-weight:bold;width:120px'>License</td>
                        <td style='font-size:12px;font-weight:bold;width:120px'>Vin</td>
                    </tr>
                    <tr>
                        <td id="tdGroup" runat="server">xx</td>
                        <td id="tdModelcode" runat="server">xx</td>
                        <td id="tdModel" runat="server">xx</td>
                        <td id="tdUnit" runat="server">xx</td>
                        <td id="tdLicense" runat="server">xx</td>
                        <td id="tdVin" runat="server">xx</td>
                    </tr>
                    <tr>
                        <td colspan="7"><hr /></td>
                    </tr>
                    <tr>
                        <td style='font-size:12px;font-weight:bold;'>Charged</td>
                        <td style='font-size:12px;font-weight:bold;'>Last Document</td>
                        <td style='font-size:12px;font-weight:bold;'>LSTWWD</td>
                        <td style='font-size:12px;font-weight:bold;'>LSTDATE</td>
                        <td style='font-size:12px;font-weight:bold;'>Kilometers</td>
                        <td style='font-size:12px;font-weight:bold;'></td>
                    </tr>
                    <tr>
                        <td id="tdCharged"  runat="server">xx</td>
                        <td id="tdLastDoc" runat="server">xx</td>
                        <td id="tdLstwwd" runat="server">xx</td>
                        <td id="tdLstdate" runat="server">xx</td>
                        <td id="tdLstmlg" runat="server">xx</td>
                        <td></td>
                    </tr>
                    <tr>
                        <td colspan="7"><hr /></td>
                    </tr>
                    <tr>
                        <td style='font-size:12px;font-weight:bold;'>Operstat</td>
                        <td style='font-size:12px;font-weight:bold;'>MoveType</td>
                        <td style='font-size:12px;font-weight:bold;'>DUEWWD</td>
                        <td style='font-size:12px;font-weight:bold;'>DUEDATE</td>
                        <td style='font-size:12px;font-weight:bold;'>DUETIME</td>
                        <td style='font-size:12px;font-weight:bold;'>Driver Name</td>
                    </tr>
                    <tr>
                        <td id="tdOp" runat="server">xx</td>
                        <td id="tdMt" runat="server">xx</td>
                        <td id="tdDuewwd" runat="server">xx</td>
                        <td id="tdDuedate" runat="server">xx</td>
                        <td id="tdDuetime" runat="server">xx</td>
                        <td id="tdDriver" runat="server">xx</td>
                    </tr>
                    <tr>
                        <td colspan="7"><hr /></td>
                    </tr>
                    <tr>
                        <td style='font-size:12px;font-weight:bold;'>Non-Rev</td>
                        <td style='font-size:12px;font-weight:bold;'>Reg Date</td>
                        <td style='font-size:12px;font-weight:bold;'>Blockdate</td>
                        <td style='font-size:12px;font-weight:bold;'>Remark Date</td>
                        <td style='font-size:12px;font-weight:bold;'>OWNAREA</td>
                        <td style='font-size:12px;font-weight:bold;'>CarHold</td>
                    </tr>
                    <tr>
                        <td id="tdNonrev" runat="server">xx</td>
                        <td id="tdRegdate" runat="server">xx</td>
                        <td id="tdBlockdate" runat="server">xx</td>
                        <td id="tdRemarkdate" runat="server">xx</td>
                        <td id="tdOwnarea" runat="server">xx</td>
                        <td id="tdCarhold" runat="server">xx</td>
                    </tr>
                    <tr>
                        <td colspan="7"><hr /></td>
                    </tr>
                    <tr>
                        <td style='font-size:12px;font-weight:bold;'>BD Days</td>
                        <td></td>
                        <td style='font-size:12px;font-weight:bold;'>MM Days</td>
                        <td></td>
                        <td style='font-size:12px;font-weight:bold;'>PREWWD</td>
                        <td></td>
                    </tr>
                    <tr>
                        <td id="tdBddays" runat="server">xx</td>
                        <td></td>
                        <td id="tdMmdays" runat="server">xx</td>
                        <td></td>
                        <td id="tdPrewwd" runat="server">xx</td>
                        <td></td>
                    </tr>
                    <tr>
                        <td colspan="7"><hr /></td>
                    </tr>
                    <tr>
                        <td style='font-size:12px;font-weight:bold;'>Remarks</td>
                        <td colspan='5'><textarea rows='5' cols='64' id="remarksText" runat="server"></textarea></td>
                    </tr>
                    <tr>
                        <td colspan="4"></td>
                        <td>
                            <asp:Button ID="ButtonSave" runat="server" Text="Save remark" 
                                onclick="ButtonSave_Click" />
                        </td>
                        <td>
                            <asp:Button ID="ButtonClose" runat="server" Text="Close" 
                                onclick="ButtonClose_Click" />
                        </td>
                    </tr>
                </table>
            </div>

    </ContentTemplate>
</asp:UpdatePanel>