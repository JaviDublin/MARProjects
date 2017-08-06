<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SingleDateTimePicker.ascx.cs" Inherits="Mars.App.UserControls.DatePicker.SingleDateTimePicker" %>



<table>
    <tr>
        <td>
            <asp:TextBox runat="server" ID="tbDateSelection" CssClass ="SingleDatePicker" Width="70px" /> -
        </td>
        <td>
            
            <asp:TextBox runat="server" ID="tbTimeSelection" CssClass="SingleTimePicker" Width="65px" />
        </td>
    </tr>
</table>