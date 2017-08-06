<%@ Page Title="Pooling - Alerts" Language="C#" MasterPageFile="~/App.MasterPages/Application.Master" AutoEventWireup="true" CodeBehind="Alerts.aspx.cs" Inherits="Mars.Site.Pooling.Alerts" %>

<%@ Register Src="../../App.UserControls/Pooling/TopFeedback2.ascx" TagName="TopFeedback" TagPrefix="uc1" %>

<%@ Register Src="../../App.UserControls/Pooling/BottomDropDown.ascx" TagName="BottomDropDown" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceholderHead" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceholderMainContent" runat="server">
    <center>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                                <div style="float: left; vertical-align: top; margin-left: 25px;">
                    <asp:CheckBox runat="server" ID="cbRemoveLongterm" Text="Exclude Longterm for off-airport" ToolTip="Longterm = rental length > 27 days"/>
                </div>
                <div style="float: right; margin-right: 25px;">
                    <asp:Button ID="btnExport" runat="server" Text="Export to Excel" OnClick="btnExport_Clicked" CssClass="PlainDataExportButton" Width="120px" />
                </div>

                <uc1:TopFeedback ID="TopFeedback1" runat="server" />
                                    
                    

                <div style="float: right; vertical-align: top; margin-right: 25px;">

                    <b>
                    Future Limits - 
                    </b>
                    <asp:TextBox runat="server" ID="tbDate"  Width="70px" />
                    <asp:CalendarExtender ID="ceDateExtender" runat="server" TargetControlID="tbDate" Format="dd/MM/yyyy" />
                </div>
                <div id="divTable" runat="server" clientidmode="Static">
                    <h2></h2>
                </div>
                <asp:Button ID="btnLoadData" runat="server" Text="Calculate Alerts" OnClick="btnLoadData_Click" />
                <script src="../../App.Scripts/Pooling/PoolingGeneral.js" type="text/javascript"></script>
                <script type="text/javascript" src="../../App.Scripts/Pooling/Alerts.js"></script>
                <input type="hidden" runat='server' id='hdIsPostback' value='false' clientidmode='static' />


                <div id='AlertCarList' style="color: #0000CC; position: absolute; width: 140px; height: 80px; display: none; 
                    border: 2px solid #99CCFF; opacity: 0.8; filter: alpha(opacity:80); background-color: #ffffff">Guessing...</div>

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
                    OnDropDownListCarGroupEvent="UpdateController" />

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
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <img src="../../App.Images/ajax-loader.gif" alt="Please Wait" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </center>
</asp:Content>
