<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FleetStatusChart.ascx.cs" Inherits="Mars.App.UserControls.Phase4.Availability.FleetStatusChart" %>

<asp:HiddenField runat="server" ID="hfShowLegend" Value="True"/>
<asp:HiddenField runat="server" ID="hfTodaysData" Value="True" />

<asp:Chart ID="chrtFleetStatus" CssClass="insetBorders" runat="server" Width="1000" Height="410" ImageType="Png" 
            OnClick="chrtFleetStatus_Click"  >
    
    <Legends>
        <asp:Legend Name="Legend1" Enabled="false" Docking="Top" Alignment="Center" />
        <asp:Legend Name="SlideLegend" Docking="Right" Enabled="True" />
        <asp:Legend Name="RightLegend" Docking="Right" Enabled="True" />
                    
        <asp:Legend Name="LeftLegend" Docking="Left" Enabled="True" Alignment="Center" />
    </Legends>
    <ChartAreas>
    </ChartAreas>
</asp:Chart>
