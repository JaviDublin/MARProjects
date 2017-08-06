<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DateRangePicker.ascx.cs"
    Inherits="App.UserControls.DatePicker.DateRangePicker" %>
<asp:Panel ID="pnlCustomDateRange" runat="server" Width="170px">
    <fieldset style="height: 87px">
        <legend>
            <asp:Label runat="server" Text="<%$ Resources:LocalizedParameterControl, DatePickerRangeLegend %>" /></legend>
        <table>
            <tr>
                <td>
                    <asp:Label runat="server" Text="<%$ Resources:LocalizedParameterControl, DatePickerDurationLabel %>" />
                </td>
                <td>
                    <asp:DropDownList ID="ddlDateRange" runat="server" AutoPostBack="true" Width="95px">
                        <asp:ListItem Text="<%$ Resources:LocalizedParameterControl, DatePickerOptionCustom %>"
                            Value="Custom" />
                        <asp:ListItem Text="<%$ Resources:LocalizedParameterControl, DatePickerOptionThisWeek %>"
                            Value="ThisWeek" Enabled="false"  />
                        <asp:ListItem Text="<%$ Resources:LocalizedParameterControl, DatePickerOptionNextWeek %>"
                            Value="NextWeek"  Enabled="false" />
                        
                        <asp:ListItem Text="<%$ Resources:LocalizedParameterControl, DatePickerOption7Days %>"
                            Value="7" />
                        <asp:ListItem Text="<%$ Resources:LocalizedParameterControl, DatePickerOption30Days %>"
                            Value="30" Selected="True" />
                        <asp:ListItem Text="<%$ Resources:LocalizedParameterControl, DatePickerOption60Days %>"
                            Value="60" />
                        <asp:ListItem Text="<%$ Resources:LocalizedParameterControl, DatePickerOption90Days %>"
                            Value="90" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblStartDate" runat="server" Text="<%$ Resources:LocalizedParameterControl, DatePickerFromLabel %>" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="tbFromDate" Width="65px" OnTextChanged="DateChanged" />
                    <asp:CompareValidator ID="cvFromDate" runat="server" ControlToValidate="tbFromDate" ErrorMessage="Invalid start Date Format"
                        Text="*" Operator="DataTypeCheck" Type="Date" ValidationGroup="Dates" ForeColor="Red"/>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblToDate" runat="server" Text="<%$ Resources:LocalizedParameterControl, DatePickerToLabel %>" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="tbToDate" Width="65px" OnTextChanged="DateChanged" />
                    <asp:CompareValidator ID="cvToDate" runat="server" ControlToValidate="tbToDate" ErrorMessage="Invalid end Date Format"
                        Text="*" Operator="DataTypeCheck" Type="Date" ValidationGroup="Dates" ForeColor="Red"/>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="RangeValidator" ControlToValidate="tbFromDate" Type="Date"></asp:RangeValidator>
                </td>
            </tr>
        </table>
        <asp:ValidationSummary ID="vsDatePicker" 
                runat="server" EnableClientScript="true" 
                HeaderText="Please correct date format"
                ValidationGroup="Dates"/>

            
    </fieldset>
</asp:Panel>
<asp:CalendarExtender ID="ceFromDateExtender" runat="server" TargetControlID="tbFromDate" CssClass="chartPanelBackground"
    Format="dd/MM/yyyy" />
<asp:CalendarExtender ID="ceToDateExtender" runat="server" TargetControlID="tbToDate" CssClass="chartPanelBackground"
    Format="dd/MM/yyyy" />

