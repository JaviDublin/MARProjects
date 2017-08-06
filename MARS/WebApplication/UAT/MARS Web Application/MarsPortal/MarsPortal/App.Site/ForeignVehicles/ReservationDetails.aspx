<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/App.MasterPages/Mars.Master"
    CodeBehind="ReservationDetails.aspx.cs"
    Inherits="Mars.App.Site.ForeignVehicles.ReservationDetails" %>

<%@ Register Src="~/App.UserControls/Phase4/ReservationParameters.ascx" TagName="ReservationParameters" TagPrefix="uc" %>

<%@ Register Src="~/App.UserControls/Phase4/Reservations/ReservationsGrid.ascx" TagName="ReservationsGrid" TagPrefix="uc" %>
<%@ Register Src="~/App.UserControls/Phase4/Reservations/ReservationDetails.ascx" TagName="ReservationDetails" TagPrefix="uc" %>
<%@ Register Src="~/App.UserControls/Phase4/Common/UpdateProgress.ascx" TagName="UpdateProgress" TagPrefix="uc" %>


<asp:Content ID="ContentMain" ContentPlaceHolderID="ContentPlaceholderMainContent"
    runat="server">
    
    <table style="height: 360px; text-align: center; margin-left: auto; margin-right: auto;">
        <tr style="margin-top: 0; margin-bottom: auto; vertical-align: top;">
            <td>
                <div id="tabbedPanel" style="width: 1010px; margin-left: auto; margin-right: auto; text-align: left; background-color: transparent;">
                    <ul>
                        <li style="width: 100%; text-align: center;">
                            <asp:UpdatePanel runat="server" ID="upnlUpdatedTime" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div style="float: right; position: absolute; right: 10px;">
                                        Reservation Update: &nbsp;
                                        <asp:Label ID="lblLastUpdate" runat="server" />
                                    </div>
                                    <h1>Reservation Details</h1>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </li>
                    </ul>

                    <div id="tabs-1">
                        <table style="text-align: center; margin-left: auto; margin-right: auto;">
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlMultiview" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <uc:ReservationsGrid ID="ucReservationsGrid" runat="server" />
                                            <asp:Panel ID="pnlCarSearch" runat="server" CssClass="Phase4ModalPopup">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <div style="float: right;">
                                                                <asp:ImageButton runat="server" ID="ibClose" ImageUrl="~/App.Images/Icons/close.png" />
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <uc:ReservationDetails runat="server" ID="ucReservationDetails" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <asp:Button ID="btnDummy" runat="server" CssClass="hidden" />
                                            <asp:ModalPopupExtender
                                                ID="mpeCarSearch"
                                                runat="server"
                                                PopupControlID="pnlCarSearch"
                                                TargetControlID="btnDummy"
                                                DropShadow="True"
                                                BackgroundCssClass="modalBackgroundGray" />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnLoad" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
    </table>

    <table style="margin-left: auto; margin-right: auto;">
        <tr>
            <td style="text-align: center;">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:Button runat="server" ID="btnLoad" CssClass="StandardButton" Text="Load" Width="80px" OnClick="btnLoad_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr style="vertical-align: top;">
            <td>
                <uc:ReservationParameters runat="server" ID="ucParameters"  />
            </td>
        </tr>
    </table>
    <uc:UpdateProgress ID="ucUpdateProgress" runat="server" />

    <script type="text/javascript">
        $(function () {
            $("#tabbedPanel").tabs();
        });
    </script>
</asp:Content>