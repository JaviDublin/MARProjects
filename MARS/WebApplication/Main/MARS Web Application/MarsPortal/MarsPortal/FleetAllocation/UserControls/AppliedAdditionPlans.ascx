<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AppliedAdditionPlans.ascx.cs" Inherits="Mars.FleetAllocation.UserControls.AppliedAdditionPlans" %>

<%@ Register Src="~/FleetAllocation/UserControls/AutoGrid.ascx" TagPrefix="uc" TagName="AutoGrid" %>



<asp:HiddenField runat="server" ID="hfSelectedScenarioId" />

<table style="width: 100%;">
    <tr>
        <td>
            <table style="width: 400px;">
                <tr>
                    <td>Country:
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlCountry" AutoPostBack="True"
                            CssClass="SingleDropDownList" />
                    </td>
                    <td style="text-align: right;">
                        <asp:CheckBox runat="server" ID="cbActiveOnly" Text="Active Only:" AutoPostBack="True" OnCheckedChanged="RefreshHistory" />
                    </td>
                </tr>
            </table>
        </td>
        <td style="text-align: center;">
            <asp:Button runat="server" ID="btnRefreshHistory" CssClass="StandardButton"
                Text="Refresh" OnClick="RefreshHistory" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Panel runat="server" ScrollBars="Vertical" Height="200px" Width="800px">
                <uc:AutoGrid runat="server" ID="agAdditionPlanHistory" AutoGridWidth="750" />
            </asp:Panel>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Panel runat="server" ID="pnlAdditionPlanDetails" Visible="False">
                <fieldset>
                    <legend>Addition Plan Details</legend>

                    <table style="width: 100%;">

                        <tr>
                            <td style="vertical-align: top;">

                                <table style="width: 67%;">
                                    <tr>
                                        <td colspan="2">
                                            <asp:Button runat="server" ID="btnApplyAdditionPlan" CssClass="StandardButton" Text="Apply"
                                                OnClick="btnApplyAdditionPlan_Click" />
                                            <asp:Button runat="server" ID="btnUnApplyAdditionPlan" CssClass="StandardButton" Text="Un-Apply"
                                                OnClick="btnUnApplyAdditionPlan_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left;">Addition Plan:
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:Label runat="server" ID="lblAdditionPlanName" CssClass="BiggerText" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left;">Created:
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:Label runat="server" ID="lblAdditionPlanDateCreated" CssClass="BiggerText" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left;" colspan="2">Lessons Learnt:
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:TextBox runat="server" ID="tbLessonsLearnt" Height="60px" Width="300px" ReadOnly="True"
                                                TextMode="MultiLine" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Button runat="server" ID="btnUpdateLesson" CssClass="StandardButton"
                                                Text="Update Lesson" Width="100"  OnClick="ShowLessonPopup" />
                                        </td>
                                    </tr>
                                </table>

                            </td>
                            <td>
                                <uc:AutoGrid runat="server" ID="agAdditionPlanDetails" AutoGridWidth="400" 
                                    HorizontalAlignment="Center" HideLastColumn="True" Visible="False" 
                                    ShowSideExportButton="True"
                                    ExportDataFileName="Applied Addition Plan" />
                            </td>
                        </tr>

                    </table>
                </fieldset>
            </asp:Panel>
        </td>
    </tr>
</table>

<asp:Panel runat="server" ID="pnlUpdateLesson" CssClass="Phase4ModalPopup" Width="450" Height="280">
    <table style="width: 100%; text-align: center;">
        <tr>
            <td>
                <div style="float: right;">
                    <asp:ImageButton runat="server" ID="ibClose" ImageUrl="~/App.Images/Icons/close.png" />
                </div>
                <div style="height: 10px;">
                    &nbsp;
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <asp:HiddenField runat="server" ID="hfUpdateLessonLabel" Value="Update Lesson for {0}" />
                <asp:Label runat="server" ID="lblUpdateLessonLabel" />
            </td>
        </tr>
        <tr>
            <td style="font-size: larger; font-weight: bold;">
                <asp:TextBox runat="server" ID="tbUpdatedLesson" Height="200px" Width="400px" MaxLength="2000"
                    TextMode="MultiLine" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button runat="server" ID="btnSaveUpdatedLesson" CssClass="StandardButton"
                        Text="Save" OnClick="SaveUpdatedLesson" />
            </td>
        </tr>
    </table>
</asp:Panel>

<asp:Button ID="btnDummy" runat="server" CssClass="hidden" />
<asp:ModalPopupExtender
    ID="mpeUpdateLesson"
    runat="server"
    PopupControlID="pnlUpdateLesson"
    TargetControlID="btnDummy"
    DropShadow="True"
    BackgroundCssClass="modalBackgroundGray" />
