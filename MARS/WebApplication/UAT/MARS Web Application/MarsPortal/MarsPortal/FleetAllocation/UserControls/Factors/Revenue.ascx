<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Revenue.ascx.cs" 
    Inherits="Mars.FleetAllocation.UserControls.Factors.Revenue" %>


<%@ Register Src="~/FleetAllocation/UserControls/FaoParameter.ascx" TagPrefix="uc" TagName="FaoParameter" %>
<%@ Register Src="~/FleetAllocation/UserControls/AutoGrid.ascx" TagPrefix="uc" TagName="AutoGrid" %>
<%@ Register Src="~/App.UserControls/HelpIcon.ascx" TagPrefix="uc" TagName="HelpIcon" %>
<%@ Register Src="~/FleetAllocation/UserControls/MissingRevenueEntries.ascx" TagPrefix="uc" TagName="MissingRevenueEntries" %>


<asp:HiddenField runat="server" ID="hfSelectedPanel" Value="0" />
<asp:HiddenField runat="server" ID="hfFaoUploadRevFile" Value="" />

<table style="width: 950px; text-align: center; margin-left: auto; margin-right: auto;">
    <tr>
        <td>
            <table>
                <tr>
                    <td>
                        <fieldset style="width: 550px; height: 120px; margin-left: auto; margin-right: auto;">
                <legend>Revenue by Commerical Car Segment Upload</legend>
                <table style="width: 100%">
                    <tr>
                        <td colspan="2">
                            <table>
                                <tr>
                                    <td style="width: 100px;">
                                        Country:
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
                        <td style="width: 300px;" >
                            <asp:FileUpload runat="server" ID="fuFaoRevenueFile" Width="300px"
                                ClientIDMode="Static" onchange="submitRevFile()" />
                        </td>
                        <td style="text-align: left;">
                            <uc:HelpIcon runat="server" ID="HelpIcon" HoverImage="~/App.Images/FaoRevenueSample.PNG" />
                        </td>
                        <td style="float: right;">
                            <asp:ConfirmButtonExtender runat="server" ID="cbeUploadWarning"
                                TargetControlID="btnUpload" 
                                ConfirmText=""/>
                            <asp:Button runat="server" ID="btnUpload" CssClass="StandardButton" Text="Upload" Visible="False" 
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
                        <uc:MissingRevenueEntries runat="server" ID="MissingRevenueEntries" />
                    </td>
                </tr>
            </table>
            
        </td>
    </tr>
    <tr>
        <td>
            <uc:FaoParameter runat="server" ID="ucParameters" ShowMonthSelector="True" ShowCommercialCarSegment="True" />
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
                    <asp:HiddenField runat="server" ID="hfRecordCount" Value="0" />
                    <uc:AutoGrid runat="server" ID="ucMaxFactors" />
                </ContentTemplate>
            </asp:UpdatePanel>

        </td>
    </tr>
</table>

<script type="text/javascript">
    function submitRevFile() {

        $("#<%= hfFaoUploadRevFile.ClientID %>").val("1");
        $("form:first").submit();


    }
</script>