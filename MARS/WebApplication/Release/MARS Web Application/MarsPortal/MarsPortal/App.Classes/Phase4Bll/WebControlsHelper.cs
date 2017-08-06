using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Castle.Core.Internal;

namespace Mars.App.Classes.Phase4Bll
{
    public static class WebControlsHelper
    {
        public static void BindDropDownList<T>(DropDownList dropdownControl, List<T> datasource, string dataText,
            string dataValue, string selectedValue)
        {
            if (datasource.Count > 0)
            {
                dropdownControl.DataSource = datasource;

                if (!dataText.IsNullOrEmpty())
                    dropdownControl.DataTextField = dataText;

                if (!dataValue.IsNullOrEmpty())
                    dropdownControl.DataValueField = dataValue;

                dropdownControl.DataBind();
                dropdownControl.Items.Insert(0, new ListItem("[Select Value]", "-1"));

                if (selectedValue != null)
                    dropdownControl.SelectedValue = selectedValue;
                else
                    dropdownControl.SelectedIndex = 0;
            }
        }
    }
}