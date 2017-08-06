<%@ Page Title="Fleet Comparison" Language="C#" MasterPageFile="~/App.MasterPages/Application.Master" AutoEventWireup="true" CodeBehind="FleetComparison.aspx.cs" Inherits="Mars.App.Site.Pooling.FleetComparison" %>

<%@ Register Src="../../App.UserControls/Pooling/GeneralTopFeedback.ascx" TagName="TopFeedback" TagPrefix="uc1" %>

<%@ Register Src="../../App.UserControls/Pooling/BottomDropDown.ascx" TagName="BottomDropDown" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceholderHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceholderMainContent" runat="server">
    <center>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div style="float: left; vertical-align: top; margin-left: 25px;">
                    <asp:CheckBox runat="server" ID="cbRemoveLongterm" Text="Exclude Longterm for off-airport" ToolTip="Longterm = rental length > 27 days" 
                        OnCheckedChanged="cbRemoveLongterm_SelectionChanged" AutoPostBack="True" />
                </div>
                <div style="float: right; margin-right: 25px;">
                    <asp:Button ID="btnExport" runat="server" Text="Export to Excel" OnClick="btnExport_Clicked" CssClass="PlainDataExportButton" Width="120px" />
                </div>
                <uc1:TopFeedback ID="TopFeedback1" runat="server" OnSwitchButtonEvent="SwitchButtonClicked" />


                <div runat="server" id="scdivTable">
                    <h2>Loading Data...</h2>
                </div>
                <uc2:BottomDropDown ID="BottomDropDown1" runat="server"
                    EventCallback=""
                    OnDropDownListCountryEvent="DropDownListCountry_SelectedIndexChanged"
                    OnCMSRadioButtonEvent="CMSRadioButtonLogic_Changed"
                    OnOPSRadioButtonEvent="OPSRadioButtonLogic_Changed"
                    OnDropDownListPoolEvent="DropDownListPool_SelectedIndexChanged"
                    OnDropDownListLocationGroupEvent="DropDownListLocationGroup_SelectedIndexChanged"
                    OnDropDownListBranchEvent="DropDownListBranch_SelectedIndexChanged"
                    OnDropDownListCarSegmentEvent="DropDownListCarSegment_SelectedIndexChanged"
                    OnDropDownListCarClassEvent="DropDownListCarClass_SelectedIndexChanged"
                    OnDropDownListCarGroupEvent="DropDownListCarGroup_SelectedIndexChanged" />


                <div style='padding: 10px;'>
                    Topic Selection : 
                    <asp:DropDownList ID="DropDownListTopic" AutoPostBack="true" runat="server"
                        OnSelectedIndexChanged="UpdateController" Width="150px" />
                </div>
                <hr style='width: 96%;' />
                <script src="../../App.Scripts/Pooling/PoolingGeneral.js" type="text/javascript"></script>
                <script type="text/javascript" src="../../App.Scripts/Pooling/SiteComparison.js"></script>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:HiddenField ID="BrowserWidth" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="BrowserHeight" runat="server" ClientIDMode="Static" />
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
</asp:Content>
