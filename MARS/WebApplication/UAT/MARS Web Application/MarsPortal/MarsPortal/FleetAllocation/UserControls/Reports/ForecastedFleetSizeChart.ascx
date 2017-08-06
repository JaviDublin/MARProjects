<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ForecastedFleetSizeChart.ascx.cs" Inherits="Mars.FleetAllocation.UserControls.Reports.ForecastedFleetSizeChart" %>

<asp:HiddenField runat="server" ID="hfShowLegend" Value="True" />
<asp:HiddenField runat="server" ID="hfValues" Value="True"/>

<asp:Chart ID="chrtForecastedFleetSize" runat="server" Width="890" Height="350" ImageType="Png" 
            OnClick="chrtForecastedFleetSize_Click">
    <Legends>
        <asp:Legend Name="Legend1" Enabled="false" Docking="Top" Alignment="Center" />
        <asp:Legend Name="SlideLegend" Docking="Right" Enabled="True" />
        <asp:Legend Name="RightLegend" Docking="Right" Enabled="True" />
        <asp:Legend Name="LeftLegend" Docking="Left" Enabled="True" Alignment="Center" />
    </Legends>
    <ChartAreas>
    </ChartAreas>
</asp:Chart>