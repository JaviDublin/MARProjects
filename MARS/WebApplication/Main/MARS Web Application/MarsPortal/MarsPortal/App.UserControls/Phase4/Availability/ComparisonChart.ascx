<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ComparisonChart.ascx.cs" 
        Inherits="Mars.App.UserControls.Phase4.Availability.ComparisonChart" %>


<asp:HiddenField runat="server" ID="hfSiteComparison" Value="True"/>
<asp:HiddenField runat="server" ID="hfPercentageValues" Value="Values"/>
<asp:HiddenField runat="server" ID="hfShowLegend" Value="True" />

<asp:Chart ID="chrtComparison" CssClass="insetBorders" runat="server" Width="1000" 
            Height="440" ImageType="Png" 
            OnClick="chrtFleetComparison_Click">
    <Legends>
        <asp:Legend Name="Legend1" Enabled="false" Docking="Top" Alignment="Center" />
        <asp:Legend Name="SlideLegend" Docking="Right" Enabled="True" BorderWidth="0" />
        <asp:Legend Name="RightLegend" Docking="Right" Enabled="True" BorderWidth="0" />
        
    </Legends>
    <ChartAreas>
    </ChartAreas>
</asp:Chart>
