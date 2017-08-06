<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReportSelections.ascx.cs" Inherits="App.UserControls.Reports.ReportSelections" %>
<%--Report Selection Statistics Tool --%>
<asp:Panel ID="PanelStatisticsReportSelection" runat="server" CssClass="panelReportSelection" Visible="false">
    <table class="tableReportSelection">
        <tr>
            <td class="labelInformationReportSelection">
                <asp:Label ID="LabelStatisticsCountry" runat="server" Text="<%$ Resources:lang, ReportSelectionCountry %>"></asp:Label>
            </td>
            <td class="labelDisplayReportSelection">
                <asp:Label ID="LabelStatisticsCountrySelection" runat="server"></asp:Label>
            </td>
            <td class="labelInformationReportSelection">
                <asp:Label ID="LabelStatisticsOPSRegion" runat="server" Text="<%$ Resources:lang, ReportSelectionOPSRegion %>"></asp:Label>
                <asp:Label ID="LabelStatisticsCMSPool" runat="server" Text="<%$ Resources:lang, ReportSelectionCMSPool %>"></asp:Label>
            </td>
            <td class="labelDisplayReportSelection">
                <asp:Label ID="LabelStatisticsOPSRegionSelection" runat="server"></asp:Label>
                <asp:Label ID="LabelStatisticsCMSPoolSelection" runat="server"></asp:Label>
            </td>
            <td class="labelInformationReportSelection">
                <asp:Label ID="LabelStatisticsStartDate" runat="server" Text="<%$ Resources:lang, ReportSelectionDateRangeStart %>"></asp:Label>
            </td>
            <td class="labelDisplayReportSelection">
                <asp:Label ID="LabelStatisticsStartDateSelection" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
            </td>
            <td class="labelInformationReportSelection">
                <asp:Label ID="LabelStatisticsOPSArea" runat="server" Text="<%$ Resources:lang, ReportSelectionOPSArea %>"></asp:Label>
                <asp:Label ID="LabelStatisticsCMSLocationGroup" runat="server" Text="<%$ Resources:lang, ReportSelectionCMSLocationGroup %>"></asp:Label>
            </td>
            <td class="labelDisplayReportSelection">
                <asp:Label ID="LabelStatisticsOPSAreaSelection" runat="server"></asp:Label>
                <asp:Label ID="LabelStatisticsCMSLocationGroupSelection" runat="server"></asp:Label>
            </td>
            <td class="labelInformationReportSelection">
                <asp:Label ID="LabelStatisticsEndDate" runat="server" Text="<%$ Resources:lang, ReportSelectionDateRangeEnd %>"></asp:Label>
            </td>
            <td class="labelDisplayReportSelection">
                <asp:Label ID="LabelStatisticsEndDateSelection" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
            </td>
            <td class="labelInformationReportSelection">
                <asp:Label ID="LabelStatisticsBranch" runat="server" Text="<%$ Resources:lang, ReportSelectionLocation %>"></asp:Label>
            </td>
            <td class="labelDisplayReportSelection">
                <asp:Label ID="LabelStatisticsBranchSelection" runat="server"></asp:Label>
            </td>
            <td class="labelInformationReportSelection">
                <asp:Label ID="LabelStatisticsRacfid" runat="server" Text="<%$ Resources:lang, ReportSettingsRACFID %>"></asp:Label>
            </td>
            <td class="labelDisplayReportSelection">
                <asp:Label ID="LabelStatisticsRacfidSelection" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    <hr />
</asp:Panel>
<%-- Report Selection Availability --%>
<asp:Panel ID="PanelAvailabilityReportSelection" runat="server" CssClass="panelReportSelection" Visible="false">
    <table class="tableReportSelection">
        <tr>
            <td class="labelInformationReportSelection">
                <asp:Label ID="LabelAvailabilityCountry" runat="server" Text="<%$ Resources:lang, ReportSelectionCountry %>"></asp:Label>
            </td>
            <td class="labelDisplayReportSelection">
                <asp:Label ID="LabelAvailabilityCountrySelection" runat="server"></asp:Label>
            </td>
            <td class="labelInformationReportSelection">
                <asp:Label ID="LabelAvailabilityOPSRegion" runat="server" Text="<%$ Resources:lang, ReportSelectionOPSRegion %>"></asp:Label>
                <asp:Label ID="LabelAvailabilityCMSPool" runat="server" Text="<%$ Resources:lang, ReportSelectionCMSPool %>"></asp:Label>
            </td>
            <td class="labelDisplayReportSelection">
                <asp:Label ID="LabelAvailabilityOPSRegionSelection" runat="server"></asp:Label>
                <asp:Label ID="LabelAvailabilityCMSPoolSelection" runat="server"></asp:Label>
            </td>
            <td class="labelInformationReportSelection">
                <asp:Label ID="LabelAvailabilityCarSegment" runat="server" Text="<%$ Resources:lang, ReportSelectionCarSegment %>"></asp:Label>
            </td>
            <td class="labelDisplayReportSelection">
                <asp:Label ID="LabelAvailabilityCarSegmentSelection" runat="server"></asp:Label>
            </td>
            <td class="labelInformationReportSelection">
                <asp:Label ID="LabelAvailabilityStartDate" runat="server" Text="<%$ Resources:lang, ReportSelectionDateRangeStart %>"></asp:Label>
            </td>
            <td class="labelDisplayReportSelection">
                <asp:Label ID="LabelAvailabilityStartDateSelection" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
            </td>
            <td class="labelInformationReportSelection">
                <asp:Label ID="LabelAvailabilityOPSArea" runat="server" Text="<%$ Resources:lang, ReportSelectionOPSArea %>"></asp:Label>
                <asp:Label ID="LabelAvailabilityCMSLocationGroup" runat="server" Text="<%$ Resources:lang, ReportSelectionCMSLocationGroup %>"></asp:Label>
            </td>
            <td class="labelDisplayReportSelection">
                <asp:Label ID="LabelAvailabilityOPSAreaSelection" runat="server"></asp:Label>
                <asp:Label ID="LabelAvailabilityCMSLocationGroupSelection" runat="server"></asp:Label>
            </td>
            <td class="labelInformationReportSelection">
                <asp:Label ID="LabelAvailabilityCarClass" runat="server" Text="<%$ Resources:lang, ReportSelectionCarClass %>"></asp:Label>
            </td>
            <td class="labelDisplayReportSelection">
                <asp:Label ID="LabelAvailabilityCarClassSelection" runat="server"></asp:Label>
            </td>
            <td class="labelInformationReportSelection">
                <asp:Label ID="LabelAvailabilityDayOfWeek" runat="server" Text="<%$ Resources:lang, ReportSettingsDayOfWeek %>"></asp:Label>
            </td>
            <td class="labelDisplayReportSelection">
                <asp:Label ID="LabelAvailabilityDayOfWeekSelection" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
            </td>
            <td class="labelInformationReportSelection">
                <asp:Label ID="LabelAvailabilityBranch" runat="server" Text="<%$ Resources:lang, ReportSelectionLocation %>" Visible="false"></asp:Label>
            </td>
            <td class="labelDisplayReportSelection">
                <asp:Label ID="LabelAvailabilityBranchSelection" runat="server" Visible="false"></asp:Label>
            </td>
            <td class="labelInformationReportSelection">
                <asp:Label ID="LabelAvailabilityCarGroup" runat="server" Text="<%$ Resources:lang, ReportSelectionCarGroup %>" Visible="false"></asp:Label>
            </td>
            <td class="labelDisplayReportSelection">
                <asp:Label ID="LabelAvailabilityCarGroupSelection" runat="server" Visible="false"></asp:Label>
            </td>
            <td class="labelInformationReportSelection">
                <asp:Label ID="LabelAvailabilityDateRange" runat="server" Text="<%$ Resources:lang, ReportSettingsDateRange %>"></asp:Label>
            </td>
            <td class="labelDisplayReportSelection">
                <asp:Label ID="LabelAvailabilityDateRangeSelection" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    <hr />
</asp:Panel>
