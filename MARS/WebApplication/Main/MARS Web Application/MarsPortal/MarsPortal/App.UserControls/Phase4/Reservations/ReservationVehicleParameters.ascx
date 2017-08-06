<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReservationVehicleParameters.ascx.cs" Inherits="Mars.App.UserControls.Phase4.Reservations.ReservationVehicleParameters" %>

<table style="width: 700px; margin-left: auto; margin-right: auto; border-collapse: separate; border-spacing: 0;  text-align: left;">
    <tr>
        <td colspan="4" style="text-align: center; font-size: 12px; font-weight: bold;">
            Check Out
        </td>
        <td colspan="2" style="text-align: center; font-size: 12px; font-weight: bold;">
            Check In
        </td>

    </tr>
    <tr >
        <td colspan="4" style="text-align: center; 
                        border-top-style: solid; border-top-color: lightgray; border-top-width: 3px; padding-top: 5px; 
                        border-left-style: solid; border-left-color: lightgray; border-left-width: 3px;
                        border-right-style: solid; border-right-color: lightgray; border-right-width: 3px;">
            Country:
            <asp:DropDownList runat="server" ID="ddlCheckOutCountry" CssClass="SingleDropDownList" AutoPostBack="True" OnSelectedIndexChanged="ParameterChanged" />
        </td>

        <td style="padding-left: 15px;
                    border-top-style: solid; border-top-color: lightgray; border-top-width: 3px; padding-top: 5px;
                    ">
            Country:
        </td>
        <td style="padding-right: 15px;
                    border-top-style: solid; border-top-color: lightgray; border-top-width: 3px; padding-top: 5px;
                    border-right-style: solid; border-right-color: lightgray; border-right-width: 3px;">
            <asp:DropDownList runat="server" ID="ddlCheckInCountry" CssClass="SingleDropDownList" AutoPostBack="True" OnSelectedIndexChanged="ParameterChanged" />
        </td>
    </tr>
    <tr >
        <td colspan="2" style="border-left-style: solid; border-left-color: lightgray; border-left-width: 3px; padding-left: 15px;" >
            <asp:RadioButtonList runat="server" ID="rblCmsOpsLogic" OnSelectedIndexChanged="rblCmsOpsLogic_SelectionChanged"
                RepeatDirection="Horizontal" AutoPostBack="True">
                <asp:ListItem Text="CMS" Value="Cms" Selected="True" />
                <asp:ListItem Text="OPS" Value="Ops" />
            </asp:RadioButtonList>
        </td>
        <td>&nbsp;
        </td>
        <td style="border-right-style: solid; border-right-color: lightgray; border-right-width: 3px;">
            <asp:RadioButtonList runat="server" ID="rblUpgradedLogic" RepeatDirection="Horizontal">
                <asp:ListItem Text="Reserved" Value="" Selected="True" />
                <asp:ListItem Text="Upgraded" Value="True" />
            </asp:RadioButtonList>

        </td>
        <td colspan="2" style="border-right-style: solid; border-right-color: lightgray; border-right-width: 3px;"></td>
    </tr>
    <tr >
        <td style="border-left-style: solid; border-left-color: lightgray; border-left-width: 3px; padding-left: 15px;">
            <asp:Label runat="server" ID="lblCheckOutPool" Text="Pool:" />
            <asp:Label runat="server" ID="lblCheckOutRegion" Text="Region:" />
        </td>
        <td style="border-right-style: solid; border-right-color: lightgray; border-right-width: 3px;">
            <asp:DropDownList runat="server" ID="ddlCheckOutPool" CssClass="SingleDropDownList" AutoPostBack="True" OnSelectedIndexChanged="ParameterChanged" />
            <asp:DropDownList runat="server" ID="ddlCheckOutRegion" CssClass="SingleDropDownList" AutoPostBack="True" OnSelectedIndexChanged="ParameterChanged" />
        </td>
        <td>Car Segment:</td>
        <td style="border-right-style: solid; border-right-color: lightgray; border-right-width: 3px; padding-right: 15px;">
            <asp:DropDownList runat="server" ID="ddlCarSegment" CssClass="SingleDropDownList" AutoPostBack="True" OnSelectedIndexChanged="ParameterChanged" />
        </td>
        <td style="padding-left: 15px; ">
            <asp:Label runat="server" ID="lblCheckInPool" Text="Pool:" />
            <asp:Label runat="server" ID="lblCheckInRegion" Text="Region:" />
        </td>
        <td style="border-right-style: solid; border-right-color: lightgray; border-right-width: 3px;">
            <asp:DropDownList runat="server" ID="ddlCheckInPool" CssClass="SingleDropDownList" AutoPostBack="True" OnSelectedIndexChanged="ParameterChanged" />
            <asp:DropDownList runat="server" ID="ddlCheckInRegion" CssClass="SingleDropDownList" AutoPostBack="True" OnSelectedIndexChanged="ParameterChanged" />
        </td>
    </tr>
    <tr>
        <td style="border-left-style: solid; border-left-color: lightgray; border-left-width: 3px; padding-left: 15px;">
            <asp:Label runat="server" ID="lblCheckOutLocationGroup" Text="Location Group:" />
            <asp:Label runat="server" ID="lblCheckOutArea" Text="Area:" />
        </td>
        <td>
            <asp:DropDownList runat="server" ID="ddlCheckOutLocationGroup" CssClass="SingleDropDownList" AutoPostBack="True" OnSelectedIndexChanged="ParameterChanged" />
            <asp:DropDownList runat="server" ID="ddlCheckOutArea" AutoPostBack="True" CssClass="SingleDropDownList" OnSelectedIndexChanged="ParameterChanged" />
        </td>
        <td>Car Class:</td>
        <td style="border-right-style: solid; border-right-color: lightgray; border-right-width: 3px;">
            <asp:DropDownList runat="server" ID="ddlCarClass" CssClass="SingleDropDownList" AutoPostBack="True" OnSelectedIndexChanged="ParameterChanged" />
        </td>
        <td style="padding-left: 15px">
            <asp:Label runat="server" ID="lblCheckInLocationGroup" Text="Location Group:" />
            <asp:Label runat="server" ID="lblCheckInArea" Text="Area:" />
        </td>
        <td style="border-right-style: solid; border-right-color: lightgray; border-right-width: 3px;">
            <asp:DropDownList runat="server" ID="ddlCheckInLocationGroup" CssClass="SingleDropDownList" AutoPostBack="True" OnSelectedIndexChanged="ParameterChanged" />
            <asp:DropDownList runat="server" ID="ddlCheckInArea" AutoPostBack="True" CssClass="SingleDropDownList" OnSelectedIndexChanged="ParameterChanged" />
        </td>
    </tr>
    <tr>
        <td style="border-left-style: solid; border-left-color: lightgray; border-left-width: 3px; padding-left: 15px;">
            Location:
        </td>
        <td>
            <asp:DropDownList runat="server" ID="ddlCheckOutLocation" CssClass="SingleDropDownList" AutoPostBack="True" OnSelectedIndexChanged="ParameterChanged" />
        </td>
        <td>Car Group:</td>
        <td style="border-right-style: solid; border-right-color: lightgray; border-right-width: 3px;">
            <asp:DropDownList runat="server" ID="ddlCarGroup" CssClass="SingleDropDownList" />
        </td>
        <td style="padding-left: 15px">
            Location:
        </td>
        <td style="border-right-style: solid; border-right-color: lightgray; border-right-width: 3px;">
            <asp:DropDownList runat="server" ID="ddlCheckInLocation" CssClass="SingleDropDownList" AutoPostBack="True" OnSelectedIndexChanged="ParameterChanged" />
        </td>
    </tr>
    <tr>
        <td style="border-left-style: solid; border-left-color: lightgray; border-left-width: 3px;
                    border-bottom-style: solid; border-bottom-color: lightgray; border-bottom-width: 3px; padding-bottom: 5px;
                    padding-left: 15px;">
            Quick Location:
        </td>
        <td style="border-bottom-style: solid; border-bottom-color: lightgray; border-bottom-width: 3px; padding-bottom: 5px;">
            <asp:TextBox runat="server" ID="tbQuickCheckOutLocation" CssClass="AutoCompleteTextBox" Width="155px"
                onkeydown="keyPressedLocation()" />
            <asp:AutoCompleteExtender ID="acCheckOutLocation" runat="server" ServiceMethod="GetBranchList"
                TargetControlID="tbQuickCheckOutLocation" ServicePath="~/AutoComplete.asmx" MinimumPrefixLength="1"
                CompletionInterval="500" EnableCaching="true" CompletionSetCount="8" OnClientItemSelected="UpdateOutLocation"
                UseContextKey="True" />
        </td>
        <td style="border-bottom-style: solid; border-bottom-color: lightgray; border-bottom-width: 3px; padding-bottom: 5px;">
            <asp:Label runat="server" ID="lblQuickCarGroup" Text="Quick Car Group:" />
        </td>
        <td style="border-right-style: solid; border-right-color: lightgray; border-right-width: 3px; 
                    border-bottom-style: solid; border-bottom-color: lightgray; border-bottom-width: 3px; padding-bottom: 5px;">
            <asp:TextBox runat="server" ID="tbQuickCarGroup" CssClass="AutoCompleteTextBox" Width="155px"
                onkeydown="keyPressedCarGroup()" />
            <asp:AutoCompleteExtender ID="acCarGroup" runat="server" ServiceMethod="SearchCarGroup"
                TargetControlID="tbQuickCarGroup" ServicePath="~/AutoComplete.asmx" MinimumPrefixLength="1"
                CompletionInterval="500" EnableCaching="true" CompletionSetCount="8" OnClientItemSelected="UpdateCarGroupSingle"
                UseContextKey="True" />
        </td>
        <td style="padding-left: 15px; border-bottom-style: solid; border-bottom-color: lightgray; border-bottom-width: 3px; padding-bottom: 5px;">
            Quick Location:
        </td>
        <td style="border-right-style: solid; border-right-color: lightgray; border-right-width: 3px;
             border-bottom-style: solid; border-bottom-color: lightgray; border-bottom-width: 3px; padding-bottom: 5px;">
            <asp:TextBox runat="server" ID="tbQuickCheckInLocation" CssClass="AutoCompleteTextBox" Width="155px"
                onkeydown="keyPressedLocation()" />
            <asp:AutoCompleteExtender ID="acCheckInLocation" runat="server" ServiceMethod="GetBranchList"
                TargetControlID="tbQuickCheckInLocation" ServicePath="~/AutoComplete.asmx" MinimumPrefixLength="1"
                CompletionInterval="500" EnableCaching="true" CompletionSetCount="8" OnClientItemSelected="UpdateInLocation"
                UseContextKey="True" />
        </td>
    </tr>
</table>


