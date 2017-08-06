using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL;

namespace App.UserControls.Mappings.Gridviews
{
    public partial class OPSAreaGridview : System.Web.UI.UserControl
    {
        #region "Properties and Fields"

        private string _country;
        private int _ops_area_id;

        private int _selection;
        public string Country
        {
            get { return _country; }
        }

        public int OPS_Area_Id
        {
            get { return _ops_area_id; }
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
            this.PagerControlOPSAreas.GridviewToPage = this.GridviewOPSAreas;
            this.PagerControlOPSAreas.GridviewSessionValues = (int)App.BLL.Gridviews.GridviewToPage.MappingOPSArea;
        }

        public void LoadOPSAreasControl()
        {


            //Settings for pager control
            //Set Default Sort Order
            SessionHandler.MappingOPSAreaSortOrder = "ASC";
            //Set current Page number and size
            SessionHandler.MappingOPSAreaCurrentPageNumber = 1;
            SessionHandler.MappingOPSAreaPageSize = 10;
            this.PagerControlOPSAreas.PagerDropDownListRows.SelectedValue = "10";


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


            MappingsOPSAreas.SelectOPSAreas(Convert.ToInt32(SessionHandler.MappingOPSAreaPageSize), Convert.ToInt32(SessionHandler.MappingOPSAreaCurrentPageNumber), sortExpression,
                    this.PanelOPSAreas, this.PagerControlOPSAreas.PagerButtonFirst, this.PagerControlOPSAreas.PagerButtonNext, this.PagerControlOPSAreas.PagerButtonPrevious,
                        this.PagerControlOPSAreas.PagerButtonLast, this.PagerControlOPSAreas.PagerLabelTotalPages, this.PagerControlOPSAreas.PagerDropDownListPage,
            this.GridviewOPSAreas, this.LabelTotalRecordsDisplay, this.EmptyDataTemplateOPSAreas, SessionHandler.MappingSelectedCountry, Convert.ToInt32(SessionHandler.MappingSelectedOPSRegionId));


        }
        #endregion

        #region "Gridview Events"

        public event GridViewCommandEventHandler GridviewCommand;

        protected void GridviewOPSAreas_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {


            int rowIndex = -1;
            int ops_area_id = -1;

            switch (e.CommandName)
            {

                case "SelectLocations":

                    rowIndex = Convert.ToInt32(e.CommandArgument);
                    _ops_area_id = Convert.ToInt32(this.GridviewOPSAreas.DataKeys[rowIndex].Values[0]);
                    _country = Convert.ToString(this.GridviewOPSAreas.DataKeys[rowIndex].Values[1]);
                    _selection = (int)App.BLL.Mappings.Type.Locations;

                    //Raise custom event from parent page
                    if (GridviewCommand != null)
                    {
                        GridviewCommand(this, e);
                    }


                    break;
                case "EditOPSArea":
                    rowIndex = Convert.ToInt32(e.CommandArgument);
                    ops_area_id = Convert.ToInt32(this.GridviewOPSAreas.DataKeys[rowIndex].Values[0]);

                    List<MappingsOPSAreas.OPSAreas> results = MappingsOPSAreas.SelectOPSAreaById(ops_area_id);

                    if ((results != null))
                    {
                        foreach (MappingsOPSAreas.OPSAreas item in results)
                        {
                            this.MappingOPSAreaDetails.OPS_Area_Id = item.OPS_Area_Id;
                            this.MappingOPSAreaDetails.OPS_Area = item.OPS_Area;
                            this.MappingOPSAreaDetails.OPS_Region_Id = item.OPS_Region_Id;
                        }
                        SessionHandler.MappingOPSAreaDefaultMode = (int)App.BLL.Mappings.Mode.Edit;
                        SessionHandler.MappingOPSAreaValidationGroup = "OPSAreaEdit";
                        this.MappingOPSAreaDetails.LoadDetails();
                        this.MappingOPSAreaDetails.ModalExtenderMapping.Show();
                        this.UpdatePanelMappingGridview.Update();
                    }

                    break;
                case "DeleteOPSArea":
                    rowIndex = Convert.ToInt32(e.CommandArgument);
                    ops_area_id = Convert.ToInt32(this.GridviewOPSAreas.DataKeys[rowIndex].Values[0]);


                    int result = MappingsOPSAreas.DeleteOPSArea(ops_area_id);

                    if (result == 0)
                    {
                        this.GridviewSortingAndPaging(null);
                        this.LabelMessage.Text = Resources.lang.MessageDeleteOPSArea;

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

        protected void GridviewOPSAreas_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {


            string sortExpression = string.Empty;
            if ((SessionHandler.MappingOPSAreaSortOrder == " ASC"))
            {
                SessionHandler.MappingOPSAreaSortOrder = " DESC";
                SessionHandler.MappingOPSAreaSortDirection = (int)App.BLL.Mappings.SortDirection.Descending;
                sortExpression = e.SortExpression.ToString() + SessionHandler.MappingOPSAreaSortOrder;
            }
            else
            {
                SessionHandler.MappingOPSAreaSortOrder = " ASC";
                SessionHandler.MappingOPSAreaSortDirection = (int)App.BLL.Mappings.SortDirection.Ascending;
                sortExpression = e.SortExpression.ToString();
            }

            SessionHandler.MappingOPSAreaSortExpression = sortExpression.ToString();

            //Load Data
            GridviewSortingAndPaging(sortExpression);


        }

        protected void GridviewOPSAreas_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
                            

                            string sortExpression = SessionHandler.MappingOPSAreaSortExpression;
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
                                    if (SessionHandler.MappingOPSAreaSortDirection == (int)App.BLL.Mappings.SortDirection.Ascending)
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

        protected void GridviewOPSAreas_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {



            if ((e.Row != null) && e.Row.RowType == DataControlRowType.DataRow)
            {
                int cellCount = this.GridviewOPSAreas.Columns.Count;
                int lastColumnIndex = cellCount - 1;

                LinkButton linkButtonDelete = (LinkButton)e.Row.Cells[lastColumnIndex].FindControl("LinkButtonDelete");
                string uniqueId = linkButtonDelete.UniqueID;
                string opsArea = Convert.ToString(e.Row.Cells[0].Text);
                string message = "Are you sure you want to delete the OPS Area " + opsArea + " ?";
                linkButtonDelete.Attributes.Add("onclick", "return ShowConfirm('" + uniqueId + "','" + message + "')");

            }


        }

        protected void GetPageIndex(object sender, System.EventArgs e)
        {
            {
                //Load Data
                GridviewSortingAndPaging(SessionHandler.MappingOPSAreaSortExpression);
            }

        }

        protected void DropDownListRows_SelectedIndexChanged(object sender, System.EventArgs e)
        {

            int totalRecords = Convert.ToInt32(this.LabelTotalRecordsDisplay.Text);
            if (totalRecords > 10)
            {
                //Load Data
                GridviewSortingAndPaging(SessionHandler.MappingOPSAreaSortExpression);
            }

        }

        protected void DropDownListPage_SelectedIndexChanged(object sender, System.EventArgs e)
        {

            //Load Data
            GridviewSortingAndPaging(SessionHandler.MappingOPSAreaSortExpression);

        }

        protected void GridviewSortingAndPaging(string sortExpression)
        {


            if (sortExpression == null)
            {
                sortExpression = SessionHandler.MappingOPSAreaSortExpression;
            }
            this.LoadData(sortExpression);
            this.UpdatePanelMappingGridview.Update();


        }

        #endregion

        #region "Click Events"
        protected void ButtonAddOPSArea_Click(object sender, System.EventArgs e)
        {

            SessionHandler.MappingOPSAreaDefaultMode = (int)App.BLL.Mappings.Mode.Insert;
            SessionHandler.MappingOPSAreaValidationGroup = "OPSAreaInsert";
            this.MappingOPSAreaDetails.LoadDetails();
            this.MappingOPSAreaDetails.ModalExtenderMapping.Show();
            this.UpdatePanelMappingGridview.Update();

        }


        protected void MappingDetailsSave_Click(object sender, System.EventArgs e)
        {

            this.LabelMessage.Text = this.MappingOPSAreaDetails.ErrorMessage;
            this.GridviewSortingAndPaging(null);
            this.MappingOPSAreaDetails.ModalExtenderMapping.Hide();
            this.UpdatePanelMappingGridview.Update();

        }
        #endregion
    }
}