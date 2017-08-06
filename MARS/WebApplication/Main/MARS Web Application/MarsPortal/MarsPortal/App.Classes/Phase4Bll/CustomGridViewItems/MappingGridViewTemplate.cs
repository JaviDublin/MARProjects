using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Dal.Administration.MappingEntities;
using Label = System.Web.UI.WebControls.Label;

namespace Mars.App.Classes.Phase4Bll.CustomGridViewItems
{
    public class MappingGridViewTemplate : ITemplate
    {

        readonly ListItemType TemplateType;
        readonly string ColumnName;

        public const string IdColumnName = "Id";
        public const string EditColumnName = "Edit";

        private readonly string _enityType;

        public MappingGridViewTemplate(ListItemType type, string colname, string entityType = null)
        {
            TemplateType = type;
            ColumnName = colname;
            _enityType = entityType;
        }

        void ITemplate.InstantiateIn(Control container)
        {
            switch (TemplateType)
            {
                case ListItemType.Header:
                    var lb2 = new LinkButton {Text = ColumnName};
                    lb2.CommandName = "Sort";
                    if(ColumnName != string.Empty)
                    {
                        lb2.CommandArgument = HeadingTranslator.TranslateHeader(ColumnName);    
                    }
                    
                    container.Controls.Add(lb2);
                    break;

                case ListItemType.Item:
                    if (ColumnName == EditColumnName)
                    {
                        var lb = new System.Web.UI.WebControls.LinkButton();
                        lb.DataBinding += LinkButton_DataBinding;
                        container.Controls.Add(lb);
                    }
                    else
                    {
                        var lbl2 = new Label();
                        lbl2.DataBinding += LinkButton_DataBinding;
                        container.Controls.Add(lbl2);
                    }
                    break;
            }
        }

        private void LinkButton_DataBinding(object sender, EventArgs e)
        {


            if (ColumnName == EditColumnName)
            {
                var lb = (LinkButton)sender;
                var container = (GridViewRow)lb.NamingContainer;
                object dataValue = DataBinder.Eval(container.DataItem, IdColumnName);
                if (dataValue == DBNull.Value) return;
                lb.Enabled = true;
                lb.CommandName = _enityType;
                
                lb.CommandArgument = dataValue.ToString();
                lb.Text = "Edit";
                var dcfc = lb.Parent as DataControlFieldCell;
                dcfc.HorizontalAlign = HorizontalAlign.Center;
                dcfc.ForeColor = Color.DarkBlue;
            }

            else
            {
                var lb = (Label)sender;
                var container = (GridViewRow)lb.NamingContainer;
                object dataValue = DataBinder.Eval(container.DataItem , HeadingTranslator.TranslateHeader(ColumnName));
                if (dataValue == DBNull.Value) return;
                lb.Text = dataValue.ToString();
            }

        }

    }
}