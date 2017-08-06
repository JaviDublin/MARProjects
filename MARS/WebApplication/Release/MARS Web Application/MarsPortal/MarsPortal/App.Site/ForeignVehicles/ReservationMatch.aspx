<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/App.MasterPages/Mars.Master"
    CodeBehind="ReservationMatch.aspx.cs" Inherits="Mars.App.Site.ForeignVehicles.ReservationMatch" %>

<%@ Register Src="~/App.UserControls/Phase4/ForeignVehicles/FleetMatchGrid.ascx" TagName="FleetMatchGrid" TagPrefix="uc" %>
<%@ Register Src="~/App.UserControls/Phase4/ForeignVehicles/ReservationMatchGrid.ascx" TagName="ReservationMatchGrid" TagPrefix="uc" %>
<%@ Register Src="~/App.UserControls/Phase4/ReservationParameters.ascx" TagName="ReservationParameters" TagPrefix="uc" %>
<%@ Register Src="~/App.UserControls/Phase4/Common/UpdateProgress.ascx" TagName="UpdateProgress" TagPrefix="uc" %>


<%@ Register Src="~/App.UserControls/Phase4/HelpIcons/ExportToExcel.ascx" TagName="ExportToExcel" TagPrefix="uc" %>




<asp:Content ID="ContentMain" ContentPlaceHolderID="ContentPlaceholderMainContent"
    runat="server">

    <table style="height: 400px; text-align: center; margin-left: auto; margin-right: auto;">
        <tr style="margin-top: 0; margin-bottom: auto; vertical-align: top;">
            <td>
                <div id="tabbedPanel" style="width: 1050px; margin-left: auto; margin-right: auto; text-align: left; background-color: transparent;">
                    <ul>
                        <li style="width: 100%; text-align: center; height: 35px;">
                            <asp:UpdatePanel runat="server" ID="upnlUpdatedTime" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table style="float: right; position: absolute; right: 10px;">
                                        <tr>
                                            <td style="text-align: right;">
                                                Fleet Update:
                                            </td>
                                            <td>
                                                <asp:Label ID="lblLastUpdate" runat="server" />        
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;">
                                                Reservation Update:
                                            </td>        
                                            <td>
                                                <asp:Label ID="lblReservationUpdate" runat="server" />        
                                            </td>
                                        </tr>
                                    </table>   
                                    <h1>Reservation Match</h1>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </li>
                        <li style="text-align: center !important; width: 72%;">
                            
                        </li>
                    </ul>

                    <div id="tabs-1">
                        <table style="text-align: center; margin-left: 0; margin-right: auto;">
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlMultiview" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Panel runat="server" ID="pnlOverviewGrid" ScrollBars="Vertical" Width="100%" Height="380px" Visible="False">
                                                <table>

                                                    <tr style="vertical-align: top;">
                                                        <td>
                                                            <uc:ReservationMatchGrid ID="ucReservationMatchGrid" runat="server" />
                                                        </td>
                                                        <td>
                                                            <uc:FleetMatchGrid ID="ucFleetMatchGrid" runat="server" ShowSelectColumn="False" />
                                                        </td>
                                                    </tr>

                                                </table>

                                            </asp:Panel>
                                            <table style="width: 1000px;">
                                                <tr>
                                                    <td style="width: 90%">
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <uc:ExportToExcel ID="ucExportToExcel" runat="server" Visible="False" />    
                                                    </td>
                                                </tr>
                                            </table>
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
                <uc:ReservationParameters runat="server" ID="ucParameters" />
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
