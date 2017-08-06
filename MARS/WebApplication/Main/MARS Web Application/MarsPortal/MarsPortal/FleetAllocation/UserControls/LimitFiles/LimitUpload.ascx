<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LimitUpload.ascx.cs" Inherits="Mars.FleetAllocation.UserControls.LimitFiles.LimitUpload" %>

<%@ Register Src="~/FleetAllocation/UserControls/AutoGrid.ascx" TagPrefix="uc" TagName="AutoGrid" %>
<%@ Register Src="~/App.UserControls/HelpIcon.ascx" TagPrefix="uc" TagName="HelpIcon" %>
<%@ Register Src="~/App.UserControls/Phase4/Common/UpdateProgress.ascx" TagPrefix="uc" TagName="UpdateProgress" %>



<table style="width: 100%;">
    <tr>
        <td>
            <table>
                <tr>
                    <td>
                        Country:
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlCountry" OnSelectedIndexChanged="RefreshGrids" 
                            AutoPostBack="True" />
                    </td>
                </tr>
                <tr>
                    <td>Year:
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlYearSelection" OnSelectedIndexChanged="RefreshGrids" AutoPostBack="True" />
                    </td>
                </tr>
                <tr>
                    <td>Month:
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlMonthSelection" OnSelectedIndexChanged="RefreshGrids" AutoPostBack="True" />
                    </td>
                </tr>
                <tr>
                    <td>Car Segment:
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlCarSegment" OnSelectedIndexChanged="RefreshGrids" AutoPostBack="True" />
                    </td>
                </tr>
            </table>
        </td>
        <td>
            <fieldset style="width: 600px; height: 80px;">
                <legend>Monthly Limit Upload</legend>
                <table style="width: 100%">
                    <tr>
                        <td style="width: 300px;" >
                            <asp:FileUpload runat="server" ID="fuFaoMonthlyAdditionFile" Width="300px"
                                ClientIDMode="Static" onchange="this.form.submit()" />
                        </td>
                        <td style="text-align: left;">
                            <uc:HelpIcon runat="server" ID="HelpIcon" HoverImage="~/App.Images/FaoMonthlySample.PNG" />
                        </td>
                        <td style="float: right;">
                            
                            <asp:ConfirmButtonExtender runat="server" ID="cbeUploadWarning"
                                TargetControlID="btnUpload" 
                                ConfirmText=""/>
                            <asp:Button runat="server" ID="btnUpload" CssClass="StandardButton" Text="Upload" 
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
    </tr>
</table>



<div style="height: 30px;">
    &nbsp;
</div>

<asp:HiddenField runat="server" ID="hfSummaryText" Value="Total Additions for " />
<table style="width: 1000px;">
    <tr>
        <td style="text-align: center;">
            <asp:Label runat="server" ID="lblMonthlySummary" />
        </td>
        <td style="text-align: center; width: 100px;">
            <asp:Label runat="server" ID="lblDifference" />
        </td>
        <td>
            <asp:Label runat="server" ID="lblWeeklySummary" />
        </td>
    </tr>
    <tr>
        <td>
            <uc:AutoGrid runat="server" ID="ucMonthlyLimit" AutoGridWidth="400" ShowHeaderWhenEmpty="True" 
                PageSize="30" HideLastColumn="True" />
        </td>
        <td>&nbsp;
        </td>
        <td style="vertical-align: top;">
            <uc:AutoGrid runat="server" ID="ucWeeklyLimit" AutoGridWidth="400" ShowHeaderWhenEmpty="True"  />
        </td>
    </tr>
    <tr>
        <td></td>
    </tr>
</table>

<asp:HiddenField runat="server" ID="hfWeeklyIdToUpdate" />

<asp:Panel runat="server" ID="pnlChangeWeeklyLimit" CssClass="Phase4ModalPopup" Width="500" Height="150">
    <table style="width: 100%;">
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
            <td style="height: 30px;">&nbsp;
            </td>
        </tr>
        <tr>
            <td style="font-size: 13px;">
                <asp:Label runat="server" ID="lblWeeklyInfo" />
                <asp:HiddenField runat="server" ID="hfWeeklyInfo" Value="Year {0} Week {1}: Set Weekly limit to:" />
            </td>
            <td>
                <asp:TextBox runat="server" ID="tbNewWeeklyAdditionLimit" Width="50px" Text="0" />

            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:RangeValidator runat="server" ControlToValidate="tbNewWeeklyAdditionLimit" Type="Integer"
                    MinimumValue="0" MaximumValue="10000" Text="Weekly additions must be between 0 and 10000"
                    ForeColor="Red" />
            </td>
        </tr>
        <tr>
            <td style="height: 30px;">&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnSaveWeeklyAdditionChange" runat="server" CssClass="StandardButton"
                    Text="Save" OnClick="btnSaveWeeklyAdditionChange_Click" />
            </td>
        </tr>
    </table>
</asp:Panel>


<asp:Button ID="btnDummy" runat="server" CssClass="hidden" />
<asp:ModalPopupExtender
    ID="mpeEditWeeklyLimit2"
    runat="server"
    PopupControlID="pnlChangeWeeklyLimit"
    TargetControlID="btnDummy"
    DropShadow="True"
    BackgroundCssClass="modalBackgroundGray" />


<uc:UpdateProgress runat="server" ID="UpdateProgress" />
