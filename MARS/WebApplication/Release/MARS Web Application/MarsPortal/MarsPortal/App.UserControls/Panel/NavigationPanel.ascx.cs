using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using App.BLL;

namespace App.UserControls.Panel
{
    public partial class NavigationPanel : System.Web.UI.UserControl
    {
        public List<NavigationMenu> LoadControlData(int selectedMenu)
        {
            var results = NavigationMenu.SelectMenuItem(selectedMenu, this.Page);
            this.DataListNavigationMenu.DataSource = results;
            this.DataListNavigationMenu.DataBind();
            return results;
        }

        public delegate void TabbedMenuHandler(object sender, Navigation e);
        public event TabbedMenuHandler NavigationMenuClick;

        protected void DataListTabbedMenu_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                if (NavigationMenuClick != null)
                {
                    int index = Convert.ToInt32(e.CommandArgument);
                    foreach (DataListItem item in this.DataListNavigationMenu.Items)
                    {
                        System.Web.UI.WebControls.Panel tabPanel = (System.Web.UI.WebControls.Panel)item.FindControl("PanelNavigationMenu");
                        HiddenField indexHiddenField = (HiddenField)item.FindControl("HiddenFieldIndex");
                        tabPanel.CssClass = (index == Convert.ToInt32(indexHiddenField.Value)) ? "panel-PanelMenuItem-Selected" : "panel-PanelMenuItem";
                    }
                    //Create new args for event handler
                    Navigation args = new Navigation(index);
                    //Fire the event
                    NavigationMenuClick(this, args);

                }
            }
        }

        public void SetMenuStyle(int activeIndex)
        {
            foreach (DataListItem item in this.DataListNavigationMenu.Items)
            {
                System.Web.UI.WebControls.Panel tabPanel = (System.Web.UI.WebControls.Panel)item.FindControl("PanelNavigationMenu");
                HiddenField indexHiddenField = (HiddenField)item.FindControl("HiddenFieldIndex");

                tabPanel.CssClass = (activeIndex == Convert.ToInt32(indexHiddenField.Value)) ? "panel-PanelMenuItem-Selected" : "panel-PanelMenuItem";
            }
        }
    }
}