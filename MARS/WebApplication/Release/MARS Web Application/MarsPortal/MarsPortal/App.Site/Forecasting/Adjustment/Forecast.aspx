<%@ Page Title="" Language="C#" MasterPageFile="~/App.Masterpages/Application.Master" 
AutoEventWireup="true" CodeBehind="Forecast.aspx.cs" Inherits="App.Management.Forecast" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="ContentPlaceholderHead" runat="server">
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="ContentPlaceholderMainContent" runat="server">
    <asp:UpdatePanel ID="upForecast" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <center> 
                <br />
                <div class="accordianManagement">
                    <div id="accordion" style="background-color:White;">
                        <h3 class="accordianHeader">
                            <a href="#">Forecasting Adjustment Section</a>
                        </h3>
                            <div class="accordionContent">
                                <uc:ForecastAdjustment ID="forecastAdjust" runat="server" />
                            </div>
                        <h3 class="accordianHeader">
                            <a href="#">Forecasting Reconciliation Section</a>
                        </h3>
                            <div class="accordionContent">
                                <uc:ForecastReconciliation ID="forecastRecon" runat="server" />
                            </div>
                    </div>
                </div>
        
            </center>

            <asp:HiddenField ID="hidAccordionIndex" runat="server" Value="false" />

            <script language="javascript" type="text/javascript">

                $(function () {
                    var intVal = $('#<%=hidAccordionIndex.ClientID %>').val();
                    var activeIndex = null;

                    if (intVal != "false")
                        activeIndex = parseInt(intVal);
                    else
                        activeIndex = false;

                    $("#accordion").accordion({
                        event: "mousedown",
                        active: activeIndex,
                        clearStyle: true,
                        autoheight: false,
                        collapsible: true,
                        change: function (event, ui) {
                            var index = $(this).children('h3').index(ui.newHeader);
                            $('#<%=hidAccordionIndex.ClientID %>').val(index);
                        }
                    });
                });
            </script>
        </ContentTemplate>       
    </asp:UpdatePanel>

    <asp:UpdateProgress runat="server" ID="upLoading" AssociatedUpdatePanelID="upForecast" DisplayAfter="1000">
        <ProgressTemplate>
            <uc:LoadingScreen ID="clsLoadingScreen" runat="server" ShowGenericPleaseWait="True" />
        </ProgressTemplate>
    </asp:UpdateProgress>

</asp:Content>
