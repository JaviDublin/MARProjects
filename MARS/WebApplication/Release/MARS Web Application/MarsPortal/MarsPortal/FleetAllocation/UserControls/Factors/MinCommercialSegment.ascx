<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MinCommercialSegment.ascx.cs" Inherits="Mars.FleetAllocation.UserControls.Factors.MinCommercialSegment" %>


<%@ Register Src="~/FleetAllocation/UserControls/FaoParameter.ascx" TagPrefix="uc" TagName="FaoParameter" %>
<%@ Register Src="~/FleetAllocation/UserControls/AutoGrid.ascx" TagPrefix="uc" TagName="AutoGrid" %>


<table style="width: 950px; text-align: center; margin-left: auto; margin-right: auto;">
    <tr>
        <td>
            <uc:FaoParameter runat="server" ID="ucParameters" HideCarClass="True" HideCarGroup="True"
                ShowCommercialCarSegment="True" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:Button runat="server" ID="btnLoad" Text="Load" CssClass="StandardButton" OnClick="btnLoad_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
    <tr>
        <td style="width: 100%;">
            <fieldset style="width: 300px; margin-left: 0; margin-right: auto;">
                <legend>Update Minimum Commercial Segment</legend>
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <table style="width: 100%;">
                            <tr>
                                <td style="width: 40px;">Min Commercial Segment:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="tbNewPercent" CssClass="AutoCompleteTextBox" />
                                </td>
                                <td style="margin-right: 0;">
                                    <asp:Button runat="server" ID="btnSubmitChange" Text="Update"
                                        CssClass="StandardButton" OnClick="btnSubmitChange_Click" />
                                    <asp:ConfirmButtonExtender runat="server" ID="cbeChangeConfirm"
                                        ConfirmText="test"
                                        TargetControlID="btnSubmitChange" />
                                    <asp:HiddenField runat="server" ID="hfChangeConfirmMessage"
                                        Value="Clicking OK will update {0} records" />
                                    <asp:HiddenField runat="server" ID="hfRefreshDataMessage"
                                        Value="Click load to refresh data before updating" />
                                    <asp:HiddenField runat="server" ID="hfRecordCount" Value="0" />

                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </fieldset>
        </td>
    </tr>
    <tr>
        <td>
            <asp:UpdatePanel runat="server" ID="upGrid" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc:AutoGrid runat="server" ID="ucMinCommSegGrid" />
                </ContentTemplate>
            </asp:UpdatePanel>

        </td>
    </tr>
</table>
