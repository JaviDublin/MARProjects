<%@ Page Title="" Language="C#" MasterPageFile="~/App.MasterPages/Application.Master"
    AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="App.AvailabilityTool.Statistics.Default" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="ContentPlaceholderHead" runat="server">
</asp:Content>
<asp:Content ID="ContentMain" ContentPlaceHolderID="ContentPlaceholderMainContent"
    runat="server">
    <%-- Update Panel --%>
    <asp:UpdatePanel ID="UpdatePanelStatistics" runat="server" UpdateMode="Conditional"
        ChildrenAsTriggers="false">
        <ContentTemplate>
            <%-- Control Wapper --%>
            <div class="divControlContent">
                <%-- Page Information --%>
                <uc:PageInformation ID="UserControlPageInformation" runat="server" />
                <%-- Report Selections --%>
                <uc:ReportSelection ID="UserControlReportSelections" runat="server" />
                <%-- Statistics Gridview Results --%>
                <div class="divMARSStatistics">
                    <div class="divMARSStatisticsGridview">
                        <asp:Panel ID="PanelStatisticsSelection" runat="server" CssClass="panelStatistics"
                            Visible="false">
                            <table cellpadding="0" cellspacing="0" class="tableStatisticsGridview">
                                <tr>
                                    <td>
                                        <asp:GridView ID="GridviewStatisticsSelection" runat="server" AllowSorting="True"
                                            AutoGenerateColumns="False" OnRowCreated="GridviewStatisticsSelection_RowCreated"
                                            OnSorting="GridviewStatisticsSelection_Sorting" CssClass="GridViewStyle">
                                            <HeaderStyle CssClass="GridHeaderStyle" />
                                            <AlternatingRowStyle CssClass="GridAlterenatingRowStyle" />
                                            <RowStyle CssClass="GridRowStyle" />
                                            <EditRowStyle CssClass="GridEditRowStyle" />
                                            <Columns>
                                                <asp:BoundField DataField="header" HeaderText="SELECTION" ReadOnly="True" SortExpression="header" />
                                                <asp:BoundField DataField="fleetStatus" HeaderText="FLEET STATUS" ReadOnly="True"
                                                    SortExpression="fleetStatus">
                                                    <ItemStyle Width="100px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="historicalTrend" HeaderText="HISTORICAL TREND" ReadOnly="True"
                                                    SortExpression="historicalTrend">
                                                    <ItemStyle Width="150px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="sitecomparison" HeaderText="SITE COMPARISON" ReadOnly="True"
                                                    SortExpression="sitecomparison">
                                                    <ItemStyle Width="150px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="fleetcomparison" HeaderText="FLEET COMPARISON" ReadOnly="True"
                                                    SortExpression="fleetcomparison">
                                                    <ItemStyle Width="150px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="kpi" HeaderText="KPI" SortExpression="kpi" ReadOnly="True">
                                                    <ItemStyle Width="65px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="kpiDownload" ReadOnly="True" HeaderText="KPI DOWNLOAD"
                                                    SortExpression="kpiDownload">
                                                    <ItemStyle Width="100px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="carSearch" ReadOnly="True" HeaderText="CAR SEARCH" SortExpression="carSearch">
                                                    <ItemStyle Width="100px" />
                                                </asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                        <asp:GridView ID="GridviewStatisticsSelectionPooling" runat="server" AllowSorting="True"
                                            AutoGenerateColumns="False" OnRowCreated="GridviewStatisticsSelection_RowCreated"
                                            OnSorting="GridviewStatisticsSelection_Sorting" CssClass="GridViewStyle">
                                            <HeaderStyle CssClass="GridHeaderStyle" />
                                            <AlternatingRowStyle CssClass="GridAlterenatingRowStyle" />
                                            <RowStyle CssClass="GridRowStyle" />
                                            <EditRowStyle CssClass="GridEditRowStyle" />
                                            <Columns>
                                                <asp:BoundField DataField="header" HeaderText="SELECTION" ReadOnly="True" SortExpression="header" />
                                                <asp:BoundField DataField="status" HeaderText="STATUS" ReadOnly="True"
                                                    SortExpression="Status">
                                                    <ItemStyle Width="100px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Alerts" HeaderText="Alerts" ReadOnly="True"
                                                    SortExpression="Alerts">
                                                    <ItemStyle Width="150px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="sitecomparison" HeaderText="SITE COMPARISON" ReadOnly="True"
                                                    SortExpression="sitecomparison">
                                                    <ItemStyle Width="150px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="fleetcomparison" HeaderText="FLEET COMPARISON" ReadOnly="True"
                                                    SortExpression="fleetcomparison">
                                                    <ItemStyle Width="150px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Reservations" HeaderText="Reservations" SortExpression="Reservations" ReadOnly="True">
                                                    <ItemStyle Width="65px" />
                                                </asp:BoundField>
                                                
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="GridviewStatisticsSelectionTotals" runat="server" AutoGenerateColumns="false"
                                            ShowHeader="False" CssClass="GridViewStyleStatistics">
                                            <HeaderStyle CssClass="GridHeaderStyleStatistics" />
                                            <RowStyle CssClass="GridRowStyleStatistics" />
                                            <EditRowStyle CssClass="GridEditRowStyleStatistics" />
                                            <Columns>
                                                <asp:BoundField DataField="header" ReadOnly="True" SortExpression="header" />
                                                <asp:BoundField DataField="fleetStatus" ReadOnly="True" SortExpression="fleetStatus">
                                                    <ItemStyle Width="100px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="historicalTrend" ReadOnly="True" SortExpression="historicalTrend">
                                                    <ItemStyle Width="150px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="sitecomparison" ReadOnly="True" SortExpression="sitecomparison">
                                                    <ItemStyle Width="150px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="fleetcomparison" ReadOnly="True" SortExpression="fleetcomparison">
                                                    <ItemStyle Width="150px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="kpi" SortExpression="kpi" ReadOnly="True">
                                                    <ItemStyle Width="65px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="kpiDownload" ReadOnly="True" SortExpression="kpiDownload">
                                                    <ItemStyle Width="100px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="carSearch" ReadOnly="True" SortExpression="carSearch">
                                                    <ItemStyle Width="100px" />
                                                </asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                        <asp:GridView ID="GridviewStatisticsSelectionTotalsPooling" runat="server" AutoGenerateColumns="false"
                                            ShowHeader="False" CssClass="GridViewStyleStatistics">
                                            <HeaderStyle CssClass="GridHeaderStyleStatistics" />
                                            <RowStyle CssClass="GridRowStyleStatistics" />
                                            <EditRowStyle CssClass="GridEditRowStyleStatistics" />
                                            <Columns>
                                                <asp:BoundField DataField="header" ReadOnly="True" SortExpression="header" />
                                                <asp:BoundField DataField="Status" ReadOnly="True" SortExpression="Status">
                                                    <ItemStyle Width="100px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Alerts" ReadOnly="True" SortExpression="Alerts">
                                                    <ItemStyle Width="150px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="sitecomparison" ReadOnly="True" SortExpression="sitecomparison">
                                                    <ItemStyle Width="150px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="fleetcomparison" ReadOnly="True" SortExpression="fleetcomparison">
                                                    <ItemStyle Width="150px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Reservations" SortExpression="Reservations" ReadOnly="True">
                                                    <ItemStyle Width="65px" />
                                                </asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rowPager">
                                        <div class="divPagerRecordsSelected">
                                            <asp:Label ID="LabelTotalRecordsStatisticsSelection" runat="server" Text="<%$ Resources:lang, LabelTotalRecords %>"></asp:Label>
                                            <asp:Label ID="LabelTotalRecordsDisplayStatisticsSelection" runat="server" CssClass="labelRecordsSelected"></asp:Label>
                                        </div>
                                        <div class="divPagerControl">
                                            <uc:PagerControl ID="PagerControlStatisticsSelection" runat="server" OnDropDownListPageSelectedIndexChanged="DropDownListPageSelection_SelectedIndexChanged"
                                                OnDropDownListRowsSelectedIndexChanged="DropDownListRowsSelection_SelectedIndexChanged"
                                                OnPageIndexCommand="GetPageIndexSelection" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="divMARSStatisticsInformation">
                                <asp:Label ID="LabelStatisticsInformation" runat="server" Text="<%$ Resources:lang, StatisticsInfo %>"></asp:Label>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="PanelStatisticsSelectionEmptyDataTemplate" runat="server" CssClass="panelStatisticsNoData"
                            Visible="false">
                            <uc:EmptyDataTemplate ID="EmptyDataTemplateStatisticsSelection" runat="server" />
                        </asp:Panel>
                    </div>
                    <div class="divMARSStatisticsGridview">
                        <asp:Panel ID="PanelStatisticsDate" runat="server" CssClass="panelStatistics" Visible="false">
                            <table cellpadding="0" cellspacing="0" class="tableStatisticsGridview">
                                <tr>
                                    <td>
                                        <asp:GridView ID="GridviewStatisticsDate" runat="server" AllowSorting="true" AutoGenerateColumns="false"
                                            OnRowCreated="GridviewStatisticsDate_RowCreated" OnSorting="GridviewStatisticsDate_Sorting"
                                            CssClass="GridViewStyle">
                                            <HeaderStyle CssClass="GridHeaderStyle" />
                                            <AlternatingRowStyle CssClass="GridAlterenatingRowStyle" />
                                            <RowStyle CssClass="GridRowStyle" />
                                            <EditRowStyle CssClass="GridEditRowStyle" />
                                            <Columns>
                                                <asp:BoundField DataField="header" HeaderText="REPORT DATE" ReadOnly="True" SortExpression="header" />
                                                <asp:BoundField DataField="fleetStatus" HeaderText="FLEET STATUS" ReadOnly="True"
                                                    SortExpression="fleetStatus">
                                                    <ItemStyle Width="100px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="historicalTrend" HeaderText="HISTORICAL TREND" ReadOnly="True"
                                                    SortExpression="historicalTrend">
                                                    <ItemStyle Width="150px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="sitecomparison" HeaderText="SITE COMPARISON" ReadOnly="True"
                                                    SortExpression="sitecomparison">
                                                    <ItemStyle Width="150px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="fleetcomparison" HeaderText="FLEET COMPARISON" ReadOnly="True"
                                                    SortExpression="fleetcomparison">
                                                    <ItemStyle Width="150px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="kpi" HeaderText="KPI" SortExpression="kpi" ReadOnly="True">
                                                    <ItemStyle Width="65px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="kpiDownload" ReadOnly="True" HeaderText="KPI DOWNLOAD"
                                                    SortExpression="kpiDownload">
                                                    <ItemStyle Width="100px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="carSearch" ReadOnly="True" HeaderText="CAR SEARCH" SortExpression="carSearch">
                                                    <ItemStyle Width="100px" />
                                                </asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                        <asp:GridView ID="GridviewStatisticsDatePooling" runat="server" AllowSorting="true" AutoGenerateColumns="false"
                                             OnSorting="GridviewStatisticsDate_Sorting"
                                            CssClass="GridViewStyle">
                                            <HeaderStyle CssClass="GridHeaderStyle" />
                                            <AlternatingRowStyle CssClass="GridAlterenatingRowStyle" />
                                            <RowStyle CssClass="GridRowStyle" />
                                            <EditRowStyle CssClass="GridEditRowStyle" />
                                            <Columns>
                                                <asp:BoundField DataField="header" HeaderText="REPORT DATE" ReadOnly="True" SortExpression="header" />
                                                <asp:BoundField DataField="Status" HeaderText="STATUS" ReadOnly="True"
                                                    SortExpression="fleetStatus">
                                                    <ItemStyle Width="100px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Alerts" HeaderText="HISTORICAL TREND" ReadOnly="True"
                                                    SortExpression="Alerts">
                                                    <ItemStyle Width="150px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="sitecomparison" HeaderText="SITE COMPARISON" ReadOnly="True"
                                                    SortExpression="sitecomparison">
                                                    <ItemStyle Width="150px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="fleetcomparison" HeaderText="FLEET COMPARISON" ReadOnly="True"
                                                    SortExpression="fleetcomparison">
                                                    <ItemStyle Width="150px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Reservations" HeaderText="Reservations" SortExpression="Reservations" ReadOnly="True">
                                                    <ItemStyle Width="65px" />
                                                </asp:BoundField>

                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="GridviewStatisticsDateTotals" runat="server" AutoGenerateColumns="false"
                                            ShowHeader="False" CssClass="GridViewStyleStatistics">
                                            <HeaderStyle CssClass="GridHeaderStyleStatistics" />
                                            <RowStyle CssClass="GridRowStyleStatistics" />
                                            <EditRowStyle CssClass="GridEditRowStyleStatistics" />
                                            <Columns>
                                                <asp:BoundField DataField="header" ReadOnly="True" SortExpression="header" />
                                                <asp:BoundField DataField="fleetStatus" ReadOnly="True" SortExpression="fleetStatus">
                                                    <ItemStyle Width="100px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="historicalTrend" ReadOnly="True" SortExpression="historicalTrend">
                                                    <ItemStyle Width="150px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="sitecomparison" ReadOnly="True" SortExpression="sitecomparison">
                                                    <ItemStyle Width="150px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="fleetcomparison" ReadOnly="True" SortExpression="fleetcomparison">
                                                    <ItemStyle Width="150px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="kpi" SortExpression="kpi" ReadOnly="True">
                                                    <ItemStyle Width="65px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="kpiDownload" ReadOnly="True" SortExpression="kpiDownload">
                                                    <ItemStyle Width="100px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="carSearch" ReadOnly="True" SortExpression="carSearch">
                                                    <ItemStyle Width="100px" />
                                                </asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                        <asp:GridView ID="GridviewStatisticsDateTotalsPooling" runat="server" AutoGenerateColumns="false"
                                            ShowHeader="False" CssClass="GridViewStyleStatistics">
                                            <HeaderStyle CssClass="GridHeaderStyleStatistics" />
                                            <RowStyle CssClass="GridRowStyleStatistics" />
                                            <EditRowStyle CssClass="GridEditRowStyleStatistics" />
                                            <Columns>
                                                <asp:BoundField DataField="header" ReadOnly="True" SortExpression="header" />
                                                <asp:BoundField DataField="status" ReadOnly="True" SortExpression="status">
                                                    <ItemStyle Width="100px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="alerts" ReadOnly="True" SortExpression="alerts">
                                                    <ItemStyle Width="150px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="sitecomparison" ReadOnly="True" SortExpression="sitecomparison">
                                                    <ItemStyle Width="150px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="fleetcomparison" ReadOnly="True" SortExpression="fleetcomparison">
                                                    <ItemStyle Width="150px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="reservations" SortExpression="reservations" ReadOnly="True">
                                                    <ItemStyle Width="65px" />
                                                </asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rowPager">
                                        <div class="divPagerRecordsSelected">
                                            <asp:Label ID="LabelTotalRecordsStatisticsDate" runat="server" Text="<%$ Resources:lang, LabelTotalRecords %>"></asp:Label>
                                            <asp:Label ID="LabelTotalRecordsDisplayStatisticsDate" runat="server" CssClass="labelRecordsSelected"></asp:Label>
                                        </div>
                                        <div class="divPagerControl">
                                            <uc:PagerControl ID="PagerControlStatisticsDate" runat="server" OnDropDownListPageSelectedIndexChanged="DropDownListPageDate_SelectedIndexChanged"
                                                OnDropDownListRowsSelectedIndexChanged="DropDownListRowsDate_SelectedIndexChanged"
                                                OnPageIndexCommand="GetPageIndexDate" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="PanelStatisticsDateEmptyDataTemplate" runat="server" CssClass="panelStatisticsNoData"
                            Visible="false">
                            <uc:EmptyDataTemplate ID="EmptyDataTemplateStatisticsDate" runat="server" />
                        </asp:Panel>
                    </div>
                </div>
            </div>
            <table>
                <tr>
                    <td>
                        Section:
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlAvailabilityOrPooling" Enabled="False">
                            <asp:ListItem Value="3" Text="Availability" Selected="True" />
                            <asp:ListItem Value="4" Text="Pooling" />
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <%-- Report Settings --%>
            <uc:ReportSettings ID="UserControlReportSettings" runat="server" OnGenerateReport="ButtonGenerateReport_Click" />
            <%-- Confirmation Modal Dialog --%>
            <uc:ModalConfirm ID="ModalConfirmStatistics" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress runat="server" ID="upChartProgress" AssociatedUpdatePanelID="UpdatePanelStatistics"
        DisplayAfter="1000">
        <ProgressTemplate>
            <uc:LoadingScreen ID="clsLoadingScreen" runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
