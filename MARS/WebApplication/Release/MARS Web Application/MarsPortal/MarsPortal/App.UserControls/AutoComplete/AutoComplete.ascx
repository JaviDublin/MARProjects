<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="AutoComplete.ascx.cs" Inherits="App.UserControls.AutoComplete.AutoComplete" %>
 <asp:UpdatePanel runat="server" ID="UpdatePanelAutocomplete" UpdateMode="Conditional" ChildrenAsTriggers="true">
<ContentTemplate>
<rad:AutoComplete ID="AutoCompleteControl" runat="server"  
WebServiceUrl="~/App.Webservices/SearchTerm.asmx/SearchTermSerial" 
AutoPostBack="true" OnAutoCompleteCommand="OnAutoCompleteCommand" CssClass="dropDownListFormSettings" />
</ContentTemplate>
</asp:UpdatePanel>