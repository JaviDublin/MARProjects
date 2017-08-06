<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ScenarioSelection.ascx.cs" Inherits="Mars.FleetAllocation.UserControls.ScenarioSelection.ScenarioSelection" %>

<asp:HiddenField runat="server" ID="hfScenarioType" />

<table style="width: 950px; height: 100px;" >
    <tr>
        <td style="width: 500px; vertical-align: top;">
            <table>
                <tr>
                    <td style="text-align: right;">
                        Country:
                    </td>
                    <td style="text-align: left;">
                        <asp:DropDownList runat="server" ID="ddlCountry" AutoPostBack="True"
                            CssClass="SingleDropDownList" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">Scenario Selection:
                    </td>
                    <td style="text-align: left;">
                        <asp:DropDownList runat="server" ID="ddlScenarioSelector" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlScenarioSelector_SelectionChanged"
                            CssClass="SingleDropDownList" />
                    </td>
                    <td>
                        <asp:Button ID="btnRenameScenario" runat="server" CssClass="StandardButton"
                            Text="Rename Scenario" Width="140px" OnClick="btnRenameScenario_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblSummary" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table>
                            <tr>
                                <td >
                                    <asp:TextBox runat="server" ID="tbScenarioDescription" TextMode="MultiLine" Width="300px" 
                                        Height="70px" ReadOnly="True" />
                                </td>
                                <td>
                                    <asp:Label ID="lblMessage" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="vertical-align: top;">
                        <asp:Button ID="btnUpdateDescription" runat="server" CssClass="StandardButton"
                            Text="Edit Description" Width="140px" OnClick="btnUpdateDescription_Click"  />
                    </td>
                </tr>
            </table>
        </td>
        <td style="vertical-align: top;">
            <table style="border-spacing: 10px;">
                <tr>
                    <td>
                        <asp:Button ID="btnNewBlankScenario" runat="server" CssClass="StandardButton"
                            Text="New Blank Scenario" Width="140px" OnClick="btnNewBlankScenario_Click" />
                    </td>
                    <td>
                        <asp:Button ID="btnDeleteScenario" runat="server" CssClass="StandardButton"
                            Text="Delete Selected Scenario" Width="140px" OnClick="btnDeleteScenario_Click" />
                        <asp:ConfirmButtonExtender runat="server" TargetControlID="btnDeleteScenario"
                            ConfirmText="Are you sure you wish to delete this Scenario" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnCloneSelectedScenario" runat="server" CssClass="StandardButton"
                            Text="Clone Selected Scenario" Width="140px" OnClick="btnCloneScenarioScenario_Click" />
                    </td>

            </table>

        </td>
    </tr>

</table>

<asp:Panel runat="server" ID="pnlNewNameScenario" CssClass="Phase4ModalPopup" Width="300" Height="130">
    <table style="width: 100%;">
        <tr>
            <td colspan="2">
                <div style="float: right;">
                    <asp:ImageButton runat="server" ID="ibClose" ImageUrl="~/App.Images/Icons/close.png" />
                </div>
                <div style="height: 50px;">
                    &nbsp;
                </div>
            </td>
        </tr>
        <tr>
            <td>New Scenario Name:
            </td>
            <td>
                <asp:TextBox runat="server" ID="tbNewScenarioName" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div style="height: 20px;">
                    &nbsp;
                </div>
                <asp:Button ID="btnSaveNewBlankScenario" runat="server" Text="Submit" CssClass="StandardButton"
                    OnClick="btnSaveNewBlankScenario_Click" />
                <asp:Button ID="btnCloneScenario" runat="server" Text="Submit" CssClass="StandardButton"
                    OnClick="btnCloneScenario_Click" />
                <asp:Button ID="btnConfirmRenameScenario" runat="server" Text="Submit" CssClass="StandardButton"
                    OnClick="btnConfirmRenameScenario_Click" />
            </td>
        </tr>
    </table>
</asp:Panel>



<asp:Panel runat="server" ID="pnlEditDescription" CssClass="Phase4ModalPopup" Width="500" Height="300">
    <table style="width: 100%;">
        <tr>
            <td>
                <div style="float: right;">
                    <asp:ImageButton runat="server" ID="ibClose2" ImageUrl="~/App.Images/Icons/close.png" />
                </div>
                <div style="height: 10px;">
                    &nbsp;
                </div>
            </td>
        </tr>
        <tr>
            <td style="font-size: 14px;">
                Scenario Description
            </td>
        </tr>
        <tr>
            
            <td>
                <asp:TextBox runat="server" ID="tbEditScenarioDescription" TextMode="MultiLine" Width="450px" 
                        Height="200px"   />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnSaveDescription" runat="server" CssClass="StandardButton"
                        Text="Save Description" Width="140px" OnClick="btnSaveDescription_Click" />
            </td>
        </tr>
    </table>
</asp:Panel>


<asp:Button ID="btnDummy" runat="server" CssClass="hidden" />
<asp:ModalPopupExtender
    ID="mpeNewName"
    runat="server"
    PopupControlID="pnlNewNameScenario"
    TargetControlID="btnDummy"
    DropShadow="True"
    BackgroundCssClass="modalBackgroundGray" />

<asp:Button ID="btnDummy2" runat="server" CssClass="hidden" />
<asp:ModalPopupExtender
    ID="mpeEditDescription"
    runat="server"
    PopupControlID="pnlEditDescription"
    TargetControlID="btnDummy2"
    DropShadow="True"
    BackgroundCssClass="modalBackgroundGray" />
