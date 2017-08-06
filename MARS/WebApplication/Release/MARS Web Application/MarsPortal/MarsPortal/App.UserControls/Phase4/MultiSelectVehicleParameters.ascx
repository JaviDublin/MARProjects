<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MultiSelectVehicleParameters.ascx.cs" Inherits="Mars.App.UserControls.Phase4.MultiSelectVehicleParameters" %>
<asp:UpdatePanel ID="upnlParameters" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table style="width: 450px; margin-left: auto; 
                margin-right: auto; border-collapse: separate;
                border-spacing: 6px; text-align: left;">
            <tr>
                <td>Location Country:</td>
                <td>
                    <asp:ListBox runat="server" ID="lbLocationCountry" CssClass="MultiDropDownListShowSelected"
                        SelectionMode="Multiple" />
                </td>
                <td>Owning Country:</td>
                <td>
                    <asp:ListBox runat="server" ID="lbOwningCountry" CssClass="MultiDropDownListShowSelected"
                        SelectionMode="Multiple" />
                </td>
            </tr>
            <tr>
                <td colspan="2" >
                    <table style="width: 100%">
                        <tr>
                            <td>
                                <asp:RadioButtonList runat="server" ID="rblCmsOpsLogic" OnSelectedIndexChanged="rblCmsOpsLogic_SelectionChanged"
                                    RepeatDirection="Horizontal" AutoPostBack="True">
                                    <asp:ListItem Text="CMS" Value="Cms" Selected="True" />
                                    <asp:ListItem Text="OPS" Value="Ops" />
                                </asp:RadioButtonList>
                            </td>
                            <td style="float: right;">
                                <asp:RadioButtonList runat="server" ID="rblLocationLogic" RepeatDirection="Horizontal" Visible="True">
                                    <asp:ListItem Text="Check In" Value="" Selected="True" />
                                    <asp:ListItem Text="Check Out" Value="1" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>

                </td>
                <td colspan="2">
                    <table style="width: 100%">
                        <tr>
                            <td style="float: right;">
                                <asp:RadioButtonList runat="server" ID="rblUpgradedLogic" RepeatDirection="Horizontal" Visible="False">
                                    <asp:ListItem Text="Reserved" Value="" Selected="True" />
                                    <asp:ListItem Text="Upgraded" Value="True" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>
                                
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblPool" Text="Pool:"  />
                    <asp:Label runat="server" ID="lblRegion" Text="Region:"  />
                </td>
                <td>
                    <asp:ListBox runat="server" ID="lbPool" CssClass="MultiDropDownListShowSelected" SelectionMode="Multiple"
                        AutoPostBack="False" />
                    <asp:ListBox runat="server" ID="lbRegion" CssClass="MultiDropDownListShowSelected" SelectionMode="Multiple"
                        AutoPostBack="False" />
                </td>
                <td>Car Segment:</td>
                <td>
                    <asp:ListBox runat="server" ID="lbCarSegment" CssClass="MultiDropDownListShowSelected" 
                        SelectionMode="Multiple" />
                </td>

            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblLocationGroup" Text="Location Group:" />
                    <asp:Label runat="server" ID="lblArea" Text="Area:" />
                </td>
                <td>
                    <asp:ListBox runat="server" ID="lbLocationGroup" CssClass="MultiDropDownListShowSelected"
                        SelectionMode="Multiple" />
                    <asp:ListBox runat="server" ID="lbArea" CssClass="MultiDropDownListShowSelected"
                        SelectionMode="Multiple" />
                </td>
                <td>Car Class:</td>
                <td>
                    <asp:ListBox runat="server" ID="lbCarClass" CssClass="MultiDropDownListShowSelected" SelectionMode="Multiple" />
                </td>

            </tr>
            <tr>
                <td>Location:</td>
                <td>
                    <asp:ListBox runat="server" ID="lbLocation" CssClass="MultiDropDownListShowSelected"
                        SelectionMode="Multiple" />
                </td>
                <td>Car Group:</td>
                <td>
                    <asp:ListBox runat="server" ID="lbCarGroup" CssClass="MultiDropDownListShowSelected" SelectionMode="Multiple" />
                </td>
            </tr>
            <tr>
                <td>Quick Location:
                </td>
                <td>
                    <asp:TextBox runat="server" ID="tbQuickLocation" CssClass="AutoCompleteTextBox" Width="155px"
                        onkeydown="QuickSelectMultiple" />
                    <asp:AutoCompleteExtender ID="acLocation" runat="server" ServiceMethod="GetBranchList"
                        TargetControlID="tbQuickLocation" ServicePath="~/AutoComplete.asmx" MinimumPrefixLength="1"
                        CompletionInterval="500" EnableCaching="true" CompletionSetCount="8" 
                        OnClientItemSelected="QuickSelectMultiple"
                        UseContextKey="True" />

     
                </td>
                <td>
                    <asp:Label runat="server" ID="lblQuickCarGroup" Text="Quick Car Group:" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="tbQuickCarGroup" CssClass="AutoCompleteTextBox" Width="155px"
                        onkeydown="QuickSelectMultiple" />
                    <asp:AutoCompleteExtender ID="acCarGroup" runat="server" ServiceMethod="SearchCarGroup"
                        TargetControlID="tbQuickCarGroup" ServicePath="~/AutoComplete.asmx" MinimumPrefixLength="1"
                        CompletionInterval="500" EnableCaching="true" CompletionSetCount="8" OnClientItemSelected="QuickSelectMultiple"
                        UseContextKey="True" />
                </td>
            </tr>
        </table>

    </ContentTemplate>
</asp:UpdatePanel>

<script>
    $(document).ready(function () {

        applyMultiSelects();
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(panelLoaded);

        function panelLoaded(sender, args) {
            if (args.get_panelsUpdated().length > 0) {
                applyMultiSelects();
                
            }
        }


        function applyMultiSelects() {

            $('.MultiDropDownListShowSelected').multiselect({
                autoOpen: false,
                minWidth: 160,
                close: function (event, ui) {
                    var elements = $(this).find(":selected");

                    //if (elements.length != 0) {
                        __doPostBack("<%= upnlParameters.ClientID %>", $(this).attr('id'));
                        
                    ////} 
                },
                click: function(e) {
                    if ($(this).multiselect("widget").find("input:checked").length > 20) {
                        return false;
                    } else {
                        
                    }
                },
                selectedList: 20,
                position: {
                    my: 'top',
                    at: 'top'
                }
            });
        }

        applyMultiSelects();

    });
</script>



