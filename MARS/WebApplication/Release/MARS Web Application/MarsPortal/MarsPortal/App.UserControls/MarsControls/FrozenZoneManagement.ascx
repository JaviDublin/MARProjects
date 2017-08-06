<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FrozenZoneManagement.ascx.cs" Inherits="App.UserControls.FrozenZoneManagement" %>
<%@ Register Src="~/App.UserControls/DatePicker/DateRangePicker.ascx" TagName="DateRange" TagPrefix="drp" %>

<asp:UpdatePanel ID="updFrozenZone" runat="server">
    <ContentTemplate>
        <center>
            <fieldset>
                <legend>Please select criteria:</legend>
                <div style="width:450px;">
                    <div style="width:100px; float:left;">
                        <asp:Label ID="lblSelectCountry" runat="server" Text="Select Country:  " 
                            meta:resourcekey="lblSelectCountryResource2"></asp:Label>
                        <br />
                        <asp:DropDownList ID="ddlCountryList" runat="server" AutoPostBack="True" 
                            onselectedindexchanged="ddlCountryList_SelectedIndexChanged" 
                            meta:resourcekey="ddlCountryListResource2"></asp:DropDownList>
                    </div>                
                    <div style="width:250px; float:left;">
                        <asp:Label ID="lblselectDatePrompt" runat="server" 
                            Text="Candidate Week for Approval: " 
                            meta:resourcekey="lblselectDatePromptResource2"></asp:Label>
                        <br />
                        <asp:Calendar ID="calWeekSelector" runat="server" BorderColor="LightCyan" 
                            BorderWidth="1px" CellPadding="1"  
                            OnDayRender="DayRenderEventHandler" 
                            onselectionchanged="calWeekSelector_SelectionChanged" 
                            UseAccessibleHeader="False" meta:resourcekey="calWeekSelectorResource2" >
                            <SelectorStyle ForeColor="Blue" />
                        </asp:Calendar>                     
                        <asp:Label ID="lblSelectedWeekNumber" runat="server" 
                            Text="No week selected for approval" 
                            meta:resourcekey="lblSelectedWeekNumberResource2"></asp:Label>
                    </div>
                    <div style="width:100px; float:left;">
                         <br />
                        <asp:Button ID="btnApproveFrozenZone" runat="server" CssClass="chartbuttonoptions"
                        Text="Approve Frozen Zone" onclick="btnApproveFrozenZone_Click" 
                             meta:resourcekey="btnApproveFrozenZoneResource2" />
                        <br />
                    </div>
                </div>
                <div>
                    <asp:Label ID="lblInfo" runat="server" ForeColor="Red" 
                        meta:resourcekey="lblInfoResource2" ></asp:Label>
                </div>                           
            </fieldset>
            <br />

            <div style="overflow:scroll; overflow-x:hidden; height:300px;">
            <asp:GridView ID="grdFrozenZone"         
            AutoGenerateColumns="False"
            runat="server"
            AllowPaging="True"
            AllowSorting="True"
            ShowHeaderWhenEmpty="True"
            EmptyDataText="No Data available"
            ShowFooter="True"
            CellPadding="2"
            CellSpacing="2"
            HorizontalAlign="Center"        
            GridLines="Horizontal" 
            onpageindexchanging="grdFrozenZone_PageIndexChanging" 
            onsorting="grdFrozenZone_Sorting"
            CssClass="mgt-GridView" ondatabound="grdFrozenZone_DataBound" 
            onprerender="grdFrozenZone_PreRender" 
                meta:resourcekey="grdFrozenZoneResource2">
            <Columns>                
            
                <asp:TemplateField SortExpression="Country" 
                    meta:resourcekey="TemplateFieldResource6">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkCountry" runat="server" Text="Country" 
                        CommandArgument="Country" CommandName="sort" 
                            meta:resourcekey="lnkCountryResource2"></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblCountry" runat="server" Text='<%# Eval("Country") %>' 
                            meta:resourcekey="lblCountryResource2"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                                  
                <asp:TemplateField SortExpression="Year" 
                    meta:resourcekey="TemplateFieldResource7">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkYear" runat="server" Text="Year" 
                        CommandArgument="Year" CommandName="sort" meta:resourcekey="lnkYearResource2"></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblYear" runat="server" Text='<%# (Eval("Year")) %>' 
                            meta:resourcekey="lblYearResource2"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField SortExpression="AcceptedWeekNumber" 
                    meta:resourcekey="TemplateFieldResource8">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkAcceptedWeekNumber" runat="server" Text="Accepted Week Number" 
                        CommandArgument="AcceptedWeekNumber" CommandName="sort" 
                            meta:resourcekey="lnkAcceptedWeekNumberResource2"></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblAcceptedWeekNumber" runat="server" 
                            Text='<%# (Eval("AcceptedWeekNumber")) %>' 
                            meta:resourcekey="lblAcceptedWeekNumberResource2"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>  

                <asp:TemplateField SortExpression="UploadedBy" 
                    meta:resourcekey="TemplateFieldResource9">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkUser" runat="server" Text="Accepted By" 
                        CommandArgument="AcceptedBy" CommandName="sort" 
                            meta:resourcekey="lnkUserResource2"></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblUser" runat="server" Text='<%# Eval("AcceptedBy") %>' 
                            meta:resourcekey="lblUserResource2"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField SortExpression="UploadedDate" 
                    meta:resourcekey="TemplateFieldResource10">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkAcceptedDate" runat="server" Text="Accepted Date" 
                        CommandArgument="AcceptedDate" CommandName="sort" 
                            meta:resourcekey="lnkAcceptedDateResource2"></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblAcceptedDate" runat="server" 
                            Text='<%# ((DateTime)Eval("AcceptedDate")).ToString() %>' 
                            meta:resourcekey="lblAcceptedDateResource2"></asp:Label>
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
                [Page <%=grdFrozenZone.PageIndex + 1%> of <%= grdFrozenZone.PageCount%>] &nbsp; &nbsp;
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
                [Records <%= grdFrozenZone.PageIndex * grdFrozenZone.PageSize + 1%> - <%= grdFrozenZone.PageIndex * grdFrozenZone.PageSize + grdFrozenZone.Rows.Count%>]
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
                    <asp:ListItem Text="3" Value="3" meta:resourcekey="ListItemResource5"></asp:ListItem>
                    <asp:ListItem Text="5" Value="5" meta:resourcekey="ListItemResource6"></asp:ListItem>
                    <asp:ListItem Text="10" Value="10" Selected="True" 
                        meta:resourcekey="ListItemResource7"></asp:ListItem>
                    <asp:ListItem Text="15" Value="15" meta:resourcekey="ListItemResource8"></asp:ListItem>
                </asp:DropDownList>
            </PagerTemplate>
        </asp:GridView>
        </div>
        </center>
    </ContentTemplate>
</asp:UpdatePanel>