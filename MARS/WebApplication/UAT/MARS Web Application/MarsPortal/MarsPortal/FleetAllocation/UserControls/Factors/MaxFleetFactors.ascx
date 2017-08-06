<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MaxFleetFactors.ascx.cs" Inherits="Mars.FleetAllocation.UserControls.Factors.NonRevenuePercent" %>



<%@ Register Src="~/FleetAllocation/UserControls/FaoParameter.ascx" TagPrefix="uc" TagName="FaoParameter" %>
<%@ Register Src="~/FleetAllocation/UserControls/AutoGrid.ascx" TagPrefix="uc" TagName="AutoGrid" %>
<%@ Register Src="~/FleetAllocation/UserControls/ScenarioSelection/ScenarioSelection.ascx" TagPrefix="uc" TagName="ScenarioSelection" %>


<table style="width: 950px; text-align: center; margin-left: auto; margin-right: auto;">
        <tr>
        <td>
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <uc:ScenarioSelection runat="server" id="ucScenarioSelection" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <hr />
        </td>
    </tr>

    <tr>
        <td>
            <uc:FaoParameter runat="server" ID="ucParameters" ShowDayOfWeek="True" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:Button runat="server" ID="btnLoad" Text="Load" CssClass="StandardButton" OnClick="btnLoad_Click" />

                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
    <tr>
        <td style="width: 100%;">


            <fieldset style="width: 300px; margin-left: 0; margin-right: auto;">
                <legend>Update NonRev and Utilization</legend>
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        
                    <asp:HiddenField runat="server" ID="hfChangeConfirmMessage"
                        Value="Clicking OK will update {0} records" />
                    <asp:HiddenField runat="server" ID="hfRefreshDataMessage"
                        Value="Click load to refresh data before updating" />
                    <asp:HiddenField runat="server" ID="hfRecordCount" Value="0" />

                        <table>
                            <tr>
                                <td>Non Rev:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="tbNewNonRev" CssClass="AutoCompleteTextBox" />
                                </td>
                                <td>
                                    <asp:Button runat="server" ID="btnSubmitNonRev" Text="Update" 
                                        CssClass="StandardButton" OnClick="btnSubmitNonRev_Click" />
                                    <asp:ConfirmButtonExtender runat="server" ID="cbeChangeNonRevConfirm"
                                        ConfirmText="test"
                                        TargetControlID="btnSubmitNonRev" />
                                    <asp:HiddenField runat="server" ID="hfNonRevUpdate"
                                        Value="Clicking OK will update {0:#,#}  records" />
                                </td>
                            </tr>
                            <tr>
                                <td>Utilization:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="tbNewUtilization" CssClass="AutoCompleteTextBox" />
                                </td>
                                <td>
                                    <asp:Button runat="server" ID="btnSubmitUtilization" Text="Update" 
                                        CssClass="StandardButton" OnClick="btnSubmitUtilization_Click" />
                                    <asp:ConfirmButtonExtender runat="server" ID="cbeChangeUtilizationConfirm"
                                        ConfirmText="test"
                                        TargetControlID="btnSubmitUtilization" />
                                    <asp:HiddenField runat="server" ID="hfUltilizationUpdate"
                                        Value="Clicking OK will update {0:#,#} records" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </fieldset>
        </td>
    </tr>
    <tr>
        <td>
            <asp:UpdatePanel runat="server" ID="upGrid" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc:AutoGrid runat="server" ID="ucMaxFactors" />
                </ContentTemplate>
            </asp:UpdatePanel>

        </td>
    </tr>
</table>
