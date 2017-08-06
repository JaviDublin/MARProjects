<%@ Page Title="" Language="C#" MasterPageFile="~/App.MasterPages/Application.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MarsPortal.Default" %>
<%-- Virtual Reference to Master Page so we can directly access functions / properties --%>


<%@ Register src="~/App.UserControls/Panel/CommunicationsPanel.ascx" tagname="CommunicationsPanel" tagprefix="uc" %>


<%@ MasterType VirtualPath="~/App.MasterPages/Application.Master" %>

<%-- Header Content --%>
<asp:Content ID="ContentHead" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
</asp:Content>

<%-- Main Content --%>
<asp:Content ID="ContentMain" ContentPlaceHolderID="ContentPlaceHolderMainContent" runat="server">
    

    <uc:CommunicationsPanel id="Communications" runat="server"/>
    
</asp:Content>
