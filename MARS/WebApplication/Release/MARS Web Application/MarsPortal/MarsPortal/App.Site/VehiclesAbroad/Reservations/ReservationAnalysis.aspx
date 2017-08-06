<%@ Page Title="Reservation Analysis" Theme="MarsV3" Language="C#" MasterPageFile="~/App.MasterPages/Application.Master" AutoEventWireup="true" CodeBehind="ReservationAnalysis.aspx.cs" Inherits="MarsV2.VehiclesAbroad.ReservationAnalysis" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceholderHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceholderMainContent" runat="server">
    <br />
    <h2 style="text-align:left;">Vehicles Abroad - Reservation Analysis</h2>
    <br />
    <br />

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <center>
                <asp:Panel ScrollBars="Horizontal" runat="server">

                    <asp:GridView ID="GridViewReservationAnalysis" runat="server"
                        onrowdatabound="GridViewReservationAnalysis_RowDataBound" EmptyDataText="No data available">
                    </asp:GridView>
                </asp:Panel>
                <br />

                <table>
                    <tr>
                        <td>
                            <fieldset >
                                <legend>Filters : </legend>
                                <table id="filterTable" style='padding:5px;'>
                                    <tr>
                                        <td> Reservation country : </td>
                                        <td>
                                            <asp:DropDownList ID="DropDownListReservationCountry" runat="server" 
                                                AutoPostBack="true" onselectedindexchanged="updateController">
                                            </asp:DropDownList>
                                        </td>
                                        <td> Number of days : </td>
                                        <td>
                                            <asp:DropDownList ID="DropDownListNoOfDays" runat="server" AutoPostBack="true" ToolTip="To return date"
                                             onselectedindexchanged="updateController">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td> Return Country : </td>
                                        <td>
                                            <asp:DropDownList ID="DropDownListReturnCountry" runat="server" AutoPostBack="true"
                                             onselectedindexchanged="updateController">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                </table>

            </center>

        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
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
