<%@ Page Title="" Language="C#" MasterPageFile="~/App.MasterPages/Application.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="App.AvailabilityTool.FleetComparison.Default" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="ContentPlaceholderHead" runat="server">
</asp:Content>
<asp:Content ID="ContentMain" ContentPlaceHolderID="ContentPlaceholderMainContent" runat="server">
    <%-- Update Panel --%>
    <asp:UpdatePanel ID="UpdatePanelFleetComparison" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
        <ContentTemplate>
            <%-- Control Wapper --%>
            <div class="divControlContent">
                <%-- Page Information --%>
                <uc:PageInformation ID="UserControlPageInformation" runat="server" />
                <%-- Report Selections --%>
                <uc:ReportSelection ID="UserControlReportSelections" runat="server" />
                <%-- Chart --%>
                <uc:EmptyDataTemplate ID="EmptyDataTemplateFleetComparison" runat="server" Visible="false" />
                <uc:AvailabilityChart ID="AvailibilityChartFleetComparison" runat="server" Visible="false" />
            </div>

             <%-- Report Settings --%>
            <uc:ReportSettings ID="UserControlReportSettings" runat="server" OnGenerateReport="ButtonGenerateReport_Click" />
            <%-- Confirmation Modal Dialog --%>
            <uc:ModalConfirm ID="ModalConfirmFleetComparison" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>


        <asp:UpdateProgress runat="server" ID="upChartProgress" AssociatedUpdatePanelID="UpdatePanelFleetComparison" DisplayAfter="1000">
        <ProgressTemplate>
            <uc:LoadingScreen ID="clsLoadingScreen" runat="server" />
         </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
