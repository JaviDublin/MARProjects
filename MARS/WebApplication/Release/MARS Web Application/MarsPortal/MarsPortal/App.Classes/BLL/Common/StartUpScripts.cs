using System;

namespace App.BLL
{
    public class StartUpScripts
    {
        public static void ShowConfirmLoad(Type csType, System.Web.UI.Page page, string clientId)
        {
            string script = "$(function() {" + "var button = document.getElementById('" + clientId + "');" + "$('#dialog-confirm-load').dialog({" + "autoOpen: true," + "resizable: false," + "bgiframe: true," + "modal: true," + "title: \"Please confirm to continue\"," + "width: 400," + "height: 150," + "show: 'blind'," + "buttons:{ " + "'Continue': function(){$(this).dialog('close');button.click();}," + "'Cancel': function(){$(this).dialog('close');}" + "}" + "});" + "});";

            if (!page.ClientScript.IsStartupScriptRegistered("LoadConfirm"))
            {
                page.ClientScript.RegisterStartupScript(csType, "LoadConfirm", script, true);
            }


        }
    }
}