<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserDetails.ascx.cs" Inherits="App.UserControls.Details.UserDetails" %>
<script type="text/javascript" language="javascript">

    function validateRACFID(source, args) {
        if (args.Value.length >= 1 && args.Value.length <= 20)
            args.IsValid = true;
        else
            args.IsValid = false;
    }

</script>
<asp:Panel runat="server" CssClass="modalPopupUserDetails" ID="PanelPopupUserDetails" Style="display: none;">
    <asp:Panel ID="PanelUserDetailsTitlePrint" runat="server" CssClass="PanelUserDetailsTitlePrint" Style="display: none;">
        <asp:Image ID="ImageTitlePrint" runat="server" AlternateText="Hertz Title" ImageUrl="~/App.images/mars_title_print.jpg" />
    </asp:Panel>
    <asp:Panel ID="PanelUserDetailsTitle" runat="server" CssClass="PanelUserDetailsTitle">
        <div class="divHertzLogoCarSearch">
            <asp:Image ID="ImageLogo" runat="server" AlternateText="Hertz Logo" ImageUrl="~/App.Images/hertz-logo.jpg" />
        </div>
        <div class="divApplicationLogoUserDetails">
            <asp:Image ID="ImageButtonApplicationLogo" runat="server" AlternateText="Application Logo" ImageUrl="~/App.Images/application-logo.gif" />
        </div>
    </asp:Panel>
    <asp:Panel ID="PanelUserDetailsEdit" runat="server" DefaultButton="ButtonCloseUserDetails">
        <div class="divUserDetails">
            <div class="divUserDetailsLeft">
                <table cellspacing="4" cellpadding="8" class="tableUserDetails">
                    <tr>
                        <td class="columnLabelUserDetails">
                            <asp:Label ID="LabelRACFID" runat="server" Text="<%$ Resources:lang, UsersRACFID %>" AssociatedControlID="TextBoxRACFID"></asp:Label>
                        </td>
                        <td class="columnDataUserDetails">
                            <asp:TextBox ID="TextBoxRACFID" runat="server" MaxLength="20" Width="120px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:CustomValidator ID="CustomValidatorRACFID" runat="server" ValidateEmptyText="true" CssClass="ValidatorText"
                                                 ClientValidationFunction="validateRACFID" ValidationGroup="UserInsert" ControlToValidate="TextBoxRACFID" 
                                                 OnServerValidate="RACFID_Validate" Text="<%$ Resources:lang,ErrorMessageRACFID %>"/>
                        </td>
                    </tr>
                    <tr>
                        <td class="columnLabelUserDetails">
                            <asp:Label ID="LabelName" runat="server" Text="<%$ Resources:lang, UsersName %>" AssociatedControlID="TextBoxName"></asp:Label>
                        </td>
                        <td class="columnDataUserDetails">
                            <asp:TextBox ID="TextBoxName" runat="server" MaxLength="50" Width="120px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorName" runat="server" ValidationGroup="UserInsert" Text="<%$ Resources:lang,ErrorMessageUsername %>" ControlToValidate="TextBoxName" CssClass="ValidatorText"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="columnLabelUserDetails">
                            <asp:Label ID="LabelRoles" runat="server" Text="<%$ Resources:lang, UsersRoleNames %>"></asp:Label>
                        </td>
                        <td class="columnDataUserDetails">
                            <uc:MultiSelectDropDownList ID="MultiSelectDropDownListRoles" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:Label ID="LabelMessageRoles" runat="server" Visible="false" CssClass="ValidatorText" Text="<%$ Resources:lang, ErrorMessageRoles %>"></asp:Label>
                        </td>
                    </tr>
                </table>
                <div class="divUserDetailsFooter">
                    <asp:Button ID="ButtonOk" runat="server" Text="<%$ Resources:lang, UsersDetailsSave %>" CausesValidation="true" ValidationGroup="UserInsert" OnClick="ButtonOk_Click" />
                    <asp:Button ID="ButtonCloseUserDetails" runat="server" Text="<%$ Resources:lang, ButtonClose %>" />
                </div>
            </div>
            <div class="divUserDetailsRight" style="float: right">
                <asp:GridView ID="GridviewRoleDetails" runat="server" AutoGenerateColumns="False" GridLines="None">
                    <Columns>
                        <asp:TemplateField></asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="LabelRoleDetailsHeader" runat="server" Text="<%$ Resources:lang,UserRolesTitle %>" Font-Size="12px"></asp:Label><br />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <strong>
                                                <asp:Label ID="LabelRoleName" runat="server" Text='<%#Eval("RoleName","{0} - ") %>'></asp:Label>
                                            </strong>
                                        </td>
                                        <td>
                                            <asp:Label ID="LabelRoleDetails" runat="server" Text='<%#Eval("Description") %>'></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <!-- Alteration by Gavin MarsV3 3/4/12 -->
            <div style="float: right; margin-right: 10px; padding-top: 10px;">
                Maximum Car Search Rows
                <br />
                <asp:TextBox ID="TextBoxCarRows" Width="162" runat="server" />
            </div>
        </div>
    </asp:Panel>
</asp:Panel>
<asp:Button runat="server" ID="ButtonModalPopupUserDetails" Style="display: none" />
<asp:ModalPopupExtender ID="ModalPopupExtenderUserDetails" runat="server" PopupControlID="PanelPopupUserDetails" TargetControlID="ButtonModalPopupUserDetails" BackgroundCssClass="modalBackground" PopupDragHandleControlID="PanelUserDetailsTitle" CancelControlID="ButtonCloseUserDetails">
</asp:ModalPopupExtender>
