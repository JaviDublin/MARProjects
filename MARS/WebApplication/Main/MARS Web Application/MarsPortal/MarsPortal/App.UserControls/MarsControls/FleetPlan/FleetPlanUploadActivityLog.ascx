<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FleetPlanUploadActivityLog.ascx.cs" Inherits="App.UserControls.FleetPlanUploadActivityLog" %>
<%@ Register Src="~/App.UserControls/DatePicker/DateRangePicker.ascx" TagName="DateRange" TagPrefix="drp" %>

<asp:UpdatePanel ID="upActivity" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <center>
            <fieldset>
                <legend>Upload Activity log:</legend>
                    
                <asp:GridView ID="grdfleetPlanActivity"         
                AutoGenerateColumns="False"
                runat="server"
                AllowSorting="True"
                ShowHeaderWhenEmpty="True"
                EmptyDataText="Please select a country"
                ShowFooter="True"
                CellPadding="2"
                CellSpacing="2"
                HorizontalAlign="Center"        
                GridLines="Horizontal" 
                onsorting="grdfleetPlanActivity_Sorting"
                CssClass="mgt-GridView" 
                meta:resourcekey="grdfleetPlanActivityResource2">
                <Columns>                
            
                    <asp:TemplateField SortExpression="Country" 
                        meta:resourcekey="TemplateFieldResource8">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkCountry" runat="server" Text="Country" 
                            CommandArgument="Country" CommandName="sort" 
                                meta:resourcekey="lnkCountryResource2"></asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblScenarioID" runat="server" Text='<%# Eval("Country") %>' 
                                meta:resourcekey="lblScenarioIDResource2"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                                  
                    <asp:TemplateField SortExpression="FleetPlan" 
                        meta:resourcekey="TemplateFieldResource9">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkFleetPlan" runat="server" Text="FleetPlan" 
                            CommandArgument="FleetPlan" CommandName="sort" 
                                meta:resourcekey="lnkFleetPlanResource2"></asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblFleetPlan" runat="server" Text='<%# Eval("FleetPlan") %>' 
                                meta:resourcekey="lblFleetPlanResource2"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField SortExpression="IsAddition" 
                        meta:resourcekey="TemplateFieldResource10">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkIsAddition" runat="server" Text="Plan Type" 
                            CommandArgument="IsAddition" CommandName="sort" 
                                meta:resourcekey="lnkIsAdditionResource2"></asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblIsAddition" runat="server" Text='<%# Convert.ToBoolean(Eval("IsAddition")) ? "Addition" : "Deletion" %>' 
                                meta:resourcekey="lblIsAdditionResource2"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField SortExpression="UploadedBy" 
                        meta:resourcekey="TemplateFieldResource11">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkUser" runat="server" Text="Uploaded By" 
                            CommandArgument="UploadedBy" CommandName="sort" 
                                meta:resourcekey="lnkUserResource2"></asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblUser" runat="server" Text='<%# Eval("UploadedBy") %>' 
                                meta:resourcekey="lblUserResource2"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField SortExpression="UploadedDate" 
                        meta:resourcekey="TemplateFieldResource12">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkUploadDate" runat="server" Text="Upload Date" 
                            CommandArgument="UploadedDate" CommandName="sort" 
                                meta:resourcekey="lnkUploadDateResource2"></asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblUploadDate" runat="server" 
                                Text='<%# ((DateTime)Eval("UploadedDate")).ToShortDateString() %>' 
                                meta:resourcekey="lblUploadDateResource2"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>                                                    
                </Columns>
            </asp:GridView>
            </fieldset>
        </center>
    </ContentTemplate>
</asp:UpdatePanel>
