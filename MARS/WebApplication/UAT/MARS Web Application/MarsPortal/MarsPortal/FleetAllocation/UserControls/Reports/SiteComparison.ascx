<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SiteComparison.ascx.cs" Inherits="Mars.FleetAllocation.UserControls.Reports.SiteComparison" %>

<%@ Register Src="~/FleetAllocation/UserControls/AutoGrid.ascx" TagPrefix="uc" TagName="AutoGrid" %>
<%@ Register Src="~/FleetAllocation/UserControls/Reports/SiteComparisonChart.ascx" TagPrefix="uc" TagName="SiteComparisonChart" %>


<table style="height: 360px; text-align: center; margin-left: auto; margin-right: auto;">
        <tr style="margin-top: 0; margin-bottom: auto; vertical-align: top;">
            <td>
                <div id="siteComparisonGridPanel" class="FaoReportPanel"
                    style="width: 900px; margin-left: auto; margin-right: auto; text-align: left; border-style: inset;
                        background-color: transparent;">
                    <ul>
                        <li><a href="#SiteCompChart">Chart</a></li>
                        <li><a href="#SiteCompGrid">Grid</a></li>
                    </ul>
                    <table style="height: 400px; width: 900px; text-align: center;
                                margin-left: auto; margin-right: auto;" >
                        <tr>
                            <td style="margin-top: 0; margin-bottom: auto; vertical-align: top;">
                                <div id="SiteCompChart">
                                    <uc:SiteComparisonChart runat="server" id="ucSiteComparisonChart" />
                                </div>
                            </td>
                            <td style="vertical-align: top;" >
                                <div id="SiteCompGrid">
                                    <uc:AutoGrid runat="server" ID="agSiteComparison" AutoGridWidth="500" 
                                        ShowSideExportButton="True"
                                        ExportDataFileName="FAO Site Comparison" />
                                </div>
                            </td>
                            </tr>
                    </table>     
                </div>
            </td>
        </tr>
    </table>
   