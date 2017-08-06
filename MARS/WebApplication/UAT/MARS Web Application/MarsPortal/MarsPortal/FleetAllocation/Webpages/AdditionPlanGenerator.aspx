<%@ Page Title="" Language="C#" MasterPageFile="~/App.MasterPages/Mars.Master" AutoEventWireup="true"
    CodeBehind="AdditionPlanGenerator.aspx.cs" Inherits="Mars.FleetAllocation.Webpages.AdditionPlanGenerator" %>


<%@ Register Src="~/FleetAllocation/UserControls/AdditionPlanGenerator.ascx" TagPrefix="uc" TagName="AdditionPlanGenerator" %>
<%@ Register Src="~/FleetAllocation/UserControls/AdditionPlanHistory.ascx" TagPrefix="uc" TagName="AdditionPlanHistory" %>
<%@ Register Src="~/FleetAllocation/UserControls/AppliedAdditionPlans.ascx" TagPrefix="uc" TagName="AppliedAdditionPlans" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceholderHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceholderMainContent" runat="server">

    <table style=" text-align: center; margin-left: auto; margin-right: auto;">
        <tr style="margin-top: 0; margin-left: 0; margin-bottom: auto; vertical-align: top;">
            <td>
                <div id="tabbedPanel" style="width: 1050px; text-align: left; background-color: transparent;">
                    <ul>
                        <li><a href="#tabs-1">Addition Plan Generator</a></li>
                        <li><a href="#tabs-2">Addition Plan Comparison</a></li>
                        <li><a href="#tabs-3">Addition Plan Application and Lessons</a></li>
                    </ul>
                    <div id="tabs-1" style=" text-align: left;">
                        <table style="text-align: center; margin-left: 0; margin-right: auto;">
                            <tr style="vertical-align: top;">
                                <td>
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <uc:AdditionPlanGenerator runat="server" id="ucAdditionsPlanGenerator" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="tabs-2">
                        <table style="text-align: center; margin-left: 0; margin-right: auto;">
                            <tr style="vertical-align: top;">
                                <td>
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <uc:AdditionPlanHistory runat="server" ID="ucAdditionPlanHistory" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="tabs-3">
                        <table style="text-align: center; margin-left: 0; margin-right: auto;">
                            <tr style="vertical-align: top;">
                                <td>
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <uc:AppliedAdditionPlans runat="server" ID="ucAppliedAdditionPlans" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <script type="text/javascript">
        $(function () {
            $("#tabbedPanel").tabs();
        });
    </script>
</asp:Content>
