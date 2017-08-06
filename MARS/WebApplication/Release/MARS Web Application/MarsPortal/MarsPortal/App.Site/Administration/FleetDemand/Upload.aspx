<%@ Page Title="" Language="C#" MasterPageFile="~/App.MasterPages/Application.Master" AutoEventWireup="true" CodeBehind="Upload.aspx.cs" Inherits="Mars.App.Site.Administration.FleetDemand.OpsFleetUpload" %>
<%@ Register Src="~/App.UserControls/FleetDemand/LocationChoice.ascx" TagName="LocationChoice" TagPrefix="ucl" %>
<%@ Register Src="~/App.UserControls/FleetDemand/LocationCarClassChoice.ascx" TagName="LocationCarClassChoice" TagPrefix="ucl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceholderHead" runat="server" class="">
    <style type="text/css">
        fieldset {
            margin-top: 10px;
            padding-top: 5px;
            padding-bottom: 5px;
        }

        th, .date-picker {
            padding: 2px;
        }

        .ui-datepicker-calendar {
            display: none;
        }

        .saveParams {
            border-color: grey; 
            border-style: solid; 
            border-width: 1px; 
            margin: 5px;
            margin-top: 15px;
        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceholderMainContent" runat="server">
    <div style="padding: 10px;">
        <h2>Logistic Dashboard Uploads</h2>

        <div id="tabbedPanel">
            <ul>
                <li><a href="#tabs-1">CMS Demand to Operational Fleet Required</a></li>
                <li><a href="#tabs-2">Priceable Share of Demand</a></li>
                <li><a href="#tabs-3">Average Revenue Per Day</a></li>
                <li><a href="#tabs-4">One Way Rentals</a></li>
            </ul>
            <div id="tabs-1">
                <h2>CMS Demand to Operational Fleet Required</h2>
                <fieldset>
                    <legend>Bulk Update</legend>
                    <table>
                        <tr>
                            <td><asp:FileUpload id="fuCMSFileUpload" runat="server" Width="600px" style="padding: 4px;"></asp:FileUpload></td>
                            <td><asp:Button id="btnCMSUpload" Text="Upload file" runat="server" onclick="btnCMSUpload_Click" style="padding-top: 2px; padding-bottom: 2px; padding-left: 15px; padding-right: 15px;"></asp:Button></td>
                        </tr>
                    </table>
                    <div style="margin: 10px;">&nbsp;<asp:Label id="lblCMSUploadStatus" runat="server" EnableViewState="false"  ForeColor="Green"></asp:Label></div>
                </fieldset>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                <fieldset>
                    <legend>Individual Update</legend>
                    <ucl:LocationChoice ID="lcCMS" runat="server" />

                    <table class="saveParams">
                        <tbody>
                            <tr>
                                <td>Day Of Week:</td>
                                <td><asp:DropDownList ID="ddlCMSDayOfWeek" runat="server" Width="12em"></asp:DropDownList></td>
                                <td style="padding-left: 20px;>CMS Demand Operational Fleet Required:</tdstyle="padding-left:>
                                <td><asp:TextBox ID="tbCMSParameter" runat="server"></asp:TextBox></td>
                                <td style="padding-left: 20px;"><asp:Button id="btnCMSSave" Text="Save Parameter" runat="server" style="padding-top: 2px; padding-bottom: 2px; padding-left: 15px; padding-right: 15px;" OnClick="btnCMSSave_Click"></asp:Button></td>
                            </tr>
                        </tbody>
                    </table>

                    <div style="margin: 10px;">&nbsp;<asp:Label id="lblCMSIndividualUploadStatus" runat="server" EnableViewState="false"  ForeColor="Green"></asp:Label></div>

                    <asp:Repeater ID="rptCMSLog" runat="server" OnPreRender="rptCMSLog_PreRender" EnableViewState="false">
                        <HeaderTemplate>
                            <table style="margin-left: 5px; background-color: #ffefcf; border-color: #7c7c7c; border-width: 1px; border-style: solid; padding: 5px;">
                                <thead style="background-color: #efcfaf;">
                                    <tr>
                                        <th>Country</th>
                                        <th>Pool</th>
                                        <th>Location Group</th>
                                        <th>Location Code</th>
                                        <th>Fleet Ratio</th>
                                        <th>Day Of Week</th>
                                        <th>Uploaded Date</th>
                                        <th>Added By</th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>                                
                                    <tr>
                                        <td><%#Eval("Country")%></td>
                                        <td><%#Eval("CmsPool")%></td>
                                        <td><%#Eval("LocationGroupCode")%></td>
                                        <td><%#Eval("LocationCode")%></td>
                                        <td><%#Eval("FleetRatio")%></td>
                                        <td><%#Eval("DayOfWeek")%></td>
                                        <td><%#((DateTime)Eval("UploadedDate")).ToShortDateString()%></td>
                                        <td><%#Eval("AddedBy")%></td>
                                    </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                                </tbody>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </fieldset>
                </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div id="tabs-2">
                <h2>Priceable Share of Demand</h2>
                <fieldset>
                    <legend>Bulk Update</legend>
                    <table>
                        <tr>
                            <td><asp:FileUpload id="fuPSDFileUpload" runat="server" Width="600px" style="padding: 4px;"></asp:FileUpload></td>
                            <td><asp:Button id="btnPSDUpload" Text="Upload file" runat="server" onclick="btnPSDUpload_Click" style="padding-top: 2px; padding-bottom: 2px; padding-left: 15px; padding-right: 15px;"></asp:Button></td>
                        </tr>
                    </table>
                    <div style="margin: 10px;">&nbsp;<asp:Label id="lblPSDUploadStatus" runat="server" EnableViewState="false" ForeColor="Green"></asp:Label></div>
                </fieldset>
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
                <fieldset>
                    <legend>Individual Update</legend>
                    <ucl:LocationCarClassChoice ID="lcPSD" runat="server" />
                    <table class="saveParams">
                        <tbody>
                            <tr>
                                <td>Month Year:</td>
                                <td><asp:TextBox ID="PSDDate" runat="server" class="date-picker"></asp:TextBox></td>
                                <td style="padding-left: 20px;">Priceable Share of Demand:</td>
                                <td><asp:TextBox ID="tbPSDParameter" runat="server"></asp:TextBox></td>
                                <td style="padding-left: 20px;"><asp:Button id="btnPSDSave" Text="Save Parameter" runat="server" style="padding-top: 2px; padding-bottom: 2px; padding-left: 15px; padding-right: 15px;" OnClick="btnPSDSave_Click"></asp:Button></td>
                            </tr>
                        </tbody>
                    </table>

                    <div style="margin: 10px;">&nbsp;<asp:Label id="lblPSDIndividualUploadStatus" runat="server" EnableViewState="false" ForeColor="Green"></asp:Label></div>

                    <asp:Repeater ID="rptPSDLog" runat="server" OnPreRender="rptPSDLog_PreRender" EnableViewState="false">
                        <HeaderTemplate>
                            <table style="margin-left: 5px; background-color: #ffefcf; border-color: #7c7c7c; border-width: 1px; border-style: solid; padding: 5px;">
                                <thead style="background-color: #efcfaf;">
                                    <tr>
                                        <th>Country</th>
                                        <th>Pool</th>
                                        <th>Location Group</th>
                                        <th>Location Code</th>
                                        <th>Car Class</th>
                                        <th>Year</th>
                                        <th>Month</th>
                                        <th>Priceable Percent</th>
                                        <th>Uploaded Date</th>
                                        <th>Added By</th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>                                                            
                                    <tr>
                                        <td><%#Eval("Country")%></td>
                                        <td><%#Eval("CmsPool")%></td>
                                        <td><%#Eval("LocationGroupCode")%></td>
                                        <td><%#Eval("LocationCode")%></td>
                                        <td><%#Eval("CarClass")%></td>
                                        <td><%#Eval("ReportDateKey").ToString().Substring(0,4)%></td>
                                        <td><%#System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(Int32.Parse(Eval("ReportDateKey").ToString().Substring(4,2))).Substring(0,3)%></td>
                                        <td><%#Eval("PriceablePercent")%></td>
                                        <td><%#((DateTime)Eval("UploadedDate")).ToShortDateString()%></td>
                                        <td><%#Eval("AddedBy")%></td>
                                    </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                                </tbody>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </fieldset>
                </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div id="tabs-3">
                <h2>Average Revenue Per Day</h2>
                <fieldset>
                    <legend>Bulk Update</legend>
                    <table>
                        <tr>
                            <td><asp:FileUpload id="fuRPDFileUpload" runat="server" Width="600px" style="padding: 4px;"></asp:FileUpload></td>
                            <td><asp:Button id="btnRPDUpload" Text="Upload file" runat="server" onclick="btnRPDUpload_Click" style="padding-top: 2px; padding-bottom: 2px; padding-left: 15px; padding-right: 15px;"></asp:Button></td>
                        </tr>
                    </table>
                    <div style="margin: 10px;">&nbsp;<asp:Label id="lblRPDUploadStatus" runat="server" EnableViewState="false" ForeColor="Green"></asp:Label></div>
                </fieldset>
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                <ContentTemplate>
                <fieldset>
                    <legend>Individual Update</legend>
                    <ucl:LocationCarClassChoice ID="lcRPD" runat="server" />
                    
                    <table class="saveParams">
                        <tbody>
                            <tr>
                                <td>Month Year:</td>
                                <td><asp:TextBox ID="RPDDate" runat="server" class="date-picker"></asp:TextBox></td>
                                <td style="padding-left: 20px;">Average Revenue Per Day:</td>
                                <td><asp:TextBox ID="tbRPDParameter" runat="server"></asp:TextBox></td>
                                <td style="padding-left: 20px;"><asp:Button id="btnRPDSave" Text="Save Parameter" runat="server" style="padding-top: 2px; padding-bottom: 2px; padding-left: 15px; padding-right: 15px;" OnClick="btnRPDSave_Click"></asp:Button></td>
                            </tr>
                        </tbody>
                    </table>

                    <div style="margin: 10px;">&nbsp;<asp:Label id="lblRPDIndividualUploadStatus" runat="server" EnableViewState="false" ForeColor="Green"></asp:Label></div>

                    <asp:Repeater ID="rptRPDLog" runat="server" EnableViewState="false" OnPreRender="rptRPDLog_PreRender">
                        <HeaderTemplate>
                            <table style="margin-left: 5px; background-color: #ffefcf; border-color: #7c7c7c; border-width: 1px; border-style: solid; padding: 5px;">
                                <thead style="background-color: #efcfaf;">
                                    <tr>
                                        <th>Country</th>
                                        <th>Pool</th>
                                        <th>Location Group</th>
                                        <th>Location Code</th>
                                        <th>Car Class</th>
                                        <th>Year</th>
                                        <th>Month</th>
                                        <th>Average RPD</th>
                                        <th>Uploaded Date</th>
                                        <th>Added By</th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>                                                            
                                    <tr>
                                        <td><%#Eval("Country")%></td>
                                        <td><%#Eval("CmsPool")%></td>
                                        <td><%#Eval("LocationGroupCode")%></td>
                                        <td><%#Eval("LocationCode")%></td>
                                        <td><%#Eval("CarClass")%></td>
                                        <td><%#Eval("ReportDateKey").ToString().Substring(0,4)%></td>
                                        <td><%#System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(Int32.Parse(Eval("ReportDateKey").ToString().Substring(4,2))).Substring(0,3)%></td>
                                        <td><%#Eval("AvgRpd")%></td>
                                        <td><%#((DateTime)Eval("UploadedDate")).ToShortDateString()%></td>
                                        <td><%#Eval("AddedBy")%></td>
                                    </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                                </tbody>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </fieldset>                    
                </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div id="tabs-4">
                <h2>One Way Rentals</h2>
                <fieldset>
                    <legend>Bulk Update</legend>
                    <table>
                        <tr>
                            <td><asp:FileUpload id="fuOWRFileUpload" runat="server" Width="600px" style="padding: 4px;"></asp:FileUpload></td>
                            <td><asp:Button id="btnOWRUpload" Text="Upload file" runat="server" onclick="btnOWRUpload_Click" style="padding-top: 2px; padding-bottom: 2px; padding-left: 15px; padding-right: 15px;"></asp:Button></td>
                        </tr>
                    </table>
                    <div style="margin: 10px;">&nbsp;<asp:Label id="lblOWRUploadStatus" runat="server" EnableViewState="false" ForeColor="Green"></asp:Label></div>
                </fieldset>     
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                <fieldset>
                    <legend>Individual Update</legend>
                    <ucl:LocationChoice ID="lcOWR" runat="server" />
                    <table class="saveParams">
                        <tbody>
                            <tr>
                                <td>Day Of Week:</td>
                                <td><asp:DropDownList ID="ddlOWRDayOfWeek" runat="server" Width="12em"></asp:DropDownList></td>
                                <td style="padding-left: 20px;">One Way Rentals:</td>
                                <td><asp:TextBox ID="tbOWRParameter" runat="server"></asp:TextBox></td>
                                <td style="padding-left: 20px;"><asp:Button id="btnOWRSave" Text="Save Parameter" runat="server" style="padding-top: 2px; padding-bottom: 2px; padding-left: 15px; padding-right: 15px;" OnClick="btnOWRSave_Click"></asp:Button></td>
                            </tr>
                        </tbody>
                    </table>

                    <div style="margin: 10px;">&nbsp;<asp:Label id="lblOWRIndividualUploadStatus" runat="server" EnableViewState="false" ForeColor="Green"></asp:Label></div>

                    <asp:Repeater ID="rptOWRLog" runat="server" OnPreRender="rptOWRLog_PreRender" EnableViewState="false">
                        <HeaderTemplate>
                            <table style="margin-left: 5px; background-color: #ffefcf; border-color: #7c7c7c; border-width: 1px; border-style: solid; padding: 5px;">
                                <thead style="background-color: #efcfaf;">
                                    <tr>
                                        <th>Country</th>
                                        <th>Pool</th>
                                        <th>Location Group</th>
                                        <th>Location Code</th>
                                        <th>One Way</th>
                                        <th>Day Of Week</th>
                                        <th>Uploaded Date</th>
                                        <th>Added By</th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>                                                            
                                    <tr>
                                        <td><%#Eval("Country")%></td>
                                        <td><%#Eval("CmsPool")%></td>
                                        <td><%#Eval("LocationGroupCode")%></td>
                                        <td><%#Eval("LocationCode")%></td>
                                        <td><%#Eval("OneWay")%></td>
                                        <td><%#Eval("DayOfWeek")%></td>
                                        <td><%#((DateTime)Eval("UploadedDate")).ToShortDateString()%></td>
                                        <td><%#Eval("AddedBy")%></td>
                                    </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                                </tbody>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </fieldset>  
                </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
      
    </div>

<script type="text/javascript">
     $(function () {
         $("#tabbedPanel").tabs({ selected: <%= SelectedTab %> });
     });

    function InitYearMonthPicker() {
        $('.date-picker').datepicker( {
            changeMonth: true,
            changeYear: true,
            showButtonPanel: true,
            dateFormat: 'MM yy',
            onClose: function(dateText, inst) { 
                var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
                var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
                $(this).datepicker('setDate', new Date(year, month, 1));
            },
            beforeShow : function(input, inst) {
                if ((datestr = $(this).val()).length > 0) {
                    year = datestr.substring(datestr.length-4, datestr.length);
                    month = jQuery.inArray(datestr.substring(0, datestr.length-5), $(this).datepicker('option', 'monthNames'));
                    $(this).datepicker('option', 'defaultDate', new Date(year, month, 1));
                    $(this).datepicker('setDate', new Date(year, month, 1));
                }
            }
        });
    }

    $(InitYearMonthPicker);

    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitYearMonthPicker);

    
</script>
</asp:Content>
