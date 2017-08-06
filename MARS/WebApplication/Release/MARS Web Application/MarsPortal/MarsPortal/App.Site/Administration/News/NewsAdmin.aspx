<%@ Page Title="News Administration" Language="C#" MasterPageFile="~/App.MasterPages/Application.Master"
    AutoEventWireup="true" CodeBehind="NewsAdmin.aspx.cs" Inherits="Mars.App.Site.Administration.News.NewsAdmin" %>

<%-- Main Content --%>
<asp:Content ID="ContentMain" ContentPlaceHolderID="ContentPlaceHolderMainContent"
    runat="server">
    <div class="divTitle">
        <br />
        <asp:Label ID="lblHome" runat="server" CssClass="labelPageTitle" Text="NEWS ADMINISTRATION"></asp:Label>
    </div>
    <br />
    <br />
    <br />
    <br />

    <div class="divControlContent">
    <asp:Table ID="tblNews" runat="server">
        <asp:TableRow runat="server">
            <asp:TableCell ID="TableCell1" runat="server" Width="100px"></asp:TableCell>
            <asp:TableCell ID="TableCell2" runat="server">
                <asp:Button ID="btnAddNews" runat="server" Text="Add News Item" OnClick="btnAddNews_Click" width = "150px"/>
            </asp:TableCell>
            <asp:TableCell ID="TableCell3" runat="server" Width="100px"></asp:TableCell>
        </asp:TableRow>
        
        <asp:TableRow runat="server">
            <asp:TableCell runat="server" Width="100px"></asp:TableCell>
            <asp:TableCell runat="server">
                <asp:Panel ID="pnlAddNews" runat="server" GroupingText="Add News" Font-Size ="Small" >
                    <asp:HiddenField ID="hfNewsID" runat="server" />
                    <br />
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Heading: "></asp:Label>
                            </td>
                            <td  colspan="2">
                                <asp:TextBox ID="txtHeading" runat="server" MaxLength="250" Width="500px" TabIndex="1" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Details:"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtDetails" runat="server" MaxLength="1000" TextMode="MultiLine"
                                    Width="500px" TabIndex="2" Height="100" Font-Names="Arial"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Show News Item: "></asp:Label>
                            </td>
                            <td class="style1" colspan="2">
                                <asp:RadioButtonList ID="rblIsActive" runat="server" RepeatDirection="Horizontal"
                                    TabIndex="3">
                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                    <asp:ListItem Value="0">No</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="Show as Priority:"></asp:Label>
                            </td>
                            <td class="style1" colspan="2">
                                <asp:RadioButtonList ID="rblIsPriority" runat="server" RepeatDirection="Horizontal"
                                    TabIndex="4">
                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                    <asp:ListItem Value="0">No</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr><td>  <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                        </tr>
                        <tr>
                            <td>
                               
                            </td>
                            <td align="right">
                                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" TabIndex="5" Text="Save" Width ="80px" />
                              <br />
                            </td>
                            <td align = "right">
                                <asp:Button ID="btnCancelNews" runat="server" OnClick="btnCancelNews_Click" Text="Cancel" width ="80px"/>
                            </td>
                        </tr>
                    </table>
                  
                </asp:Panel>
            </asp:TableCell>
            <asp:TableCell runat="server" Width="100px"></asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <br />
    <br />
      
    <br />

    <asp:Table ID="tblGridNews" runat="server"  >
        <asp:TableRow ID="rowHeading" runat="server">
            <asp:TableCell runat="server" Width="100px"></asp:TableCell>
            <asp:TableCell runat="server">
                <div class="divHeading" style="font-size: medium; font-weight: bold; color: #808080;">
                    <asp:Label ID="lblPreviousNews" runat="server" Text="News Items"></asp:Label>
                </div>
            </asp:TableCell>
            <asp:TableCell runat="server"></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow runat="server">
            <asp:TableCell runat="server" Width="100px"></asp:TableCell>
            <asp:TableCell runat="server">
                <asp:GridView ID="grdNews" runat="server" AutoGenerateColumns="False" OnRowCommand="grdNewsEdit" CssClass="GridViewStyle">
                    <HeaderStyle CssClass="GridHeaderStyle" />
                                    <AlternatingRowStyle CssClass="GridAlterenatingRowStyle" />
                                    <RowStyle CssClass="GridRowStyle" />
                                    <EditRowStyle CssClass="GridEditRowStyle" />
                    <Columns>
                        <asp:BoundField DataField="CommunicationsID" HeaderText="News ID" />
                        <asp:BoundField DataField="CommDate" HeaderText="Date" DataFormatString="{0:d}" />
                        <asp:BoundField DataField="UpdatedBy" HeaderText="Updated By " />
                        <asp:BoundField DataField="Heading" HeaderText="News Heading" />
                        <asp:BoundField DataField="Details" HeaderText="News Details" />
                        <asp:CheckBoxField DataField="IsActive" HeaderText="Show Story" />
                        <asp:CheckBoxField DataField="Priority" HeaderText="Priority" />
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbEdit" runat="server" CausesValidation="false" CommandName="EditItem"
                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CommunicationsID")%>'
                                    Text="Edit"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                          <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbDelete" runat="server" CausesValidation="false" CommandName="DeleteItem"
                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CommunicationsID")%>'
                                    Text="Delete"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:TableCell>
            <asp:TableCell runat="server" Width="100px"></asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    </div>
</asp:Content>
