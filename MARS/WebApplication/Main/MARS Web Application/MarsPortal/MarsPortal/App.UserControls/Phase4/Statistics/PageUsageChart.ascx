<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PageUsageChart.ascx.cs" Inherits="Mars.App.UserControls.Phase4.Statistics.PageUsageChart" %>


<asp:HiddenField runat="server" ID="hfShowLegend" Value="True" />

<asp:Chart ID="chrtPageUseageComparison" runat="server" Width="890" Height="350" ImageType="Png" 
            OnClick="chrtPageUseageComparison_Click">
    <Legends>
        <asp:Legend Name="Legend1" Enabled="false" Docking="Top" Alignment="Center" />
        <asp:Legend Name="SlideLegend" Docking="Right" Enabled="True" />
        <asp:Legend Name="RightLegend" Docking="Right" Enabled="True" />
        <asp:Legend Name="LeftLegend" Docking="Left" Enabled="True" Alignment="Center" />
    </Legends>
    <ChartAreas>
    </ChartAreas>
</asp:Chart>