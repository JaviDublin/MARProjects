<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PopupCheckBoxListForm.ascx.cs" Inherits="App.UserControls.CheckBoxList.PopupCheckBoxListForm" %>

<script type="text/javascript" language="javascript">

    Sys.Application.add_load(ApplicationLoadHandler);
    function ApplicationLoadHandler(sender, args) 
    {
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm.get_isInAsyncPostBack()) 
        {
            var checkBoxClientId = '<%=CheckBoxListPopUp.ClientID%>';
            var txtBoxClientId = '<%=TextBoxPopupCheckBoxList.ClientID%>';
            var checkBoxSelectAll = '<%=CheckBoxSelectAll.ClientID%>';
            checkBoxSelectAll.checked = true;
            
            SetCheckBoxValues(checkBoxClientId, txtBoxClientId, checkBoxSelectAll, '2', null);
        }
    }

    function SetCheckBoxValues(checkBoxClientId, txtBoxClientId, checkBoxSelectAll, sender, itemCount) 
    {
        var allSelectedText = '*** All ***';
        var noneSelectedText = 'None Selected';
        var seletecdItemsToolTip = '';
        var selectedItemsText = '';

        //Get controls to work with
        var checkBoxSelectAll = document.getElementById(checkBoxSelectAll);
        var checkBoxList = document.getElementById(checkBoxClientId);
        var textBoxSelectedItems = document.getElementById(txtBoxClientId);

        if (sender == 1) {
            //CheckBox Select All is sender
            //Check that there are items in array
            if (itemCount >= 1) {
                var checkBoxListArray = checkBoxList.getElementsByTagName('input');
                for (i = 0; i < checkBoxListArray.length; i++) 
                {
                    //Get the checkboxlist item   
                    var checkBox = document.getElementById(checkBoxList.id + '_' + [i]);
                    if (checkBoxSelectAll.checked) 
                    {
                        if (checkBox != null) 
                        {
                            checkBox.checked = true;
                        }
                    }
                    else {
                        if (checkBox != null) {
                            checkBox.checked = false;
                        }
                    }
                }
            }

            if (checkBoxSelectAll.checked) 
            {
                //Set text to show all selected
                textBoxSelectedItems.value = allSelectedText;
                textBoxSelectedItems.title = allSelectedText;
            }
            else 
            {
                //Set the text box to show none selected
                textBoxSelectedItems.value = noneSelectedText;
                textBoxSelectedItems.title = noneSelectedText;
            }
        }
        else if (sender == 2) {
            if (checkBoxList != null) 
            {
                //Check Box List is sender

                var checkBoxListArray = checkBoxList.getElementsByTagName('input');
                var selectedItemsCount = 0;
                for (i = 0; i < checkBoxListArray.length; i++) 
                {
                    //Get the checkboxlist item   
                    var checkBoxListItem = checkBoxListArray[i];

                    if (checkBoxListItem.checked) 
                    {
                        selectedItemsCount++;
                        var labelArray = checkBoxListItem.parentNode.getElementsByTagName('label');

                        if (labelArray.length > 0) 
                        {
                            if (selectedItemsCount == 1) 
                            {
                                selectedItemsText += labelArray[0].innerHTML;
                            }
                            else 
                            {
                                selectedItemsText += ', ';
                                selectedItemsText += labelArray[0].innerHTML;
                            }
                        }
                    }
                }

                textBoxSelectedItems.value = selectedItemsText;
                textBoxSelectedItems.title = selectedItemsText;

                if (selectedItemsCount == checkBoxListArray.length) 
                {
                    checkBoxSelectAll.checked = true;
                    //Set text to show all selected
                    textBoxSelectedItems.value = allSelectedText;
                    textBoxSelectedItems.title = allSelectedText;
                }
                else if (selectedItemsCount == 0) 
                {
                    //Set the text box to show none selected
                    textBoxSelectedItems.value = noneSelectedText;
                    textBoxSelectedItems.title = noneSelectedText;
                    checkBoxSelectAll.checked = false;
                }
                else 
                {
                    checkBoxSelectAll.checked = false;
                }
            }
        }
    }

</script>
<asp:UpdatePanel ID="UpdatePanelPopupCheckBoxList" runat="server">
    <ContentTemplate>
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <asp:TextBox ID="TextBoxPopupCheckBoxList" runat="server" Width="95px" ReadOnly="true" CssClass="textBoxCheckBoxListForm"></asp:TextBox>
                </td>
                <td>
                    <asp:CheckBox ID="CheckBoxSelectAll" runat="server"/>
                </td>
            </tr>
        </table>
        <asp:Panel ID="PanelPopupCheckBoxList" runat="server" CssClass="panelPopupCheckBoxList" Style="display: none;">
            <asp:Panel ID="PanelCheckBoxList" runat="server" CssClass="divPopUpCheckBoxList">
                <asp:CheckBoxList ID="CheckBoxListPopUp" runat="server" Width="108px" AutoPostBack="true" OnSelectedIndexChanged="CheckBoxSelectAll_CheckedChanged" />                
            </asp:Panel>
        </asp:Panel>
        <asp:PopupControlExtender ID="PopUpControlExtenderCheckBoxList" runat="server" TargetControlID="TextBoxPopupCheckBoxList" PopupControlID="PanelPopupCheckBoxList" Position="Bottom"/>
        
    </ContentTemplate>
</asp:UpdatePanel>