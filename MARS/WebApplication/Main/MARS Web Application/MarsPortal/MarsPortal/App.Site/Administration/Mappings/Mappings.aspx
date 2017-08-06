<%@ Page Title="" Language="C#" MasterPageFile="~/App.MasterPages/Mars.Master" AutoEventWireup="true"
    CodeBehind="Mappings.aspx.cs" Inherits="Mars.App.Site.Administration.Mappings.Mappings" %>

<%@ Register Src="~/App.UserControls/Phase4/Administration/Mapping/EntityGrid.ascx" TagPrefix="uc" TagName="EntityGrid" %>
<%@ Register Src="~/App.UserControls/Phase4/Administration/Mapping/EntityParameter.ascx" TagPrefix="uc" TagName="EntityParameter" %>

<%@ Register Src="~/App.UserControls/Phase4/Administration/Mapping/EntityPopups/CountryPopup.ascx" TagPrefix="uc" TagName="CountryPopup" %>
<%@ Register Src="~/App.UserControls/Phase4/Administration/Mapping/EntityPopups/PoolPopup.ascx" TagPrefix="uc" TagName="PoolPopup" %>
<%@ Register Src="~/App.UserControls/Phase4/Administration/Mapping/EntityPopups/LocationGroupPopup.ascx" TagPrefix="uc" TagName="LocationGroupPopup" %>
<%@ Register Src="~/App.UserControls/Phase4/Administration/Mapping/EntityPopups/RegionPopup.ascx" TagPrefix="uc" TagName="RegionPopup" %>
<%@ Register Src="~/App.UserControls/Phase4/Administration/Mapping/EntityPopups/AreaPopup.ascx" TagPrefix="uc" TagName="AreaPopup" %>
<%@ Register Src="~/App.UserControls/Phase4/Administration/Mapping/EntityPopups/LocationPopup.ascx" TagPrefix="uc" TagName="LocationPopup" %>
<%@ Register Src="~/App.UserControls/Phase4/Administration/Mapping/EntityPopups/CarSegmentPopup.ascx" TagPrefix="uc" TagName="CarSegmentPopup" %>
<%@ Register Src="~/App.UserControls/Phase4/Administration/Mapping/EntityPopups/CarClassPopup.ascx" TagPrefix="uc" TagName="CarClassPopup" %>
<%@ Register Src="~/App.UserControls/Phase4/Administration/Mapping/EntityPopups/CarGroupPopup.ascx" TagPrefix="uc" TagName="CarGroupPopup" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceholderHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceholderMainContent" runat="server">
    <table style="height: 360px; text-align: left; margin-left: auto; margin-right: auto;">
        <tr style="margin-top: 0; margin-bottom: auto; vertical-align: top;">
            <td>
                <div id="tabbedPanel" style="font-size: 12px; width: 1000px; height: 600px; margin-left: auto; margin-right: auto; text-align: left; background-color: transparent;">
                    <ul>
                        <li><a href="#tabs-1">Country</a></li>
                        <li><a href="#tabs-2">Pool</a></li>
                        <li><a href="#tabs-3">Location Group</a></li>
                        <li><a href="#tabs-4">Region</a></li>
                        <li><a href="#tabs-5">Area</a></li>
                        <li><a href="#tabs-6">Location</a></li>
                        <li><a href="#tabs-7">Car Segment</a></li>
                        <li><a href="#tabs-8">Car Class</a></li>
                        <li><a href="#tabs-9">Car Group</a></li>
                        <li style="text-align: center !important; width: 20%;">
                            <h1>Mappings</h1>
                        </li>
                    </ul>
                    <div id="tabs-1">
                        <asp:UpdatePanel ID="upCountries" runat="server">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <uc:EntityParameter ID="ucCountryParameter" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <uc:EntityGrid ID="ucCountryGrid"
                                                SessionStringName="CountryEntityGridSessionStringName" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <uc:CountryPopup runat="server" ID="ucCountryPopup" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="tabs-2">
                        <asp:UpdatePanel ID="upPools" runat="server">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <uc:EntityParameter ID="ucPoolParameter" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <uc:EntityGrid ID="ucPoolGrid"
                                                SessionStringName="PoolEntityGridSessionStringName" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <uc:PoolPopup runat="server" ID="ucPoolPopup" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="tabs-3">
                        <asp:UpdatePanel ID="upLocationGroups" runat="server">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <uc:EntityParameter ID="ucLocationGroupParameters" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <uc:EntityGrid ID="ucLocationGroupGrid"
                                                SessionStringName="LocationGroupEntityGridSessionStringName" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <uc:LocationGroupPopup runat="server" ID="ucLocationGroupPopup" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="tabs-4">
                        <asp:UpdatePanel ID="upRegion" runat="server">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <uc:EntityParameter ID="ucRegionParameters" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <uc:EntityGrid ID="ucRegionGrid"
                                                SessionStringName="RegionEntityGridSessionStringName" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <uc:RegionPopup runat="server" ID="ucRegionPopup" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="tabs-5">
                        <asp:UpdatePanel ID="upArea" runat="server">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <uc:EntityParameter ID="ucAreaParameters" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <uc:EntityGrid ID="ucAreaGrid"
                                                SessionStringName="AreaEntityGridSessionStringName" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <uc:AreaPopup runat="server" ID="ucAreaPopup" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="tabs-6">
                        <asp:UpdatePanel ID="upLocations" runat="server">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <uc:EntityParameter ID="ucLocationParameters" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <uc:EntityGrid ID="ucLocationGrid"
                                                SessionStringName="LocationEntityGridSessionStringName" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <uc:LocationPopup runat="server" ID="ucLocationPopup" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="tabs-7">
                        <asp:UpdatePanel ID="upCarSegments" runat="server">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <uc:EntityParameter ID="ucCarSegmentParameters" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <uc:EntityGrid ID="ucCarSegmentGrid"
                                                SessionStringName="CarSegmentEntityGridSessionStringName" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <uc:CarSegmentPopup runat="server" ID="ucCarSegmentPopup" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="tabs-8">
                        <asp:UpdatePanel ID="upCarClasses" runat="server">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <uc:EntityParameter ID="ucCarClassParamters" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <uc:EntityGrid ID="ucCarClassGrid"
                                                SessionStringName="CarClassEntityGridSessionStringName" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <uc:CarClassPopup runat="server" ID="ucCarClassPopup" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="tabs-9">
                        <asp:UpdatePanel ID="upCarGroups" runat="server">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <uc:EntityParameter ID="ucCarGroupParameters" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <uc:EntityGrid ID="ucCarGroupGrid"
                                                SessionStringName="CarGroupEntityGridSessionStringName" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <uc:CarGroupPopup runat="server" ID="ucCarGroupPopup" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </td>
        </tr>
    </table>

    <script type="text/javascript">

        $(document).ready(function () {

            $("#tabbedPanel").tabs();
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(panelLoaded);

            function panelLoaded(sender, args) {
                if (args.get_panelsUpdated().length > 0) {
                    $("#tabbedPanel").tabs();
                }
            }
        });

    </script>
</asp:Content>
