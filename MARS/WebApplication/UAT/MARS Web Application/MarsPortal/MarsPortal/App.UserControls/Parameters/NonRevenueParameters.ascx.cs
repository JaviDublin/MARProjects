using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL;
using Mars.App.Classes.DAL.NonRevReWrite.Enums;
using Resources;

namespace Mars.App.UserControls.Parameters
{
    public partial class NonRevenueParameters : UserControl
    {
        
        

        internal FleetName SelectedFleetEnum
        {
            get
            {
                var f = ddlFleet.SelectedItem.Value;
                if(f == LocalizedParameterControl.AllParameterSelection)
                {
                    return FleetName.All;
                }
                FleetName name;
                if (Enum.TryParse(f, true, out name))
                {
                    return name;
                }
                throw new InvalidOperationException("Invalid Fleet Name inserted to a drop down");
            }
        }
        internal RemarksCountGroup SelectedGroupEnum
        {
            get
            {
                var rcg = ddlGroupType.SelectedItem.Value;
                RemarksCountGroup countGroup;
                if(Enum.TryParse(rcg, true, out countGroup))
                {
                    return countGroup;
                }
                throw new InvalidOperationException("Invalid Count Group inserted to a drop down");
            }
        }

        internal bool RemarksReportTypeSelected
        {
            get { return rblReportType.SelectedValue == "Remarks"; }
        }

        internal List<string> SelectedCarGroup 
        { 
            get
            {
                var itemsSelected = GroupCodes.Items.Cast<ListItem>().Where(d => d.Selected).Select(d => d.Value).ToList();
                return itemsSelected;
            } 
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                SetFleetSelections();
                SetGroupBySelections();
            }
        }



        protected void Page_PreRender(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
               BindMultiSelect();
            }
            
        }

        public void BindMultiSelect()
        {
            var javascript = new StringBuilder();
            javascript.Append(@"$(document).ready(function () {
                    $('.MultiDropDownList').dropdownchecklist({ width: 85 });
                });");

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "registerInitializer", javascript.ToString(), true);    
        }

        private void SetFleetSelections()
        {
            ddlFleet.Items.Clear();
            var li = new ListItem(LocalizedParameterControl.AllParameterSelection,
                           LocalizedParameterControl.AllParameterSelection);
            ddlFleet.Items.Insert(0, li);
            foreach (var f in ReportLookups.GetFleet())
            {
                FleetName name;
                
                if (Enum.TryParse(f.Fleet_Name.Replace(" ", ""), true, out name))
                {
                    var spacedName = Regex.Replace(name.ToString(), "(\\B[A-Z])", " $1");
                    ddlFleet.Items.Add(new ListItem(spacedName, name.ToString()));
                }
                
            }
            ddlFleet.DataBind();

        }

        private void SetGroupBySelections()
        {
            ddlGroupType.Items.Insert(0, new ListItem(RemarksCountGroup.Kci.ToString()));
            ddlGroupType.Items.Insert(1, new ListItem(RemarksCountGroup.OperStat.ToString()));
        }



    }
}