<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Administration.aspx.cs"
    MasterPageFile="~/App.MasterPages/Mars.Master"
    Inherits="Mars.App.Site.NonRevenue.Administration.Administration" %>

<%@ Register Src="~/App.UserControls/Helpicon.ascx" TagName="HelpIcon" TagPrefix="uc" %>


<asp:Content ID="ContentMain" ContentPlaceHolderID="ContentPlaceholderMainContent"
    runat="server">
    <div style="position: absolute; margin-top: 2px;">
        <h1>
            ADMINISTRATION
        </h1>
    </div>
    
    <div id="tabbedPanel" style="width: 800px; margin-left: auto; margin-right: auto; text-align: center;">
        <ul>
            <li><a href="#tabs-1">Reason Maintenance</a></li>
            <li><a href="#tabs-2">Reason Upload</a></li>
        </ul>
    <table style="width: 800px; height: 400px; text-align: center; margin-left: auto; margin-right: auto;" >
        <tr>
            <td>
            
            <div id="tabs-1">
                <asp:UpdatePanel ID="upReasonMaintenance" runat="server">
                    <ContentTemplate>
                        
                        <table width="750px;"  >
                            <tr style="text-align: left;">
                                <td>
                                    <asp:Button ID="btnNewReason" runat="server" Text="New Reason" OnClick="btnNewReason_Click"
                                         CssClass="StandardButton" />
                                </td>
                                
                            </tr>
                            <tr>
                                <td>
                                    
                                    <asp:GridView ID="gvReasons" runat="server" AutoGenerateColumns="False" HorizontalAlign="Center"  Width="500px"
                                            OnRowCommand="gvReasons_Edit" CssClass="GridViewStyle">
                                        <HeaderStyle CssClass="GridHeaderStyle" />
                                        <AlternatingRowStyle CssClass="GridAlterenatingRowStyle" />
                                        <RowStyle CssClass="GridRowStyle" />
                                        <EditRowStyle CssClass="GridEditRowStyle" />
                                        <Columns>
                                            <asp:BoundField DataField="RemarkId" Visible="False" />
                                            <asp:BoundField DataField="RemarkText" HeaderText="Remark" >
                                                <ItemStyle HorizontalAlign="Left"/>
                                            </asp:BoundField>
                                            <asp:CheckBoxField DataField="IsActive" HeaderText="Active" >
                                                    <ItemStyle HorizontalAlign="Center"/>
                                            </asp:CheckBoxField>
                                            <asp:TemplateField ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbEdit" runat="server" CausesValidation="false" CommandName="EditItem"
                                                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "RemarkId")%>'
                                                                    Text="Edit"/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                        
                                </td>
                            </tr>
                        </table>
                        
                        
                        
                        <asp:HiddenField runat="server" ID="hfSelectedReason" />

                        <asp:Panel ID="pnlReasonPopup" runat="server" CssClass="modalPopup" Width="350px" Height="150px">
                            <div style="float: right; height: 30px;">
                                <asp:ImageButton runat="server" ID="ibClose" ImageUrl="~/App.Images/Icons/close.png" />
                            </div>
                            
                            <fieldset style="height: 120px; width: 100%;">
                                <legend>
                                    <asp:Label runat="server" ID="lblSummary" Text="Update Reason" />
                                </legend>
                            <table style="text-align: center; padding-top: 10px;">
                                <tr>
                                    <td>
                                        Reason:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="tbReason" Width="250px" />
                                    </td>
                                    
                                    
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr style="text-align: left;">
                                    <td>
                                        Active:
                                    </td>
                                    <td >
                                        <asp:CheckBox runat="server" ID="cbActive" />
                                    </td>
                                </tr>
                            </table>
                            <div style="height: 15px;">
                                <asp:Label runat="server" ID="lblErrorMessage" ForeColor="Red" />
                            </div>
                            <div style="float: right;">
                                <asp:Button ID="btnSaveReason" runat="server" CssClass="StandardButton" Text="Save" 
                                    OnClick="btnSaveReason_Click" />
                            </div>
                            </fieldset>
                        </asp:Panel>

                        <asp:Button ID="btnDummy" runat="server" CssClass="hidden" />
                        <asp:ModalPopupExtender
                            ID="mpeNonRevOverview"
                            runat="server"
                            PopupControlID="pnlReasonPopup"
                            TargetControlID="btnDummy"
                            DropShadow="True"
                            BackgroundCssClass="modalBackgroundGray" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>    
            <div id="tabs-2">
                <asp:UpdatePanel ID="upReasonUpload" runat="server">
                    <ContentTemplate>
                        <table width="750px;" style="text-align: left;"  >
                            <tr style="vertical-align: top;">
                                <td style="font-size: 14px; text-align: right; width: 180px;">
                                    Reasons Upload:
                                                            
                                </td>
                                <td >
                                    <uc:HelpIcon runat="server" HoverImage="~/App.Images/SampleNonRevUpload.png" />
                                    &nbsp;&nbsp;
                                </td>
                                <td>
                                    <asp:FileUpload runat="server" ID="fuReasonsUpload" Height="23px" Enabled="True"
                                        Width="365px" />
                                </td>
                                
                            </tr>
                            <tr style="text-align: center;">
                                <td colspan="3">
                                    <asp:Button ID="btnParse" runat="server" Text="Check File" OnClientClick=""
                                        CssClass="StandardButton" OnClick="btnParse_Click" />
                                </td>
                            </tr>
                            <tr style="text-align: center;">
                                <td colspan="3">
                                    <fieldset>
                                        <legend>Upload Results</legend>
                                        <asp:TextBox runat="server" ID="tbUploadResults"
                                            TextMode="MultiLine" Width="400px" Height="200px" />
                                    </fieldset>
                                </td>
                            </tr>
                            <tr style="text-align: center;">
                                <td colspan="3">
                                    <asp:Button runat="server" ID="btnSubmitUpload" CssClass="StandardButton"
                                        Text="Submit" OnClick="btnSubmitUpload_Click" />
                                </td>
                            </tr>
                        </table>



                        <asp:Panel ID="pnlConfirmModalUpload" Width="200px" CssClass="modalPopup" Height="100px" runat="server">
                            <table style="width: 100%; height: 100%; text-align: center;">
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="lblConfirmMessage" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button runat="server" ID="btnOk" CssClass="StandardButton" Text="Ok" />
                                    </td>
                                </tr>
                            </table>


                        </asp:Panel>

                        <asp:Button ID="btnDummy2" runat="server" CssClass="hidden" />
                        <asp:ModalPopupExtender
                            ID="mpeConfirmWindow"
                            runat="server"
                            PopupControlID="pnlConfirmModalUpload"
                            TargetControlID="btnDummy2"
                            DropShadow="True"
                            BackgroundCssClass="modalBackgroundGray" />

                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnParse" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>    
                    
            </td>
        </tr>
    </table>


    </div>

    <div id="FileUploading" style="display: none;">
        <asp:Panel ID="PanelBackgroundCover2" runat="server" CssClass="backgroundCover">
        </asp:Panel>
        <asp:Panel ID="PanelLoadData2" runat="server" CssClass="loadData">
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp; Loading Data.....
                <br />
            <asp:Image ID="Image2" runat="server" CssClass="loadDataImage" ImageUrl="~/App.Images/ajax-loader.gif"
                AlternateText="Please wait..." />
        </asp:Panel>
    </div>
    <script type="text/javascript">
        $(function () {
            $("#tabbedPanel").tabs({ selected: <%= SelectedTab %> });
            $(".ui-tabs-panel").css("background", "none");
        });
  

        $(document).ready(function () {

            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(PageLoaded);


            function PageLoaded(sender, args) {
                $('#FileUploading').hide();

                $('.StandardButton').click(function () {

                    $('#FileUploading').show();
                });
            };

        });

    </script>
</asp:Content>
