<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopFeedback2.ascx.cs" Inherits="Mars.App.UserControls.Pooling.TopFeedback2" %>
 <table cellpadding="2" width="96%">
        <tr>
            <td colspan="2" width="32%" class="heading"><asp:Label ID="labelHeading" runat="server" 
                    meta:resourcekey="labelHeadingResource1"></asp:Label></td>
            <td colspan='3' width="48%"></td>
            <td width="16%">
                <table>
                    <tr>
                        <td class="pooling-topFeedback">    
                            
                            <asp:Label ID="labelDBUpdate" runat="server" ClientIDMode="Static" 
                                       meta:resourcekey="labelDBUpdateResource1"/></td>
                    </tr>
                    <tr>
                        <td class="pooling-topFeedback">
                            
                            <asp:Label ID="labelDBUpdateError" runat="server" ClientIDMode="Static" 
                                                                   meta:resourcekey="labelDBUpdateErrorResource1"/>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="6"><hr /></td>
        </tr>
        <tr>
            <td width="16%" class="pooling-topFeedback"><asp:Label ID="labelStaticCountry" runat="server" 
                    meta:resourcekey="labelStaticCountryResource1">Country : </asp:Label></td>
            <td width="16%"class="pooling-topFeedback"><asp:Label ID="labelCountry" runat="server" 
                    meta:resourcekey="labelCountryResource1"></asp:Label></td>
            <td width="16%"class="pooling-topFeedback"><asp:Label ID="labelStaticTopCMS" runat="server" 
                    meta:resourcekey="labelStaticTopCMSResource1"></asp:Label></td>
            <td width="16%"class="pooling-topFeedback"><asp:Label ID="labelCMSPool" runat="server" 
                    meta:resourcekey="labelCMSPoolResource1"></asp:Label></td>
            <td width="16%"class="pooling-topFeedback"><asp:Label ID="labelStaticCarSegment" runat="server" 
                    meta:resourcekey="labelStaticCarSegmentResource1">Car Segment : </asp:Label></td>
            <td width="16%"class="pooling-topFeedback"><asp:Label ID="labelCarSegment" runat="server" 
                    meta:resourcekey="labelCarSegmentResource1"></asp:Label></td>
        </tr>
        <tr>
            <td></td>
            <td></td>
            <td class="pooling-topFeedback"><asp:Label ID="labelStaticTopLocation" runat="server" 
                    meta:resourcekey="labelStaticTopLocationResource1"></asp:Label></td>
            <td class="pooling-topFeedback"><asp:Label ID="labelLocationGroup" runat="server" 
                    meta:resourcekey="labelLocationGroupResource1"></asp:Label></td>
            <td class="pooling-topFeedback"><asp:Label ID="labelStaticCarClass" runat="server" 
                    meta:resourcekey="labelStaticCarClassResource1">Car Class : </asp:Label></td>
            <td class="pooling-topFeedback"><asp:Label ID="labelCarClass" runat="server" 
                    meta:resourcekey="labelCarClassResource1"></asp:Label></td>
        </tr>
        <tr>
            <td></td>
            <td></td>
            <td class="pooling-topFeedback"><asp:Label ID="labelStaticBranch" runat="server" 
                    meta:resourcekey="labelStaticBranchResource1">Branch : </asp:Label></td>
            <td class="pooling-topFeedback"><asp:Label ID="labelBranch" runat="server" 
                    meta:resourcekey="labelBranchResource1"></asp:Label></td>
            <td class="pooling-topFeedback"><asp:Label ID="labelStaticCarGroup" runat="server" 
                    meta:resourcekey="labelStaticCarGroupResource1">Car Group : </asp:Label></td>
            <td class="pooling-topFeedback"><asp:Label ID="labelCarGroup" runat="server" 
                    meta:resourcekey="labelCarGroupResource1"></asp:Label></td>
        </tr>
    </table>