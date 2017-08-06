<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReasonEntryForm.ascx.cs" Inherits="Mars.App.UserControls.Phase4.NonRev.ReasonEntryForm" %>
<div style="vertical-align: top; width: 100%; text-align: left;" class="StandardBorder">
    
    <script type="text/javascript">
    function checkDate(sender, args) {
        var d = new Date();
        d.setHours(0, 0, 0, 0);
        if (sender._selectedDate < d) 
        {
            alert("An estimated resolution date can not be in the past");
            sender._selectedDate = new Date();
            // set the date back to the current date
            sender._textbox.set_Value(sender._selectedDate.format(sender._format));
        }
    }
    </script>

    <asp:Panel ID="pnlReasonEntry" runat="server">
        <table style="width: 100%; background-color: white; text-align: right;"  >
            <tr >
                <td style="width: 110px; font-weight: bold;">Reason:</td>
                <td style="text-align: left;" colspan="2">
                    <asp:DropDownList runat="server" ID="ddlReasonCode" CssClass="dropDownListFormSettings"
                        Width="250px" Font-Size="12px" />
                    <asp:Label runat="server" ID="lblErrorMessage" ForeColor="Red" />
                </td>
            </tr>
            <tr >
                <td style="font-weight: bold;">Esimtated Resolution Date:</td>
                <td style="text-align: left;" colspan="2">
                    <asp:TextBox runat="server"  ID="tbEstimatedResolvedDate" Height="16px"  />
                    <asp:CalendarExtender runat="server" TargetControlID="tbEstimatedResolvedDate" 
                        Format="dd/MM/yyyy" OnClientDateSelectionChanged="checkDate" />
                    <asp:CompareValidator
                        id="cvResolutionDate" runat="server" 
                        Type="Date"
                        Operator="DataTypeCheck"
                        ControlToValidate="tbEstimatedResolvedDate" 
                        ErrorMessage="Please enter a valid date." />
                </td>
            </tr>
            <tr>
                <td style="vertical-align: top;font-weight: bold;">
                    Remark:
                </td>
                <td style="text-align: left;">
                    <asp:TextBox ID="taRemarks" runat="server" TextMode="MultiLine" CssClass="dropDownListFormSettings"
                        Width="340px" Height="60px" />
                </td>
                <td  style="vertical-align: bottom; ">
                    <asp:Button ID="btnAddEntry" CssClass="StandardButton" runat="server" Text="Add" Width="70px" OnClick="btnAddEntry_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    
   
</div>
