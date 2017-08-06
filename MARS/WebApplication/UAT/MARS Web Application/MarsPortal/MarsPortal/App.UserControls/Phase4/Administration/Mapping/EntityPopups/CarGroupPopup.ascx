<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CarGroupPopup.ascx.cs" Inherits="Mars.App.UserControls.Phase4.Administration.Mapping.EntityPopups.CarGroupPopup" %>



<asp:Panel ID="pnlCarGroup" runat="server" Width="450px" Height="250px" CssClass="Phase4ModalPopup">

    <asp:HiddenField runat="server" ID="hfCarGroupId" />
    <table style="width: 100%;">
        <tr>

            <td style="height: 30px; text-align: center;" colspan="2">
                <h1>Car Group</h1>

            </td>
            <td>
                <div style="float: right;">
                    <asp:ImageButton runat="server" ID="ibClose" ImageUrl="~/App.Images/Icons/close.png" />
                </div>
            </td>

        </tr>
        <tr>
            <td style="height: 20px;">&nbsp;
            </td>
        </tr>
        <tr style="vertical-align: top;">
            <td>
                <table>
                    <tr>
                        <td>
                            <label style="font-weight: bold;">Country:</label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblCountryName" Width="120px" Height="16px" BorderWidth="1" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label style="font-weight: bold;">Car Group:</label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblCarGroup" Width="120px" Height="16px" BorderWidth="1"  />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label style="font-weight: bold;">Car Segment:</label>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlCarSegment" CssClass="StandardAdminDropdown" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlCarSegment_SelectionChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label style="font-weight: bold;">Car Class:</label>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlCarClass" CssClass="StandardAdminDropdown" />
                        </td>
                    </tr>

                    <tr>
                        <td>
                            <label style="font-weight: bold;">Is Active:</label>
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="cbActive" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label runat="server" ID="lblMessage" ForeColor="Red" />
                        </td>
                    </tr>


                </table>
            </td>
            <td>
                <fieldset>
                    <legend>Upgrade:</legend>

                    <table>
                        <tr>
                            <td>
                                <label style="font-weight: bold;">Gold:</label>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlGoldUpgrade"  Width="50px"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label style="font-weight: bold;">Five Star:</label>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlFiveStarUpgrade"  Width="50px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label style="font-weight: bold;">President Circle:</label>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlPresidentCircle"  Width="50px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label style="font-weight: bold;">Platinum:</label>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlPlatinum" Width="50px" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
        </tr>
        <tr>
            <td style="height: 20px;">&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="2" style="text-align: center;">
                <asp:Button runat="server" ID="bnSavePopup" Text="Save" OnClick="bnSavePopup_Click"
                    ValidationGroup="CheckControls" CssClass="StandardButton" />
            </td>

        </tr>
    </table>

</asp:Panel>


<asp:Button ID="btnDummy" runat="server" CssClass="hidden" />
<asp:ModalPopupExtender
    ID="mpCarGroup"
    runat="server"
    PopupControlID="pnlCarGroup"
    TargetControlID="btnDummy"
    DropShadow="True"
    BackgroundCssClass="modalBackgroundGray" />
