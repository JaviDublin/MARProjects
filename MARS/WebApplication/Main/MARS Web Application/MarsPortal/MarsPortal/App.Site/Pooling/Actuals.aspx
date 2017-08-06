<%@ Page Title="" Language="C#" MasterPageFile="~/App.MasterPages/Mars.Master" AutoEventWireup="true" CodeBehind="Actuals.aspx.cs" Inherits="Mars.App.Site.Pooling.Actuals" %>

<%@ Register Src="~/App.UserControls/Phase4/Pooling/ActualsGrid.ascx" TagPrefix="uc" TagName="ActualsGrid" %>
<%@ Register Src="~/App.UserControls/Phase4/AvailabilityParameters.ascx" TagPrefix="uc" TagName="AvailabilityParameters" %>
<%@ Register Src="~/App.UserControls/Phase4/Pooling/OverdueGrid.ascx" TagPrefix="uc" TagName="OverdueGrid" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceholderHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceholderMainContent" runat="server">
    
    
    
    
    <table style="text-align: center; margin-left: auto; margin-right: auto;">
        <tr style="margin-top: 0; margin-left: 0; margin-bottom: auto; vertical-align: top;">
            <td>
                <div id="tabbedPanel" style="width: 1050px; height: 350px; text-align: left; background-color: transparent;">
                    <ul>
                        <li><a href="#tabs-1">Vehicle Sublease</a></li>

                    </ul>
                    <div id="tabs-1" style="text-align: left;">
                        <table style="text-align: center; margin-left: 0; margin-right: auto; height: 280px;">
                            <tr style="vertical-align: top;">
                                <td>
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <uc:OverdueGrid runat="server" id="ucOverdueGrid" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <uc:ActualsGrid runat="server" id="ucActualsGrid" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnRefresh"/>
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnRefresh" runat="server" Text="Load" CssClass="StandardButton" OnClick="LoadReport"/>
            </td>
        </tr>

        <tr style="vertical-align: top;">
            <td>
                <asp:UpdatePanel ID="upnlParameters" runat="server">
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td style="vertical-align: top;">
                                    <uc:AvailabilityParameters runat="server" ID="ucAvailabilityParameters" 
                                        ShowExcludeLongtermForOffAirport="True"
                                        ShowVehicleFields="False"  ShowValuesAs="False" AllowMultiSelect="True"
                                        ShowAdditionalFields="False" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>




    <script type="text/javascript">
        $(function () {
            $("#tabbedPanel").tabs();

        });
    </script>

</asp:Content>
