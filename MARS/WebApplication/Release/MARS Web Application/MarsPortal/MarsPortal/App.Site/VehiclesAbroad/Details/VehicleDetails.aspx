<%@ Page Title="Vehicle Details" Theme="MarsV3" Language="C#" AutoEventWireup="true" MasterPageFile="~/App.MasterPages/Application.Master" CodeBehind="VehicleDetails.aspx.cs" Inherits="MarsV2.VehiclesAbroad.VehicleDetails2" %>

<%@ Register src="~/App.UserControls/MarsControls/VehiclesAbroadDetails.ascx" tagname="VehiclesAbroadDetails" tagprefix="uc1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceholderHead" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceholderMainContent" runat="server">
    <center>        
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table cellpadding="4" width="98%">
                    <tr>
                        <td colspan="4"style="padding:10px; font-size: large; font-weight: bold;">Vehicles Abroad - Vehicle Details</td>
                    </tr>
                    <tr><td colspan="10"><hr /></td></tr>
                    <tr>
                        <td>Vehicle Predicament : </td>
                        <td><asp:Label ID="labelVehiclePredicament" runat="server"></asp:Label></td>
                        <td></td>
                        <td></td>
                        <td>Destination Country : </td>
                        <td><asp:Label ID="labelDueCountry" runat="server"></asp:Label></td>
                        <td>Owning Country : </td>
                        <td><asp:Label ID="labelOwnCounty" runat="server"></asp:Label></td>
                        <td>Car Segment : </td>
                        <td><asp:Label ID="labelCarSegment" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>Non-Rev</td>
                        <td><asp:Label ID="labelNonRev" runat="server"></asp:Label></td>
                        <td>Operstat : </td>
                        <td><asp:Label ID="labelOperstat" runat="server"></asp:Label></td>
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
                        <td>Move-Type : </td>
                        <td><asp:Label ID="labelMoveType" runat="server"></asp:Label></td>
                        <td>Location Group : </td>
                        <td><asp:Label ID="labelLocationGroup" runat="server"></asp:Label></td>
                        <td></td>
                        <td></td>
                        <td>Car Group : </td>
                        <td><asp:Label ID="labelCarGroup" runat="server"></asp:Label></td>
                    </tr>
                </table>
                <asp:GridView ID="GridViewVehicleDetails" runat="server" AutoGenerateSelectButton="true"
                SelectedIndex="0" EmptyDataText = "No data available" AllowPaging="true" 
                    AllowSorting="true" 
                    onselectedindexchanging="GridViewVehicleDetails_SelectedIndexChanging" 
                    onpageindexchanging="GridViewVehicleDetails_PageIndexChanging" 
                    onsorting="GridViewVehicleDetails_Sorting">
                <PagerSettings FirstPageImageUrl="~/App.Images/pager-first.png" 
                    LastPageImageUrl="~/App.Images/pager-last.png" Mode="NextPreviousFirstLast"
                    NextPageImageUrl="~/App.Images/pager-next.png"  
                    PreviousPageImageUrl="~/App.Images/pager-previous.png" />
                <PagerStyle  HorizontalAlign="Right" />
                </asp:GridView>
                <i>
                    You are viewing page&nbsp;
                    <%=GridViewVehicleDetails.PageIndex + 1%>
                    &nbsp;of&nbsp;
                    <%=GridViewVehicleDetails.PageCount%>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    Maximum number of rows:&nbsp;
                    <asp:DropDownList ID="DropDownListPagerMaxRows" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="UpdateController">
                    </asp:DropDownList>
                </i>
                <%--Container table--%>
                <table width="98%">
                    <tr>
                        <td>
                            <fieldset>   
                                <legend>Specialised Filters:</legend>
                                <table width="98%" cellpadding="10">
                                    <tr>
                                        <td>Vehicle predicament : </td>
                                        <td>
                                            <asp:DropDownList ID="DropDownListVehiclePredicament" runat="server" 
                                                Width="240px" AutoPostBack="True" 
                                                onselectedindexchanged="UpdateController">
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td>Destination Country</td>
                                        <td>    
                                            <asp:DropDownList ID="DropDownListDueCountry" runat="server" 
                                                AutoPostBack="True" 
                                                onselectedindexchanged="DropDownListDueCountry_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>                                        
                                    </tr>
                                    <tr>
                                        <td>Non-Rev</td>
                                        <td>
                                            <asp:DropDownList ID="dropDownListNonRev" runat="server" AutoPostBack="true" 
                                                onselectedindexchanged="UpdateController">
                                            </asp:DropDownList>
                                        </td>
                                        <td>Operstat</td>
                                        <td>
                                            <asp:DropDownList ID="DropDownListOperstat" runat="server" AutoPostBack="true" 
                                                onselectedindexchanged="UpdateController">
                                            </asp:DropDownList>
                                        </td>
                                        <td>Pool</td>
                                        <td>
                                            <asp:DropDownList ID="DropDownListPool" runat="server" AutoPostBack="True" 
                                                onselectedindexchanged="DropDownListPool_SelectedIndexChanged">
                                            </asp:DropDownList>                                                
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td>Move-Type</td>
                                        <td>
                                            <asp:DropDownList ID="DropDownListMoveType" runat="server" AutoPostBack="true" 
                                                onselectedindexchanged="UpdateController">
                                            </asp:DropDownList>
                                        </td>
                                        <td>Location Group</td>
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
                                <table width="98%" cellpadding="10">                            
                                    <tr>
                                        <td>
                                            Owning Country
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DropDownListOwnCountry" runat="server" 
                                                AutoPostBack="True" 
                                                onselectedindexchanged="DropDownListOwnCountry_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>Car Segment</td>
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
                                        <td>Car Class</td>
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

                <uc1:VehiclesAbroadDetails ID="VehiclesAbroadDetailsUserControl" runat="server" />

                <div id="carFilters" class="divReportSettingsWrapper">
                    <div style="text-align:left;">                    
                        Vehicle Filters:</div>
                    <hr />
                    <table width="98%" cellpadding="10px">
                        <tr>
                            <td>Unit:</td>
                            <td><asp:TextBox ID="TextBoxUnit" runat="server"></asp:TextBox></td>
                            <td>License:</td>
                            <td><asp:TextBox ID="TextBoxLicense" runat="server"></asp:TextBox></td>
                            <td>Model:</td>
                            <td><asp:TextBox ID="TextBoxModel" runat="server"></asp:TextBox></td>
                            <td>Model Description:</td>
                            <td><asp:TextBox ID="TextBoxModelDescription" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>VIN:</td>
                            <td><asp:TextBox ID="TextBoxVin" runat="server"></asp:TextBox></td>
                            <td>Customer Name:</td>
                            <td><asp:TextBox ID="TextBoxCustomerName" runat="server"></asp:TextBox></td>
                            <td>Colour:</td>
                            <td><asp:TextBox ID="TextBoxColour" runat="server"></asp:TextBox></td>
                            <td>Mileage:</td>
                            <td><asp:TextBox ID="TextBoxMileage" runat="server"></asp:TextBox></td>
                        </tr>
                    </table>    
                    <hr />
                    <asp:Button ID="ButtonFilterGrid" runat="server" Text="Filter Grid" 
                        onclick="ButtonFilterGrid_Click" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="ButtonClearFilters" runat="server" Text="Clear Filters" 
                        onclick="ButtonClearFilters_Click" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="ButtonDownloadCSV" runat="server" Text="Download CSV" 
                        ClientIDMode="Static" onmousedown='ButtonDownloadCSV_Click()' onclick="ButtonDownloadCSV_Click" />
                </div>
            
        </ContentTemplate>
    </asp:UpdatePanel>
    </center>
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
    <script language="javascript" type="text/javascript">
        function ButtonDownloadCSV_Click() {
            var keeper = $get("UpdateProgress1").innerHTML;
            $get("UpdateProgress1").innerHTML = "";
            setTimeout(function () { $get("UpdateProgress1").innerHTML = keeper; }, 5000);
            }
    </script>
</asp:Content>
