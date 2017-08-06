<%@ Page Title="Fleet Overview" Theme="MarsV3" MasterPageFile="~/App.Masterpages/Application.master" Language="C#" AutoEventWireup="true" CodeBehind="FleetOverview.aspx.cs" Inherits="App.Site.VehiclesAbroad.Overview.FleetOverview2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceholderHead" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceholderMainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>  
            <center>
            <br />
                <table cellpadding="4" width="98%">
                    <tr>
                        <td colspan="4"style="padding:10px; font-size: large; font-weight: bold;">Vehicles Abroad - Fleet Overview</td>
                    </tr>
                    <tr><td colspan="8"><hr /></td></tr>
                    <tr>
                        <td>Vehicle Predicament : </td>
                        <td><asp:Label ID="labelVehiclePredicament" runat="server"></asp:Label></td>
                        <td>Destination Country : </td>
                        <td><asp:Label ID="labelDueCountry" runat="server"></asp:Label></td>
                        <td>Owning Country : </td>
                        <td><asp:Label ID="labelOwnCounty" runat="server"></asp:Label></td>
                        <td>Car Segment : </td>
                        <td><asp:Label ID="labelCarSegment" runat="server"></asp:Label></td>                        
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td>Pool : </td>
                        <td><asp:Label ID="labelPool" runat="server"></asp:Label></td>
                        <td></td>
                        <td></td>
                        <td>Car Class : </td>
                        <td><asp:Label ID="labelCarClass" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td>Location Group : </td>
                        <td><asp:Label ID="labelLocationGroup" runat="server"></asp:Label></td>
                        <td></td>
                        <td></td>
                        <td>Car Group : </td>
                        <td><asp:Label ID="labelCarGroup" runat="server"></asp:Label></td>
                    </tr>
                </table>
                <div id="DataTableFleetOverview" runat="server">Loading data...</div>            
                <%--Container table--%>
                <table cellpadding="4" width="98%">
                    <tr>
                        <td>
                            <fieldset>   
                                <legend>Specialised Filters </legend>
                                <table width="98%">
                                    <tr>
                                        <td>Vehicle predicament : </td>
                                        <td>
                                            <asp:DropDownList ID="DropDownListVehiclePredicament" runat="server" 
                                                AutoPostBack="True" Width="240px"
                                                onselectedindexchanged="UpdateController">
                                            </asp:DropDownList>
                                        </td>
                                        <td>Destination Country : </td>
                                        <td>
                                            <asp:DropDownList ID="DropDownListDueCountry" runat="server" 
                                                AutoPostBack="True" 
                                                onselectedindexchanged="DropDownListDueCountry_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>                                        
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td>Pool : </td>
                                        <td>
                                            <asp:DropDownList ID="DropDownListPool" runat="server" AutoPostBack="True" 
                                                onselectedindexchanged="DropDownListPool_SelectedIndexChanged">
                                            </asp:DropDownList>                                                
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td>Location Group : </td>
                                        <td>
                                            <asp:DropDownList ID="DropDownListLocationGroup" runat="server" 
                                                AutoPostBack="True" 
                                                onselectedindexchanged="UpdateController">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                        <td>
                            <fieldset>   
                                <legend>General Filters:</legend>
                                <table width="98%" cellpadding="2">                            
                                    <tr>
                                        <td>Owning Country : </td>
                                        <td>
                                            <asp:DropDownList ID="DropDownListOwnCountry" runat="server" 
                                                AutoPostBack="True" 
                                                onselectedindexchanged="DropDownListOwnCountry_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>Car Segment : </td>
                                        <td>
                                            <asp:DropDownList ID="DropDownListCarSegment" runat="server" 
                                                AutoPostBack="True" 
                                                onselectedindexchanged="DropDownListCarSegment_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td>Car Class : </td>
                                        <td>
                                            <asp:DropDownList ID="DropDownListCarClass" runat="server" AutoPostBack="True" 
                                                onselectedindexchanged="DropDownListCarClass_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td>Car Group</td>
                                        <td>
                                            <asp:DropDownList ID="DropDownListCarGroup" runat="server" AutoPostBack="True" 
                                                onselectedindexchanged="UpdateController">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>  
                            </fieldset>
                        </td>
                    </tr>      
                </table>
                <%--end of containing table --%>  
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>

        <asp:UpdateProgress ID="UpdateProgress1" runat="server" ClientIDMode="Static" DisplayAfter="2000">
        <ProgressTemplate>
            <asp:Panel ID="PanelBackgroundCover" runat="server" SkinID="backgroundCover"></asp:Panel>
            <asp:Panel ID="PanelLoadData" runat="server" SkinID="loadData">
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;
                Loading Data.....
                <br />
                <asp:Image ID="Image1" runat="server" SkinID="loadDataImage" />
            </asp:Panel>
        </ProgressTemplate>        
    </asp:UpdateProgress>

</asp:Content>
