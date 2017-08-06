<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EntityParameter.ascx.cs" Inherits="Mars.App.UserControls.Phase4.Administration.Mapping.EntityParameter" %>


<asp:HiddenField runat="server" ID="hfParameterType" />
<asp:HiddenField runat="server" ID="hfActiveOnly" />

<table>
    <tr style="vertical-align: top;">
        <td>
            <table style="width: 300px;">
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblActive" />

                    </td>
                    <td>
                        <asp:RadioButtonList runat="server" ID="rbActive"
                            RepeatDirection="Horizontal"
                            CssClass="StandardRadioButtonList"
                            AutoPostBack="True" OnSelectedIndexChanged="rbActive_SelectionChanged">
                            <asp:ListItem Text="Yes" Value="0" Selected="True" />
                            <asp:ListItem Text="No" Value="1" />
                            <asp:ListItem Text="All" Value="2" />
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblQuickComplete" />
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="tbQuickComplete" onkeyup="RefreshUpdatePanel();"  
                             OnTextChanged="AutoCompleteEntity" onfocus="this.value = this.value;" />
                    </td>
                </tr>

                <tr style="height: 20px;">
                    <td colspan="2">&nbsp;
                    </td>
                </tr>
            </table>
        </td>
        <td>
            <table style="width: 300px;">
                <tr>
                    <td colspan="2">
                        <asp:RadioButtonList runat="server" ID="rblCmsOps"
                            RepeatDirection="Horizontal" Visible="False"
                            CssClass="StandardRadioButtonList"
                            AutoPostBack="True" OnSelectedIndexChanged="rblCmsOps_SelectionChanged">
                            <asp:ListItem Text="CMS" Value="0" Selected="True" />
                            <asp:ListItem Text="OPS" Value="1" />
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblCountry" Text="Country:" Visible="False" />
                        
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlCountry" AutoPostBack="True" Visible="False"
                            OnSelectedIndexChanged="ddlCountry_SelectionChanged" CssClass="StandardAdminDropdown" />
                        
                            <asp:CheckBox runat="server" ID="cbActiveCountry" Text="Active Only" CssClass="StandardCheckBox"
                                Checked="True" AutoPostBack="True" OnCheckedChanged="cbActiveCountry_Changed"
                                Font-Size="9px" Visible="False" />
                        
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblPool" Text="Pool:" Visible="False" />
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlPool" AutoPostBack="True" Visible="False" CssClass="StandardAdminDropdown"
                            OnSelectedIndexChanged="ddlPool_SelectionChanged" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblRegion" Text="Region:" Visible="False" />
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlRegion" AutoPostBack="True" Visible="False" CssClass="StandardAdminDropdown"
                            OnSelectedIndexChanged="ddlRegion_SelectionChanged" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblLocationGroup" Text="Location Group:" Visible="False" />
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlLocationGroup" AutoPostBack="True" Visible="False" CssClass="StandardAdminDropdown"
                            OnSelectedIndexChanged="ParameterChanged" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblArea" Text="Area:" Visible="False" />
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlArea" AutoPostBack="True" Visible="False" CssClass="StandardAdminDropdown"
                            OnSelectedIndexChanged="ParameterChanged" />
                    </td>
                </tr>
            </table>
        </td>
        <td>
            <table style="width: 200px;">
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblCarSegment" Text="Car Segment:" Visible="False" />
                        
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlCarSegment" AutoPostBack="True" Visible="False" CssClass="StandardAdminDropdown"
                            OnSelectedIndexChanged="ddlCarSegment_SelectionChanged" />
                        
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblCarClass" Text="Car Class:" Visible="False" />
                        <asp:Label runat="server" ID="lblCompany" Visible="False" Text="Company:" />
                    </td>

                    <td>
                        <asp:DropDownList runat="server" ID="ddlCarClass" AutoPostBack="True" Visible="False" CssClass="StandardAdminDropdown"
                            OnSelectedIndexChanged="ParameterChanged" />
                        <asp:DropDownList runat="server" ID="ddlCompany" Visible="False" AutoPostBack="True" OnSelectedIndexChanged="ParameterChanged"
                            Width="220px" />
                    </td>
                </tr>
            </table>
        </td>
        <td>
            <asp:Button runat="server" ID="btnAddNew" Text="" OnClick="btnAddNew_Click" CssClass="StandardButton" Width="120px"/>
        </td>
    </tr>
    <tr>
        <td colspan="3" >
            <table style="text-align: center; width: 100%;">
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblMessage" ForeColor="Red" Text="" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button runat="server" ID="btnLoad" Text="Load" OnClick="btnLoad_Click" CssClass="StandardButton" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>



<script language="JavaScript" type="text/javascript">

    $(document).keypress(function (e) {
        if (e.keyCode === 13) {
            e.preventDefault();
            return false;
        }
    });

 

    var t;

    function RefreshUpdatePanel() {
        if (t) {
            clearTimeout(t);
            t = setTimeout(myCallback, 1000);
        }
        else {
            t = setTimeout(myCallback, 1000);
        }

    };

    function myCallback() {
        __doPostBack('<%= tbQuickComplete.ClientID %>', '');
    }



    

</script>
