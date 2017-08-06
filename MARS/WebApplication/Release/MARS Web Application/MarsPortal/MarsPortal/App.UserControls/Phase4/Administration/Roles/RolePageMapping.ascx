<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RolePageMapping.ascx.cs" 
        Inherits="Mars.App.UserControls.Phase4.Administration.Roles.RolePageMapping" %>

<table>
    <tr>
        <td>
            <asp:RadioButtonList runat="server" ID="rblCompanyTypes" RepeatDirection="Horizontal" 
                OnSelectedIndexChanged="rblCompanyTypes_SelectionChanged" AutoPostBack="True"
                Width="185px"/>
                
            
        </td>
    </tr>
    <tr style="vertical-align: top;">
        
        <td>
            <asp:ListBox runat="server" ID="lbRoles" Width="185px" Height="300px" AutoPostBack="True"
                OnSelectedIndexChanged="lbRoles_Selected" />
            
        </td>
        <td>
            <asp:Panel ID="pnlRolesAssignment" runat="server" ScrollBars="Both" Height="500" Width="600">
                <asp:Repeater ID="rptPages" runat="server" OnItemCommand="RepeaterCommand">
                    <HeaderTemplate>
                        <table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Label runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "PageName") %>'
                                    ForeColor='<%# DataBinder.Eval(Container.DataItem, "ForeColour") %>'
                                    Visible=' <%#Eval("IsBranch") %>' />

                            </td>
                            <td>
                                <asp:LinkButton runat="server" ID="lbBranch" Text='<%# ( (bool) Eval("Assigned")) ? "Unassign" : "Assign" %>'
                                    CommandName='<%# ( (bool) Eval("Assigned")) ? UnassignCommand : AssignCommand  %>'
                                    CommandArgument='<%#DataBinder.Eval(Container.DataItem, "UrlId") %>'
                                    Enabled=' <%#Eval("Enabled") %>'
                                    Visible=' <%#Eval("IsBranch") %>' />
                            </td>
                            <td>
                                <asp:Label runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "PageName") %>'
                                    ForeColor='<%# DataBinder.Eval(Container.DataItem, "ForeColour") %>'
                                    Visible=' <%# (bool) Eval("IsBranch") != true %>' />
                            </td>
                            <td>
                                <asp:LinkButton runat="server" ID="lbLeaf" Text='<%# ( (bool) Eval("Assigned")) ? "Unassign" : "Assign" %>'
                                    CommandName='<%# ( (bool) Eval("Assigned")) ? UnassignCommand : AssignCommand %>'
                                    CommandArgument='<%#DataBinder.Eval(Container.DataItem, "UrlId") %>'
                                    Enabled=' <%#Eval("Enabled") %>'
                                    Visible=' <%# (bool) Eval("IsBranch") != true %>' />
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </asp:Panel>
        </td>
    </tr>

</table>

<script>
    var xPos, yPos;
    var prm = Sys.WebForms.PageRequestManager.getInstance();

    function BeginRequestHandler(sender, args) {
        if ($get('<%=pnlRolesAssignment.ClientID%>') != null) {
              // Get X and Y positions of scrollbar before the partial postback
              xPos = $get('<%=pnlRolesAssignment.ClientID%>').scrollLeft;
            yPos = $get('<%=pnlRolesAssignment.ClientID%>').scrollTop;
        }
    }

    function EndRequestHandler(sender, args) {
        if ($get('<%=pnlRolesAssignment.ClientID%>') != null) {
             // Set X and Y positions back to the scrollbar
             // after partial postback
             $get('<%=pnlRolesAssignment.ClientID%>').scrollLeft = xPos;
             $get('<%=pnlRolesAssignment.ClientID%>').scrollTop = yPos;
         }
     }

     prm.add_beginRequest(BeginRequestHandler);
     prm.add_endRequest(EndRequestHandler);
</script>