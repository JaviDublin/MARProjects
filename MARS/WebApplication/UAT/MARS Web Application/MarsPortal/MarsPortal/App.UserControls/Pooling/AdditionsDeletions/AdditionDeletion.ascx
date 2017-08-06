<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdditionDeletion.ascx.cs" Inherits="Mars.App.UserControls.Pooling.AdditionsDeletions.AdditionDeletion" %>

<%@ Register Src="~/App.UserControls/DatePicker/SingleDateTimePicker.ascx" TagName="SingleDatePicker" TagPrefix="uc" %>
<style type="text/css">
    .QuickLocationGroupInput
    {
    }
</style>

<asp:UpdatePanel runat="server" ID="upAddDel">
    <ContentTemplate>
        
        <table style="text-align: center;">
            <tr>
                <td>&nbsp;
                </td>
                <td>WWD
                </td>
                <td>Car Group
                </td>
                <td>Move Date &amp; Time
                </td>
                <td>Amount
                </td>
            </tr>
            <tr>
                <td>
                    <asp:RadioButtonList ID="rblMoveType" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Addition" Value="1" Selected="True" />
                        <asp:ListItem Text="Deletion" Value="0" />
                    </asp:RadioButtonList>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="tbWwd" CssClass="QuickLocationGroupInput"
                        onkeydown="return (event.keyCode!=13);" Width="65px" />

                    <asp:AutoCompleteExtender ID="acWwd" runat="server" ServiceMethod="GetBranchList"
                        TargetControlID="tbWwd" ServicePath="~/AutoComplete.asmx" MinimumPrefixLength="1"
                        CompletionInterval="500" EnableCaching="true" CompletionSetCount="8"
                        UseContextKey="True" />
                </td>
                <td>

                    <asp:TextBox runat="server" ID="tbCarGroup" Width="41px" />
                    <asp:AutoCompleteExtender ID="acCarGroup" runat="server" ServiceMethod="SearchCarGroup"
                        TargetControlID="tbCarGroup" ServicePath="~/AutoComplete.asmx" MinimumPrefixLength="1"
                        CompletionInterval="500" EnableCaching="true" CompletionSetCount="8"
                        UseContextKey="True" />
                </td>

                <td>
                    <uc:SingleDatePicker ID="sdpRepDate" runat="server"  />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="tbValue" Width="28px" />
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" />
                </td>
            </tr>

        </table>
        <div style="width: 100%; text-align: right">
            <asp:Button runat="server" ID="btnAdd" Text="Add" OnClick="btnAdd_Click" Width="130px" />
        </div>
        <script language="JavaScript" type="text/javascript">
            function UpdateGeneralParams() {
                var evt = window.event;
                if (evt.type == "mousedown")
                    __doPostBack("<%= upAddDel.ClientID %>", "");
            }
        </script>

    </ContentTemplate>
</asp:UpdatePanel>
