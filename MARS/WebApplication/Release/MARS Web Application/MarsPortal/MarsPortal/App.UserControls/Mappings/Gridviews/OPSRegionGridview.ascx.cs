using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL;

namespace App.UserControls.Mappings.Gridviews
{
    public partial class OPSRegionGridview : System.Web.UI.UserControl
    {
        #region "Properties and Fields"

        private string _country;
        private int _ops_region_id;

        private int _selection;
        public string Country
        {
            get { return _country; }
        }

        public int OPS_Region_Id
        {
            get { return _ops_region_id; }
        }

        public int Selection
        {
            get { return _selection; }
        }

        public Label MessageLabel
        {
            get { return this.LabelMessage; }
        }

        #endregion

        #region "Page Events"

        protected void Page_Load(object sender, System.EventArgs e)
        {
            //Settings for pager control
            this.PagerControlOPSRegions.GridviewToPage = this.GridviewOPSRegions;
            this.PagerControlOPSRegions.GridviewSessionValues = (int)App.BLL.Gridviews.GridviewToPage.MappingOPSRegion;
        }

        public void LoadOPSRegionsControl()
        {


            //Settings for pager control
            //Set Default Sort Order
            SessionHandler.MappingOPSRegionSortOrder = "ASC";
            //Set current Page number and size
            SessionHandler.MappingOPSRegionCurrentPageNumber = 1;
            SessionHandler.MappingOPSRegionPageSize = 10;
            this.PagerControlOPSRegions.PagerDropDownListRows.SelectedValue = "10";


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


            MappingsOPSRegions.SelectOPSRegions(Convert.ToInt32(SessionHandler.MappingOPSRegionPageSize), Convert.ToInt32(SessionHandler.MappingOPSRegionCurrentPageNumber),
                sortExpression, this.PanelOPSRegions, this.PagerControlOPSRegions.PagerButtonFirst, this.PagerControlOPSRegions.PagerButtonNext,
                    this.PagerControlOPSRegions.PagerButtonPrevious, this.PagerControlOPSRegions.PagerButtonLast, this.PagerControlOPSRegions.PagerLabelTotalPages,
                    this.PagerControlOPSRegions.PagerDropDownListPage, this.GridviewOPSRegions, this.LabelTotalRecordsDisplay, this.EmptyDataTemplateOPSRegions,
                    SessionHandler.MappingSelectedCountry);


        }
        #endregion

        #region "Gridview Events"

        public event GridViewCommandEventHandler GridviewCommand;

        protected void GridviewOPSRegions_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {


            int rowIndex = -1;
            int ops_region_id = -1;

            switch (e.CommandName)
            {

                case "SelectOPSAreas":

                    rowIndex = Convert.ToInt32(e.CommandArgument);
                    _ops_region_id = Convert.ToInt32(this.GridviewOPSRegions.DataKeys[rowIndex].Values[0]);
                    _country = Convert.ToString(this.GridviewOPSRegions.DataKeys[rowIndex].Values[1]);
                    _selection = (int)App.BLL.Mappings.Type.OPSAreas;

                    //Raise custom event from parent page
                    if (GridviewCommand != null)
                    {
                        GridviewCommand(this, e);
                    }


                    break;
                case "EditOPSRegion":
                    rowIndex = Convert.ToInt32(e.CommandArgument);
                    ops_region_id = Convert.ToInt32(this.GridviewOPSRegions.DataKeys[rowIndex].Values[0]);

                    List<MappingsOPSRegions.OPSRegions> results = MappingsOPSRegions.SelectOPSRegionById(ops_region_id);


                    if ((results != null))
                    {
                        foreach (MappingsOPSRegions.OPSRegions item in results)
                        {
                            this.MappingOPSRegionDetails.OPS_region_Id = item.OPS_Region_Id;
                            this.MappingOPSRegionDetails.OPS_Region = item.OPS_Region;
                            this.MappingOPSRegionDetails.Country = item.Country;
                        }
                        SessionHandler.MappingOPSRegionDefaultMode = (int)App.BLL.Mappings.Mode.Edit;
                        SessionHandler.MappingOPSRegionValidationGroup = "OPSRegionEdit";
                        this.MappingOPSRegionDetails.LoadDetails();
                        this.MappingOPSRegionDetails.ModalExtenderMapping.Show();
                        this.UpdatePanelMappingGridview.Update();
                    }

                    break;
                case "DeleteOPSRegion":
                    rowIndex = Convert.ToInt32(e.CommandArgument);
                    ops_region_id = Convert.ToInt32(this.GridviewOPSRegions.DataKeys[rowIndex].Values[0]);

                    int result = MappingsOPSRegions.DeleteOPSRegion(ops_region_id);
                    if (result == 0)
                    {
                        this.GridviewSortingAndPaging(null);
                        this.LabelMessage.Text = Resources.lang.MessageDeleteOPSRegion;


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

        protected void GridviewOPSRegions_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {


            string sortExpression = string.Empty;
            if ((SessionHandler.MappingOPSRegionSortOrder == " ASC"))
            {
                SessionHandler.MappingOPSRegionSortOrder = " DESC";
                SessionHandler.MappingOPSRegionSortDirection = (int)App.BLL.Mappings.SortDirection.Descending;
                sortExpression = e.SortExpression.ToString() + SessionHandler.MappingOPSRegionSortOrder;
            }
            else
            {
                SessionHandler.MappingOPSRegionSortOrder = " ASC";
                SessionHandler.MappingOPSRegionSortDirection = (int)App.BLL.Mappings.SortDirection.Ascending;
                sortExpression = e.SortExpression.ToString();
            }

            SessionHandler.MappingOPSRegionSortExpression = sortExpression.ToString();

            //Load Data
            GridviewSortingAndPaging(sortExpression);


        }

        protected void GridviewOPSRegions_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
                            Image image = new Image();
                            

                            string sortExpression = SessionHandler.MappingOPSRegionSortExpression;
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
                                    if (SessionHandler.MappingOPSRegionSortDirection == (int)App.BLL.Mappings.SortDirection.Ascending)
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

        protected void GridviewOPSRegions_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {

            if ((e.Row != null) && e.Row.RowType == DataControlRowType.DataRow)
            {
                int cellCount = this.GridviewOPSRegions.Columns.Count;
                int lastColumnIndex = cellCount - 1;

                LinkButton linkButtonDelete = (LinkButton)e.Row.Cells[lastColumnIndex].FindControl("LinkButtonDelete");
                string uniqueId = linkButtonDelete.UniqueID;
                string opsRegion = Convert.ToString(e.Row.Cells[0].Text);
                string message = "Are you sure you want to delete the OPS Region " + opsRegion + " ?";
                linkButtonDelete.Attributes.Add("onclick", "return ShowConfirm('" + uniqueId + "','" + message + "')");

            }



        }

        protected void GetPageIndex(object sender, System.EventArgs e)
        {

            //Load Data
            GridviewSortingAndPaging(SessionHandler.MappingOPSRegionSortExpression);

        }

        protected void DropDownListRows_SelectedIndexChanged(object sender, System.EventArgs e)
        {

            int totalRecords = Convert.ToInt32(this.LabelTotalRecordsDisplay.Text);
            if (totalRecords > 10)
            {
                //Load Data
                GridviewSortingAndPaging(SessionHandler.MappingOPSRegionSortExpression);

            }

        }

        protected void DropDownListPage_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            //Load Data
            GridviewSortingAndPaging(SessionHandler.MappingOPSRegionSortExpression);


        }

        protected void GridviewSortingAndPaging(string sortExpression)
        {

            if (sortExpression == null)
            {
                sortExpression = SessionHandler.MappingOPSRegionSortExpression;
            }
            this.LoadData(sortExpression);
            this.UpdatePanelMappingGridview.Update();



        }

        #endregion

        #region "Click Events"
        protected void ButtonAddOPSRegion_Click(object sender, System.EventArgs e)
        {

            SessionHandler.MappingOPSRegionDefaultMode = (int)App.BLL.Mappings.Mode.Insert;
            SessionHandler.MappingOPSRegionValidationGroup = "OPSRegionInsert";
            this.MappingOPSRegionDetails.LoadDetails();
            this.MappingOPSRegionDetails.ModalExtenderMapping.Show();
            this.UpdatePanelMappingGridview.Update();

        }

        protected void MappingDetailsSave_Click(object sender, System.EventArgs e)
        {

            this.LabelMessage.Text = this.MappingOPSRegionDetails.ErrorMessage;
            this.GridviewSortingAndPaging(null);
            this.MappingOPSRegionDetails.ModalExtenderMapping.Hide();
            this.UpdatePanelMappingGridview.Update();

        }
        #endregion
    }
}