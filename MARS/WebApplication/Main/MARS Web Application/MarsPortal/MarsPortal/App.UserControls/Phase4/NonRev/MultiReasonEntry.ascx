<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MultiReasonEntry.ascx.cs" Inherits="Mars.App.UserControls.Phase4.NonRev.MultiReasonEntry" %>

<%@ Register Src="~/App.UserControls/Phase4/NonRev/ReasonEntryForm.ascx" TagName="ReasonEntry" TagPrefix="uc" %>

<div style=" width: 500px; background-color: white;" class="StandardBorder">
    <table >
        <tr>
            <td >
                <div style="float: right;">
                    <asp:ImageButton runat="server" ID="ibClose" ImageUrl="~/App.Images/Icons/close.png" 
                         OnClick="ibClose_Click" />
                </div>
                <h1>
                    Multiple Reason Entry
                </h1>
            </td>
        </tr>

        <tr>
            <td style="width: 100%; text-align: center;">
                <div style="height: 400px; overflow: scroll;">
                    <asp:GridView runat="server" Width="450px" ID="gvMultiReasonList" AutoGenerateColumns="False" HorizontalAlign="Center" >
                    <HeaderStyle CssClass="GridHeaderStyle" />
                    <RowStyle CssClass="GridRowStyle" />
                    <AlternatingRowStyle CssClass="GridAlterenatingRowStyle" />
                    <Columns>
                        <asp:BoundField DataField="Country" HeaderText="Country">
                        <HeaderStyle VerticalAlign="Middle" />
                    </asp:BoundField>
                        <asp:BoundField DataField="Vin" HeaderText="Serial">
                        <HeaderStyle VerticalAlign="Middle" />
                    </asp:BoundField>
                        <asp:BoundField DataField="Group" HeaderText="Group">
                        <HeaderStyle VerticalAlign="Middle" />
                    </asp:BoundField>
                        <asp:BoundField DataField="Model" HeaderText="Model">
                        <HeaderStyle VerticalAlign="Middle" />
                    </asp:BoundField>
                </Columns>
                </asp:GridView>
                </div>
                
                <br/><br/>
            </td>
        </tr>
        <tr>
            <td>
                
                    <uc:ReasonEntry ID="ucReasonEntry" runat="server" ShowAddButton="False" />
                
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button runat="server" ID="btnSubmit" Text="Submit" CssClass="StandardButton" UseSubmitBehavior="False" OnClick="btnSubmit_Click"/>
            </td>
        </tr>
    </table>
</div>
