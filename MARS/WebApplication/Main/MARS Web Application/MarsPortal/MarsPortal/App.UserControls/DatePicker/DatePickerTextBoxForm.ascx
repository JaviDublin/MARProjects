<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DatePickerTextBoxForm.ascx.cs" Inherits="App.UserControls.DatePicker.DatePickerTextBoxForm" %>
<script type="text/javascript" language="javascript">

    Sys.Application.add_load(LoadCalendar);

    function LoadCalendar() {
        $(document).ready(function () {

            var imageUrl = document.getElementById('<%=ImageCalendar.ClientID%>').src;
            $('#<%= TextBoxDatePicker.ClientID %>').datepicker({
                showOn: 'both',
                buttonImage: imageUrl,
                buttonImageOnly: true,
                buttonText: '',
                dateFormat: 'dd/mm/yy',
                firstDay: 1,
                showButtonPanel: true
            });
        });
    }

    function ValidateDate(source, args) {
        var regexPattern = /^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((1[6-9]|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$/;
        var dateValue = args.Value;

        if (dateValue.length < 10) {
            args.IsValid = false;
        }
        else {
            if (dateValue.match(regexPattern)) {
                args.IsValid = true;
            }
            else {
                args.IsValid = false;
            }
        }
    }
  
</script>
<table cellpadding="0" cellspacing="0">
    <tr>
        <td style="padding-right: 5px;">
            <asp:TextBox ID="TextBoxDatePicker" runat="server" MaxLength="10" CssClass="textBoxListForm" Width="80px"></asp:TextBox>
        </td>
        <td>
            <asp:CustomValidator ID="CustomValidatorDate" runat="server" ClientValidationFunction="ValidateDate" ControlToValidate="TextBoxDatePicker" Display="Dynamic" ValidateEmptyText="true"></asp:CustomValidator>
        </td>
    </tr>
</table>
<asp:Image ID="ImageCalendar" runat="server" ImageUrl="~/App.Images/calendar.png" Style="display: none;" AlternateText="Calendar hidden image" />
