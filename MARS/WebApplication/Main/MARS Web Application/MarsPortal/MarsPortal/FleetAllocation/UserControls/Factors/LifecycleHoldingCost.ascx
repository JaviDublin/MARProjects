<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LifecycleHoldingCost.ascx.cs" Inherits="Mars.FleetAllocation.UserControls.Factors.LifecycleHoldingCost" %>


<%@ Register Src="~/FleetAllocation/UserControls/FaoParameter.ascx" TagPrefix="uc" TagName="FaoParameter" %>
<%@ Register Src="~/App.UserControls/HelpIcon.ascx" TagPrefix="uc" TagName="HelpIcon" %>
<%@ Register Src="~/FleetAllocation/UserControls/AutoGrid.ascx" TagPrefix="uc" TagName="AutoGrid" %>
<%@ Register Src="~/FleetAllocation/UserControls/MissingHoldingCosts.ascx" TagPrefix="uc" TagName="MissingHoldingCosts" %>


<asp:HiddenField runat="server" ID="hfRecordCount" Value="0" />
<asp:HiddenField runat="server" ID="hfSelectedPanel" Value="0" />
<asp:HiddenField runat="server" ID="hfFaoUploadHoldingCostFile" Value="" />

<table style="width: 950px; text-align: center; margin-left: auto; margin-right: auto;">
    <tr>
        <td>
            <table>
                <tr>
                    <td>
                        <fieldset style="width: 550px; height: 120px; margin-left: auto; margin-right: auto;">
                            <legend>Lifecycle Holding Cost Upload</legend>
                            <table style="width: 100%">
                                <tr>
                                    <td colspan="2">
                                        <table>
                                            <tr>
                                                <td style="width: 100px;">Country:
                                                </td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlCountry" AutoPostBack="True"
                                                        CssClass="SingleDropDownList" />
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" Text="Uploaded data must be in Local Currency" ForeColor="Red" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 300px;">
                                        <asp:FileUpload runat="server" ID="fuFaoHoldingCostFile" Width="300px"
                                            ClientIDMode="Static" onchange="submittHoldingCostFile()" />
                                    </td>
                                    <td style="text-align: left;">
                                        <uc:HelpIcon runat="server" ID="HelpIcon" HoverImage="~/App.Images/FaoHoldingCostSample.PNG" />
                                    </td>
                                    <td style="float: right;">
                                        <asp:ConfirmButtonExtender runat="server" ID="cbeUploadWarning"
                                            TargetControlID="btnUpload"
                                            ConfirmText="" />
                                        <asp:Button runat="server" ID="btnUpload" CssClass="StandardButton" Visible="False" Text="Upload"
                                            OnClick="btnUpload_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" style="text-align: left;">
                                        <asp:Label runat="server" ID="lblFileUploadSummary1" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" style="text-align: left;">
                                        <asp:Label runat="server" ID="lblFileUploadSummary2" />
                                    </td>
                                </tr>
                            </table>

                        </fieldset>
                    </td>
                    <td>
                        <uc:MissingHoldingCosts runat="server" ID="ucMissingHoldingCosts" />
                    </td>
                </tr>
            </table>

        </td>
    </tr>
    <tr>
        <td>
            <uc:FaoParameter runat="server" ID="ucParameters" ShowMonthSelector="True" HideLocationBranch="True" />
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
        <td>
            <asp:UpdatePanel runat="server" ID="upGrid" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc:AutoGrid runat="server" ID="ucMaxFactors" />
                </ContentTemplate>
            </asp:UpdatePanel>

        </td>
    </tr>
</table>

<script type="text/javascript">
    function submittHoldingCostFile() {

        $("#<%= hfFaoUploadHoldingCostFile.ClientID %>").val("1");
        $("form:first").submit();


    }
</script>
