<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FleetComparison.ascx.cs" Inherits="Mars.FleetAllocation.UserControls.Reports.FleetComparison" %>


<%@ Register Src="~/FleetAllocation/UserControls/AutoGrid.ascx" TagPrefix="uc" TagName="AutoGrid" %>

<%@ Register Src="~/FleetAllocation/UserControls/Reports/FleetComparisonChart.ascx" TagPrefix="uc" TagName="FleetComparisonChart" %>


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
                                    <uc:FleetComparisonChart runat="server" id="ucFleetComparisonChart" />
                                </div>
                            </td>
                            <td style="vertical-align: top;" >
                                <div id="FleetCompGrid">
                                    <uc:AutoGrid runat="server" ID="agFleetComparison" AutoGridWidth="500" 
                                        ShowSideExportButton="True" ExportDataFileName="FAO Fleet Comparison" />
                                </div>
                            </td>
                            </tr>
                    </table>     
                </div>
            </td>
        </tr>
    </table>
   