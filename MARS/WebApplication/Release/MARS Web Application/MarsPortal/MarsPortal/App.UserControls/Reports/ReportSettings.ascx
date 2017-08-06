<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReportSettings.ascx.cs"
    Inherits="App.UserControls.Reports.ReportSettings" %>
<asp:UpdatePanel ID="UpdatePanelReportSettings" runat="server" ChildrenAsTriggers="false"
    UpdateMode="Conditional">
    <ContentTemplate>
        <div class="divNoPrint">
            <div class="divReportSettingsWrapper">
                <%-- Report Settings Title --%>
                <asp:Panel ID="PanelTitleReportSettings" runat="server" CssClass="panelReportSettingsTitle"
                    Visible="false">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="LabelReportOptions" runat="server" Text="<%$ Resources:lang, ReportSettingsTitle %>"
                                    CssClass="labelReportSettings"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <%-- Report Settings Availability Tool --%>
                <asp:Panel ID="PanelAvailabilityReportSettings" runat="server" CssClass="panelReportSettings"
                    Visible="false" DefaultButton="ButtonAvailabilityGenerateReport">
                    <asp:Panel ID="PanelAvailabilityReportSettingsSearchCriteria" runat="server" CssClass="panelReportSettingsSearchCriteria">
                        <hr />
                        <table class="tableReportSettingsSearchCriteria">
                            <tr>
                                <td class="labelColumnReportSettings">
                                    <asp:Label ID="LabelAvailabilityCountry" runat="server" Text="<%$ Resources:lang, ReportSettingsCountry %>"
                                        CssClass="labelReportSettings"> </asp:Label>
                                </td>
                                <td class="dataColumnReportSettings">
                                    <asp:DropDownList ID="DropDownListAvailabilityCountry" runat="server" AutoPostBack="true"
                                        Width="120px" CssClass="dropDownListReportSettings" DataTextField="country" DataValueField="country"
                                        OnSelectedIndexChanged="DropDownListAvailabilityCountry_SelectedIndexChanged"
                                        ToolTip="<%$ Resources:lang, ToolTipCountry %>">
                                    </asp:DropDownList>
                                </td>
                                <td class="labelColumnReportSettings">
                                    <asp:Label ID="LabelAvailabilityLogic" runat="server" Text="<%$ Resources:lang, ReportSettingsLogic %>"
                                        CssClass="labelReportSettings"></asp:Label>
                                </td>
                                <td class="dataColumnReportSettings">
                                    <asp:RadioButtonList ID="RadioButtonListAvailabilityLogic" runat="server" RepeatDirection="Horizontal"
                                        AutoPostBack="true" ToolTip="<%$ Resources:lang, ToolTipLogic %>" onselectedindexchanged="RadioButtonListAvailabilityLogic_SelectedIndexChanged">
                                        <asp:ListItem Text="<%$ Resources:lang, ReportSettingsLogicCMS %>" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="<%$ Resources:lang, ReportSettingsLogicOPS %>" Value="2"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td class="labelColumnReportSettings">
                                    <asp:Label ID="LabelAvailabilityFleet" runat="server" CssClass="labelReportSettings"
                                        Text="<%$ Resources:lang, ReportSettingsFleet %>"></asp:Label>
                                </td>
                                <td class="dataColumnReportSettings">
                                    <asp:DropDownList ID="DropDownListAvailabilityFleet" runat="server" CssClass="dropDownListReportSettings"
                                        DataValueField="fleet_name" DataTextField="fleet_name" Width="120px">
                                    </asp:DropDownList>
                                </td>
                                <td class="labelColumnReportSettings" style="position:relative">
                                    <asp:Label ID="LabelAvailabilityDate" runat="server" CssClass="labelReportSettings"
                                        Text="<%$ Resources:lang, ReportSettingsDate %>"></asp:Label>
                                    <asp:Label ID="LabelAvailabilityStatus" runat="server" CssClass="labelReportSettings"
                                        Text="<%$ Resources:lang, ReportSettingsStatus %>"></asp:Label>
                                </td>
                                <td class="dataColumnReportSettings">
                                    <uc:DatePickerTextBox ID="DatePickerTextBoxAvailabilityDate" runat="server" />
                                    <uc:PopupCheckBoxList ID="PopupCheckBoxListStatus" runat="server" Visible="false"
                                        ListBoxCSSClass="divPopUpCheckBoxListStatus" ListBoxWidth="150" CheckBoxSelectAllToolTip="<%$ Resources:lang, CheckBoxSelectAllToolTipStatus %>" />                                    
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td class="labelColumnReportSettings">
                                    <asp:Label ID="LabelAvailabilityCMSPool" runat="server" Text="<%$ Resources:lang, ReportSettingsCMSPool %>"
                                        CssClass="labelReportSettings"></asp:Label>
                                    <asp:Label ID="LabelAvailabilityOPSRegion" runat="server" Text="<%$ Resources:lang, ReportSettingsOPSRegion %>"
                                        CssClass="labelReportSettings"></asp:Label>
                                </td>
                                <td class="dataColumnReportSettings">
                                    <asp:DropDownList ID="DropDownListAvailabilityCMSPool" runat="server" AutoPostBack="true"
                                        Width="120px" CssClass="dropDownListReportSettings" DataTextField="cms_pool"
                                        DataValueField="cms_pool_id" ToolTip="<%$ Resources:lang, ToolTipCMSPool %>" onselectedindexchanged="DropDownListAvailabilityCMSPool_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="DropDownListAvailabilityOPSRegion" runat="server" AutoPostBack="true"
                                        Width="120px" CssClass="dropDownListReportSettings" DataTextField="ops_region"
                                        DataValueField="ops_region_id" ToolTip="<%$ Resources:lang, ToolTipOPSRegion %>" onselectedindexchanged="DropDownListAvailabilityOPSRegion_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td class="labelColumnReportSettings">
                                    <asp:Label ID="LabelAvailabilityCarSegement" runat="server" Text="<%$ Resources:lang, ReportSettingsCarSegment %>"
                                        CssClass="labelReportSettings"></asp:Label>
                                </td>
                                <td class="dataColumnReportSettings">
                                    <asp:DropDownList ID="DropDownListAvailabilityCarSegment" runat="server" AutoPostBack="true"
                                        Width="120px" CssClass="dropDownListReportSettings" DataTextField="car_segment"
                                        DataValueField="car_segment_id" ToolTip="<%$ Resources:lang, ToolTipCarSegment %>" onselectedindexchanged="DropDownListAvailabilityCarSegment_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td class="labelColumnReportSettings">
                                    <asp:Label ID="LabelAvailabilityDayOfWeek" runat="server" CssClass="labelReportSettings"
                                        Text="<%$ Resources:lang, ReportSettingsDayOfWeek %>"></asp:Label>
                                    <asp:Label ID="LabelAvailabilityNoRev" runat="server" CssClass="labelReportSettings"
                                        Text="<%$ Resources:lang, ReportSettingsNoRev %>"></asp:Label>
                                    <asp:Label ID="LabelAvailabilityDateRangeKPI" runat="server" CssClass="labelReportSettings"
                                        Text="<%$ Resources:lang, ReportSettingsDateRange %>" Visible="false"></asp:Label>
                                </td>
                                <td class="dataColumnReportSettings">
                                    <asp:DropDownList ID="DropDownListAvailabilityDayOfWeek" runat="server" CssClass="dropDownListReportSettings"
                                        Width="120px">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="DropDownListAvailabilityNoRev" runat="server" CssClass="dropDownListReportSettings"
                                        Width="120px">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="DropDownListAvailabilityDateRangeKPI" runat="server" CssClass="dropDownListReportSettings"
                                        Width="120px" Visible="false" 
                                        onselectedindexchanged="DropDownListAvailabilityDateRange_SelectedIndexChanged" AutoPostBack="true" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td class="labelColumnReportSettings">
                                    <asp:Label ID="LabelAvailabilityCMSLocationGroup" runat="server" Text="<%$ Resources:lang, ReportSettingsCMSLocationGroup %>"
                                        CssClass="labelReportSettings"></asp:Label>
                                    <asp:Label ID="LabelAvailabilityOPSArea" runat="server" Text="<%$ Resources:lang, ReportSettingsOPSArea %>"
                                        CssClass="labelReportSettings"></asp:Label>
                                </td>
                                <td class="dataColumnReportSettings">
                                    <asp:DropDownList ID="DropDownListAvailabilityCMSLocationGroup" runat="server" AutoPostBack="true"
                                        Width="120px" CssClass="dropDownListReportSettings" DataTextField="cms_location_group"
                                        DataValueField="cms_location_group_id" ToolTip="<%$ Resources:lang, ToolTipCMSLocationGroup %>" onselectedindexchanged="DropDownListAvailabilityCMSLocationGroup_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="DropDownListAvailabilityOPSArea" runat="server" AutoPostBack="true"
                                        Width="120px" CssClass="dropDownListReportSettings" DataTextField="ops_area"
                                        DataValueField="ops_area_id" ToolTip="<%$ Resources:lang, ToolTipOPSArea %>" onselectedindexchanged="DropDownListAvailabilityOPSArea_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td class="labelColumnReportSettings">
                                    <asp:Label ID="LabelAvailabilityCarClass" runat="server" Text="<%$ Resources:lang, ReportSettingsCarClass %>"
                                        CssClass="labelReportSettings"></asp:Label>
                                </td>
                                <td class="dataColumnReportSettings">
                                    <asp:DropDownList ID="DropDownListAvailabilityCarClass" runat="server" AutoPostBack="True"
                                        Width="120px" CssClass="dropDownListReportSettings" DataTextField="car_class"
                                        DataValueField="car_class_id" ToolTip="<%$ Resources:lang, ToolTipCarClass %>" onselectedindexchanged="DropDownListAvailabilityCarClass_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td class="labelColumnReportSettings">
                                    <asp:Label ID="LabelAvailabilityDateRange" runat="server" CssClass="labelReportSettings"
                                        Text="<%$ Resources:lang, ReportSettingsDateRange %>"></asp:Label>
                                    <asp:Label ID="LabelAvailabilityLSTWWD" runat="server" CssClass="labelReportSettings"
                                        Text="<%$ Resources:lang, ReportSettingsLSTWWD %>"></asp:Label>
                                </td>
                                <td class="dataColumnReportSettings">
                                    <!-- Altered to add customisation to the date range -->
                                    <asp:DropDownList ID="DropDownListAvailabilityDateRange" runat="server" CssClass="dropDownListReportSettings"
                                        Width="120px" AutoPostBack="true" onselectedindexchanged="DropDownListAvailabilityDateRange_SelectedIndexChanged" />
                                    <asp:RadioButton ID="RadioButtonAvailabilityLSTWWD" runat="server" GroupName="CarSearchSelectBy" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td class="labelColumnReportSettings">
                                    <asp:Label ID="LabelAvailabilityBranch" runat="server" Text="<%$ Resources:lang, ReportSettingsBranch %>"
                                        CssClass="labelReportSettings"></asp:Label>
                                </td>
                                <td class="dataColumnReportSettings">
                                    <asp:DropDownList ID="DropDownListAvailabilityLocations" runat="server" DataTextField="location"
                                        Width="120px" CssClass="dropDownListReportSettings" DataValueField="location"
                                        ToolTip="<%$ Resources:lang, ToolTipLocation %>">
                                    </asp:DropDownList>
                                </td>
                                <td class="labelColumnReportSettings">
                                    <asp:Label ID="LabelAvailabilityCarGroup" runat="server" Text="<%$ Resources:lang, ReportSettingsCarGroup %>"
                                        CssClass="labelReportSettings"></asp:Label>
                                </td>
                                <td class="dataColumnReportSettings">
                                    <asp:DropDownList ID="DropDownListAvailabilityCarGroup" runat="server" DataTextField="car_group"
                                        Width="120px" CssClass="dropDownListReportSettings" DataValueField="car_group_id"
                                        ToolTip="<%$ Resources:lang, ToolTipCarGroup %>">
                                    </asp:DropDownList>
                                </td>
                                <td class="labelColumnReportSettings">
                                    <asp:Label ID="LabelAvailabilityDUEWWD" runat="server" CssClass="labelReportSettings"
                                        Text="<%$ Resources:lang, ReportSettingsDUEWWD %>"></asp:Label>

                                    <!-- Start Date for drop down list selection Custom -->
                                    <asp:Label ID="lbStartDate" runat="server" Text="<%$ Resources:lang, ReportSettingsStartDate %>" Visible="false"  CssClass="labelReportSettings" />
                                    <uc:DatePickerTextBox ID="dptbStartDate" runat="server" Visible="false" />
                                </td>
                                <td class="dataColumnReportSettings">
                                    <asp:RadioButton ID="RadioButtonAvailabilityDUEWWD" runat="server" GroupName="CarSearchSelectBy" />

                                    <!-- Start Date for drop down list selection Custom -->
                                    <asp:Label ID="lbEndDate" runat="server" Text="<%$ Resources:lang, ReportSettingsEndDate %>" Visible="false"  CssClass="labelReportSettings" />
                                    <uc:DatePickerTextBox ID="dptbEndDate" runat="server" Visible="false" />
                                </td>
                            </tr>

                            <%-- Added for CAL filtering of Location for MARSV3 --%>
                            <tr>
                                <td colspan="2"></td>
                                <td class="labelReportSettings">CAL:</td>
                                <td>
                                    <asp:DropDownList ID="DropDownListCAL" runat="server" ToolTip="Select to filter Branches" 
                                        CssClass="dropDownListReportSettings" AutoPostBack="True" 
                                        onselectedindexchanged="DropDownListCAL_SelectedIndexChanged">
                                        <asp:ListItem Selected="True" Value="*">*** All ***</asp:ListItem>
                                        <asp:ListItem Value="C">Corporate</asp:ListItem>
                                        <asp:ListItem Value="A">Agency</asp:ListItem>
                                        <asp:ListItem Value="L">Licensee</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td colspan="2"/>
                                <td>
                                    <asp:Label runat="server" ID="lblOnRentType" Visible="False" Text="On Rent Type:" CssClass="labelReportSettings"  />

                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlOnRentType" Visible="False" Width="120px" CssClass="dropDownListReportSettings">
                                        <asp:ListItem Value="0" Text="On Rent" Selected="True" /> 
                                        <asp:ListItem Value="1" Text="On Rent Max" />
                                        <asp:ListItem Value="2" Text="On Rent Max Absolute" />
                                    </asp:DropDownList>
                                </td>
                                
                            </tr>
                            
                            <tr>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td class="labelColumnReportSettings">
                                    <asp:Label ID="LabelAvailabilityOwnarea" runat="server" CssClass="labelReportSettings"
                                        Text="<%$ Resources:lang, ReportSettingsOwnarea %>"></asp:Label>
                                </td>
                                <td class="dataColumnReportSettings">
                                    <uc:PopupCheckBoxList ID="PopupCheckBoxListAvailabilityOwnArea" runat="server" ListBoxWidth="100"
                                        Visible="false" ListBoxCSSClass="divPopUpCheckBoxList" CheckBoxSelectAllToolTip="<%$ Resources:lang, CheckBoxSelectAllToolTipOwnArea %>" />
                                </td>
                                <td class="labelColumnReportSettings">
                                    <asp:Label ID="LabelAvailabilityModelcode" runat="server" CssClass="labelReportSettings"
                                               Text="<%$ Resources:lang, ReportSettingsModelcode %>" Visible="False" />
                                </td>
                                <td class="dataColumnReportSettings">
                                    <uc:PopupCheckBoxList ID="PopupCheckBoxListAvailabilityModelcode" runat="server"
                                        Visible="false" ListBoxCSSClass="divPopUpCheckBoxList" ListBoxWidth="100" />
                                </td>
                                <td>
                                </td>                                    
                                <td>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:UpdatePanel ID="UpdatePanelAvailabilityReportSettingsFooter" runat="server"
                        ChildrenAsTriggers="false" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel ID="PanelAvailabilityReportSettingsFooter" runat="server" CssClass="panelReportSettingsFooter">
                                <hr />
                                <table class="tableReportSettingsFooter">
                                    <tr>
                                        <td class="buttonsColumnReportSettingsFooter">
                                            <asp:Button ID="ButtonAvailabilityGenerateReport" runat="server" Text="<%$ Resources:lang, ReportSettingsGenerateReport %>" onclick="ButtonAvailabilityGenerateReport_Click" />
                                            <asp:Button ID="ButtonAvailabilityDownloadReport" CssClass="MarsV1GenerateReportButton" runat="server" Text="<%$ Resources:lang, ReportSettingsGenerateReportAndDownload %>" onclick="ButtonAvailabilityDownloadReport_Click" />
                                        </td>
                                        <td>
                                            <asp:Label ID="LabelAvailabilityTopic" runat="server" Text="<%$ Resources:lang, ReportSettingsTopic %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DropDownListAvailabilityTopic" runat="server" AutoPostBack="True"
                                                DataTextField="operstat_desc" DataValueField="operstat_name" Width="170px" onselectedindexchanged="DropDownListAvailabilityTopic_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="DropDownListAvailabilityKPI" runat="server" AutoPostBack="True" onselectedindexchanged="DropDownListAvailabilityKPI_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="RadioButtonListAvailabilityValue" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="<%$ Resources:lang, ReportSettingsValue %>" Value="NUMERIC" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:lang, ReportSettingsPercent %>" Value="PERCENTAGE"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td style="padding-left: 50px;">
                                            <asp:Button ID="ButtonAvailabilityReturnToFleetStatusReport" Visible="false" runat="server"
                                                Text="<%$ Resources:lang, ReturnToFleetStatusReport %>" OnClick="ButtonAvailabilityReturnToFleetStatusReport_Click">
                                            </asp:Button>
                                            <asp:Button ID="ButtonAvailabilityKPIDownload" runat="server" Text="<%$ Resources:lang, ReportSettingsGoToKPIDownload %>" onclick="ButtonAvailabilityKPIDownload_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
                <%-- Report Settings Statistics --%>
                <asp:Panel ID="PanelStatisticsReportSettings" runat="server" CssClass="panelReportSettings"
                    Visible="false" DefaultButton="ButtonStatisticsGenerateReport">
                    <asp:Panel ID="PanelStatisticsReportSettingsSearchCriteria" runat="server" CssClass="panelReportSettingsSearchCriteria">
                        <hr />
                        <table class="tableReportSettingsSearchCriteria">
                            <tr>
                                <td class="labelColumnReportSettings">
                                    <asp:Label ID="LabelStatisticsCountry" runat="server" Text="<%$ Resources:lang, ReportSettingsCountry %>"
                                        CssClass="labelReportSettings"></asp:Label>
                                </td>
                                <td class="dataColumnReportSettings">
                                    <asp:DropDownList ID="DropDownListStatisticsCountry" runat="server" AutoPostBack="true"
                                        Width="120px" CssClass="dropDownListReportSettings" DataTextField="country" DataValueField="country"
                                        OnSelectedIndexChanged="DropDownListStatisticsCountry_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td class="labelColumnReportSettings">
                                    <asp:Label ID="LabelStatisticsLogic" runat="server" Text="<%$ Resources:lang, ReportSettingsLogic %>"
                                        CssClass="labelReportSettings"></asp:Label>
                                </td>
                                <td class="dataColumnReportSettings">
                                    <asp:RadioButtonList ID="RadioButtonListStatisticsLogic" runat="server" RepeatDirection="Horizontal"
                                        AutoPostBack="true" onselectedindexchanged="RadioButtonListStatisticsLogic_SelectedIndexChanged">
                                        <asp:ListItem Text="<%$ Resources:lang, ReportSettingsLogicCMS %>" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="<%$ Resources:lang, ReportSettingsLogicOPS %>" Value="2"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td class="labelColumnReportSettings">
                                    <asp:Label ID="LabelStatisticsStartDate" runat="server" Text="<%$Resources:lang, ReportSettingsStartDate %>"
                                        CssClass="labelReportSettings"></asp:Label>
                                </td>
                                <td class="dataColumnReportSettings">
                                    <uc:DatePickerTextBox ID="DatePickerTextBoxStatisticsStartDate" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td class="labelColumnReportSettings">
                                    <asp:Label ID="LabelStatisticsCMSPool" runat="server" Text="<%$ Resources:lang, ReportSettingsCMSPool %>"
                                        CssClass="labelReportSettings"></asp:Label>
                                    <asp:Label ID="LabelStatisticsOPSRegion" runat="server" Text="<%$ Resources:lang, ReportSettingsOPSRegion %>"
                                        CssClass="labelReportSettings"></asp:Label>
                                </td>
                                <td class="dataColumnReportSettings">
                                    <asp:DropDownList ID="DropDownListStatisticsCMSPool" runat="server" AutoPostBack="true"
                                        Width="120px" CssClass="dropDownListReportSettings" DataTextField="cms_pool"
                                        DataValueField="cms_pool_id" onselectedindexchanged="DropDownListStatisticsCMSPool_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="DropDownListStatisticsOPSRegion" runat="server" AutoPostBack="true"
                                        Width="120px" CssClass="dropDownListReportSettings" DataTextField="ops_region"
                                        DataValueField="ops_region_id" onselectedindexchanged="DropDownListStatisticsOPSregion_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td class="labelColumnReportSettings">
                                    <asp:Label ID="LabelStatisticsEndDate" runat="server" Text="<%$Resources:lang, ReportSettingsEndDate %>"
                                        CssClass="labelReportSettings"></asp:Label>
                                </td>
                                <td class="dataColumnReportSettings">
                                    <uc:DatePickerTextBox ID="DatePickerTextBoxStatisticsEndDate" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td class="labelColumnReportSettings">
                                    <asp:Label ID="LabelStatisticsCMSLocationGroup" runat="server" Text="<%$ Resources:lang, ReportSettingsCMSLocationGroup %>"
                                        CssClass="labelReportSettings"></asp:Label>
                                    <asp:Label ID="LabelStatisticsOPSArea" runat="server" Text="<%$ Resources:lang, ReportSettingsOPSArea %>"
                                        CssClass="labelReportSettings"></asp:Label>
                                </td>
                                <td class="dataColumnReportSettings">
                                    <asp:DropDownList ID="DropDownListStatisticsCMSLocationGroup" runat="server" AutoPostBack="true"
                                        Width="120px" CssClass="dropDownListReportSettings" DataTextField="cms_location_group"
                                        DataValueField="cms_location_group_id" onselectedindexchanged="DropDownListStatisticsCMSLocationGroup_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="DropDownListStatisticsOPSArea" runat="server" AutoPostBack="true"
                                        Width="120px" CssClass="dropDownListReportSettings" DataTextField="ops_area"
                                        DataValueField="ops_area_id" onselectedindexchanged="DropDownListStatisticsOPSArea_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td class="labelColumnReportSettings">
                                    <asp:Label ID="LabelStatisticsRACFID" runat="server" CssClass="labelReportSettings"
                                        Text="<%$ Resources:lang, ReportSettingsRACFID %>"></asp:Label>
                                </td>
                                <td class="dataColumnReportSettings">
                                    <asp:TextBox ID="TextBoxStatisticsRACFID" runat="server" CssClass="textBoxReportSettings"
                                        Width="80px" MaxLength="10"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td class="labelColumnReportSettings">
                                    <asp:Label ID="LabelStatisticsBranch" runat="server" Text="<%$ Resources:lang, ReportSettingsBranch %>"
                                        CssClass="labelReportSettings"></asp:Label>
                                </td>
                                <td class="dataColumnReportSettings">
                                    <asp:DropDownList ID="DropDownListStatisticsLocations" runat="server" DataTextField="location"
                                        Width="120px" CssClass="dropDownListReportSettings" DataValueField="location">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="PanelStatisticsReportSettingsFooter" runat="server" CssClass="panelReportSettingsFooter">
                        <hr />
                        <table class="tableReportSettingsFooter">
                            <tr>
                                <td>
                                    <asp:Button ID="ButtonStatisticsGenerateReport"  runat="server" Text="<%$ Resources:lang, ReportSettingsGenerateReport %>" onclick="ButtonStatisticsGenerateReport_Click" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:Panel>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
