<%@ Page Title="" Language="C#" MasterPageFile="~/App.MasterPages/Application.Master"
    AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="App.Site.NonRevenue.CarSearch.Default" %>

<asp:Content ID="ContentHeader" ContentPlaceHolderID="ContentPlaceholderHead" runat="server">
</asp:Content>
<asp:Content ID="ContentMain" ContentPlaceHolderID="ContentPlaceholderMainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanelCarSearchNonRev" runat="server" UpdateMode="Conditional"
        ChildrenAsTriggers="false">
        <ContentTemplate>
            <%-- Control Wapper --%>
            <div class="divControlContent">
            <fieldset>
            <legend>CAR SEARH - Non Revenue</legend>
                <table>
                <tr>
                    <td class="ColumnForm">
                    <uc:FormFilter runat="server" ID="FormFilter" OnGenerateReport="ButtonGenerateReport_Click" /> 
                    </td>
                    <td class="ColumnView" valign="top">
                    <uc:NavigationPanel runat="server" ID="NavigationPanelNonRev" OnNavigationMenuClick="NavigationMenuClick" />
                    <asp:MultiView runat="server" ID="MultiViewNonRev" ActiveViewIndex="0">
                    <asp:View runat="server" ID="List">
                   
                   

                    </asp:View>
                    <asp:View runat="server" ID="Form">
                    
                    Form Car Details

                    </asp:View>
                     <asp:View runat="server" ID="Chart">
                    
                    Chart

                    </asp:View>
                    </asp:MultiView>
                    </td>
                </tr>
             </table>
            </fieldset>

             
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress runat="server" ID="upChartProgress" AssociatedUpdatePanelID="UpdatePanelCarSearchNonRev"
        DisplayAfter="1000">
        <ProgressTemplate>
            <uc:LoadingScreen ID="clsLoadingScreen" runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
