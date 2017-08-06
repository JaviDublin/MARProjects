<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdditionPlanHistory.ascx.cs" Inherits="Mars.FleetAllocation.UserControls.AdditionPlanHistory" %>
<%@ Register Src="~/FleetAllocation/UserControls/AutoGrid.ascx" TagPrefix="uc" TagName="AutoGrid" %>
<%@ Register Src="~/FleetAllocation/UserControls/FaoParameter.ascx" TagPrefix="uc" TagName="FaoParameter" %>


<table style="width: 1000px;" >
    <tr>
        <td>
            <table>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                Country:
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlCountry" AutoPostBack="True"
                                                    CssClass="SingleDropDownList"  />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="text-align: center;">
                                    <asp:Button runat="server" ID="btnRefreshHistory" CssClass="StandardButton"
                                        Text="Refresh" OnClick="btnRefreshHistory_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Panel runat="server" ScrollBars="Vertical" Height="200px" Width="530px">
                                        <uc:AutoGrid runat="server" ID="agAdditionPlanHistory" AutoGridWidth="500" />
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">Comparison Type:
                                </td>
                                <td style="text-align: left;">
                                    <asp:RadioButtonList runat="server" ID="rblComparisonType" Width="180px" RepeatDirection="Horizontal"
                                        OnSelectedIndexChanged="rblComparisonType_SelectionChanged" AutoPostBack="True" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;" rowspan="2">Grouping Level:
                                </td>
                                <td style="text-align: left;">
                                    <asp:RadioButtonList runat="server" ID="rblLocationGrouping" RepeatDirection="Horizontal" Width="300px"
                                        OnSelectedIndexChanged="GroupingChanged" AutoPostBack="True" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left;">
                                    <asp:RadioButtonList runat="server" ID="rblCarGrouping" RepeatDirection="Horizontal" Width="250px"
                                        OnSelectedIndexChanged="GroupingChanged" AutoPostBack="True" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="vertical-align: top;">
                        <table>
                            <tr>
                                <td>
                                    <fieldset style="width: 460px;">
                                        <legend>Filter</legend>
                                        <uc:FaoParameter runat="server" ID="FaoParameter" />
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button runat="server" ID="btnFilterHistory" CssClass="StandardButton"
                                        Text="Filter" OnClick="btnFilterHistory_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <hr />
        </td>
    </tr>
    <tr>
        <td>
            <table style="text-align: center; width:100%;">
                <tr>
                    <td style="vertical-align: top;">
                        <asp:Panel ID="pnlScenarioA" runat="server" BorderColor="LightGreen" BorderWidth="2" CssClass="FaoScenarioPanel" >
                        <table style="width: 50%;" >
                            <tr>
                                <td>
                                    <asp:RadioButton runat="server" ID="rbAScenario" GroupName="ScenarioSelection" Text="A" AutoPostBack="True" 
                                        OnCheckedChanged="SelectedScenarioChanged"
                                        Checked="True"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                Min Scenario:            
                                            </td>
                                            <td>
                                                <asp:Label ID="lblMinScenASelected" runat="server"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Max Scenario:
                                            </td>
                                            <td>
                                                <asp:Label ID="lblMaxScenASelected" runat="server"/>
                                            </td>
                                        </tr>
                                    </table>
                                    
                                </td>
                                
                            </tr>
                            <tr>
                                <td>
                                    <asp:HiddenField runat="server" ID="hfSelectedScenarioAId" />
                                    <uc:AutoGrid runat="server" ID="agV1AdditionPlanMinMaxValues" AutoGridWidth="480" HorizontalAlignment="Center" />
                                    <uc:AutoGrid runat="server" ID="agV1AdditionPlanEntries" AutoGridWidth="480"  HorizontalAlignment="Center"/>
                                </td>
                            </tr>
                        </table>
                        </asp:Panel>
                    </td>
                    <td>
                        <hr style="width: 1px; height: 400px;" >
                    </td>
                    <td style="vertical-align: top;">
                        <asp:Panel runat="server" ID="pnlScenarioB" BorderColor="LightGreen" BorderWidth="2" CssClass="FaoScenarioPanel">
                            <table style="width: 50%;" >
                                <tr>
                                    <td>
                                        <asp:RadioButton runat="server" ID="rbBScenario" GroupName="ScenarioSelection" Text="B" 
                                            OnCheckedChanged="SelectedScenarioChanged"
                                            AutoPostBack="True" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    Min Scenario:            
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblMinScenBSelected" runat="server"/>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Max Scenario:
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblMaxScenBSelected" runat="server"/>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td>
                                        <asp:HiddenField runat="server" ID="hfSelectedScenarioBId" />
                                        <uc:AutoGrid runat="server" ID="agV2AdditionPlanMinMaxValues" AutoGridWidth="480" HorizontalAlignment="Center" />
                                        <uc:AutoGrid runat="server" ID="agV2AdditionPlanEntries" AutoGridWidth="480" HorizontalAlignment="Center" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
      
        </td>
    </tr>
</table>


<asp:HiddenField ID="hfSelectedViewTab" runat="server" Value="0" />

<script type="text/javascript">
    $(document).ready(function () {

        var tabIndex = $('#<%= hfSelectedViewTab.ClientID %>').val();

        $('#viewSelectionPanel').tabs({
            select: function (event, ui) {
                $('#<%= hfSelectedViewTab.ClientID %>').val(ui.index);
            },
            selected: parseInt(parseInt(tabIndex))
        });

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(panelLoaded);

 


        function panelLoaded(sender, args) {
            if (args.get_panelsUpdated().length > 0) {

                var tabIndex = $('#<%= hfSelectedViewTab.ClientID %>').val();

                $('#viewSelectionPanel').tabs({
                    select: function (event, ui) {
                        $('#<%= hfSelectedViewTab.ClientID %>').val(ui.index);
                    },
                    selected: parseInt(parseInt(tabIndex))
                });

        
            }
            };
    });

</script>
