<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NonRevenueParameters.ascx.cs"
    Inherits="Mars.App.UserControls.Parameters.NonRevenueParameters" %>
<%--<script type="text/javascript">
    $(document).ready(function () {
        $(".MultiDropDownList").dropdownchecklist({ width: 85 });
    });
</script>--%>
<asp:Panel runat="server" ID="pnlReportTypes">
    <fieldset style="height: 130px; text-align: left;">
        <legend>
            <asp:Label runat="server" Text="<%$ Resources:LocalizedParameterControl, ReportTypeLegend %>" /></legend>
        <table>
            <tr>
                <td colspan="2">
                    <asp:RadioButtonList runat="server" ID="rblReportType" RepeatDirection="Horizontal">
                        <asp:ListItem Value="Remarks" Text="Remarks" Selected="True" />
                        <asp:ListItem Value="Ageing" Text="Ageing" />
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                    Day Group:
                </td>
                <td>
                    <select id="GroupCodes" runat="server" multiple="True" class="MultiDropDownList">
                        <option selected="selected">0-2</option>
                        <option selected="selected">3</option>
                        <option selected="selected">4-6</option>
                        <option selected="selected">7</option>
                        <option selected="selected">8-10</option>
                        <option selected="selected">11-15</option>
                        <option selected="selected">16-30</option>
                        <option selected="selected">31-60</option>
                        <option selected="selected">60+</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td>
                    Fleet:
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlFleet" />
                </td>
            </tr>
            <tr>
                <td>
                    Group Type:
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlGroupType" />
                </td>
            </tr>
        </table>
    </fieldset>
</asp:Panel>
