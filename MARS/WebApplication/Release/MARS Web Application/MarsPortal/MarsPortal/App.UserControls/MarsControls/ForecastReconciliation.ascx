<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ForecastReconciliation.ascx.cs" Inherits="App.UserControls.ForecastReconciliation" %>


<%@ Import Namespace="App.Entities" %>
<asp:UpdatePanel ID="upForecastAdjustment" runat="server" >
    <ContentTemplate>
        <asp:Panel ID="pnlReconciliationparams" runat="server" 
            CssClass="adjustmentdynamicParams" 
            meta:resourcekey="pnlReconciliationparamsResource2">
           <uc:DynamicParameters ID="dpForecastReconciliation" 
                runat="server"  
                ShowQuickLocationGroupBox="false" 
                SingleDateSelection="true"
                RootParameterPostback="true"
                NextNinetyDayOnly="true"
                GridviewDatePicker="true"/>                           
        </asp:Panel>
        
        <div style="overflow:scroll; overflow-x:hidden; height:300px;">
            <asp:GridView ID="grdForecastReconciliation"         
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
            ondatabinding="grdForecastReconciliation_DataBinding" 
            ondatabound="grdForecastReconciliation_DataBound" 
            onrowdatabound="grdForecastReconciliation_RowDataBound" 
            onpageindexchanging="grdForecastReconciliation_PageIndexChanging" 
            onsorting="grdForecastReconciliation_Sorting" 
            onrowcommand="grdForecastReconciliation_RowCommand" 
            onrowediting="grdForecastReconciliation_RowEditing" 
                meta:resourcekey="grdForecastReconciliationResource2">
            <Columns>
                <asp:TemplateField AccessibleHeaderText="Date" 
                    meta:resourcekey="TemplateFieldResource17">
                    <HeaderTemplate>Date</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblDate" runat="server" 
                            Text='<%# ((DateTime)Eval("Date")).ToShortDateString() %>' 
                            meta:resourcekey="lblDateResource2"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                   
                 <asp:TemplateField AccessibleHeaderText="Count" 
                    meta:resourcekey="TemplateFieldResource18">
                    <HeaderTemplate>Count</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblCount" runat="server" Text='<%# Eval("Count") %>' 
                            meta:resourcekey="lblCountResource2"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                             
                <asp:TemplateField AccessibleHeaderText="PoolID" 
                    meta:resourcekey="TemplateFieldResource19">
                    <HeaderTemplate>PoolID</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblPoolID" runat="server" 
                            Text='<%# ((CMSPool)Eval("CMSPool")).PoolID %>' 
                            meta:resourcekey="lblPoolIDResource2"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField AccessibleHeaderText="PoolDescription" 
                    SortExpression="CMSPool.PoolDescription" 
                    meta:resourcekey="TemplateFieldResource20">
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
                    meta:resourcekey="TemplateFieldResource21">
                    <HeaderTemplate>LocationGroupID</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblLocationGroupID" runat="server" 
                            Text='<%# ((LocationGroup)Eval("LocationGroup")).LocationGroupID %>' 
                            meta:resourcekey="lblLocationGroupIDResource2"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField AccessibleHeaderText="LocationGroupName" 
                    SortExpression="LocationGroup.LocationGroupName" 
                    meta:resourcekey="TemplateFieldResource22">
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
                    meta:resourcekey="TemplateFieldResource23">
                    <HeaderTemplate>SegmentID</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblSegmentID" runat="server" 
                            Text='<%# ((CarSegment)Eval("CarSegment")).CarSegmentId %>' 
                            meta:resourcekey="lblSegmentIDResource2"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField AccessibleHeaderText="CarSegmentName" 
                    SortExpression="CarSegment.CarSegmentName" 
                    meta:resourcekey="TemplateFieldResource24">
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
                    meta:resourcekey="TemplateFieldResource25">
                    <HeaderTemplate>CarClassID</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblCarClassID" runat="server" 
                            Text='<%# ((CarClass)Eval("CarClass")).CarClassID %>' 
                            meta:resourcekey="lblCarClassIDResource2"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField AccessibleHeaderText="CarGroupDescription" 
                    SortExpression="CarClass.CarclassDescription" 
                    meta:resourcekey="TemplateFieldResource26">
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
                    meta:resourcekey="TemplateFieldResource27">
                    <HeaderTemplate>CarGroupID</HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblGroupID" runat="server" 
                            Text='<%# ((CarGroup)Eval("CarGroup")).CarGroupID %>' 
                            meta:resourcekey="lblGroupIDResource2"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField AccessibleHeaderText="CarclassDescription" 
                    SortExpression="CarGroup.CarGroupDescription" 
                    meta:resourcekey="TemplateFieldResource28">
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
                                                               
                <asp:TemplateField AccessibleHeaderText="Adjustment_TD" 
                    meta:resourcekey="TemplateFieldResource29">
                    <HeaderTemplate>TD</HeaderTemplate>
                    <ItemTemplate>
                        <div style="width: 50%; float: left;">
                            <asp:Label ID="lblAdjustment_TD" runat="server" 
                                Text='<%# Math.Round(Convert.ToDecimal(Eval("Adjustment_TD"))) %>' 
                                meta:resourcekey="lblAdjustment_TDResource2"></asp:Label>
                        </div>
                        <div style="width: 50%; float: left;">
                            <asp:CheckBox ID="chkAdjustment_TD" runat="server" CssClass="gridCheckBox" 
                                meta:resourcekey="chkAdjustment_TDResource2"/>
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="15%" />
                </asp:TemplateField>

                <asp:TemplateField AccessibleHeaderText="Adjustment_BU1" 
                    meta:resourcekey="TemplateFieldResource30" >
                    <HeaderTemplate>BU1</HeaderTemplate>
                    <ItemTemplate>
                        <div style="width: 50%; float: left;">
                            <asp:Label ID="lblAdjustment_BU1" runat="server" 
                                Text='<%# Math.Round(Convert.ToDecimal(Eval("Adjustment_BU1"))) %>' 
                                meta:resourcekey="lblAdjustment_BU1Resource2"></asp:Label>
                        </div>
                        <div style="width: 50%; float: left;">
                            <asp:CheckBox ID="chkAdjustment_BU1" runat="server" CssClass="gridCheckBox" 
                                meta:resourcekey="chkAdjustment_BU1Resource2"/>
                        </div>
                        </ItemTemplate>
                    <ItemStyle Width="15%" />
                </asp:TemplateField>

                <asp:TemplateField AccessibleHeaderText="Adjustment_BU2" 
                    meta:resourcekey="TemplateFieldResource31">
                    <HeaderTemplate>BU2</HeaderTemplate>
                    <ItemTemplate>
                        <div style="width: 50%; float: left;">
                            <asp:Label ID="lblAdjustment_BU2" runat="server" 
                                Text='<%# Math.Round(Convert.ToDecimal(Eval("Adjustment_BU2"))) %>' 
                                meta:resourcekey="lblAdjustment_BU2Resource2"></asp:Label>
                        </div>
                        <div style="width: 50%; float: left;">
                            <asp:CheckBox ID="chkAdjustment_BU2" runat="server" CssClass="gridCheckBox" 
                                meta:resourcekey="chkAdjustment_BU2Resource2" />
                        </div>
                    </ItemTemplate>
                    <ItemStyle Width="15%" />
                </asp:TemplateField>

                <asp:TemplateField AccessibleHeaderText="Adjustment_RC" 
                    meta:resourcekey="TemplateFieldResource32">
                    <HeaderTemplate>Current RC</HeaderTemplate>
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
                    [Page <%=grdForecastReconciliation.PageIndex + 1%> of <%= grdForecastReconciliation.PageCount%>]  &nbsp; &nbsp;
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
                    [Records <%= grdForecastReconciliation.PageIndex * grdForecastReconciliation.PageSize + 1%> - <%= grdForecastReconciliation.PageIndex * grdForecastReconciliation.PageSize + grdForecastReconciliation.Rows.Count%>]
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
                        
                        <asp:ListItem Text="10" Value="0" Selected="True" 
                            meta:resourcekey="ListItemResource5"></asp:ListItem>
                        <asp:ListItem Text="20" Value="20" meta:resourcekey="ListItemResource6"></asp:ListItem>
                        <asp:ListItem Text="25" Value="25" meta:resourcekey="ListItemResource7"></asp:ListItem>
                        <asp:ListItem Text="30" Value="30" meta:resourcekey="ListItemResource8"></asp:ListItem>
                    </asp:DropDownList>
                </PagerTemplate>
                <SelectedRowStyle CssClass="SelectedRowStyle" />
            </asp:GridView>
           
        </div>
         <div style="float:right; border-color:Black; border-width:1px; border-spacing:1px;">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblReconcileDateFrom" runat="server" Text="Start Date"></asp:Label>   
                        </td>
                        <td>
                            <asp:Label ID="lblReconcileDateTo" runat="server" Text="End Date"></asp:Label>
                        </td>
                        <td>                        
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtReconcileFromDate" runat="server"></asp:TextBox>    
                            <asp:RangeValidator ID="rvReconcileFromDate" 
                                runat="server" ErrorMessage="Invalid start date" 
                                ControlToValidate="txtReconcileFromDate" Type="Date"
                                ValidationGroup="Reconcile"></asp:RangeValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtReconcileToDate" runat="server"></asp:TextBox> 
                            <asp:RangeValidator ID="rvReconcileToDate" 
                                runat="server" ErrorMessage="Invalid end date" 
                                ControlToValidate="txtReconcileToDate" Type="Date"
                                ValidationGroup="Reconcile"></asp:RangeValidator> 
                        </td>
                        <td>    
                            <asp:Button ID="btnReconcile" runat="server" Text="Reconcile" 
                                CssClass="chartbuttonoptions" onclick="btnReconcile_Click" 
                                meta:resourcekey="btnReconcileResource2" ValidationGroup="Reconcile"/>                   
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:ValidationSummary ID="vsReconcile" 
                            runat="server" 
                            HeaderText="Dates must be within tomorrow and the next 90 days" 
                            ValidationGroup="Reconcile"/>                             
                        </td> 
                    </tr>
                    
                </table>
                <asp:CalendarExtender ID="ceReconcileFromDateExtender" 
                    runat="server" TargetControlID="txtReconcileFromDate"
                     CssClass="chartPanelBackground" Format="dd/MM/yyyy" PopupPosition="TopLeft" />
                <asp:CalendarExtender ID="ceReconcileToDateExtender" 
                    runat="server" TargetControlID="txtReconcileToDate" 
                    CssClass="chartPanelBackground" Format="dd/MM/yyyy" PopupPosition="TopLeft" />
            </div>
        <script type="text/javascript">

            $('.gridCheckBox').find('input:checkbox').live('click', function (e) {
                var tr = $(this).closest('tr');
                var newCheckValue = this.checked;
                tr.find('input:checkbox').attr('checked', false);
                this.checked = newCheckValue;
            });
        </script>

    </ContentTemplate>
</asp:UpdatePanel>