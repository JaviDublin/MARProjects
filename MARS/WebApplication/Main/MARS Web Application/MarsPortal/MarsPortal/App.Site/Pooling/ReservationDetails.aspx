<%@ Page Title="Reservation Details" Theme="MarsV3" MasterPageFile="~/App.Masterpages/Application.master" Language="C#" AutoEventWireup="true" CodeBehind="ReservationDetails.aspx.cs" Inherits="App.Site.Pooling.ReservationDetails" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Src="../../App.UserControls/Pooling/ReservationDetailsModal.ascx" TagName="ReservationDetailsModal" TagPrefix="uc1" %>
<%@ Register Src="../../App.UserControls/Pooling/TopFeedback.ascx" TagName="TopFeedback" TagPrefix="uc2" %>
<%@ Register Src="../../App.UserControls/Pooling/BottomDropDown.ascx" TagName="BottomDropDown" TagPrefix="uc3" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceholderHead" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceholderMainContent" runat="server">
    <center>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div style="float: left; vertical-align: top; margin-left: 25px;">
                    <asp:CheckBox runat="server" ID="cbRemoveLongterm" Text="Exclude Longterm for off-airport"
                            OnCheckedChanged="cbRemoveLongterm_SelectionChanged" AutoPostBack="True" ToolTip="Longterm = rental length > 27 days"  />
                </div>
                <div style="float: right; margin-right: 25px;">
                    <asp:Button ID="btnExport" runat="server" Text="Export to Excel" OnClick="btnExport_Clicked" CssClass="PlainDataExportButton" Width="120px" />
                </div>
                <uc2:TopFeedback ID="TopFeedback1" runat="server" />
                <asp:GridView ID="GridViewDetails" runat="server"
                    SelectedIndex="-1" EmptyDataText="No data available" AllowPaging="True"
                    AllowSorting="True"
                    OnPageIndexChanging="GridViewDetails_PageIndexChanging"
                    OnSelectedIndexChanging="GridViewDetails_SelectedIndexChanging"
                    OnSorting="GridViewDetails_Sorting"
                    meta:resourcekey="GridViewDetailsResource1">
                    <PagerSettings FirstPageImageUrl="~/App.Images/pager-first.png" LastPageImageUrl="~/App.Images/pager-last.png"
                        Mode="NextPreviousFirstLast" NextPageImageUrl="~/App.Images/pager-next.png" PreviousPageImageUrl="~/App.Images/pager-previous.png" />
                    <PagerStyle HorizontalAlign="Right" />
                    <Columns>
                        <asp:CommandField ShowSelectButton="true" HeaderText="Select<br />Reservation" ItemStyle-HorizontalAlign="Center" />
                    </Columns>
                    <SelectedRowStyle Font-Bold="true" ForeColor="Blue" />
                </asp:GridView>
                <div style="width: 90%; margin-left: 0; text-align: left;">
                    <asp:Label runat="server" ID="lblRowCount" Text="20" />
                </div>

                <br />
                <i>You are viewing page&nbsp;
                    <%=GridViewDetails.PageIndex + 1%>
                    &nbsp;of&nbsp;
                    <%=GridViewDetails.PageCount%>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Maximum number of rows:&nbsp;
                    <asp:DropDownList ID="DropDownListPagerMaxRows" runat="server" AutoPostBack="True"
                        OnSelectedIndexChanged="UpdateController"
                        meta:resourcekey="DropDownListPagerMaxRowsResource1">
                    </asp:DropDownList>
                </i>

                <uc3:BottomDropDown ID="BottomDropDown1" runat="server"
                    OnDropDownListCountryEvent="DropDownListCountry_SelectedIndexChanged"
                    OnCMSRadioButtonEvent="CMSRadioButtonLogic_Changed"
                    OnOPSRadioButtonEvent="OPSRadioButtonLogic_Changed"
                    OnDropDownListPoolEvent="DropDownListPool_SelectedIndexChanged"
                    OnDropDownListLocationGroupEvent="DropDownListLocationGroup_SelectedIndexChanged"
                    OnDropDownListBranchEvent="DropDownListBranch_SelectedIndexChanged"
                    OnDropDownListCarSegmentEvent="DropDownListCarSegment_SelectedIndexChanged"
                    OnDropDownListCarClassEvent="DropDownListCarClass_SelectedIndexChanged"
                    OnDropDownListCarGroupEvent="DropDownListCarGroup_SelectedIndexChanged" />

                <table width="96%" cellpadding="10">
                    <tr>
                        <td style="width: 16%" class="pooling-bottomFeedback">Check In/Out:</td>
                        <td style="width: 16%" class="pooling-bottomFeedback">
                            <asp:DropDownList ID="DropDownListCheckInOut" runat="server"  width="150px"
                                OnSelectedIndexChanged="UpdateController"
                                AutoPostBack="True" meta:resourcekey="DropDownListCheckInOutResource1">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 16%" class="pooling-bottomFeedback">DateRange Start:</td>
                        <td style="width: 16%" class="pooling-bottomFeedback">
                            <asp:TextBox ID="textboxStartDate" runat="server" AutoPostBack="True"
                                OnTextChanged="textboxStartDate_TextChanged"
                                meta:resourcekey="textboxStartDateResource1" Height="16px"></asp:TextBox>
                            <asp:ImageButton ID="ImageButtonStartCalendar" runat="server" ImageUrl="~/App.Images/calendar.png"
                                AlternateText="Calendar" ToolTip="Click to show calendar"
                                meta:resourcekey="ImageButtonStartCalendarResource1" />
                            <asp:CalendarExtender ID="TextBoxStartDate_CalendarExtender" PopupButtonID="ImageButtonStartCalendar"
                                PopupPosition="Right" runat="server" Enabled="True" TargetControlID="textboxStartDate" Format="dd/MM/yyyy">
                            </asp:CalendarExtender>
                            <asp:Label ID="LabelStartDateError" runat="server" ForeColor="Red"
                                meta:resourcekey="LabelStartDateErrorResource1"></asp:Label>
                        </td>
                        <td style="width: 16%" class="pooling-bottomFeedback">DateRange End:</td>
                        <td style="width: 16%" class="pooling-bottomFeedback">
                            <asp:TextBox ID="TextBoxEndDate" runat="server" AutoPostBack="True"
                                OnTextChanged="TextBoxEndDate_TextChanged"
                                meta:resourcekey="TextBoxEndDateResource1" Height="16px"></asp:TextBox>
                            <asp:ImageButton ID="ImageButtonEndDate" runat="server" ImageUrl="~/App.Images/calendar.png"
                                AlternateText="Calendar" ToolTip="Click to show calendar"
                                meta:resourcekey="ImageButtonEndDateResource1" />
                            <asp:CalendarExtender ID="TextBoxEndDate_CalendarExtender" PopupButtonID="ImageButtonEndDate"
                                runat="server" Enabled="True" TargetControlID="textboxEndDate" Format="dd/MM/yyyy">
                            </asp:CalendarExtender>
                            <asp:Label ID="LabelEndDateError" runat="server" ForeColor="Red"
                                meta:resourcekey="LabelEndDateErrorResource1"></asp:Label>
                            </span>
                        </td>
                    </tr>
                    <tr>
                        <td class="pooling-bottomFeedback">Filter:</td>
                        <td class="pooling-bottomFeedback">
                            <asp:DropDownList ID="DropDownListFilter" runat="server" width="150px"
                                OnSelectedIndexChanged="UpdateController"
                                AutoPostBack="True" meta:resourcekey="DropDownListFilterResource1">
                            </asp:DropDownList>
                        </td>
                        <td class="pooling-bottomFeedback">Res-ID:</td>
                        <td class="pooling-bottomFeedback">
                            <asp:TextBox ID="textboxResId" runat="server"
                                meta:resourcekey="textboxResIdResource1" Height="16px" />
                            <%--   <asp:TextBoxWatermarkExtender ID="textboxResId_X" runat="server" 
                            TargetControlID="textboxResId" WatermarkText="[ResId]" 
                                WatermarkCssClass="watermark" Enabled="True"></asp:TextBoxWatermarkExtender>--%>
                        </td>
                        <td class="pooling-bottomFeedback">Customer Name:</td>
                        <td class="pooling-bottomFeedback">
                            <asp:TextBox ID="textboxCustName" runat="server"
                                meta:resourcekey="textboxCustNameResource1" Height="16px" />
                            <%--    <asp:TextBoxWatermarkExtender ID="textboxCustName_X" runat="server" 
                            TargetControlID="textboxCustName" WatermarkText="[Customer Name]" 
                                WatermarkCssClass="watermark" Enabled="True"></asp:TextBoxWatermarkExtender>--%>
                        </td>
                    </tr>
                    <tr>
                        <td class="pooling-bottomFeedback">CDP:</td>
                        <td class="pooling-bottomFeedback">
                            <asp:TextBox ID="textboxCdp" runat="server"  width="147px"
                                meta:resourcekey="textboxCdpResource1" Height="16px"></asp:TextBox>
                            <%--    <asp:TextBoxWatermarkExtender ID="textboxCdp_X" runat="server" 
                            TargetControlID="textboxCdp" WatermarkText="[CDP]" 
                                WatermarkCssClass="watermark" Enabled="True"></asp:TextBoxWatermarkExtender>--%>
                        </td>
                        <td class="pooling-bottomFeedback">#1 Gold:</td>
                        <td class="pooling-bottomFeedback">
                            <asp:TextBox ID="textbox1Gold" runat="server"
                                meta:resourcekey="textbox1GoldResource1" Height="16px"></asp:TextBox>
                            <%--   <asp:TextBoxWatermarkExtender ID="textboxGold1_X" runat="server" 
                            TargetControlID="textbox1Gold" WatermarkText="[#1 Gold]" 
                                WatermarkCssClass="watermark" Enabled="True"></asp:TextBoxWatermarkExtender>--%>
                        </td>
                        <td class="pooling-bottomFeedback">Flight Nbr:</td>
                        <td class="pooling-bottomFeedback">
                            <asp:TextBox ID="texboxFlightNbr" runat="server"
                                meta:resourcekey="texboxFlightNbrResource1" Height="16px"></asp:TextBox>
                            <%-- <asp:TextBoxWatermarkExtender ID="textboxFightNbr_X" runat="server"
                            TargetControlID="texboxFlightNbr" WatermarkText="[Flight Nbr]" 
                                WatermarkCssClass="watermark" Enabled="True"></asp:TextBoxWatermarkExtender>
                        </td>--%>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:Button runat="server" Text="Refresh" ID="btnRefresh" OnClick="btnRefresh_Click"/>
                        </td>
                    </tr>
                </table>
                <uc1:ReservationDetailsModal ID="ReservationDetailsModal1" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" ClientIDMode="Static" DisplayAfter="2000">
            <ProgressTemplate>
                <div id="divBackgroundCover" class='backgroundCover' runat="server" clientidmode="Static"></div>
                <div id="divLoadData" class='loadData' runat="server" clientidmode="Static">
                    <br />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    Loading Data.....
                    <br />
                    <img src="../../App.Images/ajax-loader.gif" alt="Please Wait" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </center>
    <script type="text/javascript">
        $("input[type=text]").keypress(function (e) {
            if (e.which === 13) {
                e.preventDefault();
                $("#UpdateProgress1").show();
                __doPostBack("<%= TheTextBoxLabel %>");
            }
        });
    </script>
</asp:Content>
