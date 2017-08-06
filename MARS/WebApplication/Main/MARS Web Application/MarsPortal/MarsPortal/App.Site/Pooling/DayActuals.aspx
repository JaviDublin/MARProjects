<%@ Page Title="Three Day Actuals" Theme="MarsV3" MasterPageFile="~/App.Masterpages/Application.master"
    Language="C#" AutoEventWireup="true" CodeBehind="DayActuals.aspx.cs" Inherits="App.Site.Pooling.ThreeDayActuals"
    meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Src="../../App.UserControls/Pooling/GeneralTopFeedback.ascx" TagName="TopFeedback"
    TagPrefix="uc1" %>
<%@ Register Src="../../App.UserControls/Pooling/BottomDropDown.ascx" TagName="BottomDropDown"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceholderHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceholderMainContent" runat="server">
    <center>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div style="float: left; vertical-align: top; margin-left: 25px;">
                    <asp:CheckBox runat="server" ID="cbRemoveLongterm" Text="Exclude Longterm for off-airport"  ToolTip="Longterm = rental length > 27 days" 
                        OnCheckedChanged="cbRemoveLongterm_SelectionChanged" AutoPostBack="True" />
                </div>
                <div style="float: right; margin-right: 25px;">
                    <asp:Button ID="btnExport" runat="server" Text="Export to Excel" OnClick="btnExport_Clicked" CssClass="PlainDataExportButton" Width="120px" />
                </div>
                <uc1:TopFeedback ID="TopFeedback1" runat="server" OnSwitchButtonEvent="SwitchButtonClicked" />
                <table cellpadding="2" width="96%">
                    <tr>
                        <td>
                            <asp:Table ID="tlbOverdue" runat="server" HorizontalAlign="Left" BorderStyle="Solid"
                                BorderColor="Silver" GridLines="Both" CellPadding="2" CellSpacing="2">
                                <asp:TableRow BorderStyle="Solid" BorderColor="Silver">
                                    <asp:TableCell HorizontalAlign="Left" Font-Bold="True" Font-Size="Small" BorderStyle="Solid"
                                        BorderColor="Silver">Overdue / Collections:&nbsp;&nbsp;&nbsp;&nbsp;
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="Right" Font-Size="Small">
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Label ID="lableODCollectionsValue" runat="server" />
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow BorderStyle="Solid" BorderColor="Silver">
                                    <asp:TableCell HorizontalAlign="Left" Font-Size="Small" Font-Bold="True" BorderStyle="Solid"
                                        BorderColor="Silver"> Overdue / Open Trips Due: &nbsp;&nbsp;&nbsp;&nbsp;
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="Right">
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Label ID="LabelODOpentripsValue" runat="server" Font-Size="Small" />
                                    </asp:TableCell>
                                </asp:TableRow>

                            </asp:Table>
                        </td>
                    </tr>
                </table>
                <table cellpadding="0" cellspacing="0" width="96%">
                    <tr>
                        <td width="26%">
                            <%--  <asp:Label ID="labelODCollections" runat="server" meta:resourcekey="labelODCollectionsResource2">Overdue/Collections:</asp:Label>
                            &nbsp;--%>
                        </td>
                        <td width="8%">
                            <td width="32%">&nbsp;
                            </td>
                            <td width="16%">
                                <asp:Button ID="buttonChart" runat="server" Text="Switch to Chart" OnClick="buttonChartClicked" Width="120px" />
                            </td>
                    </tr>


                </table>
                <asp:HiddenField ID="chartviewHidden" runat="server" Value="0" ClientIDMode="Static" />
                <div id="GridviewPanel" runat="server" class="ActualsTable_Panel">
                    <div id="dataTable" runat="server">
                        Loading Data...
                    </div>
                </div>
                <div id="ChartviewPanel" runat="server">
                    <asp:Chart ID="ChartDayActuals" runat="server">
                        <Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea1">
                            </asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>
                </div>
                <asp:HiddenField ID="ShowGrid" runat="server" Value="1" />
                <uc2:BottomDropDown ID="BottomDropDown1" runat="server" OnDropDownListCountryEvent="DropDownListCountry_SelectedIndexChanged"
                    OnCMSRadioButtonEvent="CMSRadioButtonLogic_Changed" OnOPSRadioButtonEvent="OPSRadioButtonLogic_Changed"
                    OnDropDownListPoolEvent="DropDownListPool_SelectedIndexChanged" OnDropDownListLocationGroupEvent="DropDownListLocationGroup_SelectedIndexChanged"
                    OnDropDownListBranchEvent="DropDownListBranch_SelectedIndexChanged" OnDropDownListCarSegmentEvent="DropDownListCarSegment_SelectedIndexChanged"
                    OnDropDownListCarClassEvent="DropDownListCarClass_SelectedIndexChanged" OnDropDownListCarGroupEvent="DropDownListCarGroup_SelectedIndexChanged" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:HiddenField ID="BrowserWidth" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="BrowserHeight" runat="server" ClientIDMode="Static" />
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" ClientIDMode="Static" DisplayAfter="2000">
            <ProgressTemplate>
                <div id="divBackgroundCover" class='backgroundCover' runat="server" clientidmode="Static">
                </div>
                <div id="divLoadData" class='loadData' runat="server" clientidmode="Static">
                    <br />
                    &nbsp;&nbsp;&nbsp;&nbsp; Loading Data.....
                    <br />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <img src="../../App.Images/ajax-loader.gif" alt="Please Wait" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </center>
</asp:Content>
