<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopFeedback.ascx.cs" Inherits="App.UserControls.Pooling.TopFeedback" %>
<table cellpadding="2" width="96%" >
    <tr>
        <td colspan="5" style="width: 60%; " class="heading">
            <asp:Label ID="labelHeading" runat="server" CssClass="pooling-topFeedback"
                       meta:resourcekey="labelHeadingResource1"/></td>

        <td colspan="3" style="width: 40%; float: right; ">
            <table>
                <tr>
                    <td class="pooling-topFeedback">

                        <asp:Label ID="labelDBUpdate" runat="server" ClientIDMode="Static"
                            meta:resourcekey="labelDBUpdateResource1"></asp:Label></td>
                </tr>
                <tr>
                    <td class="pooling-topFeedback">
                        <asp:Label ID="labelDBUpdateError" runat="server" ClientIDMode="Static"
                            meta:resourcekey="labelDBUpdateErrorResource1" /></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="8">
            <hr />
        </td>
    </tr>
    <tr>
        <td width="12%" class="pooling-topFeedback">
            <asp:Label ID="labelStaticCountry" runat="server"
                meta:resourcekey="labelStaticCountryResource1">Country : </asp:Label></td>
        <td width="12%" class="pooling-topFeedback">
            <asp:Label ID="labelCountry" runat="server"
                meta:resourcekey="labelCountryResource1"></asp:Label></td>
        <td width="12%" class="pooling-topFeedback">
            <asp:Label ID="labelStaticTopCMS" runat="server"
                meta:resourcekey="labelStaticTopCMSResource1"></asp:Label></td>
        <td width="12%" class="pooling-topFeedback">
            <asp:Label ID="labelCMSPool" runat="server"
                meta:resourcekey="labelCMSPoolResource1"></asp:Label></td>
        <td width="12%" class="pooling-topFeedback">
            <asp:Label ID="labelStaticCarSegment" runat="server"
                meta:resourcekey="labelStaticCarSegmentResource1">Car Segment : </asp:Label></td>
        <td width="12%" class="pooling-topFeedback">
            <asp:Label ID="labelCarSegment" runat="server"
                meta:resourcekey="labelCarSegmentResource1"></asp:Label></td>
        <td width="12%" class="pooling-topFeedback">
            <asp:Label ID="labelStaticStartDate" runat="server"
                meta:resourcekey="labelStaticStartDateResource1">Start : </asp:Label></td>
        <td width="12%" class="pooling-topFeedback">
            <asp:Label ID="labelStartDate" runat="server"
                meta:resourcekey="labelStartDateResource1"></asp:Label></td>
    </tr>
    <tr>
        <td></td>
        <td></td>
        <td class="pooling-topFeedback">
            <asp:Label ID="labelStaticTopLocation" runat="server"
                meta:resourcekey="labelStaticTopLocationResource1"></asp:Label></td>
        <td class="pooling-topFeedback">
            <asp:Label ID="labelLocationGroup" runat="server"
                meta:resourcekey="labelLocationGroupResource1"></asp:Label></td>
        <td class="pooling-topFeedback">
            <asp:Label ID="labelStaticCarClass" runat="server"
                meta:resourcekey="labelStaticCarClassResource1">Car Class : </asp:Label></td>
        <td class="pooling-topFeedback">
            <asp:Label ID="labelCarClass" runat="server"
                meta:resourcekey="labelCarClassResource1"></asp:Label></td>
        <td class="pooling-topFeedback">
            <asp:Label ID="labelStaticEndDate" runat="server"
                meta:resourcekey="labelStaticEndDateResource1">End : </asp:Label></td>
        <td class="pooling-topFeedback">
            <asp:Label ID="labelEndDate" runat="server"
                meta:resourcekey="labelEndDateResource1"></asp:Label></td>
    </tr>
    <tr>
        <td class="pooling-topFeedback">
            <asp:Label ID="labelStaticCheckInOut" runat="server"
                meta:resourcekey="labelStaticCheckInOutResource1">Check In / Out : </asp:Label></td>
        <td class="pooling-topFeedback">
            <asp:Label ID="labelCheckinOut" runat="server"
                meta:resourcekey="labelCheckinOutResource1"></asp:Label></td>
        <td class="pooling-topFeedback">
            <asp:Label ID="labelStaticBranch" runat="server"
                meta:resourcekey="labelStaticBranchResource1">Branch : </asp:Label></td>
        <td class="pooling-topFeedback">
            <asp:Label ID="labelBranch" runat="server"
                meta:resourcekey="labelBranchResource1"></asp:Label></td>
        <td class="pooling-topFeedback">
            <asp:Label ID="labelStaticCarGroup" runat="server"
                meta:resourcekey="labelStaticCarGroupResource1">Car Group : </asp:Label></td>
        <td class="pooling-topFeedback">
            <asp:Label ID="labelCarGroup" runat="server"
                meta:resourcekey="labelCarGroupResource1"></asp:Label></td>
        <td class="pooling-topFeedback">
            <asp:Label ID="labelStaticFilter" runat="server"
                meta:resourcekey="labelStaticFilterResource1">Filter : </asp:Label></td>
        <td class="pooling-topFeedback">
            <asp:Label ID="labelFilter" runat="server"
                meta:resourcekey="labelFilterResource1"></asp:Label></td>
    </tr>
</table>
