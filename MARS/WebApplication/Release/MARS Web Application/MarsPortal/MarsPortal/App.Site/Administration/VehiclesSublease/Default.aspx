<%@ Page Title="" Language="C#" MasterPageFile="~/App.Masterpages/Application.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="App.Management.vehiclesLease.Default" %>
<asp:Content ID="ContentHead" ContentPlaceHolderID="ContentPlaceholderHead" runat="server">
</asp:Content>
<asp:Content ID="ContentMain" ContentPlaceHolderID="ContentPlaceholderMainContent" runat="server">
<%-- Update Panel --%>
 <asp:UpdatePanel ID="UpdatePanelVehiclesLease" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
        <ContentTemplate>
            <%-- Control Wapper --%>
            <div class="divControlContent">
                <%-- Page Information --%>
                <uc:PageInformation ID="UserControlPageInformation" runat="server"   />
                <%-- Vehicles Lease Selection : Countries --%>
                <div class="divMappingSelection">
                <table>
                <tr>
                    <td><uc:VehiclesSelection ID="CountryOwner" ControlName="CountryOwner" runat="server" OnLoadSerials="LoadSerials_Filter" LabelMessage="Country Owner" /></td>
                    <td><uc:VehiclesSelection ID="CountryRent" ControlName="CountryRent" runat="server" OnLoadSerials="LoadSerials_Filter" LabelMessage="Country Rent" /></td>
                    <td><asp:Label ID="LabelStartDate" runat="server" Text="Start Date"/></td>
                    <td><uc:VehiclesDatePicker ID="StartDate" runat="server" OnLoadSerials="LoadSerials_Filter" Visible="False" /></td>
                    <td><asp:Label ID="LabelModelDescription" runat="server" Text="Model Description"></asp:Label></td>
                    <td><uc:VehiclesCheckBoxList ID="ModelDescription" runat="server" ListBoxWidth="150" Visible="true" ListBoxCSSClass="divPopUpCheckBoxList" CheckBoxSelectAllToolTip="Model Description" OnLoadSerials="LoadSerials_Model_Filter" /></td>
                    
                </tr>
                </table>
                </div>
                <hr />
                <%-- Vehicles Lease --%>
                <uc:MappingVehiclesLeaseGridview ID="MappingVehiclesLeaseGridview" runat="server" Visible="true" OnGridviewCommand="ShowDependents" />
            </div>
            <%-- Delete Confirm --%>
            <uc:DeleteConfirm ID="DeleteConfirm" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
