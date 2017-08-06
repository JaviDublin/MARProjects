<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FleetSize.aspx.cs" 
MasterPageFile="~/App.Masterpages/Application.Master" Inherits="App.Management.FleetSize" %>
<asp:Content ID="HeaderContent" ContentPlaceHolderID="ContentPlaceholderHead" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="ContentPlaceholderMainContent" runat="server">

    <asp:UpdatePanel ID="upFleetSize" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <center> 
            <br />
                <div class="accordianManagement">
                    <div id="accordion" style="background-color:White;">
                        <h3 class="accordianHeader">
                            <a href="#">Manual Movements Section</a>
                        </h3>
                            <div class="accordionContent">
                                <uc:ManualMovementTab ID="mmTab" runat="server" FutureDatesOnly="True" />
                            </div>
                        <h3 class="accordianHeader">
                            <a href="#">Fleet Plan Section</a>
                        </h3>
                            <div class="accordionContent">
                                <div>
                                    <uc:ManualMovementUpload id="mmUpload" runat="server" />
                                </div>
                             </div>
                        <h3 class="accordianHeader">
                            <a href="#">Frozen Section</a>
                        </h3>
                            <div class="accordionContent">
                                <uc:FrozenZone ID="fzApproval" runat="server" />
                            </div>
                        <h3 class="accordianHeader">
                            <a href="#">Necessary Fleet Section</a>
                        </h3>
                            <div class="accordionContent" style="height:350px;">
                                <uc:NecessaryFleet ID="necFleet" runat="server" />
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

                function ExportHideUpdateProcess() {
                    setTimeout("HideUpdateProcess()", 3000);
                }

                function HideUpdateProcess() {
                    var updproc = $('#<%=upLoading.ClientID %>');
                    $(updproc).css('display','none');
                }
            </script>

        </ContentTemplate>       
    </asp:UpdatePanel>

    <asp:UpdateProgress runat="server" ID="upLoading" AssociatedUpdatePanelID="upFleetSize" DisplayAfter="1000">
        <ProgressTemplate>
            <div>
                <uc:LoadingScreen ID="clsLoadingScreen" runat="server" ShowGenericPleaseWait="True" />
            </div>            
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>


