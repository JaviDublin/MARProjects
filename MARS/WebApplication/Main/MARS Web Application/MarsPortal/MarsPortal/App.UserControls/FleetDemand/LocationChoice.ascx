<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LocationChoice.ascx.cs" Inherits="Mars.App.UserControls.FleetDemand.LocationChoice" %>
<div>
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table>
                <thead>
                    <tr>
                        <td>Country</td>
                        <td>Pool</td>
                        <td>Location Group</td>
                        <td>Location</td>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td><asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" Width="12em"></asp:DropDownList></td>
                        <td><asp:DropDownList ID="ddlPool" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPool_SelectedIndexChanged" Width="12em"></asp:DropDownList></td>
                        <td><asp:DropDownList ID="ddlLocationGroup" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlLocationGroup_SelectedIndexChanged" Width="12em"></asp:DropDownList></td>
                        <td><asp:DropDownList ID="ddlLocation" runat="server" Width="30em" AutoPostBack="True"></asp:DropDownList></td>
                    </tr>
                </tbody>
            </table>                            
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlCountry" EventName="SelectedIndexChanged" />        
            <asp:AsyncPostBackTrigger ControlID="ddlPool" EventName="SelectedIndexChanged" />        
            <asp:AsyncPostBackTrigger ControlID="ddlLocationGroup" EventName="SelectedIndexChanged" />                            
        </Triggers>
    </asp:UpdatePanel>      
</div>