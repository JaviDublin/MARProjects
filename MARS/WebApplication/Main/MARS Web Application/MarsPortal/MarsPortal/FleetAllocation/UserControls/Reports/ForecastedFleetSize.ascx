<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ForecastedFleetSize.ascx.cs" Inherits="Mars.FleetAllocation.UserControls.ForecastedFleetSize" %>

<%@ Register Src="~/FleetAllocation/UserControls/Reports/ForecastedFleetSizeChart.ascx" TagPrefix="uc" TagName="ForecastedFleetSizeChart" %>
<%@ Register Src="~/FleetAllocation/UserControls/AutoGrid.ascx" TagPrefix="uc" TagName="AutoGrid" %>

<asp:HiddenField runat="server" ID="hfValues" Value="True"/>

<table style="height: 360px; width: 100%; text-align: center; margin-left: auto; margin-right: auto; " >
        <tr style="margin-top: 0; margin-bottom: auto; vertical-align: top;">
            <td>
                <div id="forecastedFleetSizeGridPanel" class="FaoReportPanel"
                    style="width: 930px; margin-left: auto; margin-right: auto; text-align: left; border-style: inset;
                        background-color: transparent;">
                    <ul>
                        <li style="float: right !important;">
                            <asp:UpdatePanel runat="server" ID="upnlUpdatedTime" UpdateMode="Conditional">
                                <ContentTemplate>
                                    Last Update: &nbsp;
                                        <asp:Label ID="lblLastUpdate" runat="server" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </li>
                        <li><a href="#ForecastChart">Chart</a></li>
                        <li><a href="#ForecastGrid">Grid</a></li>
      
                    </ul>
                    <table style="height: 400px; width: 900px; text-align: center; 
                                margin-left: auto; margin-right: auto;">
                        <tr>
                            <td style="vertical-align: top;">
                                <div id="ForecastChart" style="background-color: #dcdcdc;">
                                    <uc:ForecastedFleetSizeChart runat="server" id="ForecastedFleetSizeChart" />
                                </div>
                            </td>
                            <td style="vertical-align: top;" >
                                <div id="ForecastGrid">
                                    <table>
                                        <tr>
                                            <td>
                                                <uc:AutoGrid runat="server" ID="agForecastFleetSizeGridA" AutoGridWidth="500" 
                                                        ShowSideExportButton="True"
                                                        ExportDataFileName="FAO Forecasted Fleet Size"
                                                     />            
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <uc:AutoGrid runat="server" ID="agForecastFleetSizeGridB" AutoGridWidth="500"  
                                                    ShowSideExportButton="True"
                                                    ExportDataFileName="FAO Forecasted Fleet Size"/>
                                            </td>
                                        </tr>
                                    </table>
                                    
                                </div>
                            </td>
                            </tr>
                    </table>     
                </div>
            </td>
        </tr>
    </table>
