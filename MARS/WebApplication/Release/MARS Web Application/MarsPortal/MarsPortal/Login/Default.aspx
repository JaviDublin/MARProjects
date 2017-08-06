<%@ Page Title="" Language="C#" MasterPageFile="~/App.MasterPages/Site.Master" AutoEventWireup="true" 
    CodeBehind="Default.aspx.cs" Inherits="App.Login.Default"  %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    <!--[if lte IE 7]><style type ="text/css">.rad__div-Login-Row-Right-DropDown{height: 35px; margin-left:20px; padding-left: 20px; margin-top: 20px; margin-right: 20px; margin-bottom: 20px;}</style><![endif]-->
</asp:Content>
<asp:Content ID="ContentMain" ContentPlaceHolderID="ContentPlaceHolderContent" runat="server">
    <rad:Panel ID="PanelLogOn" runat="server" CssClass="panel-Login">

        <rad:LogOn ID="LogOnControl" runat="server" StyleTextBox="true"
            LogOnApplicationLogoUrl="~/App.Images/application-logo.png"
            ImageHelpVisible="false" LogOnRememberMeChecked="False"  LogOnRememberMeVisible="false"
            OnLoggedInCommand="OnLoggedIn_Command"  />


        <div style="text-align: center; width: 600px;">
            <asp:Label runat="server" ID="lblMessage" ForeColor="Red" />
        </div>
        


            <asp:UpdatePanel runat="server" ID="upnlNoCompany" UpdateMode="Conditional">
                <ContentTemplate>
                    &nbsp;
                    <asp:Panel runat="server" ID="pnlCountryAdmin" Visible="False" ViewStateMode="Enabled">
                    <table style="width: 700px; text-align: center">
                        <tr>
                            <td>
                                <table style="width: 500px; text-align: center; margin-left: 100px;">
                                    <tr>
                                        <td colspan="2">
                                            Your Windows ID is not linked to a Company. <br/>
                                            Please contact the Administrator for your Country: 
                                            <asp:DropDownList runat="server" ID="ddlLicenseeCountries" AutoPostBack="True" Width="120px" 
                                                ViewStateMode="Enabled"
                                                OnSelectedIndexChanged="ddlLicenseeCountries_SelectionChanged" />
                                            <br/>
                                            <br/>
                                        </td>
                                    </tr>
                       
                                </table>
                            </td>
                            
                        </tr>
                        <tr>
                            <td>
                                <asp:ListView runat="server" ID="lvLicenseeAdmins" >
                                    <LayoutTemplate>
                                        <table runat="server" width="700px;" class="StandardBorder">
                                            <tr style="font-weight: bold;">
                                                <td style="width: 200px;">Name
                                                </td>
                                                <td style="width: 300px;">Email
                                                </td>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr runat="server">
                                            <td>
                                                <asp:Label ID="lblName" runat="server" Text='<%#Eval("Name") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblEmail" runat="server" Text='<%#Eval("Email") %>' />
                                            </td>
                                        </tr>

                                    </ItemTemplate>
                                </asp:ListView>
                            </td>
                        </tr>
                    </table>
                        </asp:Panel>
                </ContentTemplate>

            </asp:UpdatePanel>


        


    </rad:Panel>
</asp:Content>
