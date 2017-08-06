<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LocationPopup.ascx.cs" Inherits="Mars.App.UserControls.Phase4.Administration.Mapping.EntityPopups.LocationPopup" %>


<asp:Panel ID="pnlLocation" runat="server" Width="600px" Height="330px" CssClass="Phase4ModalPopup">

    <asp:HiddenField runat="server" ID="hfLocationId" />

    <table style="width: 100%;">
        <tr>
            <td style="height: 30px; text-align: center;" colspan="4">
                <h1>Location</h1>
            </td>
            <td>
                <div style="float: right;">
                    <asp:ImageButton runat="server" ID="ibClose" ImageUrl="~/App.Images/Icons/close.png"  />
                </div>
            </td>
        </tr>
        <tr>
            <td style="height: 20px;">&nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <label style="font-weight: bold;">Country:</label>
            </td>
            <td>
                <asp:Label runat="server" ID="lblCountryName" Width="120px" Height="16px" BorderWidth="1" />
            </td>
        </tr>
        <tr>
            <td>
                <label style="font-weight: bold;">Location WWD:</label>
            </td>
            <td>
                <asp:TextBox runat="server" ID="tbLocation" Enabled="False" Width="120px" Height="16px" 
                    ToolTip="This field can not be edited" />
            </td>
        </tr>
        <tr>
            <td>
                <label style="font-weight: bold;">Pool:</label>
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlPool" CssClass="StandardAdminDropdown"
                    AutoPostBack="True" OnSelectedIndexChanged="ddlPool_SelectionChanged" />
            </td>
            <td>
                <label style="font-weight: bold;">Region:</label>
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlRegion" CssClass="StandardAdminDropdown"
                    AutoPostBack="True" OnSelectedIndexChanged="ddlRegion_SelectionChanged" />
            </td>
        </tr>
        <tr>
            <td>
                <label style="font-weight: bold;">Location Group:</label>
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlLocationGroup" CssClass="StandardAdminDropdown" />
            </td>
            <td>
                <label style="font-weight: bold;">Area:</label>
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlArea" CssClass="StandardAdminDropdown" />
            </td>
        </tr>
        <tr>
            <td>
                <label style="font-weight: bold;">Is Active:</label>
            </td>
            <td>
                <asp:CheckBox runat="server" ID="cbActive" />
            </td>
        </tr>
        <tr>
            <td style="height: 20px;">&nbsp;
            </td>
        </tr>
        <tr>
            <td>Location Name:
            </td>
            <td>
                <asp:TextBox runat="server" ID="tbLocationName"  Width="120px" Height="16px"/>
            </td>
            <td>Served by:
            </td>
            <td>
                <asp:TextBox runat="server" ID="tbServedBy" MaxLength="30" />
                <asp:AutoCompleteExtender ID="acServedBy" runat="server" ServiceMethod="SearchLocationText"
                        TargetControlID="tbServedBy" ServicePath="~/AutoComplete.asmx" MinimumPrefixLength="2"
                        CompletionInterval="500" EnableCaching="true" CompletionSetCount="8" 
                        UseContextKey="True"  />
            </td>
        </tr>
        <tr>
            <td>
                Location Type:
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlLocationType" CssClass="StandardAdminDropdown">
                    <asp:ListItem Value="AP" Text="Airport" />
                    <asp:ListItem Value="DT" Text="Downtown" />
                    <asp:ListItem Value="RR" Text="Railroad" />
                </asp:DropDownList>
            </td>
            <td>
                Company:
            </td>
            <td>
                <asp:DropDownList ID="ddlCompany" CssClass="StandardAdminDropdown" runat="server" AutoPostBack="True"
                    OnSelectedIndexChanged="ddlCompany_SelectionChanged"/>
            </td>
        </tr>
        <tr>
            <td>
                Turnaround Hours:
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlTurnaroundHours" CssClass="StandardAdminDropdown" >
                    <asp:ListItem Value="0" Text="0" />
                    <asp:ListItem Value="1" Text="1" />
                    <asp:ListItem Value="2" Text="2" />
                    <asp:ListItem Value="3" Text="3" />
                    <asp:ListItem Value="4" Text="4" />
                    <asp:ListItem Value="5" Text="5" />
                    <asp:ListItem Value="6" Text="6" />
                    <asp:ListItem Value="7" Text="7" />
                    <asp:ListItem Value="8" Text="8" />
                    <asp:ListItem Value="9" Text="9" />
                    <asp:ListItem Value="10" Text="10" />
                </asp:DropDownList>
            </td>
            <td>
                Owner Type:
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlOwnerType" CssClass="StandardAdminDropdown" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label runat="server" ID="lblMessage" ForeColor="Red" />
            </td>
        </tr>
        <tr>
            <td style="height: 20px;">&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="4" style="text-align: center;">
                <asp:Button runat="server" ID="bnSavePopup" Text="Save" OnClick="bnSavePopup_Click"
                    ValidationGroup="CheckControls" CssClass="StandardButton" />
            </td>
        </tr>
    </table>
</asp:Panel>


<asp:Button ID="btnDummy" runat="server" CssClass="hidden" />
<asp:ModalPopupExtender
    ID="mpPool"
    runat="server"
    PopupControlID="pnlLocation"
    TargetControlID="btnDummy"
    DropShadow="True"
    BackgroundCssClass="modalBackgroundGray" />
