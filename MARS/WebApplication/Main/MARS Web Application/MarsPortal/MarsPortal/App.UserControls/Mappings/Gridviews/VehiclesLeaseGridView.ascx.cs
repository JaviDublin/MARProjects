using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL;

namespace App.UserControls.Mappings.Gridviews
{
    public partial class VehiclesLeaseGridView : System.Web.UI.UserControl
    {
        #region "Properties and Fields"

        public Label MessageLabel
        {
            get { return this.LabelMessage; }
        }

        #endregion

        #region "Page Events"

        protected void Page_Load(object sender, EventArgs e)
        {
            //Settings for pager control

        }

        public void LoadVehiclesLeaseControl()
        {
            //Settings for pager control
            //Set Default Sort Order
            SessionHandler.MappingVehiclesLeaseSortOrder = "ASC";
            //Set current Page number and size
            SessionHandler.MappingVehiclesLeaseCurrentPageNumber = 1;
            SessionHandler.MappingVehiclesLeasePageSize = 10;
   

            //Load Data in Gridview
            this.LoadData(null);

            //Update Panel
            this.UpdatePanelMappingGridview.Update();

            if (!Page.IsPostBack)
            {
                this.LabelMessage.Text = string.Empty;
            }
        
        }

        protected void LoadData(string sortExpression)
        {
            MappingsVehiclesLease.SelectVehiclesLease(Convert.ToInt32(SessionHandler.MappingVehiclesLeasePageSize)
                                        , Convert.ToInt32(SessionHandler.MappingVehiclesLeaseCurrentPageNumber)
                                        , sortExpression
                                        , this.PanelVehiclesLease
                                        , null, null, null, null, null, null,
                                        //, this.PagerControlVehiclesLease.PagerButtonFirst
                                        //, this.PagerControlVehiclesLease.PagerButtonNext
                                        //, this.PagerControlVehiclesLease.PagerButtonPrevious
                                        //, this.PagerControlVehiclesLease.PagerButtonLast
                                        //, this.PagerControlVehiclesLease.PagerLabelTotalPages
                                        //, this.PagerControlVehiclesLease.PagerDropDownListPage,
            this.GridviewVehiclesLease, this.LabelTotalRecordsDisplay, this.EmptyDataTemplateVehiclesLease
                        , SessionHandler.MappingSelectedCountryOwner, SessionHandler.MappingSelectedCountryRent
                        , SessionHandler.MappingSelectedStartDate, SessionHandler.MappingSelectedModelDescription);
        }

        #endregion

        #region "Gridview Events"

        //public event GridViewCommandEventHandler GridviewCommand;

        protected void GridviewVehiclesLease_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            string serial = null;

            switch (e.CommandName)
            {
                case "EditSerial":
                    serial = Convert.ToString(e.CommandArgument);
                    
                    List<MappingsVehiclesLease.VehiclesLeaseList> results = MappingsVehiclesLease.SelectVehiclesLeaseBySerial(serial);
                    

                    if ((results != null))
                    {
                        //There should be only 1 row

                        foreach (MappingsVehiclesLease.VehiclesLeaseList item in results)
                        {
                            this.MappingVehiclesLeaseDetails.Serial = item.Serial;
                            this.MappingVehiclesLeaseDetails.Country_Owner = item.Country_Owner;
                            this.MappingVehiclesLeaseDetails.Country_Rent = item.Country_Rent;
                            this.MappingVehiclesLeaseDetails.StartDate = item.StartDate;
                        }

                        SessionHandler.MappingVehiclesLeaseDefaultMode = (int)App.BLL.Mappings.Mode.Edit;
                        SessionHandler.MappingVehiclesLeaseValidationGroup = "VehiclesLeaseEdit";
                        this.MappingVehiclesLeaseDetails.LoadDetails();
                        this.MappingVehiclesLeaseDetails.ModalExtenderMapping.Show();
                        this.UpdatePanelMappingGridview.Update();

                    }

                    break;
                case "DeleteSerial":

                    serial = Convert.ToString(e.CommandArgument);

                    int result = MappingsVehiclesLease.DeleteVehicleLease(serial);

                    if (result == 0)
                    {
                        this.GridviewSortingAndPaging(null);
                        this.LabelMessage.Text = Resources.lang.MessageDeleteVehiclesLease;

                    }
                    else if (result == -2)
                    {
                        this.LabelMessage.Text = Resources.lang.DeleteErrorMessageConstraint;
                    }
                    else
                    {
                        this.LabelMessage.Text = Resources.lang.ErrorMessageAdministrator;
                    }

                    this.UpdatePanelMappingGridview.Update();

                    break;
            }
        }

        protected void GridviewVehiclesLease_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            string sortExpression = string.Empty;
            if ((SessionHandler.MappingVehiclesLeaseSortOrder == " ASC"))
            {
                SessionHandler.MappingVehiclesLeaseSortOrder = " DESC";
                SessionHandler.MappingVehiclesLeaseSortDirection = (int)App.BLL.Mappings.SortDirection.Descending;
                sortExpression = e.SortExpression.ToString() + SessionHandler.MappingVehiclesLeaseSortOrder;
            }
            else
            {
                SessionHandler.MappingVehiclesLeaseSortOrder = " ASC";
                SessionHandler.MappingVehiclesLeaseSortDirection = (int)App.BLL.Mappings.SortDirection.Ascending;
                sortExpression = e.SortExpression.ToString();
            }

            SessionHandler.MappingVehiclesLeaseSortExpression = sortExpression.ToString();

            //Load Data
            GridviewSortingAndPaging(sortExpression);
        
        }

        protected void GridviewVehiclesLease_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if ((e.Row != null) && e.Row.RowType == DataControlRowType.Header)
            {
                foreach (TableCell cell in e.Row.Cells)
                {

                    if (cell.HasControls())
                    {
                        LinkButton button = (LinkButton)cell.Controls[0];
                        if ((button != null))
                        {
                            
                            

                            string sortExpression = SessionHandler.MappingVehiclesLeaseSortExpression;
                            if (!(sortExpression == null))
                            {
                                string sortColumn = null;
                                if (sortExpression.Contains("DESC"))
                                {
                                    sortColumn = sortExpression.Remove(sortExpression.Length - 5, 5);
                                }
                                else
                                {
                                    sortColumn = sortExpression;
                                }

                                if (sortColumn == button.CommandArgument)
                                {
                                    Image image = new Image();
                                    if (SessionHandler.MappingVehiclesLeaseSortDirection == (int)App.BLL.Mappings.SortDirection.Ascending)
                                    {
                                        image.ImageUrl = "~/App.Images/sort-ascending.gif";
                                    }
                                    else
                                    {
                                        image.ImageUrl = "~/App.Images/sort-descending.gif";
                                    }
                                    cell.Controls.Add(image);
                                }
                            }

                            
                        }
                    }
                }
            }
            this.UpdatePanelMappingGridview.Update();
        }

        protected void GridviewVehiclesLease_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if ((e.Row != null) && e.Row.RowType == DataControlRowType.DataRow)
            {
                int cellCount = this.GridviewVehiclesLease.Columns.Count;
                int lastColumnIndex = cellCount - 1;

                LinkButton linkButtonDelete = (LinkButton)e.Row.Cells[lastColumnIndex].FindControl("LinkButtonDelete");
                string uniqueId = linkButtonDelete.UniqueID;
                string serial = Convert.ToString(e.Row.Cells[0].Text);
                string message = "Are you sure you want to delete the vehicle " + serial + " ?";
                linkButtonDelete.Attributes.Add("onclick", "return ShowConfirm('" + uniqueId + "','" + message + "')");
            }
        }

        protected void GetPageIndex(object sender, System.EventArgs e)
        {
            //Load Data
            GridviewSortingAndPaging(SessionHandler.MappingVehiclesLeaseSortExpression);
        }

        protected void DropDownListRows_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            int totalRecords = Convert.ToInt32(this.LabelTotalRecordsDisplay.Text);
            if (totalRecords > 10)
            {
                //Load Data
                GridviewSortingAndPaging(SessionHandler.MappingVehiclesLeaseSortExpression);
            }
        }

        protected void DropDownListPage_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            //Load Data
            GridviewSortingAndPaging(SessionHandler.MappingVehiclesLeaseSortExpression);
        }

        protected void GridviewSortingAndPaging(string sortExpression)
        {
            if (sortExpression == null)
            {
                sortExpression = SessionHandler.MappingVehiclesLeaseSortExpression;
            }
            this.LoadData(sortExpression);
            this.UpdatePanelMappingGridview.Update();
        }

        
        #endregion

        #region "Click Events"

        protected void ButtonAddVehiclesLease_Click(object sender, System.EventArgs e)
        {
            SessionHandler.MappingVehiclesLeaseDefaultMode = (int)App.BLL.Mappings.Mode.Insert;
            SessionHandler.MappingVehiclesLeaseValidationGroup = "VehiclesLeaseInsert";
            this.MappingVehiclesLeaseDetails.LoadDetails();
            this.MappingVehiclesLeaseDetails.ModalExtenderMapping.Show();
            this.UpdatePanelMappingGridview.Update();
        }

        protected void ButtonDeleteVehiclesLease_Click(object sender, System.EventArgs e)
        {
            SessionHandler.MappingVehiclesLeaseDefaultMode = (int)App.BLL.Mappings.Mode.Delete;
            SessionHandler.MappingVehiclesLeaseValidationGroup = "VehiclesLeaseDelete";
            this.MappingVehiclesLeaseDetails.LoadDetails();
            this.MappingVehiclesLeaseDetails.ModalExtenderMapping.Show();
            this.UpdatePanelMappingGridview.Update();
        }

        protected void MappingDetailsSave_Click(object sender, System.EventArgs e)
        {
            this.LabelMessage.Text = this.MappingVehiclesLeaseDetails.ErrorMessage;
            this.GridviewSortingAndPaging(null);
            this.MappingVehiclesLeaseDetails.ModalExtenderMapping.Hide();
            this.UpdatePanelMappingGridview.Update();
        }

        #endregion
    }
}