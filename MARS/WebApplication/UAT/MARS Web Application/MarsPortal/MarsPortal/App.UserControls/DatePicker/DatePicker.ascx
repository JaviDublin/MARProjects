<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DatePicker.ascx.cs"
    Inherits="App.UserControls.DatePicker.DatePicker" %>
<asp:Panel ID="pnlCustomDateRange" runat="server" Width="250px">
    <fieldset>
        <legend>
            <asp:Label ID="lblDatePicker" runat="server" Text="<%$ Resources:LocalizedParameterControl, DatePickerSingleLegend %>" />
        </legend>
        <table>
            <tr>
                <td colspan="3">
                    <asp:Label ID="lblStartDate" runat="server" Text="<%$ Resources:LocalizedParameterControl, DatePickerSingleLabel %>" />
                    
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnPrevDay" runat="server" Text="<<" CausesValidation="false"
                        onclick="btnPrevDay_Click" CssClass="chartbuttonoptionsNarrow" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="tbDate" Width="70px" OnTextChanged="DateChanged" AutoPostBack="true" CausesValidation="true" />
                </td>
                <td>
                    <asp:Button ID="btnNextDay" runat="server" Text=">>" CausesValidation="false" 
                        onclick="btnNextDay_Click" CssClass="chartbuttonoptionsNarrow" />
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="RangeValidator" 
                                        ControlToValidate="tbDate" Type="Date" Enabled="False" />
                </td>
            </tr>
        </table>
                    

    </fieldset>
</asp:Panel>
<asp:CalendarExtender ID="ceFromDateExtender" runat="server" TargetControlID="tbDate"
    Format="dd/MM/yyyy" />

