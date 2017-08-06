<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HistoricalTrendChart.ascx.cs" Inherits="Mars.App.UserControls.Phase4.Availability.HistoricalTrendChart" %>

<asp:HiddenField runat="server" ID="hfPercentageValues" Value="Values"/>
<asp:HiddenField runat="server" ID="hfHiddenSeries" Value=""/>

<asp:HiddenField runat="server" ID="hfHourlySeries" Value="False" />

<asp:HiddenField runat="server" ID="hfShowLegend" Value="True" />

<asp:Chart ID="chrtHistoricalTrend" CssClass="insetBorders" runat="server" Width="1000" Height="440" ImageType="Png" 
            OnClick="chrtHistoricalTrend_Click">
    <Legends>
        <asp:Legend Name="Legend1" Enabled="false" Docking="Top" Alignment="Center" />
        <asp:Legend Name="SlideLegend" Docking="Right" Enabled="True" />
        <asp:Legend Name="RightLegend" Docking="Right" Enabled="True" />
        <asp:Legend Name="LeftLegend" Docking="Left" Enabled="True" Alignment="Center" />
    </Legends>
    <ChartAreas>
    </ChartAreas>
</asp:Chart>
   