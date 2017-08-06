<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/App.MasterPages/Mars.Master"
    CodeBehind="FleetOverview.aspx.cs"
    Inherits="Mars.App.Site.ForeignVehicles.FleetOverview" %>

<%@ Register Src="~/App.UserControls/Phase4/ForeignVehicles/VehicleOverview.ascx" TagName="VehicleOverviewGrid" TagPrefix="uc" %>
<%@ Register Src="~/App.UserControls/Phase4/AvailabilityParameters.ascx" TagName="VehicleParameters" TagPrefix="uc" %>
<%@ Register Src="~/App.UserControls/Phase4/Common/UpdateProgress.ascx" TagName="UpdateProgress" TagPrefix="uc" %>


<asp:Content ID="ContentMain" ContentPlaceHolderID="ContentPlaceholderMainContent"
    runat="server">

    <table style="height: 400px; text-align: center; margin-left: auto; margin-right: auto;">
        <tr style="margin-top: 0; margin-bottom: auto; vertical-align: top;">
            <td>
                <div id="tabbedPanel" style="width: 1010px; margin-left: auto; margin-right: auto; text-align: left; background-color: transparent;">
                    <ul>
                        <li style="width: 100%; text-align: center;">
                            <asp:UpdatePanel runat="server" ID="upnlUpdatedTime"
                                UpdateMode="Conditional">
                                <ContentTemplate>                                 
                                    <div style="float: right; position: absolute; right: 10px;">
                                        Fleet Update: &nbsp;
                                        <asp:Label ID="lblLastUpdate" runat="server" />
                                    </div>
                                    <h1>Fleet Overview</h1>
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
                                            <asp:Panel runat="server" ID="pnlOverviewGrid" ScrollBars="Both" Width="100%" Height="380px" Visible="False">
                                                <uc:VehicleOverviewGrid runat="server" ID="ucVehicleOverview" ColumnOneName="Location" OtherColumnsName="Owning Country" />
                                            </asp:Panel>
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
                <uc:VehicleParameters runat="server" ID="ucParameters" AllowMultiSelect="True"
                    ShowVehicleFields="False" ShowAdditionalFields="False" ShowDayGrouping="False" ShowOverdue="False" ShowValuesAs="False"
                    ShowRevenueStatus="False" ShowOperationalStatus="False" ShowMovementTypes="False" ShowMinDaysNonRev="True"
                    ShowOwningArea="True" ShowForeignPredicament="True" />
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
