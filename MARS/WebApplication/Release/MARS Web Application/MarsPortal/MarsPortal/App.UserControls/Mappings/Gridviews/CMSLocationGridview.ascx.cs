using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL;

namespace App.UserControls.Mappings.Gridviews
{
    public partial class CMSLocationGridview : System.Web.UI.UserControl
    {
        #region "Properties and Fields"

        private string _country;
        private int _cms_location_group_id;

        private int _selection;
        public string Country
        {
            get { return _country; }
        }

        public int CMS_Location_Group_Id
        {
            get { return _cms_location_group_id; }
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
            this.PagerControlCMSLocations.GridviewToPage = this.GridviewCMSLocations;
            this.PagerControlCMSLocations.GridviewSessionValues = (int)App.BLL.Gridviews.GridviewToPage.MappingCMSLocation;
        }

        public void LoadCMSLocationsControl()
        {



            //Settings for pager control
            //Set Default Sort Order
            SessionHandler.MappingCMSLocationSortOrder = "ASC";
            //Set current Page number and size
            SessionHandler.MappingCMSLocationCurrentPageNumber = 1;
            SessionHandler.MappingCMSLocationPageSize = 10;
            this.PagerControlCMSLocations.PagerDropDownListRows.SelectedValue = "10";


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



            MappingsCMSLocations.SelectCMSLocations(Convert.ToInt32(SessionHandler.MappingCMSLocationPageSize), Convert.ToInt32(SessionHandler.MappingCMSLocationCurrentPageNumber),
                    sortExpression, this.PanelCMSLocations, this.PagerControlCMSLocations.PagerButtonFirst, this.PagerControlCMSLocations.PagerButtonNext,
                    this.PagerControlCMSLocations.PagerButtonPrevious, this.PagerControlCMSLocations.PagerButtonLast, this.PagerControlCMSLocations.PagerLabelTotalPages,
                        this.PagerControlCMSLocations.PagerDropDownListPage, this.GridviewCMSLocations, this.LabelTotalRecordsDisplay, this.EmptyDataTemplateCMSLocationGroups,
                                SessionHandler.MappingSelectedCountry, Convert.ToInt32(SessionHandler.MappingSelectedCMSPoolId));


        }
        #endregion

        #region "Gridview Events"

        public event GridViewCommandEventHandler GridviewCommand;

        protected void GridviewCMSLocations_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {


            int rowIndex = -1;
            int cms_location_group_id = -1;

            switch (e.CommandName)
            {

                case "SelectLocations":

                    rowIndex = Convert.ToInt32(e.CommandArgument);
                    _cms_location_group_id = int.Parse(GridviewCMSLocations.DataKeys[rowIndex].Values[0].ToString());
                    _country = Convert.ToString(GridviewCMSLocations.DataKeys[rowIndex].Values[1]);
                    _selection = (int)App.BLL.Mappings.Type.Locations;

                    //Raise custom event from parent page
                    if (GridviewCommand != null)
                    {
                        GridviewCommand(this, e);
                    }


                    break;
                case "EditCMSLocationGroup":
                    rowIndex = Convert.ToInt32(e.CommandArgument);
                    cms_location_group_id = int.Parse(GridviewCMSLocations.DataKeys[rowIndex].Values[0].ToString());

                    List<MappingsCMSLocations.CMSLocations> results = MappingsCMSLocations.SelectCMSLocationGroupByCode(cms_location_group_id);

                    if ((results != null))
                    {

                        foreach (MappingsCMSLocations.CMSLocations item in results)
                        {
                            this.MappingCMSLocationDetails.CMS_Location_Group_Id = item.CMS_Location_Group_Id;
                            this.MappingCMSLocationDetails.CMS_Location_Group_Code_DW = item.CMS_Location_Group_Code_DW;
                            this.MappingCMSLocationDetails.CMS_Location_Group = item.CMS_Location_group;
                            this.MappingCMSLocationDetails.CMS_Pool_Id = item.CMS_Pool_Id;

                        }

                        SessionHandler.MappingCMSLocationDefaultMode = (int)App.BLL.Mappings.Mode.Edit;
                        SessionHandler.MappingCMSLocationValidationGroup = "CMSLocationGroupEdit";
                        this.MappingCMSLocationDetails.LoadDetails();
                        this.MappingCMSLocationDetails.ModalExtenderMapping.Show();
                        this.UpdatePanelMappingGridview.Update();
                    }

                    break;
                case "DeleteCMSLocationGroup":
                    rowIndex = Convert.ToInt32(e.CommandArgument);
                    cms_location_group_id = int.Parse(this.GridviewCMSLocations.DataKeys[rowIndex].Values[0].ToString());

                    int result = MappingsCMSLocations.DeleteCMSLocationGroup(cms_location_group_id);

                    if (result == 0)
                    {
                        this.GridviewSortingAndPaging(null);
                        this.LabelMessage.Text = Resources.lang.MessageDeleteCMSLocationGroup;

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

        protected void GridviewCMSLocations_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {


            string sortExpression = string.Empty;
            if ((SessionHandler.MappingCMSLocationSortOrder == " ASC"))
            {
                SessionHandler.MappingCMSLocationSortOrder = " DESC";
                SessionHandler.MappingCMSLocationSortDirection = (int)App.BLL.Mappings.SortDirection.Descending;
                sortExpression = e.SortExpression.ToString() + SessionHandler.MappingCMSLocationSortOrder;
            }
            else
            {
                SessionHandler.MappingCMSLocationSortOrder = " ASC";
                SessionHandler.MappingCMSLocationSortDirection = (int)App.BLL.Mappings.SortDirection.Ascending;
                sortExpression = e.SortExpression.ToString();
            }

            SessionHandler.MappingCMSLocationSortExpression = sortExpression.ToString();

            //Load Data
            GridviewSortingAndPaging(sortExpression);


        }

        protected void GridviewCMSLocations_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
                            

                            string sortExpression = SessionHandler.MappingCMSLocationSortExpression;
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
                                    if (SessionHandler.MappingCMSLocationSortDirection == (int)App.BLL.Mappings.SortDirection.Ascending)
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

        protected void GridviewCMSLocations_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {


            if ((e.Row != null) && e.Row.RowType == DataControlRowType.DataRow)
            {
                int cellCount = this.GridviewCMSLocations.Columns.Count;
                int lastColumnIndex = cellCount - 1;

                LinkButton linkButtonDelete = (LinkButton)e.Row.Cells[lastColumnIndex].FindControl("LinkButtonDelete");
                string uniqueId = linkButtonDelete.UniqueID;
                string cmsLocationGroup = Convert.ToString(e.Row.Cells[2].Text);
                string message = "Are you sure you want to delete the CMS Location Group " + cmsLocationGroup + " ?";
                linkButtonDelete.Attributes.Add("onclick", "return ShowConfirm('" + uniqueId + "','" + message + "')");

            }


        }

        protected void GetPageIndex(object sender, System.EventArgs e)
        {

            //Load Data
            GridviewSortingAndPaging(SessionHandler.MappingCMSLocationSortExpression);

        }

        protected void DropDownListRows_SelectedIndexChanged(object sender, System.EventArgs e)
        {

            int totalRecords = Convert.ToInt32(this.LabelTotalRecordsDisplay.Text);
            if (totalRecords > 10)
            {
                //Load Data
                GridviewSortingAndPaging(SessionHandler.MappingCMSLocationSortExpression);
            }

        }

        protected void DropDownListPage_SelectedIndexChanged(object sender, System.EventArgs e)
        {

            //Load Data
            GridviewSortingAndPaging(SessionHandler.MappingCMSLocationSortExpression);

        }

        protected void GridviewSortingAndPaging(string sortExpression)
        {



            if (sortExpression == null)
            {
                sortExpression = SessionHandler.MappingCMSLocationSortExpression;
            }
            this.LoadData(sortExpression);
            this.UpdatePanelMappingGridview.Update();


        }

        #endregion

        #region "Click Events"
        protected void ButtonAddCMSLocation_Click(object sender, System.EventArgs e)
        {

            SessionHandler.MappingCMSLocationDefaultMode = (int)App.BLL.Mappings.Mode.Insert;
            SessionHandler.MappingCMSLocationValidationGroup = "CMSLocationGroupInsert";
            this.MappingCMSLocationDetails.LoadDetails();
            this.MappingCMSLocationDetails.ModalExtenderMapping.Show();
            this.UpdatePanelMappingGridview.Update();

        }

        protected void MappingDetailsSave_Click(object sender, System.EventArgs e)
        {

            this.LabelMessage.Text = this.MappingCMSLocationDetails.ErrorMessage;
            this.GridviewSortingAndPaging(null);
            this.MappingCMSLocationDetails.ModalExtenderMapping.Hide();
            this.UpdatePanelMappingGridview.Update();

        }
        #endregion
    }
}