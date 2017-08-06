<%@ Page Title="" Language="C#" MasterPageFile="~/App.MasterPages/Mars.Master" AutoEventWireup="true" CodeBehind="Reporting.aspx.cs" Inherits="Mars.FleetAllocation.Webpages.Reporting" %>

<%@ Register Src="~/FleetAllocation/UserControls/Reports/ForecastedFleetSize.ascx" TagPrefix="uc" TagName="ForecastedFleetSize" %>
<%@ Register Src="~/FleetAllocation/UserControls/Reports/FleetComparison.ascx" TagPrefix="uc" TagName="FleetComparison" %>
<%@ Register Src="~/FleetAllocation/UserControls/FaoParameter.ascx" TagPrefix="uc" TagName="FaoParameter" %>
<%@ Register Src="~/App.UserControls/Phase4/Common/UpdateProgress.ascx" TagPrefix="uc" TagName="UpdateProgress" %>
<%@ Register Src="~/FleetAllocation/UserControls/Reports/SiteComparison.ascx" TagPrefix="uc" TagName="SiteComparison" %>
<%@ Register Src="~/FleetAllocation/UserControls/Reports/ForecastContribution.ascx" TagPrefix="uc" TagName="ForecastContribution" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceholderHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceholderMainContent" runat="server">

    <table style="height: 360px; text-align: center; margin-left: auto; margin-right: auto;">
        <tr style="margin-top: 0; margin-bottom: auto; vertical-align: top;">
            <td>
                <asp:HiddenField runat="server" ID="hfSelectedPanel" Value="0" />
                <div id="pnlTabbedPanel" style="width: 1010px; height: 500px; margin-left: auto; margin-right: auto; text-align: left;">
                    <ul>
                        <li>
                            <a href="#tabs-1" onclick="TabClick('0')">Forecasted Fleet Size</a>
                        </li>
                        <li>
                            <a href="#tabs-2" onclick="TabClick('1')">Fleet Comparison</a>
                        </li>
                        <li>
                            <a href="#tabs-3" onclick="TabClick('2')">Site Comparison</a>
                        </li>
                        <li>
                            <a href="#tabs-4" onclick="TabClick('3')">Contribution</a>
                        </li>
                    </ul>
                    <div id="tabs-1">
                        <table style="text-align: center; margin-left: 10px; margin-right: auto;">
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <uc:ForecastedFleetSize runat="server" ID="ucForecastedFleetSize" />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnLoad" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="tabs-2">
                        <table style="text-align: center;">
                            <tr>
                                <td style="vertical-align: top;">
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <uc:FleetComparison runat="server" ID="ucFleetComparison" />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnLoad" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="tabs-3">
                        <table style="text-align: center;">
                            <tr>
                                <td style="vertical-align: top;">
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <uc:SiteComparison runat="server" id="ucSiteComparison" />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnLoad" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="tabs-4">
                        <table style="text-align: center;">
                            <tr>
                                <td style="vertical-align: top;">
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <uc:ForecastContribution runat="server" id="ucForecastContribution" />
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
        <tr>
            <td style="text-align: center;">
                <asp:Button runat="server" ID="btnLoad" CssClass="StandardButton" Text="Load" Width="80px" OnClick="btnLoad_Click" />
            </td>
        </tr>
        <tr style="vertical-align: top;">
            <td>
                <table>
                    <tr>
                        <td>
                            <uc:FaoParameter runat="server" ID="FaoParameter" />
                        </td>
                        <td style="width: 25px;">&nbsp;
                        </td>
                        <td style="vertical-align: top;">
                            <table>
                                <tr>
                                    <td>
                                        Country:
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlCountry" AutoPostBack="True"
                                                  CssClass="SingleDropDownList"  />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Addition Plan A:</>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlAdditionPlanA" CssClass="SingleDropDownList" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label id="lblAdditionPlanB">
                                            Addition Plan B:    
                                        </label>
                                    </td>
                                    <td>
                                        <div id="dvAdditionPlanB">
                                            <asp:DropDownList runat="server" ID="ddlAdditionPlanB" CssClass="SingleDropDownList"/>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;">
                                        Weeks:
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:DropDownList runat="server" ID="ddlWeeksSelection" CssClass="SingleDropDownList"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;">
                                         <label id="lblForecastType">
                                            Forecast Type:    
                                        </label>
                                    </td>
                                    <td style="text-align: left;">
                                        <div id="dvForecastType">
                                            <asp:DropDownList runat="server" ID="ddlForecastTypes" CssClass="SingleDropDownList">
                                                <asp:ListItem Text="Values" Value=""/>
                                                <asp:ListItem Text="Difference" Value="Diff"/>
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

    <uc:UpdateProgress runat="server" ID="ucUpdateProgress" />

    <script type="text/javascript">

        $(function () {

            var currTab = $("#<%= hfSelectedPanel.ClientID %>").val();
            $("#pnlTabbedPanel").tabs({ selected: currTab });


            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(panelLoaded);

            function panelLoaded(sender, args) {
                if (args.get_panelsUpdated().length > 0) {
                    $(".FaoReportPanel").tabs();
                }

            }

            $(".FaoReportPanel").tabs();
        });

        function TabClick(tabId) {

            $("#<%= hfSelectedPanel.ClientID %>").val(tabId);

            if (tabId == "0") {
                $("#lblAdditionPlanB").show();
                $("#dvAdditionPlanB").show();
                $("#lblForecastType").show();
                $("#dvForecastType").show();
            }

            if (tabId == "1" || tabId == "2")
            {
                $("#lblAdditionPlanB").hide();
                $("#dvAdditionPlanB").hide();
                $("#lblForecastType").hide();
                $("#dvForecastType").hide();
            }

            if (tabId == "3") {
                $("#lblAdditionPlanB").show();
                $("#dvAdditionPlanB").show();
                $("#lblForecastType").hide();
                $("#dvForecastType").hide();
            }
        }

        function QuickSelectMade() {
            var evt = window.event;

            var updatePanelId = "<%= FaoParameter.UpdatePanelClientId %>";

            //If Mousedown or enter or tab pressed
            if (evt.type == "mousedown" || ((event.keyCode == 13) || (event.keyCode == 9))) {
                __doPostBack(updatePanelId, "LocationSingle CarGroupSingle");
            }
        }
    </script>
</asp:Content>
