<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BottomDropDown.ascx.cs"
    Inherits="App.UserControls.Pooling.BottomDropDown" %>
<table cellpadding="10" width="96%">
    <tr>
        <td colspan="3" style="width: 48%" class="pooling-bottomFeedback">
            Please select the criteria for your report:
        </td>
        <td style="width: 16%" class="pooling-bottomFeedback">
        </td>
        <td style="width: 16%" class="pooling-bottomFeedback">
        </td>
        <td style="width: 16%" class="pooling-bottomFeedback">
        </td>
    </tr>
    <tr>
        <td colspan="6">
            <hr />
        </td>
    </tr>
    <tr>
        <td style="width: 16%" class="pooling-bottomFeedback">
            Country:
        </td>
        <td style="width: 16%" class="pooling-bottomFeedback">
            <asp:DropDownList ID="DropDownListCountry" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListCountry_SelectedIndexChanged">
            </asp:DropDownList>
        </td>
        <td style="width: 16%" class="pooling-bottomFeedback">
            Logic:
        </td>
        <td style="width: 16%" class="pooling-bottomFeedback">
            <asp:RadioButton ID="RadioButtonCMS" runat="server" Text="CMS" ClientIDMode="Static"
                AutoPostBack="True" OnCheckedChanged="RadioButtonCMS_CheckedChanged" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:RadioButton ID="RadioButtonOPS" runat="server" Text="OPS" ClientIDMode="Static"
                AutoPostBack="True" OnCheckedChanged="RadioButtonOPS_CheckedChanged" />
        </td>
        <td style="width: 16%">
        </td>
        <td style="width: 16%">
        </td>
    </tr>
    <tr>
        <td>
        </td>
        <td>
        </td>
        <td class="pooling-bottomFeedback">
            <asp:Label ID="labelBottomStaticPool" runat="server"></asp:Label>
        </td>
        <td class="pooling-bottomFeedback">
            <asp:DropDownList ID="DropDownListPool" Width="120px"
                runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListPool_SelectedIndexChanged" />
            
        </td>
        <td class="pooling-bottomFeedback">
            Car Segment:
        </td>
        <td class="pooling-bottomFeedback">
            <asp:DropDownList ID="DropDownListCarSegment" runat="server" AutoPostBack="true" 
                OnSelectedIndexChanged="DropDownListCarSegment_SelectedIndexChanged">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>
        </td>
        <td>
        </td>
        <td class="pooling-bottomFeedback">
            <asp:Label ID="labelBottomStaticLocation" runat="server"></asp:Label>
        </td>
        <td class="pooling-bottomFeedback">
            <asp:DropDownList ID="DropDownListLocationGroup" runat="server" AutoPostBack="True" Width="120px"
                OnSelectedIndexChanged="DropDownListLocationGroup_SelectedIndexChanged">
            </asp:DropDownList>
        </td>
        <td class="pooling-bottomFeedback">
            Car Class:
        </td>
        <td class="pooling-bottomFeedback">
            <asp:DropDownList ID="DropDownListCarClass" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListCarClass_SelectedIndexChanged">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>
        </td>
        <td>
        </td>
        <td class="pooling-bottomFeedback">
           <asp:Label ID="lblBranch" runat="server" Text="Branch:"> </asp:Label> 
        </td>
        <td class="pooling-bottomFeedback">
            <asp:DropDownList ID="DropDownListBranch" runat="server" AutoPostBack="True" Width="120px"
                              OnSelectedIndexChanged="DropDownListBranch_SelectedIndexChanged" />
            
        </td>
        <td class="pooling-bottomFeedback">
            <asp:Label ID="lblCarGroup" runat="server" Text="Car Group:"></asp:Label>
        </td>
        <td class="pooling-bottomFeedback">
            <asp:DropDownList ID="DropDownListCarGroup" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListCarGroup_SelectedIndexChanged">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td colspan="6">
            <hr />
        </td>
    </tr>
</table>
