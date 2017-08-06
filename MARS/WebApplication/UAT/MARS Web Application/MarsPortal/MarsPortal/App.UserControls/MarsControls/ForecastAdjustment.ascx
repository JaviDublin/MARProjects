<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ForecastAdjustment.ascx.cs" Inherits="App.UserControls.ForecastAdjustment" %>

<%@ Import Namespace="App.Entities" %>

<asp:UpdatePanel ID="upForecastAdjustment" runat="server" >
    <ContentTemplate>
        <asp:Panel ID="pnlAjustmentparams" runat="server" 
            CssClass="adjustmentdynamicParams" 
            meta:resourcekey="pnlAjustmentparamsResource2">
           <uc:DynamicParameters ID="dpForecastAdjustments" 
                runat="server"  
                ShowQuickLocationGroupBox="false" 
                SingleDateSelection="true"
                RootParameterPostback="true"
                NextNinetyDayOnly="true"
                GridviewDatePicker="true"/>                           
        </asp:Panel>

        <div style="overflow:scroll; overflow-x:hidden; height:350px; width:95%">
            <asp:GridView ID="grdForecastAdjustment"         
            AutoGenerateColumns="False"
            runat="server"
            AllowPaging="True"
            AllowSorting="True"
            ShowHeaderWhenEmpty="True"
            EmptyDataText="Please select a country"
            ShowFooter="True"
            PageSize="10"
            CellPadding="2"
            CellSpacing="2"
            BorderWidth="2px"
            HorizontalAlign="Center"        
            GridLines="Horizontal"            
            CssClass="mgt-GridView"
            ondatabinding="grdForecastAdjustment_DataBinding" 
            ondatabound="grdForecastAdjustment_DataBound" 
            onrowdatabound="grdForecastAdjustment_RowDataBound" 
            onpageindexchanging="grdForecastAdjustment_PageIndexChanging" 
            onsorting="grdForecastAdjustment_Sorting" 
            onrowcommand="grdForecastAdjustment_RowCommand" 
            onrowediting="grdForecastAdjustment_RowEditing" 
            meta:resourcekey="grdForecastAdjustmentResource2">
            <Columns>
                
                <asp:TemplateField AccessibleHeaderText="SelectedItem" 
                    meta:resourcekey="TemplateFieldResource21">
                    <HeaderTemplate>
                        <asp:CheckBox ID="chkSelectAll" runat="server" 
                            onclick="javascript:ToggleGridCheckBoxes(this.checked);" 
                            meta:resourcekey="chkSelectAllResource2"/>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkSelected" runat="server" 
                            onclick="javascript:HighlightRow(this);" 
                            meta:resourcekey="chkSelectedResource2"/>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:LinkButton ID="lnkEdit" runat="server" Text="Adjust" CommandName="Edit" 
                            meta:resourcekey="lnkEditResource2"></asp:LinkButton>
                        &nbsp;
                        <asp:LinkButton ID="lnkAdapt" runat="server" Text="Adapt" CommandName="Adapt" 
                            meta:resourcekey="lnkAdaptResource2"></asp:LinkButton>
                    </FooterTemplate>
                </asp:TemplateField>

                <asp:TemplateField AccessibleHeaderText="Date" 
                    meta:resourcekey="TemplateFieldResource22">
                    <HeaderTemplate>Date</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblDate" runat="server" 
                            Text='<%# ((DateTime)Eval("Date")).ToShortDateString() %>' 
                            meta:resourcekey="lblDateResource2"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                   
                 <asp:TemplateField AccessibleHeaderText="Count" 
                    meta:resourcekey="TemplateFieldResource23">
                    <HeaderTemplate>Count</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblCount" runat="server" Text='<%# Eval("Count") %>' 
                            meta:resourcekey="lblCountResource2"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                             
                <asp:TemplateField AccessibleHeaderText="PoolID" 
                    meta:resourcekey="TemplateFieldResource24">
                    <HeaderTemplate>PoolID</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblPoolID" runat="server" 
                            Text='<%# ((CMSPool)Eval("CMSPool")).PoolID %>' 
                            meta:resourcekey="lblPoolIDResource2"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField AccessibleHeaderText="PoolDescription" 
                    SortExpression="CMSPool.PoolDescription" 
                    meta:resourcekey="TemplateFieldResource25">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkPool" runat="server" Text="Pool" 
                            CommandArgument="CMSPool.PoolDescription" CommandName="sort" 
                            meta:resourcekey="lnkPoolResource2"></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblPool" runat="server" 
                            Text='<%# ((CMSPool)Eval("CMSPool")).PoolDescription %>' 
                            meta:resourcekey="lblPoolResource2"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField AccessibleHeaderText="LocationGroupID" 
                    meta:resourcekey="TemplateFieldResource26">
                    <HeaderTemplate>LocationGroupID</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblLocationGroupID" runat="server" 
                            Text='<%# ((LocationGroup)Eval("LocationGroup")).LocationGroupID %>' 
                            meta:resourcekey="lblLocationGroupIDResource2"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField AccessibleHeaderText="LocationGroupName" 
                    SortExpression="LocationGroup.LocationGroupName" 
                    meta:resourcekey="TemplateFieldResource27">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkLocationGroup" runat="server" Text="Location Group" 
                            CommandArgument="LocationGroup.LocationGroupName" CommandName="sort" 
                            meta:resourcekey="lnkLocationGroupResource2"></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblLocationGroup" runat="server" 
                            Text='<%# ((LocationGroup)Eval("LocationGroup")).LocationGroupName %>' 
                            meta:resourcekey="lblLocationGroupResource2"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField AccessibleHeaderText="CarSegmentId" 
                    meta:resourcekey="TemplateFieldResource28">
                    <HeaderTemplate>SegmentID</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblSegmentID" runat="server" 
                            Text='<%# ((CarSegment)Eval("CarSegment")).CarSegmentId %>' 
                            meta:resourcekey="lblSegmentIDResource2"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField AccessibleHeaderText="CarSegmentName" 
                    SortExpression="CarSegment.CarSegmentName" 
                    meta:resourcekey="TemplateFieldResource29">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkCarSegment" runat="server" Text="Segment" 
                            CommandArgument="CarSegment.CarSegmentName" CommandName="sort" 
                            meta:resourcekey="lnkCarSegmentResource2"></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblSegment" runat="server" 
                            Text='<%# ((CarSegment)Eval("CarSegment")).CarSegmentName %>' 
                            meta:resourcekey="lblSegmentResource2"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField AccessibleHeaderText="CarGroupID" 
                    meta:resourcekey="TemplateFieldResource30">
                    <HeaderTemplate>CarClassID</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblCarClassID" runat="server" 
                            Text='<%# ((CarClass)Eval("CarClass")).CarClassID %>' 
                            meta:resourcekey="lblCarClassIDResource2"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField AccessibleHeaderText="CarGroupDescription" 
                    SortExpression="CarClass.CarclassDescription" 
                    meta:resourcekey="TemplateFieldResource31">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkCarClass" runat="server" Text="Car Group" 
                            CommandArgument="CarClass.CarclassDescription" CommandName="sort" 
                            meta:resourcekey="lnkCarClassResource2"></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblCarClass" runat="server" 
                            Text='<%# ((CarClass)Eval("CarClass")).CarclassDescription %>' 
                            meta:resourcekey="lblCarClassResource2"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField AccessibleHeaderText="CarClassID" 
                    meta:resourcekey="TemplateFieldResource32">
                    <HeaderTemplate>CarGroupID</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblGroupID" runat="server" 
                            Text='<%# ((CarGroup)Eval("CarGroup")).CarGroupID %>' 
                            meta:resourcekey="lblGroupIDResource2"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField AccessibleHeaderText="CarclassDescription" 
                    SortExpression="CarGroup.CarGroupDescription" 
                    meta:resourcekey="TemplateFieldResource33">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkCarGroup" runat="server" Text="Car Class" 
                            CommandArgument="CarGroup.CarGroupDescription" CommandName="sort" 
                            meta:resourcekey="lnkCarGroupResource2"></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblGroup" runat="server" 
                            Text='<%# ((CarGroup)Eval("CarGroup")).CarGroupDescription %>' 
                            meta:resourcekey="lblGroupResource2"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                      
                <asp:TemplateField AccessibleHeaderText="ONRENT" 
                    meta:resourcekey="TemplateFieldResource34">
                    <HeaderTemplate>On Rent LY</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblONRENT" runat="server" Text='<%# Math.Round(Convert.ToDecimal(Eval("OnRent"))) %>' 
                            meta:resourcekey="lblONRENTResource2"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField AccessibleHeaderText="Constrained" 
                    meta:resourcekey="TemplateFieldResource35">
                    <HeaderTemplate>Constrained</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblConstrained" runat="server" Text='<%# Math.Round(Convert.ToDecimal(Eval("Constrained"))) %>' 
                            meta:resourcekey="lblConstrainedResource2"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField AccessibleHeaderText="UnConstrained" 
                    meta:resourcekey="TemplateFieldResource36">
                    <HeaderTemplate>UnConstrained</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblUnConstrained" runat="server" 
                            Text='<%# Math.Round(Convert.ToDecimal(Eval("UnConstrained"))) %>' 
                            meta:resourcekey="lblUnConstrainedResource2"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                                         
                <asp:TemplateField AccessibleHeaderText="Adjustment_TD" 
                    meta:resourcekey="TemplateFieldResource37">
                    <HeaderTemplate>TD</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblAdjustment_TD" runat="server" 
                            Text='<%# Math.Round(Convert.ToDecimal(Eval("Adjustment_TD"))) %>' 
                            meta:resourcekey="lblAdjustment_TDResource2"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField AccessibleHeaderText="Adjustment_BU1" 
                    meta:resourcekey="TemplateFieldResource38">
                    <HeaderTemplate>BU1</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblAdjustment_BU1" runat="server" 
                            Text='<%# Math.Round(Convert.ToDecimal(Eval("Adjustment_BU1"))) %>' 
                            meta:resourcekey="lblAdjustment_BU1Resource2"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField AccessibleHeaderText="Adjustment_BU2" 
                    meta:resourcekey="TemplateFieldResource39">
                    <HeaderTemplate>BU2</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblAdjustment_BU2" runat="server" 
                            Text='<%# Math.Round(Convert.ToDecimal(Eval("Adjustment_BU2"))) %>' 
                            meta:resourcekey="lblAdjustment_BU2Resource2"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField AccessibleHeaderText="Adjustment_RC" 
                    meta:resourcekey="TemplateFieldResource40">
                    <HeaderTemplate>RC</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblAdjustment_RC" runat="server" 
                            Text='<%# Math.Round(Convert.ToDecimal(Eval("Adjustment_RC"))) %>' 
                            meta:resourcekey="lblAdjustment_RCResource2"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>

            <pagerstyle backcolor="LightGray"
                        height="25px"
                        verticalalign="Top"
                        horizontalalign="Center" />

                <PagerTemplate>
                <div>   
                    <asp:DropDownList ID="ddlPageSelect" runat="server"  
                        OnSelectedIndexChanged="ddlPageSelect_SelectedIndexChanged" AutoPostBack="True" 
                        meta:resourcekey="ddlPageSelectResource2"></asp:DropDownList> 
                     [Page <%=grdForecastAdjustment.PageIndex + 1%> of <%= grdForecastAdjustment.PageCount%>] &nbsp; &nbsp;
                    <asp:Image ID="imgsep1" runat="server" ImageUrl="~/App.Images/sort-blank.gif" 
                        meta:resourcekey="imgsep1Resource2" />
                    &nbsp;
                    &nbsp;
                    <asp:ImageButton ID="imgButtonFirst" runat="server" CommandArgument="First" 
                        CommandName="Page" ImageUrl="~/App.Images/pager-first.png" ImageAlign="Bottom" 
                        meta:resourcekey="imgButtonFirstResource2" />
                    <asp:ImageButton ID="imgButtonPrevious" runat="server" CommandArgument="Prev" 
                        CommandName="Page" ImageUrl="~/App.Images/pager-previous.png" 
                        ImageAlign="Bottom" meta:resourcekey="imgButtonPreviousResource2" />                
                    [Records <%= grdForecastAdjustment.PageIndex * grdForecastAdjustment.PageSize + 1%> - <%= grdForecastAdjustment.PageIndex * grdForecastAdjustment.PageSize + grdForecastAdjustment.Rows.Count%>]
                    <asp:ImageButton ID="imgButtonNext" runat="server" CommandArgument="Next" 
                        CommandName="Page" ImageUrl="~/App.Images/pager-next.png" ImageAlign="Bottom" 
                        meta:resourcekey="imgButtonNextResource2" />                
                    <asp:ImageButton ID="imgButtonLast" runat="server" CommandArgument="Last" 
                        CommandName="Page" ImageUrl="~/App.Images/pager-last.png" ImageAlign="Bottom" 
                        meta:resourcekey="imgButtonLastResource2" />   
                    &nbsp;
                    &nbsp;
                    <asp:Image ID="imgsep2" runat="server" ImageUrl="~/App.Images/sort-blank.gif" 
                        meta:resourcekey="imgsep2Resource2" />
                    &nbsp;
                    &nbsp;
                    <asp:Label ID="lblitemsPerPage" runat="server" Text="Items per Page:" 
                        meta:resourcekey="lblitemsPerPageResource2" ></asp:Label>
               
                    <asp:DropDownList ID="ddlItemsPerPageSelector" runat="server"  
                        OnSelectedIndexChanged="ddlItemsPerPageSelector_SelectedIndexChanged" 
                        AutoPostBack="True" meta:resourcekey="ddlItemsPerPageSelectorResource2">
                        
                        <asp:ListItem Text="10" Value="10" Selected="True" 
                            meta:resourcekey="ListItemResource9"></asp:ListItem>
                        <asp:ListItem Text="20" Value="20" meta:resourcekey="ListItemResource10"></asp:ListItem>
                        <asp:ListItem Text="25" Value="25" meta:resourcekey="ListItemResource11"></asp:ListItem>
                        <asp:ListItem Text="30" Value="30" meta:resourcekey="ListItemResource12"></asp:ListItem>
                    </asp:DropDownList>
                </PagerTemplate>
                <SelectedRowStyle CssClass="SelectedRowStyle" />
            </asp:GridView>
            <asp:Button ID="btnDummy" runat="server" CssClass="hidden" 
                meta:resourcekey="btnDummyResource2" />
            <asp:Button ID="btnAdaptDummy" runat="server" CssClass="hidden" 
                meta:resourcekey="btnAdaptDummyResource2" />
        </div>
        <script language="javascript" type="text/javascript">
            function HighlightRow(chkB)
            {
                var IsChecked = chkB.checked;           
                if(IsChecked)
                {
                    chkB.parentElement.parentElement.style.backgroundColor='white';
                    chkB.parentElement.parentElement.style.color = '#5CACEE';
                }
                else
                {
                   chkB.parentElement.parentElement.style.backgroundColor='white';
                   chkB.parentElement.parentElement.style.color='black';
                }
           }

           function EditConfirmation() {
               if (!Page_ClientValidate())
                   return false;
               if (confirm("Apply changes to all selected items? \n Ckick OK to continue, Cancel to review.") == false)
                   return false;
           }

           function ToggleGridCheckBoxes(checked) {
               $('#<%=grdForecastAdjustment.ClientID %> >tbody >tr >td >input:checkbox').attr('checked', checked);
               $('#<%=grdForecastAdjustment.ClientID %> :eq(0)tr').find('input:checkbox').attr('checked', checked);
               
               if (checked) {
                   $('#<%=grdForecastAdjustment.ClientID %> :gt(0)tr').not(':last').each(function () {
                        $(this).css('background-color', 'white');
                        $(this).css('color', '#5CACEE');
                    });
                }
                else {
                    $('#<%=grdForecastAdjustment.ClientID %> :gt(0)tr').each(function () {
                        $(this).css('background-color', 'white');
                        $(this).css('color', 'black');
                    });
               }
           }

           function ClearValueField() {
               $('#<%=txtValue.ClientID %>').attr('value', '');
           }
        </script>
    


        <asp:Panel ID="pnlEditAdjustments" runat="server" CssClass="adjustmentModalPopup" 
            meta:resourcekey="pnlEditAdjustmentsResource2">
            <asp:Label ID="lblAdjustmentHeader" runat="server" 
                Text="Please select the adjustment, direction, value and operation for the selected adjustments" 
                meta:resourcekey="lblAdjustmentHeaderResource2"></asp:Label>
            <br />
            <table>
                <tr>
                    <td>                       
                         <asp:Label ID="lblAdjustmentStartDate" runat="server" Text="StartDate"></asp:Label>   
                    </td>
                    <td>                       
                         <asp:Label ID="lblAdjustmentEndDate" runat="server" Text="EndDate"></asp:Label>   
                    </td>
                    <td>
                        <asp:Label ID="lblAdjustment" runat="server" Text="Adjustment:" 
                            meta:resourcekey="lblAdjustmentResource2"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblDirection" runat="server" Text="Increase\Decrease:" 
                            meta:resourcekey="lblDirectionResource2"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblValue" runat="server" Text="Value:" 
                            meta:resourcekey="lblValueResource2"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblOperation" runat="server" Text="Operation:" 
                            meta:resourcekey="lblOperationResource2"></asp:Label>
                    </td>                    
                </tr>
                <tr>
                    <td valign="top">
                         <asp:TextBox ID="txtAdjustmentFromDate" runat="server"></asp:TextBox>             
                    </td>
                    <td valign="top">
                         <asp:TextBox ID="txtAdjustmentToDate" runat="server"></asp:TextBox>             
                    </td>
                    <td valign="top"> 
                        <asp:ListBox ID="lstAdjustments" runat="server" Height="55px" SelectionMode="Multiple"
                        meta:resourcekey="lstAdjustmentsResource" ></asp:ListBox>
                    </td>
                    <td valign="top">
                        <asp:DropDownList ID="ddlDirection" runat="server" 
                            meta:resourcekey="ddlDirectionResource2">
                            <asp:ListItem Text="Increase" Value="+" meta:resourcekey="ListItemResource13"></asp:ListItem>
                            <asp:ListItem Text="Decease" Value="-" meta:resourcekey="ListItemResource14"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td valign="top">
                        <asp:TextBox ID="txtValue" runat="server" meta:resourcekey="txtValueResource2" ></asp:TextBox>
                        <asp:CompareValidator ID="cvValue" runat="server" ControlToValidate="txtValue" ValidationGroup="Adjustment" 
                        Text="*" Operator="DataTypeCheck" Type="Double"></asp:CompareValidator>
                    </td>
                    <td valign="top">
                        <asp:DropDownList ID="ddlOperation" runat="server" 
                            meta:resourcekey="ddlOperationResource2">
                            <asp:ListItem Text="Per Cent" Value="PerCent" 
                                meta:resourcekey="ListItemResource15"></asp:ListItem>
                            <asp:ListItem Text="Amount" Value="Amount" 
                                meta:resourcekey="ListItemResource16"></asp:ListItem>
                        </asp:DropDownList>
                    </td>                    
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td>
                        <asp:Button ID="btnApplyChanges" runat="server" Text="Apply Changes" CssClass="chartbuttonoptions"
                            OnClientClick="return EditConfirmation();" onclick="btnApplyChanges_Click" ValidationGroup="Adjustment" 
                            meta:resourcekey="btnApplyChangesResource2" CausesValidation="true" />
                    </td>
                    <td>
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" 
                            CausesValidation="False" CssClass="chartbuttonoptions" 
                            OnClientClick="return ClearValueField();" meta:resourcekey="btnCancelResource2"/> 
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <asp:RangeValidator ID="rvAdjustmentStartDate" runat="server" ErrorMessage="RangeValidator" ControlToValidate="txtAdjustmentFromDate" Type="Date" ValidationGroup="Adapt"></asp:RangeValidator>
                    </td>             
                <tr>
                
            </table>
            <asp:CalendarExtender ID="ceAdjustmentFromDateExtender" runat="server" TargetControlID="txtAdjustmentFromDate" CssClass="chartPanelBackground"
                Format="dd/MM/yyyy" />
            <asp:CalendarExtender ID="ceAdjustmentToDateExtender" runat="server" TargetControlID="txtAdjustmentToDate" CssClass="chartPanelBackground"
                Format="dd/MM/yyyy" />
        </asp:Panel>

        <asp:ModalPopupExtender 
            ID="ajaxEditAdjustmentPopup" 
            runat="server"
            PopupControlID="pnlEditAdjustments"
            TargetControlID="btnDummy"
            DropShadow="True"
            CancelControlID="btnCancel" 
            BackgroundCssClass="modalBackground"
            OnCancelScript="ToggleGridCheckBoxes(false)" DynamicServicePath="" 
            Enabled="True">  
            </asp:ModalPopupExtender>

        <asp:Panel ID="pnlAdapt" runat="server" CssClass="adjustmentModalPopup" 
            meta:resourcekey="pnlAdaptResource2">
            <asp:Label ID="lblAdapt" runat="server" 
                Text="Please select the Target and Source criteria for Adapting" 
                meta:resourcekey="lblAdaptResource2"></asp:Label>
                <table>
                    <tr>
                        <td>                       
                         <asp:Label ID="lblAdaptDateFrom" runat="server" Text="StartDate"></asp:Label>   
                        </td>
                        <td>                       
                             <asp:Label ID="lblAdaptDateTo" runat="server" Text="EndDate"></asp:Label>   
                        </td>
                        <td>
                            <asp:Label ID="lblAdaptFrom" runat="server" Text="Take values from:" 
                                meta:resourcekey="lblAdaptFromResource2"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblAdaptTo" runat="server" Text="Apply values to:" 
                                meta:resourcekey="lblAdaptToResource2"></asp:Label>
                        </td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td valign="top">
                             <asp:TextBox ID="txtAdaptFromDate" runat="server"></asp:TextBox>             
                        </td>
                        <td valign="top">
                             <asp:TextBox ID="txtAdaptToDate" runat="server"></asp:TextBox>             
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlAdaptFrom" runat="server" 
                                meta:resourcekey="ddlAdaptFromResource2"></asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownlist ID="ddlAdaptTo" runat="server" 
                                meta:resourcekey="ddlAdaptToResource2"></asp:DropDownlist>
                        </td>
                        <td>
                            <asp:Button ID="btnAdaptApply" runat="server" Text="Apply Changes" CssClass="chartbuttonoptions"
                                OnClientClick="return EditConfirmation();" onclick="btnAdaptApply_Click" ValidationGroup="Adapt"
                                meta:resourcekey="btnAdaptApplyResource2"/>
                        </td>
                        <td>
                            <asp:Button ID="btnAdaptCancel" runat="server" Text="Cancel" 
                                CausesValidation="False" CssClass="chartbuttonoptions" 
                                meta:resourcekey="btnAdaptCancelResource2"/> 
                        </td>
                    </tr>
                    <tr>
                    <td colspan="6">
                        <asp:RangeValidator ID="rvAdaptFromDate" runat="server" ErrorMessage="RangeValidator" ControlToValidate="txtAdaptFromDate" Type="Date"></asp:RangeValidator>
                    </td>             
                <tr>
                </table>
                <asp:CalendarExtender ID="ceAdaptFromDateExtender" runat="server" TargetControlID="txtAdaptFromDate" CssClass="chartPanelBackground"
                    Format="dd/MM/yyyy" />
                <asp:CalendarExtender ID="ceAdaptToDateExtender" runat="server" TargetControlID="txtAdaptToDate" CssClass="chartPanelBackground"
                    Format="dd/MM/yyyy" />
        </asp:Panel>

        <asp:ModalPopupExtender 
            ID="ajaxEditAdaptPopup" 
            runat="server"
            PopupControlID="pnlAdapt"
            TargetControlID="btnAdaptDummy"
            DropShadow="True"
            CancelControlID="btnAdaptCancel" 
            BackgroundCssClass="modalBackground"
            OnCancelScript="ToggleGridCheckBoxes(false)" DynamicServicePath="" 
            Enabled="True">  
            </asp:ModalPopupExtender>

    </ContentTemplate>
</asp:UpdatePanel>