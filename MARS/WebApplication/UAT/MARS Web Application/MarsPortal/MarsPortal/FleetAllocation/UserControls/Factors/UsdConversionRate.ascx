<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UsdConversionRate.ascx.cs" Inherits="Mars.FleetAllocation.UserControls.Factors.UsdConversionRate" %>
<%@ Register Src="~/FleetAllocation/UserControls/AutoGrid.ascx" TagPrefix="uc" TagName="AutoGrid" %>


<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <uc:AutoGrid runat="server" ID="ucConversionRates" AutoGridWidth="500" />
    
    
    
    <asp:Panel runat="server" ID="pnlChangeCurrencyRate" CssClass="Phase4ModalPopup" Width="450" Height="150">
            
            <table style="width: 100%; text-align: center;">
                <tr>
                    <td colspan="2">
                        <div style="float: right;">
                            <asp:ImageButton runat="server" ID="ibClose2" ImageUrl="~/App.Images/Icons/close.png" />
                        </div>
                        <div style="height: 10px;">
                            &nbsp;
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="font-size: larger; font-weight: bold;" >
                        <asp:Label ID="lblYear" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                       One Euro:
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="tbEuroRate" Text="1.000" />
                        <asp:MaskedEditExtender runat="server"
                            ID="meeEuroRate"
                             TargetControlID="tbEuroRate" 
                            ClearMaskOnLostFocus="True"
                            CultureName="en-US"
                            DisplayMoney="Left"
                            
                            MaskType="Number"
                            Mask="9.999"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:MaskedEditValidator runat="server" 
                            ControlToValidate="tbEuroRate"
                             ControlExtender="meeEuroRate"
                            Enabled="True"
                             Display="Dynamic"
                             IsValidEmpty="False"
                             EmptyValueMessage="A Rate is required"
                             InvalidValueMessage="Amount is invalid"
                             MinimumValue="0.2"
                             MaximumValue="5"
                             MinimumValueMessage="Rate is too small"
                             MaximumValueMessage="Rate is too large"
                            />
                    </td>
                </tr>
                <tr>
                    <td>
                       One Pound Sterling:
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="tbGbpRate" Text="1.000" />
                        <asp:MaskedEditExtender runat="server" TargetControlID="tbGbpRate" 
                            ID="meeGbpRate"
                            ClearMaskOnLostFocus="True"
                            CultureName="en-US"
                            DisplayMoney="Left"
                            MaskType="Number"
                            Mask="9.999"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:MaskedEditValidator runat="server" 
                            ControlToValidate="tbGbpRate"
                             ControlExtender="meeGbpRate"
                            Enabled="True"
                             Display="Dynamic"
                             IsValidEmpty="False"
                             EmptyValueMessage="A Rate is required"
                             InvalidValueMessage="Amount is invalid"
                             MinimumValue="0.2"
                             MaximumValue="5"
                             MinimumValueMessage="Rate is too small"
                             MaximumValueMessage="Rate is too large"
                            />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button runat="server" ID="btnSaveRate"  Text="Save" OnClick="btnSaveRate_Click"
                            CssClass="StandardButton"/>
                    </td>
                </tr>
            </table>
    </asp:Panel>
    
    
    <asp:Button ID="btnDummy" runat="server" CssClass="hidden" />
    <asp:ModalPopupExtender
        ID="mpeCurrencyRateLimit"
        runat="server"
        PopupControlID="pnlChangeCurrencyRate"
        TargetControlID="btnDummy"
        DropShadow="True"
        BackgroundCssClass="modalBackgroundGray" />
        

    </ContentTemplate>
</asp:UpdatePanel>
