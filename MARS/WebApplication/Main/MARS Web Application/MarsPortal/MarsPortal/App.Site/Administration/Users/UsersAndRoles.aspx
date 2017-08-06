<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/App.MasterPages/Mars.Master"
    CodeBehind="UsersAndRoles.aspx.cs" Inherits="Mars.App.Site.Administration.Users.UsersAndRoles" %>

<%@ Register Src="~/App.UserControls/Phase4/Administration/Users/UserRoleMapping.ascx" TagPrefix="uc" TagName="UserRoleMapping" %>
<%@ Register Src="~/App.UserControls/Phase4/Administration/Roles/RolePageMapping.ascx" TagPrefix="uc" TagName="RolePageMapping" %>
<%@ Register Src="~/App.UserControls/Phase4/Administration/Users/CompanyFleetOwnerMapping.ascx" TagPrefix="uc" TagName="CompanyFleetOwnerMapping" %>
<%@ Register Src="~/App.UserControls/Phase4/Administration/Fleet/FleetAdmin.ascx" TagPrefix="uc" TagName="FleetAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceholderHead" runat="server" TagPrefix="uc">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceholderMainContent" runat="server">
    <table style="height: 360px; text-align: left; margin-left: auto; margin-right: auto;">
        <tr style="margin-top: 0; margin-bottom: auto; vertical-align: top;">
            <td>
                <div id="tabbedPanel" style="font-size: 12px; width: 1000px; height: 600px; margin-left: auto; margin-right: auto; text-align: left; background-color: transparent;">
                    <ul>
                        <li><a href="#tabs-1">Roles </a></li>
                        <li><a href="#tabs-2">Users </a></li>
                        <li><a href="#tabs-3">Fleets</a></li>
                        <li><a href="#tabs-4">Companies</a></li>
                        <li style="text-align: center !important; width: 30%;">
                            <h1>Users Access</h1>
                        </li>
                    </ul>
                    <div id="tabs-1">
                        <asp:UpdatePanel ID="upRoles" runat="server">
                            <ContentTemplate>
                                <uc:RolePageMapping ID="ucRoleMaintain" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="tabs-2">
                        <asp:UpdatePanel ID="upUsers" runat="server">
                            <ContentTemplate>
                                <uc:UserRoleMapping ID="ucUserMaintain" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="tabs-3">
                        <asp:UpdatePanel ID="upFleetManagement" runat="server">
                            <ContentTemplate>
                                <uc:FleetAdmin ID="ucFleetAdmin" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="tabs-4">
                        <asp:UpdatePanel ID="upFleetOwner" runat="server">
                            <ContentTemplate>
                                <uc:CompanyFleetOwnerMapping ID="ucCompanyFleetOwner" runat="server" />
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
