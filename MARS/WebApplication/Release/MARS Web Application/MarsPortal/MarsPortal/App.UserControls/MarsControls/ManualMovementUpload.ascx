<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManualMovementUpload.ascx.cs" Inherits="App.UserControls.MovementUpload" %>


<asp:UpdatePanel ID="upUploady" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel ID="pnlUpload" runat="server" meta:resourcekey="pnlUploadResource2">
            <fieldset>
                <legend>Upload Scenario:</legend>
                <table width="350px">
                    <tr>
                        <td>
                            <asp:Label ID="lblUploadCountry" runat="server" Text="Select Country:" 
                                meta:resourcekey="lblUploadCountryResource2"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblUploadFleetPlan" runat="server" Text="Select fleetPlan:" 
                                meta:resourcekey="lblUploadFleetPlanResource2"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblAdditionDeletion" runat="server" Text="Select Upload Type:" 
                                meta:resourcekey="lblAdditionDeletionResource2"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="ddlCountryList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCountryList_Click"
                                              meta:resourcekey="ddlCountryListResource2" ClientIDMode="Static" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlFleetPlan" runat="server" 
                                meta:resourcekey="ddlFleetPlanResource2"></asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlAdditionDeletion" runat="server" 
                                meta:resourcekey="ddlAdditionDeletionResource2"></asp:DropDownList>
                        </td>
                    </tr>
                </table>    
                <asp:Label ID="lblMovementUpload" runat="server" Text="File:" 
                    meta:resourcekey="lblMovementUploadResource2"></asp:Label>
                <asp:FileUpload ID="fupMovementUpload" runat="server" CssClass="upload" 
                    meta:resourcekey="fupMovementUploadResource2"  />
    
                <asp:Button runat="server" ID="btnUpload" text="Upload" 
                    OnClick="btnUpload_Click" ClientIDMode="Static"
                    CssClass="chartbuttonoptions" meta:resourcekey="btnUploadResource2" />

                <br />  
                <asp:Label runat="server" ID="lblStatus" meta:resourcekey="lblStatusResource2" ClientIDMode="Static" />
                <br />     
                <asp:Label runat="server" ID="lblUpdateStatus" ClientIDMode="Static" />
                <br />
                <uc:ActivityLog id="activityLog" runat="server"></uc:ActivityLog>
                <br />
            </fieldset> 
        </asp:Panel>
    </ContentTemplate>
     <Triggers>
        <asp:PostBackTrigger ControlID="btnUpload" />
    </Triggers>    
</asp:UpdatePanel>

<%--<script type="text/javascript">
    var _us, _ua;
    var HTTPREQUEST = 'GET';
    var WEBSERVICE = '../../../App.Webservices/Sizing/SizingService.aspx';
    var INTERVAL = 5000, RETURNCODE = 200, READYSTATE = 4;
    var MICOROSOFTACTIVE = 'Microsoft.XMLHTTP';
    var DIVUPDATELABEL = 'lblUpdateStatus', BTNUPLOADLABEL = 'btnUpload';
    var alternate = true;
    var RUNNING = 'Running', DISABLED = 'disabled';
    var COMMENT = ", Database action: ", EMPTYSTRING = '', LEFTBRACKET = '(', RIGHTBRACKET = ')';
    var _itvl = setInterval(function () { loadXMLDoc(); }, INTERVAL);
    loadXMLDoc();

    function Update() {
        document.getElementById(DIVUPDATELABEL).innerHTML = _us + COMMENT + _ua;
        alternate = true;
        document.getElementById(BTNUPLOADLABEL).disabled = EMPTYSTRING;
        if (_ua == RUNNING) {document.getElementById(BTNUPLOADLABEL).disabled = DISABLED; }
    }
    function loadXMLDoc() {
        if (!alternate) { return; }
        var xmlhttp;
        if (window.XMLHttpRequest) {// code for IE7+, Firefox, Chrome, Opera, Safari
            xmlhttp = new XMLHttpRequest();
        }
        else {// code for IE6, IE5
            xmlhttp = new ActiveXObject(MICOROSOFTACTIVE);
        }
        xmlhttp.onreadystatechange = function () {
            if (xmlhttp.readyState == READYSTATE && xmlhttp.status == RETURNCODE) {
                
                //var obj = eval(LEFTBRACKET + xmlhttp.responseText + RIGHTBRACKET);
                //_us = obj.Message;
                //_ua = obj.Action;
                Update();
            }
        };
        alternate = false;
        xmlhttp.open(HTTPREQUEST, WEBSERVICE, true);
        xmlhttp.send();
    }
</script>--%>
