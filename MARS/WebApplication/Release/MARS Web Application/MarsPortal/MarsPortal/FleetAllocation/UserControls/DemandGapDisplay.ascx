<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DemandGapDisplay.ascx.cs" Inherits="Mars.FleetAllocation.UserControls.DemandGapDisplay" %>


<%@ Register Src="~/FleetAllocation/UserControls/FaoParameter.ascx" TagPrefix="uc" TagName="FaoParameter" %>
<%@ Register Src="~/FleetAllocation/UserControls/AutoGrid.ascx" TagPrefix="uc" TagName="AutoGrid" %>
<%@ Register Src="~/App.UserControls/Phase4/Common/UpdateProgress.ascx" TagPrefix="uc" TagName="UpdateProgress" %>
<%@ Register Src="~/FleetAllocation/UserControls/DistributionLimits/MonthlyLimitDetails.ascx" TagPrefix="uc" TagName="MonthlyLimit" %>
<%@ Register Src="~/FleetAllocation/UserControls/DistributionLimits/WeeklyLimitDetails.ascx" TagPrefix="uc" TagName="WeeklyLimit" %>




<asp:UpdatePanel runat="server" ID="upnlDemandGap">
    <ContentTemplate>
        <table style="width: 1000px; text-align: center; margin-left: auto; margin-right: auto;" >
    <tr>
        <td>
            <uc:FaoParameter runat="server" ID="ucParameters" />
        </td>
    </tr>
    <tr>
        <td style="text-align: center;">
            <asp:Button runat="server" ID="btnCaluclate" Text="Calculate" OnClick="btnCalculate_Click" CssClass="StandardButton"/>
            <asp:Hiddenfield ID="hfLoading" runat="server" Value=""/>
            
            <uc:UpdateProgress runat="server" ID="ucUpdateProgress" />
        </td>
    </tr>

    <tr>
        <td>
            <asp:Panel runat="server" ID="pnlGapFill">
                <table>
                    <tr>
                        <td>
                            <uc:AutoGrid runat="server" ID="ucDemanGapStageOneGrid" AutoGridWidth="500" />                
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <uc:AutoGrid runat="server" ID="ucDgVis" AutoGridWidth="500" />                
                        </td>
                    </tr>
                    <table>
                        <tr style="vertical-align: top;">
                            <td>
                                <uc:MonthlyLimit ID="ucMonthlyLimit" runat="server" />
                            </td>
                            <td>
                                <uc:WeeklyLimit ID="ucWeeklyLimit" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <hr/>
                                Week 1:
                                <uc:AutoGrid ID="ucResult" runat="server" AutoGridWidth="500" />
                            </td>
                        </tr>
                    </table>
                    <tr>
                    </tr>
                </table>
                <asp:TextBox runat="server" ID="tbSummary" Text="test" TextMode="MultiLine" Height="100px" Width="400px"/>
            </asp:Panel>
            
        </td>
    </tr>
<%--    <tr>
        <td>
            <uc:AutoGrid runat="server" ID="ucDemandGapOne" />
        </td>
    </tr>
    <tr>
        <td>
            <uc:AutoGrid runat="server" ID="ucSpread" />
        </td>
    </tr>--%>
</table>
        
    </ContentTemplate>
</asp:UpdatePanel>


 <script type="text/javascript">
        $(document).keypress(function (e) {
            if (e.keyCode === 13) {
                e.preventDefault();
                return false;
            }
        });



        var handleSuccess = function (data, textStatus, jqXHR) {
            if (data.d == "Finished") {
                __doPostBack('<%= upnlDemandGap.ClientID %>', '');
            } 
        };

        setInterval(function () {
            
            if ($("#<%=hfLoading.ClientID %>").val() == "Loading") {
                $.ajax({
                    url: '/Webservices/FaoServices.asmx/GetStage1CalculationStatus',
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: handleSuccess,
                });
            }

        }, 5000);


        

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



