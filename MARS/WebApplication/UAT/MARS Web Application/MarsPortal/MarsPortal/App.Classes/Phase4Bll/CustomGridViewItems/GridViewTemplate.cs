using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Dal.ForeignVehicles;
using Mars.App.Classes.Phase4Dal.ForeignVehicles.Entities;
using Label = System.Web.UI.WebControls.Label;

namespace Mars.App.Classes.Phase4Bll.CustomGridViewItems
{
    public class GridViewTemplate : ITemplate
    {
        public const string LocationString = "Location";
        public const string HiddenId = "HiddenId";

        readonly ListItemType TemplateType;
        readonly string ColumnName;

        public CountryHolder GridItemData;

        public GridViewTemplate(ListItemType type, string colname, CountryHolder ch = null)
        {
            TemplateType = type;
            ColumnName = colname;
            GridItemData = ch;
        }

        void ITemplate.InstantiateIn(Control container)
        {
            switch (TemplateType)
            {
                case ListItemType.Header:
                    var lbl = new Label {Text = ColumnName};
                    container.Controls.Add(lbl);
                    break;

                case ListItemType.Item:
                    var lb = new LinkButton();
                    lb.DataBinding += LinkButton_DataBinding;
                    //container. = "RightAlign";
                    container.Controls.Add(lb);
                    break;
            }
        }

        private void LinkButton_DataBinding(object sender, EventArgs e)
        {
            var lb = (LinkButton)sender;
            
            var container = (GridViewRow)lb.NamingContainer;

            object dataValue = DataBinder.Eval(container.DataItem, ColumnName);

            
            

            if (ColumnName == VehicleOverviewDataAccess.TotalString 
                || DataBinder.Eval(container.DataItem, LocationString).ToString() == VehicleOverviewDataAccess.TotalString)
            {
                lb.Font.Bold = true;
                lb.ForeColor = Color.DarkBlue;
                lb.Enabled = false;
            }

            if (ColumnName == string.Empty)
            {
                lb.Enabled = false;
            }

            

            if (dataValue != DBNull.Value)
            {
                lb.Text = dataValue.ToString();
                if (dataValue.ToString() == "0")
                {
                    lb.Enabled = false;
                }
                else
                {
                    lb.CommandArgument = ColumnName;
                    if (GridItemData != null)
                    {
                        lb.CommandArgument = GridItemData.CountryId;    
                    }
                    
                    lb.CommandName = DataBinder.Eval(container.DataItem, HiddenId).ToString();    
                }
                
            }
            var dcfc = lb.Parent as DataControlFieldCell;
            if (dcfc == null)
            {
                return;
            }
            if (ColumnName == LocationString)
            {
                dcfc.HorizontalAlign = HorizontalAlign.Left;
                lb.Enabled = false;
            }
            else
            {
                 dcfc.HorizontalAlign = HorizontalAlign.Right; 
            }
        }

    }
}