<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CarGroupDetails.ascx.cs" Inherits="App.UserControls.Mappings.Details.CarGroupDetails" %>
<asp:Panel runat="server" CssClass="modalPopupMappingDetailsMedium" ID="PanelPopupMappingDetails" Style="display: none;">
    <asp:Panel ID="PanelMappingDetailsTitle" runat="server" CssClass="PanelMappingDetailsTitleSmall">
        <div class="divHertzLogoMappingDetails">
            <asp:Image ID="ImageLogo" runat="server" AlternateText="Hertz Logo" ImageUrl="~/App.Images/hertz-logo.jpg" />
        </div>
        <div class="divApplicationLogoMappingDetails">
            <asp:Image ID="ImageButtonApplicationLogo" runat="server" AlternateText="Application Logo" ImageUrl="~/App.Images/application-logo.gif" />
        </div>
    </asp:Panel>
    <div class="divClearFloats">
    </div>
    <table class="tableMappingDetails">
        <tr>
            <td>
                <asp:Label ID="LabelCarGroupId" runat="server" Visible="false"/>
                <asp:Label ID="LabelCarGroup" runat="server" Text="<%$ Resources:lang, MappingDetailsCarGroup %>"/>
            </td>
            <td>
                <asp:TextBox ID="TextBoxCarGroup" runat="server" MaxLength="3"/>
                <asp:Label runat="server" ID="tbCarGroupError" ForeColor="Red" />
                <asp:AutoCompleteExtender ID="acCarGroup" runat="server" ServiceMethod="SearchCarGroup"
                        TargetControlID="TextBoxCarGroup" ServicePath="~/AutoComplete.asmx" MinimumPrefixLength="1"
                        CompletionInterval="500" EnableCaching="true" CompletionSetCount="8"
                        UseContextKey="True" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorCarGroup" runat="server" ControlToValidate="TextBoxCarGroup" ValidationGroup="CarGroupInsert" CssClass="ValidatorText" Text="<%$ Resources:lang, MappingDetailsErrorMessageCarGroup %>"/>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="LabelCarGroupGold" runat="server" Text="<%$ Resources:lang, MappingDetailsCarGroupGold %>"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TextBoxCarGroupGold" runat="server" MaxLength="3"/>
                <asp:Label runat="server" ID="tbCarGroupGoldError" ForeColor="Red" />
                <asp:AutoCompleteExtender ID="acCarGroupGold" runat="server" ServiceMethod="SearchCarGroup"
                        TargetControlID="TextBoxCarGroupGold" ServicePath="~/AutoComplete.asmx" MinimumPrefixLength="1"
                        CompletionInterval="500" EnableCaching="true" CompletionSetCount="8" 
                        UseContextKey="True" />
            </td>
        </tr>
           <tr>
            <td>
                <asp:Label ID="LabelCarGroupFiveStar" runat="server" Text="Car Group Five Star"/>
            </td>
            <td>
                <asp:TextBox ID="TextBoxCarGroupFiveStar" runat="server" MaxLength="3"/>
                <asp:Label runat="server" ID="tbCarGroupFiveStarError" ForeColor="Red" />
                <asp:AutoCompleteExtender ID="acCarGroupFiveStar" runat="server" ServiceMethod="SearchCarGroup"
                        TargetControlID="TextBoxCarGroupFiveStar" ServicePath="~/AutoComplete.asmx" MinimumPrefixLength="1"
                        CompletionInterval="500" EnableCaching="true" CompletionSetCount="8" 
                        UseContextKey="True" />
            </td>
        </tr>
                <tr>
            <td>
                <asp:Label ID="LabelCarGroupPresidentCircle" runat="server" Text="Car Group President Circle"/>
            </td>
            <td>
                <asp:TextBox ID="TextBoxCarGroupPresidentCircle" runat="server" MaxLength="3"/>
                <asp:Label runat="server" ID="tbCarGroupPresidentCircleError" ForeColor="Red" />
                <asp:AutoCompleteExtender ID="acCarGroupPresidentCircle" runat="server" ServiceMethod="SearchCarGroup"
                        TargetControlID="TextBoxCarGroupPresidentCircle" ServicePath="~/AutoComplete.asmx" MinimumPrefixLength="1"
                        CompletionInterval="500" EnableCaching="true" CompletionSetCount="8" 
                        UseContextKey="True" />
            </td>
        </tr>
                <tr>
            <td>
                <asp:Label ID="LabelCarGroupPlatinum" runat="server" Text="Car Group Platinum"/>
            </td>
            <td>
                <asp:TextBox ID="TextBoxCarGroupPlatinum" runat="server" MaxLength="3"/>
                <asp:Label runat="server" ID="tbCarGroupPlatinumError" ForeColor="Red" />
                <asp:AutoCompleteExtender ID="acCarGroupPlatinum" runat="server" ServiceMethod="SearchCarGroup"
                        TargetControlID="TextBoxCarGroupPlatinum" ServicePath="~/AutoComplete.asmx" MinimumPrefixLength="1"
                        CompletionInterval="500" EnableCaching="true" CompletionSetCount="8" 
                        UseContextKey="True" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorCarGroupGold" runat="server" ControlToValidate="TextBoxCarGroupGold" ValidationGroup="CarGroupInsert" CssClass="ValidatorText" Text="<%$ Resources:lang, MappingDetailsErrorMessageCarGroupGold %>"/>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="LabelCarClass" runat="server" Text="<%$ Resources:lang, MappingDetailsCarClass %>"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="DropDownListCarClass" runat="server" DataTextField="car_class" DataValueField="car_class_id" CssClass="selectLong">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorCarClass" runat="server" ControlToValidate="DropDownListCarClass" ValidationGroup="CarGroupInsert" CssClass="ValidatorText" Text="<%$ Resources:lang, MappingDetailsErrorMessageCarClass %>" InitialValue="-1">
                </asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="LabelCarGroupSortOrder" runat="server" Text="<%$ Resources:lang, MappingDetailsSortOrder %>"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TextBoxSortOrder" runat="server" Height="18px"></asp:TextBox>
                <asp:numericupdownextender id="NumericUpDownExtenderSortOrder" runat="server" targetcontrolid="TextBoxSortOrder" width="60">
                </asp:numericupdownextender>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:CustomValidator ID="CustomValidatorSortOrder" runat="server" ControlToValidate="TextBoxSortOrder" CssClass="ValidatorText" ValidationGroup="CarClassInsert" ClientValidationFunction="validateSortOrder" OnServerValidate="SortOrder_Validate" Text="<%$ Resources:lang, MappingDetailsErrorMessageSortOrder %>" ValidateEmptyText="true"></asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td class="rowMappingDetailsFooter">
                <asp:Button ID="ButtonSave" runat="server" Text="<%$ Resources:lang, MappingDetailsSave %>" OnClick="ButtonSave_Click" CausesValidation="true" ValidationGroup="CarGroupInsert" />
                <asp:Button ID="ButtonCancel" runat="server" Text="<%$ Resources:lang, MappingDetailsCancel %>" CausesValidation="false" />
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Button runat="server" ID="ButtonModalPopupMappingDetails" Style="display: none" />
<asp:modalpopupextender id="ModalPopupExtenderMappingDetails" runat="server" popupcontrolid="PanelPopupMappingDetails" targetcontrolid="ButtonModalPopupMappingDetails" backgroundcssclass="modalBackground" popupdraghandlecontrolid="PanelMappingDetailsTitle" cancelcontrolid="ButtonCancel">
</asp:modalpopupextender>
