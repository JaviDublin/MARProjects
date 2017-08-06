<%@ Page Title="" Language="C#" MasterPageFile="~/App.MasterPages/Mars.Master" AutoEventWireup="true" 
        CodeBehind="FactorMaintenance.aspx.cs" Inherits="Mars.FleetAllocation.Webpages.FactorMaintenance" %>

<%@ Register Src="~/FleetAllocation/UserControls/Factors/MinCommercialSegment.ascx" TagPrefix="uc" TagName="MinCommercialSegment" %>
<%@ Register Src="~/FleetAllocation/UserControls/Factors/MaxFleetFactors.ascx" TagPrefix="uc" TagName="MaxFleetFactors" %>
<%@ Register Src="~/App.UserControls/Phase4/Common/UpdateProgress.ascx" TagPrefix="uc" TagName="UpdateProgress" %>
<%@ Register Src="~/FleetAllocation/UserControls/Factors/LifecycleHoldingCost.ascx" TagPrefix="uc" TagName="LifecycleHoldingCost" %>
<%@ Register Src="~/FleetAllocation/UserControls/Factors/Revenue.ascx" TagPrefix="uc" TagName="Revenue" %>
<%@ Register Src="~/FleetAllocation/UserControls/Factors/RankingDisplay.ascx" TagPrefix="uc" TagName="RankingDisplay" %>
<%@ Register Src="~/FleetAllocation/UserControls/Factors/UsdConversionRate.ascx" TagPrefix="uc" TagName="UsdConversionRate" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceholderHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceholderMainContent" runat="server">
    
    <asp:HiddenField runat="server" ID="hfSelectedPanel" Value="0" />
    <table style="height: 360px; text-align: center; margin-left: auto; margin-right: auto;"  >
        <tr style="margin-top: 0; margin-bottom: auto; vertical-align: top;">
            <td>
                <div id="tabbedPanel" style="width: 1010px; margin-left: auto; margin-right: auto; text-align: left; background-color: transparent;">
                    <ul>    
                        <li><a href="#tabs-1">Min Commerical Segment</a></li>
                        <li><a href="#tabs-2">Max Fleet Factors</a></li>
                        <li><a href="#tabs-3">Lifecycle Holding Cost</a></li>
                        <li><a href="#tabs-4">Commercial Car Segment Revenue</a></li>
                        <li><a href="#tabs-5">Ranking Display</a></li>
                        <li><a href="#tabs-6">USD to EUR Rate</a></li>
                    </ul>
                    
                    <div id="tabs-1" >
                        <table style="text-align: center; margin-left: auto; margin-right: auto;">
                            <tr>
                                <td>
                                    <uc:MinCommercialSegment runat="server" id="MinCommercialSegment" />    
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="tabs-2" >
                        <table style="text-align: center; margin-left: auto; margin-right: auto;">
                            <tr>
                                <td>
                                    <uc:MaxFleetFactors runat="server" id="MaxFleetFactors" />
                                </td>
                            </tr>                    
                        </table>
                    </div>
                    <div id="tabs-3" >
                        <table style="text-align: center; margin-left: auto; margin-right: auto;">
                            <tr>
                                <td>
                                    <uc:LifecycleHoldingCost runat="server" ID="ucLifecycleHoldingCost" />
                                </td>
                            </tr>                    
                        </table>
                    </div>
                    <div id="tabs-4" >
                        <table style="text-align: center; margin-left: auto; margin-right: auto;">
                            <tr>
                                <td>
                                    <uc:Revenue runat="server" ID="ucRevenue" />
                                </td>
                            </tr>                    
                        </table>
                    </div>
                    <div id="tabs-5" >
                        <table style="text-align: center; margin-left: auto; margin-right: auto;">
                            <tr>
                                <td>
                                    <uc:RankingDisplay runat="server" ID="ucRankingDisplay" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="tabs-6" >
                        <table style="text-align: center; margin-left: auto; margin-right: auto;">
                            <tr>
                                <td>
                                    <uc:UsdConversionRate runat="server" ID="ucUsdConversionRate" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
        
    </table>
    <uc:UpdateProgress runat="server" ID="ucUpdateProgress" />
    
    <script type="text/javascript">
        $('.AutoCompleteTextBox').keypress(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                return false;
            }
        });
        
        function QuickSelectMade() {
            var evt = window.event;
            var selectedPanel = $("#tabbedPanel div.ui-tabs-panel:not(.ui-tabs-hide)");

            var updatePanelId;
            if (selectedPanel.index() == 1) {
                updatePanelId = "<%= MinCommercialSegment.GetUpdatePanelId %>";    
            }

            if (selectedPanel.index() == 2) {
                updatePanelId = "<%= MaxFleetFactors.GetUpdatePanelId %>";
            }

            if (selectedPanel.index() == 3) {
                updatePanelId = "<%= ucLifecycleHoldingCost.GetUpdatePanelId %>";
            }

            if (selectedPanel.index() == 4) {
                updatePanelId = "<%= ucRevenue.GetUpdatePanelId %>";
            }

            if (selectedPanel.index() == 5) {
                updatePanelId = "<%= ucRankingDisplay.GetUpdatePanelId %>";
            }
            

            //If Mousedown or enter or tab pressed
            if (evt.type == "mousedown" || ((event.keyCode == 13) || (event.keyCode == 9))) {
                __doPostBack(updatePanelId, "LocationSingle CarGroupSingle");
            }
        }

        function QuickSelectMultiple() {
            var evt = window.event;
            var selectedPanel = $("#tabbedPanel div.ui-tabs-panel:not(.ui-tabs-hide)");

            var updatePanelId;
            if (selectedPanel.index() == 1) {
                updatePanelId = "<%= MinCommercialSegment.GetUpdatePanelId %>";
            }

            if (selectedPanel.index() == 2) {
                updatePanelId = "<%= MaxFleetFactors.GetUpdatePanelId %>";
            }

            if (selectedPanel.index() == 3) {
                updatePanelId = "<%= ucLifecycleHoldingCost.GetUpdatePanelId %>";
            }

            if (selectedPanel.index() == 4) {
                updatePanelId = "<%= ucRevenue.GetUpdatePanelId %>";
            }

            //If Mousedown or enter or tab pressed
            if (evt.type == "mousedown" || ((event.keyCode == 13) || (event.keyCode == 9))) {
                __doPostBack(updatePanelId, "LocationMultiple CarGroupMultiple");
            }
        }

</script>
<script type="text/javascript">
    $(function () {
        var currTab = "<%= ucLifecycleHoldingCost.SelectedPanel %>";
        if (currTab == "0") {
            currTab = "<%= ucRevenue.SelectedPanel %>";
        }
        
        if (currTab != "0") {
            $("#tabbedPanel").tabs({ selected: currTab });
        } else {
            $("#tabbedPanel").tabs();
        }
    });
</script>
</asp:Content>
