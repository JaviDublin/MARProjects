using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.UI.WebControls;
using App.BLL.Management;
using App.BLL.Utilities;
using App.Entities;
using App.BLL.ExtensionMethods;

namespace App.UserControls
{
    public partial class ManualMovementGridView : System.Web.UI.UserControl
    {
        public GridView GridView { get { return grdManualMovement; } }

        public int ScenarioID { get; set; }

        internal string SetSelectedPopupDate
        {
            set
            {
                if (value != StaticStrings.DDLDefault)
                {
                    txtAddDelDate.Text = value;
                    txtMovementDate.Text = value;
                }
            }
        }

        internal string SetSelectedLocationGroups 
        {  
            set
            {
                if (value != StaticStrings.DDLDefault)
                {
                    ddlLocationGroupFrom.SelectedValue = value;
                    ddlLocationGroupTo.SelectedValue = value;
                    ddlTarget.SelectedValue = value;
                }
            } 
        }
 
        internal string SetSelectedCarClass
        {  
            set
            {
                if (value != StaticStrings.DDLDefault)
                {
                    ddlAddDelCarClass.SelectedValue = value;
                    ddlCarClass.SelectedValue = value;
                }
            } 
        }

        public string FleetPlanID
        {
            get { return hdnFleetPlanID.Value; }
            set { hdnFleetPlanID.Value = value; }
        }

        public List<FleetPlanDetail> MovementList
        {
            get
            {
                if (Session["MovementList"] != null)
                {
                    var movementList = (List<FleetPlanDetail>)Session["MovementList"];
                    var currentList = movementList.Where(p => p.ScenarioID == Convert.ToInt32(ScenarioID)).ToList();
                    return currentList;
                }
                return new List<FleetPlanDetail>();
            }
        }
              
        public List<LocationGroup> CurrentLocationGroupList
        {
            set
            {
                ddlLocationGroupFrom.DataTextField = "LocationGroupName";
                ddlLocationGroupFrom.DataValueField = "LocationGroupID";
                ddlLocationGroupFrom.DataSource = value;
                ddlLocationGroupFrom.DataBind();

                ddlLocationGroupTo.DataTextField = "LocationGroupName";
                ddlLocationGroupTo.DataValueField = "LocationGroupID";
                ddlLocationGroupTo.DataSource = value;
                ddlLocationGroupTo.DataBind();

                ddlTarget.DataTextField = "LocationGroupName";
                ddlTarget.DataValueField = "LocationGroupID";
                ddlTarget.DataSource = value;
                ddlTarget.DataBind();
            }

        }

        public List<CarGroup> CurrentCarClassList
        {
            set
            {
                ddlCarClass.DataTextField = "CarGroupDescription";
                ddlCarClass.DataValueField = "CarGroupID";
                ddlCarClass.DataSource = value;
                ddlCarClass.DataBind();

                ddlAddDelCarClass.DataTextField = "CarGroupDescription";
                ddlAddDelCarClass.DataValueField = "CarGroupID";
                ddlAddDelCarClass.DataSource = value;
                ddlAddDelCarClass.DataBind();
            }
        }

        public event EventHandler RebindGrids;

        readonly BLLManagement bll = new BLLManagement();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                grdManualMovement.EmptyDataText = @"No Data available for selected country";
                txtAddition .Text = "0";
                txtDeletion.Text = "0";
                txtAmount.Text = "0";
                txtAddDelDate.Text = DateTime.Now.ToShortDateString();
                txtMovementDate.Text = DateTime.Now.ToShortDateString();
            }
            rvMovementDate.MinimumValue = DateTime.Today.AddDays(1).ToShortDateString();
            rvMovementDate.MaximumValue = DateTime.MaxValue.ToShortDateString();
            rvAddDelDate.MinimumValue = DateTime.Today.AddDays(1).ToShortDateString();
            rvAddDelDate.MaximumValue = DateTime.MaxValue.ToShortDateString();
            rvAddition.MaximumValue = Int32.MaxValue.ToString();
            rvDeletion.MaximumValue = Int32.MaxValue.ToString();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            var idCollection = new StringCollection();

            for (int i = 0; i < grdManualMovement.Rows.Count; i++)
            {
                var chkDelete = (CheckBox)
                   grdManualMovement.Rows[i].Cells[0].FindControl("chkDelete");
                if (chkDelete != null)
                {
                    if (chkDelete.Checked)
                    {
                        var lblFleetPlanDetailID = (Label)grdManualMovement.Rows[i].FindControl("lblFleetPlanDetailID");
                        var fleetPlanDetailID = lblFleetPlanDetailID.Text;
                        idCollection.Add(fleetPlanDetailID);
                    }
                }
            }

            //Call the method to Delete records 
            DeleteMultipleRecords(idCollection);

            // rebind the GridView
            if (RebindGrids != null)
                RebindGrids(this, new EventArgs());


        }

        //movements
        protected void btnAccept_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlLocationGroupFrom.Items.Count > 0)
                {
                    var fleetPlanEntryID = hdnFleetPlanID.Value.Length > 0 ? Convert.ToInt32(hdnFleetPlanID.Value) : 0;
                    var fleetPlanDetailID = hdnFleetPlanDetailID.Value.Length > 0
                                                ? Convert.ToInt32(hdnFleetPlanDetailID.Value)
                                                : 0;
                    var locationGroupFromID = Convert.ToInt32(ddlLocationGroupFrom.SelectedItem.Value);
                    var locationGroupToID = Convert.ToInt32(ddlLocationGroupTo.SelectedItem.Value);
                    var carclassID = ddlCarClass.SelectedItem.Value;
                    var movementDate = DateTime.Parse(txtMovementDate.Text);
                    var amount = Convert.ToInt32(txtAmount.Text);

                    hdnFleetPlanDetailID.Value = "";
                    if (fleetPlanEntryID > 0)
                    {
                        bll.FleetPlanMovementUpdate(fleetPlanEntryID,
                            fleetPlanDetailID,
                            locationGroupFromID,
                            locationGroupToID,
                            carclassID,
                            amount,
                            movementDate);
                    }
                    else
                    {
                        //Display warning
                    }

                    // rebind the GridView
                    if (RebindGrids != null)
                        RebindGrids(this, new EventArgs());
                }
            }
            catch 
            {
            }
            finally
            {
                ResetControls();
            }
        }

        private void DeleteMultipleRecords(StringCollection idCollection)
        {
            foreach (var movementID in idCollection)
            {
                bll.DeleteFleetPlanDetailByFleetPlanDetailID(Convert.ToInt32(movementID));
            }
        }

        protected void btnActionAddDel_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlTarget.Items.Count > 0)
                {
                    var fleetPlanDetailID = hdnFleetPlanDetailID.Value.Length > 0
                                                ? Convert.ToInt32(hdnFleetPlanDetailID.Value)
                                                : 0;
                    var targetID = Convert.ToInt32(ddlTarget.SelectedItem.Value);
                    var carclassID = ddlAddDelCarClass.SelectedItem.Value;
                    var movementDate = DateTime.Parse(txtAddDelDate.Text);
                    var addition = Convert.ToInt32(txtAddition.Text);
                    var deletion = Convert.ToInt32(txtDeletion.Text);
                    var fleetPlanEntryID = hdnFleetPlanID.Value.Length > 0 ? Convert.ToInt32(hdnFleetPlanID.Value) : 0;

                    if (fleetPlanEntryID > 0)
                    {
                        bll.FleetPlanAddDelUpdate(fleetPlanEntryID,
                            fleetPlanDetailID,
                            targetID,
                            carclassID,
                            addition,
                            deletion,
                            movementDate);
                    }
                    else
                    {
                    }

                    // rebind the GridView
                    if (RebindGrids != null)
                        RebindGrids(this, new EventArgs());
                }
            }
            catch 
            {
                //
            }
            finally
            {
                ResetControls();
            }
        }

        private void ResetControls()
        {
            hdnFleetPlanDetailID.Value = "";
            txtAddDelDate.Enabled = true;
            ddlTarget.Enabled = true;
            ddlAddDelCarClass.Enabled = true;
        }

        protected void grdManualMovement_RowEditing(object sender, GridViewEditEventArgs e)
        {
            int index = e.NewEditIndex;
        }

        protected void grdManualMovement_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 1)
            {
                HideAndStyleGridColumns(e);               
            }          
        }

        private void HideAndStyleGridColumns(GridViewRowEventArgs e)
        {
            e.Row.Cells[grdManualMovement.FindColumnIndex("FleetPlanEntryID")].Visible = false;
            e.Row.Cells[grdManualMovement.FindColumnIndex("FleetPlanDetailID")].Visible = false;
            e.Row.Cells[grdManualMovement.FindColumnIndex("ScenarioID")].Visible = false;
            e.Row.Cells[grdManualMovement.FindColumnIndex("LocationGroupID")].Visible = false;
            e.Row.Cells[grdManualMovement.FindColumnIndex("CarGroupID")].Visible = false;
            e.Row.Cells[grdManualMovement.FindColumnIndex("Amount")].Font.Bold = true;
            
        }

        protected void grdManualMovement_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (e.NewPageIndex >= 0)
            {
                grdManualMovement.PageIndex = e.NewPageIndex;
                grdManualMovement.DataSource = MovementList;
                grdManualMovement.DataBind();
            }
        }

        protected void grdManualMovement_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                txtAddDelDate.Enabled = false;
                ddlTarget.Enabled = false;
                ddlAddDelCarClass.Enabled = false;

                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = grdManualMovement.Rows[index];

                var lblFleetPlanDetailID = (Label)row.FindControl("lblFleetPlanDetailID");
                var fleetPlanDetailID = lblFleetPlanDetailID.Text;
                hdnFleetPlanDetailID.Value = fleetPlanDetailID;

                var lblLocationGroup = (Label)row.FindControl("lblLocationGroup");
                var locationGroup = lblLocationGroup.Text;

                var lblCarGroupID = (Label)row.FindControl("lblCarGroupID");
                var carGroupID = lblCarGroupID.Text;

                var lblMovementDate = (Label)row.FindControl("lblMovementDate");
                var movementDate = lblMovementDate.Text;

                var lblAddition = (Label)row.FindControl("lblAddition");
                var addition = lblAddition.Text;

                var lblDeletion = (Label)row.FindControl("lblDeletion");
                var deletion = lblDeletion.Text;

                ddlTarget.ClearSelection();
                ddlTarget.Items.FindByText(locationGroup).Selected = true;
                ddlAddDelCarClass.SelectedValue = carGroupID;

                txtAddition.Text = addition;
                txtDeletion.Text = deletion;
                txtAddDelDate.Text = movementDate;
                

                ajaxAddDelPopup.Show();
                
            }
        }

        protected void grdManualMovement_Sorting(object sender, GridViewSortEventArgs e)
        {
            List<FleetPlanDetail> list = MovementList;

            //toggle sorting - note this toggle applies to all gridviews in the page
            if (Session["SortDirection"] == null || Session["SortDirection"].ToString() == "des")
            {
                Session["SortDirection"] = "asc";
                e.SortDirection = SortDirection.Ascending;
            }
            else
            {
                Session["SortDirection"] = "des";
                e.SortDirection = SortDirection.Descending;
            }

            list.Sort(new GenericSorter<FleetPlanDetail>(e.SortExpression, e.SortDirection));
            Session["MovementList"] = list;

            grdManualMovement.DataSource = list;
            grdManualMovement.DataBind();
        }

        protected void ddlItemsPerPageSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetPageSize();
        }

        private void SetPageSize()
        {
            var pagerRow = grdManualMovement.BottomPagerRow;
            if (pagerRow != null)
            {
                var ddl = (System.Web.UI.WebControls.DropDownList)(pagerRow.FindControl("ddlItemsPerPageSelector"));
                grdManualMovement.PageSize = Convert.ToInt32(ddl.SelectedItem.Value);

                List<FleetPlanDetail> list = MovementList;
                grdManualMovement.DataSource = list;
                grdManualMovement.DataBind();
            }
        }

        private void SetPagerItemCountDropDown()
        {
            var pagerRow = grdManualMovement.BottomPagerRow;
            if (pagerRow != null)
            {
                var ddlItemsPerPageSelector = (System.Web.UI.WebControls.DropDownList)(pagerRow.FindControl("ddlItemsPerPageSelector"));
                ddlItemsPerPageSelector.SelectedValue = grdManualMovement.PageSize.ToString();

                var ddlPageSelect = (System.Web.UI.WebControls.DropDownList)(pagerRow.FindControl("ddlPageSelect"));
                for (int i = 0; i < grdManualMovement.PageCount; i++)
                {
                    int pageNumber = i + 1;
                    var item = new ListItem(pageNumber.ToString());

                    if (i == grdManualMovement.PageIndex)
                    {
                        item.Selected = true;
                    }

                    ddlPageSelect.Items.Add(item);
                }
            }
        }

        protected void grdManualMovement_DataBound(object sender, EventArgs e)
        {
            SetPagerItemCountDropDown();
        }


        protected void ddlPageSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            var pagerRow = grdManualMovement.BottomPagerRow;
            if (pagerRow != null)
            {
                var ddl = (System.Web.UI.WebControls.DropDownList)(pagerRow.FindControl("ddlPageSelect"));
                grdManualMovement.PageIndex = Convert.ToInt32(ddl.SelectedItem.Value) - 1;
                
            }

            grdManualMovement.DataSource = MovementList;
            grdManualMovement.DataBind();
        }

        protected void grdManualMovement_PreRender(object sender, EventArgs e)
        {
            GridViewRow pagerRow = (GridViewRow)this.grdManualMovement.BottomPagerRow;
            if (pagerRow != null && pagerRow.Visible == false)
                pagerRow.Visible = true;
        }
    }
}