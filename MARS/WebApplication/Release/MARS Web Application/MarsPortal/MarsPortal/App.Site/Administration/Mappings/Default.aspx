<%@ Page Title="" Language="C#" MasterPageFile="~/App.MasterPages/Application.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="App.Management.Mappings.Default" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="ContentPlaceholderHead" runat="server">
</asp:Content>
<asp:Content ID="ContentMain" ContentPlaceHolderID="ContentPlaceholderMainContent" runat="server">
    <%-- Update Panel --%>
    <asp:UpdatePanel ID="UpdatePanelMaintenanceMappingTable" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
        <ContentTemplate>
            <%-- Control Wapper --%>
            <div class="divControlContent">
                <%-- Page Information --%>
                <uc:PageInformation ID="UserControlPageInformation" runat="server"   />
                <%-- Mapping Selection --%>
                <uc:MappingSelection ID="MappingSelection" runat="server" OnLoadMappingTables="LoadMappingTables_Filter" />
                <%-- Countries --%>
                <uc:MappingCountryGridview ID="MappingCountryGridview" runat="server" Visible="false" OnGridviewCommand="ShowDependents" />
                <%-- Area Codes --%>
                <uc:MappingAreaCodeGridview ID="MappingAreaCodeGridview" runat="server" Visible="false" />
                <%-- CMS Pools --%>
                <uc:MappingCMSPoolGridview ID="MappingCMSPoolGridview" runat="server" Visible="false" OnGridviewCommand="ShowDependents" />
                <%-- CMS Location Groups --%>
                <uc:MappingCMSLocationGridview ID="MappingCMSLocationGridview" runat="server" Visible="false" OnGridviewCommand="ShowDependents" />
                <%-- OPS Regions --%>
                <uc:MappingOPSRegionGridview ID="MappingOPSRegionGridviews" runat="server" Visible="false" OnGridviewCommand="ShowDependents" />
                <%-- OPS Areas --%>
                <uc:MappingOPSAreaGridview ID="MappingOPSAreaGridview" runat="server" Visible="false" OnGridviewCommand="ShowDependents" />
                <%-- Locations --%>
                <uc:MappingLocationGridview ID="MappingLocationGridview" runat="server" Visible="false" />
                <%-- Car Segments --%>
                <uc:MappingCarSegmentGridview ID="MappingCarSegmentGridview" runat="server" Visible="false" OnGridviewCommand="ShowDependents" />
                <%-- Car Classes --%>
                <uc:MappingCarClassGridview ID="MappingCarClassGridview" runat="server" Visible="false" OnGridviewCommand="ShowDependents" />
                <%-- Car Groups --%>
                <uc:MappingCarGroupGridview ID="MappingCarGroupGridview" runat="server" Visible="false" />
                <%-- Model Codes --%>
                <uc:MappingModelCodeGridview ID="MappingModelCodesGridview" runat="server" Visible="false" />
            </div>
            <%-- Delete Confirm --%>
            <uc:DeleteConfirm ID="DeleteConfirm" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
