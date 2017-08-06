<%@ Page Title="" Language="C#" MasterPageFile="~/App.MasterPages/Mars.Master"
    AutoEventWireup="true" CodeBehind="FleetStatus.aspx.cs" Inherits="Mars.App.Site.Availability.FleetStatus.FleetStatus" %>

<%@ Register Src="~/App.UserControls/Phase4/AvailabilityParameters.ascx" TagName="AvailabilityParameters" TagPrefix="uc" %>
<%@ Register Src="~/App.UserControls/Phase4/Availability/FleetStatusChart.ascx" TagName="FleetStatusChart" TagPrefix="uc" %>
<%@ Register Src="~/App.UserControls/Phase4/Availability/FleetStatusGrid.ascx" TagName="FleetStatusGrid" TagPrefix="uc" %>

<asp:Content ID="ContentMain" ContentPlaceHolderID="ContentPlaceholderMainContent"
    runat="server">
    <table style="height: 360px; text-align: center; margin-left: auto; margin-right: auto;">
        <tr style="margin-top: 0; margin-bottom: auto; vertical-align: top;">
            <td>
                <div id="tabbedPanel" style="width: 1050px; margin-left: auto; margin-right: auto; text-align: left; background-color: transparent;">
                    <ul>
                        <li style="float: right !important;">
                            <asp:UpdatePanel runat="server" ID="upnlUpdatedTime" UpdateMode="Conditional">
                                <ContentTemplate>
                                        Last Update: &nbsp;
                                        <asp:Label ID="lblLastUpdate" runat="server" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </li>
                        <li><a href="#tabs-1">Chart</a></li>
                        <li><a href="#tabs-2">Grid</a></li>
                        <li style="text-align: center !important;  width: 730px;" ><h1>Fleet Status</h1></li>
                    </ul>
                    <table style="height: 400px; width: 100%; text-align: center;
                                margin-left: auto; margin-right: auto; ">
                        <tr style="vertical-align: top;">
                            <td style="margin-top: 0; margin-bottom: auto; vertical-align: top;">
                                <div id="tabs-1">
                                    <asp:UpdatePanel ID="upnlFleetStatusChart" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <uc:FleetStatusChart ID="ucFleetStatusChart" runat="server" />
                                    </ContentTemplate>
                                    <triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnLoad" />
                                    </triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </td>
                            <td >
                                <div id="tabs-2">
                                    <asp:UpdatePanel ID="upnlFleetStatusGrid" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <uc:FleetStatusGrid ID="ucFleetStatusGrid" runat="server" />
                                    </ContentTemplate>
                                    <triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnLoad" />
                                    </triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </td>
                            </tr>
                    </table>     
                </div>
            </td>
        </tr>
    </table>
    <table style="margin-left: auto; margin-right: auto;">
        <tr>
            <td style="text-align: center;">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:Button runat="server" ID="btnLoad" CssClass="StandardButton" Text="Load" Width="80px" 
                            OnClick="btnLoad_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                
                
            </td>
        </tr>
        <tr style="vertical-align: top;">
            <td>
                <uc:AvailabilityParameters runat="server" ID="generalParams" ShowDayGrouping="True" ShowVehicleFields="False"
                    ShowLocationLogic="False" />
            </td>
        </tr>
    </table>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" ClientIDMode="Static" DisplayAfter="1000">
        <ProgressTemplate>
            <asp:Panel ID="PanelBackgroundCover" runat="server" CssClass="backgroundCover">
            </asp:Panel>
            <asp:Panel ID="PanelLoadData" runat="server" CssClass="loadData">
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp; Loading Data.....
                <br />
                <asp:Image ID="Image1" runat="server" CssClass="loadDataImage" ImageUrl="~/App.Images/ajax-loader.gif"
                    AlternateText="Please wait..." />
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <script type="text/javascript">
        $(function () {
            $("#tabbedPanel").tabs();
            $(".ui-tabs-panel").css("background", "none");

            $(".DateRangeCheck").val("");
            $(".DateRangeCheck").trigger("change");

            $(".SingleReportDate").hide();
            $(".ReportDateRange").show();
        });
    </script>
</asp:Content>
