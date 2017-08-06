<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AvailabilityChart.ascx.cs" Inherits="App.UserControls.Charts.AvailabilityChart" %>
<div class="divAvailabilityCharts">
    <dcwc:Chart ID="DundasChartAvailability" runat="server" Width="970px" 
        Height="600px" BorderLineWidth="2" BorderLineStyle="Solid" 
        BorderLineColor="26, 59, 105" 
        OnCallback="DundasChartAvailability_Callback" >
        <BorderSkin SkinStyle="Emboss"></BorderSkin>
        <ChartAreas>
            <dcwc:ChartArea BorderColor="64, 64, 64, 64" ShadowColor="Transparent" BackColor="White" Name="Default">
                <Area3DStyle XAngle="15" YAngle="10" RightAngleAxes="False" Clustered="True" Perspective="10" WallWidth="0"></Area3DStyle>
            </dcwc:ChartArea>
        </ChartAreas>
    </dcwc:Chart>
    <br />&nbsp;&nbsp;
    <asp:ImageButton runat="server" ID="ImageButtonPrint" 
        OnClientClick="javascript:printChart()" 
        ImageUrl="~/App.Images/Icons/Print.png" />
</div>
