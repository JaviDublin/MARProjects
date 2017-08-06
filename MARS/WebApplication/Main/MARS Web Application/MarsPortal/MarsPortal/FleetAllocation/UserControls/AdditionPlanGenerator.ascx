<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdditionPlanGenerator.ascx.cs" 
    Inherits="Mars.FleetAllocation.UserControls.AdditionPlanGenerator" %>


<%@ Register Src="~/FleetAllocation/UserControls/FaoParameter.ascx" TagPrefix="uc" TagName="FaoParameter" %>
<%@ Register Src="~/FleetAllocation/UserControls/AutoGrid.ascx" TagPrefix="uc" TagName="AutoGrid" %>

<%@ Register Src="~/FleetAllocation/UserControls/DistributionLimits/MonthlyLimitDetails.ascx" TagPrefix="uc" TagName="MonthlyLimit" %>
<%@ Register Src="~/FleetAllocation/UserControls/DistributionLimits/WeeklyLimitDetails.ascx" TagPrefix="uc" TagName="WeeklyLimit" %>
<%@ Register Src="~/FleetAllocation/UserControls/DemandGapProgress/DemandGapCalculationProgress.ascx" TagPrefix="uc" TagName="DemandGapCalculationProgress" %>
<%@ Register Src="~/App.UserControls/HelpIcon.ascx" TagPrefix="uc" TagName="HelpIcon" %>
<%@ Register Src="~/App.UserControls/Phase4/Common/UpdateProgress.ascx" TagPrefix="uc" TagName="UpdateProgress" %>

<asp:HiddenField runat="server" ID="hfLoggedOnUser"/>

<asp:UpdatePanel runat="server" ID="upnlDemandGap">
    <ContentTemplate>
        <table style="width: 1050px; text-align: center; margin-left: auto; margin-right: auto;">
            <tr>
                <td>
                    <table style="width: 1050px; height: 300px;" >
                        <tr style="vertical-align: top;">
                            <td>
                                <uc:FaoParameter runat="server" ID="ucParameters" AllowAdvancedParameters="True"  />
                            </td>
                            <td style="vertical-align: top;">
                                <table style="width: 310px; height: 50px; border-spacing: 10px;">
                                    <tr>
                                        <td style="text-align: right;">
                                            Country:
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:DropDownList runat="server" ID="ddlCountry" AutoPostBack="True"
                                                CssClass="SingleDropDownList"  />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;">Min Comm Segment Scenario:
                                        </td>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddlMinCommSeg" CssClass="SingleDropDownList" 
                                                            OnSelectedIndexChanged="ddlMinCommSeg_SelectionChanged" AutoPostBack="True" />            
                                                    </td>
                                                    <td>
                                                        <asp:Image CssClass="StandardBorder" runat="server" ImageUrl="~/App.Images/UserInfo.gif" 
                                                            ID="imgMinFleetDescription" Height="25" Width="25" />
                                                    </td>
                                                </tr>
                                            </table>
                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;">Max Fleet Factor Scenario:
                                        </td>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddlMaxFleetFactor" CssClass="SingleDropDownList"
                                                            OnSelectedIndexChanged="ddlMaxFleetFactor_SelectionChanged" AutoPostBack="True" />            
                                                    </td>
                                                    <td>
                                                        <asp:Image CssClass="StandardBorder" runat="server" ImageUrl="~/App.Images/UserInfo.gif" 
                                                            ID="imgMaxFleetDescription" Height="25" Width="25" 
                                                            ToolTip="" />
    
                                                        
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;">
                                            Spread Range:
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:Label runat="server" ID="lblSpreadRange" />
                                        </td>
                                    </tr>
  
                                    <tr>
                                        <td style="text-align: right;">
                                            Current Day:
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:Label runat="server" ID="lblLastFleetHistory" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;">
                                            Weeks to Calculate:
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:TextBox runat="server" ID="tbWeeks" Width="30px" CssClass="SingleDropDownList" Text="12" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;">Stage of Calculation:
                                        </td>
                                        <td  style="text-align: left;">
                                            <asp:DropDownList runat="server" ID="ddlStage" CssClass="SingleDropDownList">
                                                <asp:ListItem Value="1" Text="Gap 1 - Group" />
                                                <asp:ListItem Value="2" Text="Gap 1 - Class" />
                                                <asp:ListItem Value="3" Text="Gap 2 - Group" />
                                                <asp:ListItem Value="4" Text="Gap 2 - Class" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Button runat="server" ID="btnCaluclate" Text="Calculate" OnClick="btnCalculate_Click" CssClass="StandardButton" />
                                            <asp:HiddenField ID="hfLoading" runat="server" Value="" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="vertical-align: top; text-align: center; width: 200px;">
                                <uc:DemandGapCalculationProgress runat="server" ID="ucDgcProgress" />
                                <asp:Label runat="server" ID="lblErrorMessage" Text="An Error Occured" ForeColor="Red" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel runat="server" ID="pnlGapFill">
                        <table>
                            <tr>
                                <td>Gap Calculations
                                    <uc:AutoGrid runat="server" ID="ucDemanGapStageOneGrid" AutoGridWidth="500" />
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <table>
                                        <tr style="vertical-align: top;">
                                            <td>Monhtly Limit File
                                                <uc:MonthlyLimit ID="ucMonthlyLimit" runat="server" />
                                            </td>
                                            <td>Weekly Limit File
                                                <uc:WeeklyLimit ID="ucWeeklyLimit" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            Generated Additions
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <uc:AutoGrid ID="ucResult" runat="server" AutoGridWidth="500" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        Addition Plan Name:
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server" ID="tbAdditionPlanName" ValidationGroup="SaveAdditionPlan"
                                                                            Width="120px" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button runat="server" ID="btnSaveAdditionPlan" ValidationGroup="SaveAdditionPlan"
                                                                            Text="Save" CssClass="StandardButton" OnClick="btnSaveAdditionPlan_Click"/>
                                                                    </td>
                                                                    <td>
                                                                        <asp:RequiredFieldValidator runat="server" 
                                                                            ErrorMessage="The addition plan needs a name"
                                                                            ValidationGroup="SaveAdditionPlan"
                                                                            ControlToValidate="tbAdditionPlanName" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:TextBox runat="server" ID="tbSummary" TextMode="MultiLine" Height="100px" Width="400px" />
                    <asp:Label runat="server" ID="lblProcessMessage" />
                </td>
            </tr>
        </table>
        
        
        

    </ContentTemplate>
</asp:UpdatePanel>

<uc:UpdateProgress runat="server" ID="UpdateProgress" />

<script type="text/javascript">
    $('.AutoCompleteTextBox').keypress(function (e) {
        if (e.keyCode == 13) {
            e.preventDefault();
            return false;
        }
    });



    var handleSuccess = function (data, textStatus, jqXHR) {
        if (data.d == "Refresh") {
            __doPostBack('<%= upnlDemandGap.ClientID %>', '');
        }
    };

    setInterval(function () {

        if ($("#<%=hfLoading.ClientID %>").val() == "Loading") {
            $.ajax({
                url: '/Webservices/FaoServices.asmx/GetFaoCalculationStatus',
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: handleSuccess,
            });
        }

    }, 3000);




    function QuickSelectMade() {
        var evt = window.event;

        var updatePanelId = "<%= ucParameters.UpdatePanelClientId %>";



            //If Mousedown or enter or tab pressed
            if (evt.type == "mousedown" || ((event.keyCode == 13) || (event.keyCode == 9))) {
                __doPostBack(updatePanelId, "LocationSingle CarGroupSingle");
            }
        }

        function QuickSelectMultiple() {
            var evt = window.event;


            var updatePanelId = "<%= ucParameters.UpdatePanelClientId %>";

            //If Mousedown or enter or tab pressed
            if (evt.type == "mousedown" || ((event.keyCode == 13) || (event.keyCode == 9))) {
                __doPostBack(updatePanelId, "LocationMultiple CarGroupMultiple");
            }
        }

</script>



