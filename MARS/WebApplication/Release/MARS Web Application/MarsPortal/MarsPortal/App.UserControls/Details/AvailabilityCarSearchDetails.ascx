<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AvailabilityCarSearchDetails.ascx.cs" Inherits="App.UserControls.Details.AvailabilityCarSearchDetails" %>
<%-- CarSearch Details Popup Panel --%>
<asp:Panel runat="server" CssClass="modalPopupCarSearch" ID="PanelPopupCarSearch" DefaultButton="ButtonOk" Style="display: none;">
    <asp:Panel ID="PanelCarSearchTitlePrint" runat="server" CssClass="PanelCarSearchTitlePrint" Style="display: none;">
        <asp:Image ID="ImageTitlePrint" runat="server" AlternateText="Hertz Title" ImageUrl="~/App.images/mars_title_print.jpg" />
    </asp:Panel>
    <asp:Panel ID="PanelCarSearchTitle" runat="server" CssClass="PanelCarSearchTitle">
        <div class="divHertzLogoCarSearch">
            <asp:Image ID="ImageLogo" runat="server" AlternateText="Hertz Logo" ImageUrl="~/App.Images/hertz-logo.jpg" />
        </div>
        <div class="divCarSearchTitle">
            <asp:Label ID="LabelSerialTitle" runat="server"></asp:Label>
        </div>
        <div class="divApplicationLogoCarSearch">
            <asp:Image ID="ImageButtonApplicationLogo" runat="server" AlternateText="Application Logo" ImageUrl="~/App.Images/application-logo.gif" />
        </div>
    </asp:Panel>
    <div class="divCarSearchDetails">
        <div class="divVehicleDetails">
            <div class="divVehicleDetailsRows">
                <table class="tableVehicleDetails" cellspacing="4" cellpadding="8">
                    <tr>
                        <td class="columnLabelVehicleDetailsShort">
                            <asp:Label ID="LabelGroup" runat="server" Text="<%$ Resources:lang, Group %> "></asp:Label>
                        </td>
                        <td class="columnLabelVehicleDetailsShort">
                            <asp:Label ID="LabelModelcode" runat="server" Text="<%$ Resources:lang, Modelcode %> "></asp:Label>
                        </td>
                        <td class="columnLabelVehicleDetailsModel">
                            <asp:Label ID="LabelModel" runat="server" Text="<%$ Resources:lang, Model %> " Width="120px"></asp:Label>
                        </td>
                        <td class="columnLabelVehicleDetailsShort">
                            <asp:Label ID="LabelUnit" runat="server" Text="<%$ Resources:lang, Unit %> " Width="110px"></asp:Label>
                        </td>
                        <td class="columnLabelVehicleDetailsLong">
                            <asp:Label ID="LabelLicense" runat="server" Text="<%$ Resources:lang, License %> "></asp:Label>
                        </td>
                        <td class="columnLabelVehicleDetailsShort">
                            <asp:Label ID="LabelVehicleIdentNbr" runat="server" Text="<%$ Resources:lang, VehicleIdentNbr %> " Width="160px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="columnDataVehicleDetailsShort">
                            <asp:Label ID="LabelDisplayGroup" runat="server"></asp:Label>
                        </td>
                        <td class="columnDataVehicleDetailsShort">
                            <asp:Label ID="LabelDisplayModelcode" runat="server"></asp:Label>
                        </td>
                        <td class="columnDataVehicleDetailsModel">
                            <asp:Label ID="LabelDisplayModel" runat="server"></asp:Label>
                        </td>
                        <td class="columnDataVehicleDetailsShort">
                            <asp:Label ID="LabelDisplayUnit" runat="server"></asp:Label>
                        </td>
                        <td class="columnDataVehicleDetailsLong">
                            <asp:Label ID="LabelDisplayLicense" runat="server"></asp:Label>
                        </td>
                        <td class="columnDataVehicleDetailsShort">
                            <asp:Label ID="LabelDisplayVIdNo" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <hr class="hrVehicleDetails" />
        </div>
        <div class="divVehicleDetails">
            <div class="divVehicleDetailsRows">
                <table class="tableVehicleDetails" cellspacing="4" cellpadding="8">
                    <tr>
                        <td class="columnLabelVehicleDetailsShort">
                            <asp:Label ID="LabelCharged" runat="server" Text="<%$ Resources:lang, charged %> "></asp:Label>
                        </td>
                        <td class="columnLabelVehicleDetailsLong">
                            <asp:Label ID="LabelLastDocument" runat="server" Text="<%$ Resources:lang, LastDocument %> "></asp:Label>
                        </td>
                        <td class="columnLabelVehicleDetailsShort">
                            <asp:Label ID="LabelLstWWD" runat="server" Text="<%$ Resources:lang, LstWWD %> "></asp:Label>
                        </td>
                        <td class="columnLabelVehicleDetailsShort">
                            <asp:Label ID="LabelLSTDate" runat="server" Text="<%$ Resources:lang, LSTDate %> "></asp:Label>
                        </td>
                        <td class="columnLabelVehicleDetailsShort">
                            <asp:Label ID="LabelLSTTime" runat="server" Text="<%$ Resources:lang, LSTTime %> "></asp:Label>
                        </td>
                        <td class="columnLabelVehicleDetailsShort">
                            <asp:Label ID="LabelKilometers" runat="server" Text="<%$ Resources:lang, Kilometers %> "></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="columnDataVehicleDetailsShort">
                            <asp:Label ID="LabelDisplayCharged" runat="server"></asp:Label>
                        </td>
                        <td class="columnDataVehicleDetailsLong">
                            <asp:Label ID="LabelDisplayLastDocument" runat="server"></asp:Label>
                        </td>
                        <td class="columnDataVehicleDetailsShort">
                            <asp:Label ID="LabelDisplayLstwwd" runat="server"></asp:Label>
                        </td>
                        <td class="columnDataVehicleDetailsShort">
                            <asp:Label ID="LabelDisplayLstdate" runat="server"></asp:Label>
                        </td>
                        <td class="columnDataVehicleDetailsShort">
                            <asp:Label ID="LabelDisplayLsttime" runat="server"></asp:Label>
                        </td>
                        <td class="columnDataVehicleDetailsShort">
                            <asp:Label ID="LabelDisplayKilometers" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <hr class="hrVehicleDetails" />
        </div>
        <div class="divVehicleDetails">
            <div class="divVehicleDetailsRows">
                <table class="tableVehicleDetails" cellspacing="4" cellpadding="8">
                    <tr>
                        <td class="columnLabelVehicleDetailsShort">
                            <asp:Label ID="LabelOperstat" runat="server" Text="<%$ Resources:lang, Operstat %> "></asp:Label>
                        </td>
                        <td class="columnLabelVehicleDetailsShort">
                            <asp:Label ID="LabelMovetype" runat="server" Text="<%$ Resources:lang, Movetype %> "></asp:Label>
                        </td>
                        <td class="columnLabelVehicleDetailsShort">
                            <asp:Label ID="LabelDueWWD" runat="server" Text="<%$ Resources:lang, DueWWD %> "></asp:Label>
                        </td>
                        <td class="columnLabelVehicleDetailsShort">
                            <asp:Label ID="LabelDueDate" runat="server" Text="<%$ Resources:lang, DueDate %> "></asp:Label>
                        </td>
                        <td class="columnLabelVehicleDetailsShort">
                            <asp:Label ID="LabelDueTime" runat="server" Text="<%$ Resources:lang, DueTime %> "></asp:Label>
                        </td>
                        <td class="columnLabelVehicleDetailsDriverName">
                            <asp:Label ID="LabelDriverName" runat="server" Text="<%$ Resources:lang, DriverName %> "></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="columnDataVehicleDetailsShort">
                            <asp:Label ID="LabelDisplayOperstat" runat="server"></asp:Label>
                        </td>
                        <td class="columnDataVehicleDetailsShort">
                            <asp:Label ID="LabelDisplayMovetype" runat="server"></asp:Label>
                        </td>
                        <td class="columnDataVehicleDetailsShort">
                            <asp:Label ID="LabelDisplayDueWWD" runat="server"></asp:Label>
                        </td>
                        <td class="columnDataVehicleDetailsShort">
                            <asp:Label ID="LabelDisplayDuedate" runat="server"></asp:Label>
                        </td>
                        <td class="columnDataVehicleDetailsShort">
                            <asp:Label ID="LabelDisplayDuetime" runat="server"></asp:Label>
                        </td>
                        <td class="columnDataVehicleDetailsDriverName">
                            <asp:Label ID="LabelDisplayDriverName" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <hr class="hrVehicleDetails" />
        </div>
        <div class="divVehicleDetails">
            <div class="divVehicleDetailsRows">
                <table class="tableVehicleDetails" cellspacing="4" cellpadding="8">
                    <tr>
                        <td class="columnLabelVehicleDetailsShort">
                            <asp:Label ID="LabelNonRev" runat="server" Text="<%$ Resources:lang, NonRev %> "></asp:Label>
                        </td>
                        <td class="columnLabelVehicleDetailsShort">
                            <asp:Label ID="LabelRegDate" runat="server" Text="<%$ Resources:lang, RegDate %> "></asp:Label>
                        </td>
                        <td class="columnLabelVehicleDetailsShort">
                            <asp:Label ID="LabelBlockdate" runat="server" Text="<%$ Resources:lang, Blockdate %> "></asp:Label>
                        </td>
                        <td class="columnLabelVehicleDetailsNextOnRent">
                            <asp:Label ID="LabelNextOnRentDate" runat="server" Text="<%$ Resources:lang, expectedResolutionDate %> ">
                            </asp:Label>
                        </td>
                        <td class="columnLabelVehicleDetailsShort">
                            <asp:Label ID="LabelOwnarea" runat="server" Text="<%$ Resources:lang, Ownarea %> "></asp:Label>
                        </td>
                        <td class="columnLabelVehicleDetailsShort">
                            <asp:Label ID="LabelCarhold" runat="server" Text="<%$ Resources:lang, Carhold %> "></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="columnDataVehicleDetailsShort">
                            <asp:Label ID="LabelDisplayNonrev" runat="server">
                            </asp:Label>
                        </td>
                        <td class="columnDataVehicleDetailsShort">
                            <asp:Label ID="LabelDisplayRegDate" runat="server">
                            </asp:Label>
                        </td>
                        <td class="columnDataVehicleDetailsShort">
                            <asp:Label ID="LabelDisplayBlockdate" runat="server"></asp:Label>
                        </td>
                        <td class="columnDataVehicleDetailsNextOnRent">
                            <uc:DatePickerTextBox ID="DatePickerNextOnRent" runat="server" SetDefaultDate="false" ValidationGroup="SaveRemarks" />
                        </td>
                        <td class="columnDataVehicleDetailsShort">
                            <asp:Label ID="LabelDisplayOwnarea" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="LabelDisplayCarhold" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <hr class="hrVehicleDetails" />
        </div>
        <div class="divVehicleDetails">
            <div class="divVehicleDetailsRows">
                <table class="tableVehicleDetails" cellspacing="4" cellpadding="8">
                    <tr>
                        <td class="columnLabelVehicleDetailsShort">
                            <asp:Label ID="LabelBDDays" runat="server" Text="<%$ Resources:lang, BDDays %> "></asp:Label>
                        </td>
                        <td class="columnLabelVehicleDetailsShort">
                            <asp:Label ID="LabelMMDays" runat="server" Text="<%$ Resources:lang, MMDays %> "></asp:Label>
                        </td>
                        <td class="columnLabelVehicleDetailsShort">
                            <asp:Label ID="LabelPrevWWD" runat="server" Text="<%$ Resources:lang, PrevWWD %> "></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="columnDataVehicleDetailsShort">
                            <asp:Label ID="LabelDisplayBDDays" runat="server"></asp:Label>
                        </td>
                        <td class="columnDataVehicleDetailsShort">
                            <asp:Label ID="LabelDisplayMMDays" runat="server"></asp:Label>
                        </td>
                        <td class="columnDataVehicleDetailsShort">
                            <asp:Label ID="LabelDisplayPrevWWD" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <hr class="hrVehicleDetails" />
        </div>
        <div class="divVehicleDetails">
            <div class="divVehicleDetailsRows">
                <table class="tableVehicleDetails" cellspacing="4" cellpadding="8">
                    <tr>
                        <td class="columnLabelVehicleDetailsShort">
                            <asp:Label ID="LabelRemarks" runat="server" Text="<%$ Resources:lang, Remarks %> "></asp:Label>
                        </td>
                        <td>
                            <htz:TextArea ID="TextAreaRemarks" runat="server" MaxLength="500" TextMode="MultiLine" Width="730px" Height="60px" ClientIDMode="Static"></htz:TextArea>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorRemarks" runat="server" Font-Size="11px" ControlToValidate="TextAreaRemarks" Text="Please enter remarks (max characters 500)." ValidationGroup="SaveRemarks"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="divCarSearchDetailsFooter">
            <asp:Button ID="ButtonOk" runat="server" Text="<%$ Resources:lang, CarSearchSaveRemarks %>" OnClientClick="validateTextArea();" CausesValidation="true" ValidationGroup="SaveRemarks" OnClick="ButtonOk_Click" />
            <asp:Button ID="ButtonCloseCarSearchDetails" runat="server" OnClientClick="validateTextArea();" Text="<%$ Resources:lang, ButtonClose %>" />
        </div>
    </div>
</asp:Panel>
<asp:Button runat="server" ID="ButtonModalPopupCarSearch" Style="display: none" />
<asp:ModalPopupExtender ID="ModalPopupExtenderCarSearch" runat="server" PopupControlID="PanelPopupCarSearch" TargetControlID="ButtonModalPopupCarSearch" BackgroundCssClass="modalBackground" PopupDragHandleControlID="PanelCarSearchTitle" CancelControlID="ButtonCloseCarSearchDetails">
</asp:ModalPopupExtender>
<script type="text/javascript">
    function validateTextArea() { // check for &#
        var v = document.getElementById('TextAreaRemarks').value = document.getElementById('TextAreaRemarks').value.replace('&#', ' ');        
        return true;
    }
</script>
