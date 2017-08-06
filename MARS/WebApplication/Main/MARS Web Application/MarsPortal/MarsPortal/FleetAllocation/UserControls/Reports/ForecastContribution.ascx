<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ForecastContribution.ascx.cs" Inherits="Mars.FleetAllocation.UserControls.Reports.ForecastContribution" %>

<%@ Register Src="~/FleetAllocation/UserControls/Reports/ForecastContributionChart.ascx" TagPrefix="uc" TagName="ForecastContributionChart" %>
<%@ Register Src="~/FleetAllocation/UserControls/AutoGrid.ascx" TagPrefix="uc" TagName="AutoGrid" %>


<table style="height: 360px; text-align: center; margin-left: auto; margin-right: auto;">
        <tr style="margin-top: 0; margin-bottom: auto; vertical-align: top;">
            <td>
                <div id="fleetComparisonGridPanel" class="FaoReportPanel"
                    style="width: 900px; margin-left: auto; margin-right: auto; text-align: left; border-style: inset;
                        background-color: transparent;">
                    <ul>
                        <li><a href="#FleetCompChart">Chart</a></li>
                        <li><a href="#FleetCompGrid">Grid</a></li>
                    </ul>
                    <table style="height: 400px; width: 900px; text-align: center;
                                margin-left: auto; margin-right: auto;" >
                        <tr>
                            <td style="margin-top: 0; margin-bottom: auto; vertical-align: top;">
                                <div id="FleetCompChart">
                                    <uc:ForecastContributionChart runat="server" id="ucForecastContributionChart" />
                                </div>
                            </td>
                            <td style="vertical-align: top;" >
                                <div id="FleetCompGrid">
                                    <uc:AutoGrid runat="server" ID="agForecastContribution" AutoGridWidth="500" 
                                        ShowSideExportButton="True" ExportDataFileName="FAO Forecast Contribution" />
                                </div>
                            </td>
                            </tr>
                    </table>     
                </div>
            </td>
        </tr>
    </table>