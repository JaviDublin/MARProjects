<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuickSelectLocationGroup.ascx.cs"
    Inherits="App.UserControls.Parameters.QuickSelectLocationGroup" %>
<asp:UpdatePanel ID="upQuickSelect" runat="server">
    <ContentTemplate>
        <asp:Panel runat="server" ID="pnlParameters" Width="100%">
            <fieldset style="height: 39px">
                <legend>
                    <asp:Label runat="server" Text="<%$ Resources:LocalizedParameterControl, QuickSelectionLocationGroupLegend %>" />
                </legend>
                <table> 
                <tr>
                    <td>
                        <asp:TextBox ID="tbQuickLocationGroup" runat="server" CssClass="QuickLocationGroupInput"
                        onkeydown="return (event.keyCode!=13);" Width="135px" />
                    </td>
                </tr>
                </table>
                
            </fieldset>
            <asp:AutoCompleteExtender ID="AutoCompleteMain" runat="server" ServiceMethod="GetLocationPoolList"
                TargetControlID="tbQuickLocationGroup" ServicePath="~/AutoComplete.asmx" MinimumPrefixLength="1"
                CompletionInterval="500" EnableCaching="true" CompletionSetCount="8" OnClientItemSelected="UpdateGeneralParams"
                UseContextKey="True" />

            <script language="JavaScript" type="text/javascript">
                function UpdateGeneralParams() {
                    var evt = window.event;
                    if(evt.type == "mousedown")
                        __doPostBack("<%= upQuickSelect.ClientID %>", "");
                }
            </script>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
