<%@ Page Title="" Language="C#" MasterPageFile="~/App.MasterPages/Application.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="App.AvailabilityTool.KPIDownload.Default" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="ContentPlaceholderHead" runat="server">
   
</asp:Content>
<asp:Content ID="ContentMain" ContentPlaceHolderID="ContentPlaceholderMainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanelKPIDownload" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
    <ContentTemplate >
        <%-- Control Wapper --%>
        <div class="divControlContent">
            <%-- Page Information --%>
            <uc:PageInformation ID="UserControlPageInformation" runat="server" />
            <%-- Report Selections --%>
            <uc:ReportSelection ID="UserControlReportSelections" runat="server" />
            <%-- Chart --%>
            <uc:AvailabilityChart ID="AvailabilityChartKPIDownloadVehicleStatus" runat="server" Visible="false" />
            <uc:AvailabilityChart ID="AvailabilityChartKPIDownloadIdleUnitsOnPeak" runat="server" Visible="false" />
            <uc:AvailabilityChart ID="AvailabilityChartKPIDownloadOperationalUtilization" runat="server" Visible="false" />
        </div>

           <%-- Report Settings --%>
           <uc:ReportSettings ID="UserControlReportSettings" runat="server" OnDownloadReport="ButtonDownloadReport_Click" />
           <%-- Confirmation Modal Dialog --%>
           <uc:ModalConfirm ID="ModalConfirmKPIDownload" runat="server" />

        </ContentTemplate>
    </asp:UpdatePanel>


        <asp:UpdateProgress runat="server" ID="upChartProgress" AssociatedUpdatePanelID="UpdatePanelKPIDownload" DisplayAfter="1000">
            <ProgressTemplate>
                <uc:LoadingScreen ID="clsLoadingScreen" runat="server" ShowGenericPleaseWait="true" />
            </ProgressTemplate>
        </asp:UpdateProgress>

        <script type="text/javascript" language="javascript">

                var prm = Sys.WebForms.PageRequestManager.getInstance();
                prm.add_endRequest(EndRequest);

                function EndRequest(sender, e) {
                    if (sender._postBackSettings.sourceElement.id == $('.MarsV1GenerateReportButton').attr('id')) {
                        // Create an IFRAME.
                        var iframe = document.createElement("iframe");
                        // Point the IFRAME to DownloadFile
                        iframe.src = "DownloadFile.aspx";
                        // This makes the IFRAME invisible to the user.
                        iframe.style.display = "none";
                        // Add the IFRAME to the page.  This will trigger request to save file
                        document.body.appendChild(iframe);
                    }

                }
        
        </script>
 


</asp:Content>
