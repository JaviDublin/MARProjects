<%@ Control Language="C#" AutoEventWireup="true" Inherits="App.UserControls.Charting.ChartControls"
    CodeBehind="ChartControls.ascx.cs" %>
<div id="div_chartcontrol" class="div_class_chartcontrol">
    <table border="0" width="950">
        <tr>
            <td align="left" valign="bottom" width="33%">
                <asp:Label ID="lblDataUpdated" runat="server" />
            </td>
            <td width="33%" align="center">
                <asp:Button ID="lblOptions" Text="<%$ Resources:LocalizedChartControl, ChartOptionsTitle %>"
                    runat="server" CssClass="chartbuttonoptions" Width="140px" BorderStyle="Outset"
                    BorderColor="AliceBlue" />
                <asp:Button ID="btnShowGridData" Text="Show Grid" runat="server" CssClass="chartbuttonoptions"
                    Width="140px" BorderStyle="Outset" OnClick="btnShowGridData_Click" BorderColor="AliceBlue" />
            </td>
            <td width="33%">
                &nbsp;
            </td>
        </tr>
    </table>
    <asp:Panel runat="server" ID="pnlChartOptions" CssClass="panelReportOption" Height="0"
        HorizontalAlign="Left">
        <asp:Panel ID="Panel1" runat="server" CssClass="chartPanelBackground">
            <table id="Table1" runat="server" border="1" border-collapse="collapse" cellpadding="4"
                style="height: 100%">
                <tr>
                    <td style="text-align: center;">
                        <asp:Label runat="server" Text="<%$ Resources:LocalizedChartControl, ChartOptionsChartSize %>" />
                    </td>
                    <td style="text-align: center;">
                        <asp:Label runat="server" Text="<%$ Resources:LocalizedChartControl, ChartOptionsIncreaseYMin %>" />
                    </td>
                    <td style="text-align: center;">
                        <asp:Label runat="server" Text="<%$ Resources:LocalizedChartControl, ChartOptionsChartType %>" />
                    </td>
                </tr>
                <tr>
                    <td rowspan="5">
                        <table>
                            <tr>
                                <td>
                                    <asp:TextBox ID="sldrChartHeight" runat="server" Text="750" AutoPostBack="true" />
                                </td>
                                <td valign="middle">
                                    <asp:Label runat="server" Text="<%$ Resources:LocalizedChartControl, ChartOptionsMonitorSize %>" />
                                    <br />
                                    <asp:DropDownList ID="ddlMonitorSize" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMonitorSize_SelectedIndexChanged">
                                        <asp:ListItem Text="<%$ Resources:LocalizedChartControl, ChartMonitorSizeNormal %>"
                                            Value="0" />
                                        <asp:ListItem Text="<%$ Resources:LocalizedChartControl, ChartMonitorSizeWide %>"
                                            Value="1" />
                                    </asp:DropDownList>
                                    <br />
                                    <br />
                                    <asp:Label ID="lblChartSize" runat="server" Text="" Height="30px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="sldrChartWidth" runat="server" Text="0" AutoPostBack="true" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <asp:TextBox ID="sldrYAxisZoom" runat="server" Text="0" AutoPostBack="true" OnTextChanged="sldrYAxisZoom_TextChanged" />
                        <asp:TextBox ID="tbYAxisSliderValue" runat="server" ReadOnly="true" Width="37px" />
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlGraphTypes" AutoPostBack="true" />
                    </td>
                </tr>
                <tr>
                    <td rowspan="2">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="<%$ Resources:LocalizedChartControl, ChartTypeHighlightWeekends %>" />
                                </td>
                                <td>
                                    <asp:CheckBox runat="server" ID="cbHighlightWeekends" AutoPostBack="true" OnCheckedChanged="cbHighlightWeekends_CheckedChanged" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="<%$ Resources:LocalizedChartControl, ChartOptionsLineBreakX %>" />
                                </td>
                                <td>
                                    <asp:CheckBox runat="server" ID="cbAllowXAxisBreak" AutoPostBack="true" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="<%$ Resources:LocalizedChartControl, ChartOptionsLineBreakY %>" />
                                </td>
                                <td>
                                    <asp:CheckBox runat="server" ID="cbAllowYAxisBreak" AutoPostBack="true" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="text-align: center;">
                        <asp:Label runat="server" Text="<%$ Resources:LocalizedChartControl, ChartOptionsIntervals %>" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlIntervalOptions" AutoPostBack="true" />
                    </td>
                </tr>
            </table>
            <asp:SliderExtender ID="seYMinSlider" runat="server" TargetControlID="sldrYAxisZoom"
                Minimum="0" Maximum="100" Orientation="Horizontal" Steps="100" />
            <asp:SliderExtender ID="seChartWidth" runat="server" TargetControlID="sldrChartWidth"
                Minimum="0" Maximum="3000" Orientation="Horizontal" Steps="4" />
            <asp:SliderExtender ID="xeChartHeight" runat="server" TargetControlID="sldrChartHeight"
                Minimum="0" Maximum="750" Orientation="Vertical" Steps="4" />
        </asp:Panel>
    </asp:Panel>
    <table border="0">
        <tr>
            <td align="left" style="border-style: Outset; border-color: Aliceblue; background-color: White">
                <asp:Panel ID="pnlChartHolder" runat="server" ScrollBars="Auto" Width="950" Height="360">
                    <asp:Panel ID="pnlGridData" runat="server" Visible="False">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:GridView runat="server" ID="gvChartData" AutoGenerateColumns="True" AllowSorting="True"
                                    OnSorting="gridViewSorting" CssClass="GridViewStyle" >
                                    <HeaderStyle CssClass="GridHeaderStyle" />
                                    <RowStyle CssClass="GridRowStyleSizing" />
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:GridView>
                                <asp:HiddenField runat="server" ID="hfSortDirection" Value="1" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                    <asp:Chart ID="Chart1" runat="server" OnClick="Chart1_Click" Width="950" Height="355"
                        ImageType="Png">
                        <Legends>
                            <asp:Legend Name="Legend1" Enabled="false" Docking="Top" Alignment="Center" />
                            <asp:Legend Name="Legend2" Docking="Right" Enabled="True" />
                        </Legends>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea1" />
                        </ChartAreas>
                        <Titles>
                            <asp:Title Name="ParameterTitle" Docking="Top" Alignment="TopCenter" />
                            <asp:Title Name="ParameterTitleAdditional" Docking="Top" Alignment="TopCenter" />
                        </Titles>
                    </asp:Chart>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <table border="0" width="950">
        <tr>
            <td align="center">
                <asp:Button ID="btnRefreshChart" runat="server" CssClass="chartbuttonoptions" Text="<%$ Resources:LocalizedChartControl, ChartButtonRefresh %>"
                    OnClick="btnRefreshChart_click" ValidationGroup="Dates" />
            </td>
        </tr>
    </table>
    <asp:CollapsiblePanelExtender runat="server" ID="cpeChartOptionsCollapser" Collapsed="true"
        CollapseControlID="lblOptions" ExpandControlID="lblOptions" TargetControlID="pnlChartOptions"
        ExpandDirection="Vertical" ExpandedSize="0" />
</div>
<script type="text/javascript">
    var xPos, yPos;
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    var optionsChanged;
    prm.add_beginRequest(BeginRequestHandler);
    prm.add_endRequest(EndRequestHandler);
    function BeginRequestHandler(sender, args) {
        xPos = $get('<%=pnlChartHolder.ClientID %>').scrollLeft;
        yPos = $get('<%=pnlChartHolder.ClientID %>').scrollRight;
    }
    function EndRequestHandler(sender, args) {
        $get('<%=pnlChartHolder.ClientID %>').scrollLeft = xPos;
        $get('<%=pnlChartHolder.ClientID %>').scrollRight = yPos;
    }
 
</script>
