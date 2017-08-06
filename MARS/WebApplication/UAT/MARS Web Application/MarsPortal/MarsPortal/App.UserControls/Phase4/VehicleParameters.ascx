<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VehicleParameters.ascx.cs" Inherits="Mars.App.UserControls.Phase4.VehicleParameters" %>

<table style="width: 450px; margin-left: auto; margin-right: auto; border-collapse: separate; border-spacing: 6px; text-align: left;">
    <tr>
        <td>
            <asp:Label runat="server" ID="lblLocationCountry" Text="Location Country:"/>
            
        </td>
        <td>
            
            <asp:DropDownList runat="server" ID="ddlLocationCountry" CssClass="SingleDropDownList" AutoPostBack="True" OnSelectedIndexChanged="ParameterChanged" />
        </td>
        <td>Owning Country:</td>
        <td>
            <asp:DropDownList runat="server" ID="ddlOwningCountry" CssClass="SingleDropDownList" AutoPostBack="True" OnSelectedIndexChanged="ParameterChanged" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:RadioButtonList runat="server" ID="rblCmsOpsLogic" OnSelectedIndexChanged="rblCmsOpsLogic_SelectionChanged"
                            RepeatDirection="Horizontal" AutoPostBack="True">
                            <asp:ListItem Text="CMS" Value="Cms" Selected="True" />
                            <asp:ListItem Text="OPS" Value="Ops" />
                        </asp:RadioButtonList>
                    </td>
                    <td style="float: right;">
                        <asp:RadioButtonList runat="server" ID="rblLocationLogic" RepeatDirection="Horizontal" Visible="False">
                            <asp:ListItem Text="Check Out" Value="" Selected="True" />
                            <asp:ListItem Text="Check In" Value="1" />
                        </asp:RadioButtonList>
                        <asp:RadioButtonList runat="server" ID="rblPickupReturnLogic" RepeatDirection="Horizontal" Visible="False"
                            OnSelectedIndexChanged="rblPickupReturnLogic_Changed"
                            AutoPostBack="True">
                            <asp:ListItem Text="CheckOut" Value="True" Selected="True" />
                            <asp:ListItem Text="CheckIn" Value="" />
                        </asp:RadioButtonList>
                    </td>
                </tr>
            </table>
        </td>
        <td colspan="2">
            <table style="width: 100%">
                <tr>
                    <td style="float: right;">
                        <asp:RadioButtonList runat="server" ID="rblUpgradedLogic" RepeatDirection="Horizontal" Visible="False">
                            <asp:ListItem Text="Reserved" Value="" Selected="True" />
                            <asp:ListItem Text="Upgraded" Value="True" />
                        </asp:RadioButtonList>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Panel runat="server" ID="pnlLocationBranch2">
                <asp:Label runat="server" ID="lblPool" Text="Pool:" />
                <asp:Label runat="server" ID="lblRegion" Text="Region:" />
            </asp:Panel>
        </td>
        <td>
            <asp:Panel runat="server" ID="pnlLocationBranch3">
                <asp:DropDownList runat="server" ID="ddlPool" CssClass="SingleDropDownList" AutoPostBack="True" OnSelectedIndexChanged="ParameterChanged" />
                <asp:DropDownList runat="server" ID="ddlRegion" CssClass="SingleDropDownList" AutoPostBack="True" OnSelectedIndexChanged="ParameterChanged" />
            </asp:Panel>
        </td>
        <td>Car Segment:</td>
        <td>
            <asp:DropDownList runat="server" ID="ddlCarSegment" CssClass="SingleDropDownList" AutoPostBack="True" OnSelectedIndexChanged="ParameterChanged" />
        </td>

    </tr>
    <tr>
        <td>
            <asp:Panel runat="server" ID="pnlLocationBranch4">
                <asp:Label runat="server" ID="lblLocationGroup" Text="Location Group:" />
                <asp:Label runat="server" ID="lblArea" Text="Area:" />
            </asp:Panel>
        </td>
        <td>
            <asp:Panel runat="server" ID="pnlLocationBranch5">
                <asp:DropDownList runat="server" ID="ddlLocationGroup" CssClass="SingleDropDownList" AutoPostBack="True" OnSelectedIndexChanged="ParameterChanged" />
                <asp:DropDownList runat="server" ID="ddlArea" AutoPostBack="True" CssClass="SingleDropDownList" OnSelectedIndexChanged="ParameterChanged" />
            </asp:Panel>
        </td>
        <td>
            <asp:Label runat="server" ID="lblCarClass" Text="Car Class:" />
        </td>
        <td>
            <asp:DropDownList runat="server" ID="ddlCarClass" CssClass="SingleDropDownList" AutoPostBack="True" OnSelectedIndexChanged="ParameterChanged" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label runat="server" ID="lblLocation" Text="Location:" />
        </td>
        <td>
            <asp:DropDownList runat="server" ID="ddlLocation" CssClass="SingleDropDownList" AutoPostBack="True" OnSelectedIndexChanged="ParameterChanged" />
        </td>
        <td>
            <asp:Label runat="server" ID="lblCarGroup" Text="Car Group:" />
        </td>
        <td>
            <asp:DropDownList runat="server" ID="ddlCarGroup" CssClass="SingleDropDownList" />
        </td>

    </tr>
    <tr>
        <td>
            <asp:Label runat="server" ID="lblQuickLocation" Text="Quick Location:" />
        </td>
        <td>
            <asp:TextBox runat="server" ID="tbQuickLocation" CssClass="AutoCompleteTextBox" Width="155px"
                onkeydown='QuickSelectMade' />
            <asp:AutoCompleteExtender ID="acLocation" runat="server" ServiceMethod="GetBranchList"
                TargetControlID="tbQuickLocation" ServicePath="~/AutoComplete.asmx" MinimumPrefixLength="1"
                CompletionInterval="500" EnableCaching="true" CompletionSetCount="8" OnClientItemSelected='QuickSelectMade'
                UseContextKey="True" />


        </td>
        <td>
            <asp:Label runat="server" ID="lblQuickCarGroup" Text="Quick Car Group:" />
        </td>
        <td>
            <asp:TextBox runat="server" ID="tbQuickCarGroup" CssClass="AutoCompleteTextBox" Width="155px"
                onkeydown="QuickSelectMade" />
            <asp:AutoCompleteExtender ID="acCarGroup" runat="server" ServiceMethod="SearchCarGroup"
                TargetControlID="tbQuickCarGroup" ServicePath="~/AutoComplete.asmx" MinimumPrefixLength="1"
                CompletionInterval="500" EnableCaching="true" CompletionSetCount="8" OnClientItemSelected="QuickSelectMade"
                UseContextKey="True" />
        </td>
    </tr>
</table>



