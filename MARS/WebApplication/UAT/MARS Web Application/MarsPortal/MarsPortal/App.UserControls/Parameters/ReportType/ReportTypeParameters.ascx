<%@ Control Language="C#" AutoEventWireup="true" Inherits="App.UserControls.Parameters.ReportTypeParameters"
    CodeBehind="ReportTypeParameters.ascx.cs" %>
<asp:Panel runat="server" ID="pnlReportTypes">
    <fieldset style="height: 130px">
        <legend>
            <asp:Label runat="server" Text="<%$ Resources:LocalizedParameterControl, ReportTypeLegend %>" /></legend>
        <asp:Table ID="tblReportParameters" Width="100%" runat="server">
            <asp:TableRow ID="trTimeZone" Visible="false">
                <asp:TableCell>
                    <asp:Label ID="lblReportingTimezone"  runat="server" Text="<%$ Resources:LocalizedParameterControl, ReportTypeTimezone %>" />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:DropDownList ID="ddlReportingTimeZone" Enabled="False" runat="server" Width="140" OnSelectedIndexChanged="ReportTypeChanged" AutoPostBack="true" />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="trForecastType" Visible="false" >
                <asp:TableCell>
                    <asp:Label ID="lblForecastType" runat="server" Text="<%$ Resources:LocalizedParameterControl, ReportTypeForecastType %>" />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:DropDownList ID="ddlForecastType" runat="server" Width="140" OnSelectedIndexChanged="ReportTypeChanged" />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="trTopic" Visible="false">
                <asp:TableCell>
                    <asp:Label ID="lblTopic" runat="server" Text="<%$ Resources:LocalizedParameterControl, ReportTypeTopic %>" />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:DropDownList ID="ddlTopic" runat="server" Width="190" OnSelectedIndexChanged="ReportTypeChanged" />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="trFleetPlan" Visible="false">
                <asp:TableCell>
                    <asp:Label ID="lblfleetPlan" runat="server" Text="<%$ Resources:LocalizedParameterControl, ReportTypeFleepPlan %>" />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:DropDownList ID="ddlFleetPlan" runat="server" Width="140" OnSelectedIndexChanged="ReportTypeChanged" />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="trKpiCalculation" Visible="false">
                <asp:TableCell VerticalAlign="Top">
                    <asp:Label ID="lblKpi" runat="server" Text="<%$ Resources:LocalizedParameterControl, ReportTypeKpi %>" />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:DropDownList ID="ddlKpiCalculation" runat="server" Width="140" OnSelectedIndexChanged="ReportTypeChanged" AutoPostBack="true" />
                    <br />
                    <asp:RadioButtonList runat="server" ID="rblKpiPercentageSelection" RepeatDirection="Horizontal" Visible="false" >
                        <asp:ListItem Text="Value" Value="" Selected="True" />
                        <asp:ListItem Text="Percent" Value="%" />
                    </asp:RadioButtonList>
                </asp:TableCell>
            </asp:TableRow>

            
        </asp:Table>
        <br />
    </fieldset>
</asp:Panel>
