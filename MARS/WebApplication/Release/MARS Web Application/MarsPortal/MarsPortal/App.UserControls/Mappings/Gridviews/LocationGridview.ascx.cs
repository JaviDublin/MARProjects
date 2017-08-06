using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL;

namespace App.UserControls.Mappings.Gridviews
{
    public partial class LocationGridview : System.Web.UI.UserControl
    {
        #region "Properties and Fields"
        public Label MessageLabel
        {
            get { return this.LabelMessage; }
        }
        #endregion

        #region "Page Events"

        protected void Page_Load(object sender, System.EventArgs e)
        {
            //Settings for pager control
            this.PagerControlLocations.GridviewToPage = this.GridviewLocations;
            this.PagerControlLocations.GridviewSessionValues = (int)App.BLL.Gridviews.GridviewToPage.MappingLocation;
        }

        public void LoadLocationsControl()
        {


            //Settings for pager control
            //Set Default Sort Order
            SessionHandler.MappingLocationSortOrder = "ASC";
            //Set current Page number and size
            SessionHandler.MappingLocationCurrentPageNumber = 1;
            SessionHandler.MappingLocationPageSize = 10;
            this.PagerControlLocations.PagerDropDownListRows.SelectedValue = "10";


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



            MappingsLocations.SelectLocations(Convert.ToInt32(SessionHandler.MappingLocationPageSize), Convert.ToInt32(SessionHandler.MappingLocationCurrentPageNumber),
                sortExpression, this.PanelLocations, this.PagerControlLocations.PagerButtonFirst, this.PagerControlLocations.PagerButtonNext,
                    this.PagerControlLocations.PagerButtonPrevious, this.PagerControlLocations.PagerButtonLast, this.PagerControlLocations.PagerLabelTotalPages,
                        this.PagerControlLocations.PagerDropDownListPage, this.GridviewLocations, this.LabelTotalRecordsDisplay, this.EmptyDataTemplateLocations,
                            SessionHandler.MappingSelectedCountry, SessionHandler.MappingSelectedCMSLocationGroupCode ?? 0, Convert.ToInt32(SessionHandler.MappingSelectedOPSAreaId));

        }
        #endregion

        #region "Gridview Events"

        protected void GridviewLocations_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {


            int rowIndex = -1;
            string location = null;

            switch (e.CommandName)
            {
                case "EditLocation":
                    rowIndex = Convert.ToInt32(e.CommandArgument);
                    location = Convert.ToString(this.GridviewLocations.DataKeys[rowIndex].Values[0]);

                    List<MappingsLocations.Locations> results = MappingsLocations.SelectLocationByLocation(location);


                    if ((results != null))
                    {
                        foreach (MappingsLocations.Locations item in results)
                        {
                            this.MappingLocationDetails.Location = item.Location;
                            this.MappingLocationDetails.Location_DW = item.Location_DW;
                            this.MappingLocationDetails.Real_Location_Name = item.Real_Location_Name;
                            this.MappingLocationDetails.Location_Name = item.Location_Name;
                            this.MappingLocationDetails.Location_Name_DW = item.Location_Name_DW;
                            this.MappingLocationDetails.Active = item.Active;
                            this.MappingLocationDetails.AP_DT_RR = item.AP_DT_RR;
                            this.MappingLocationDetails.CAL = item.CAL;
                            this.MappingLocationDetails.CMS_Location_Group_Id = item.CMS_Location_Group_Id;
                            this.MappingLocationDetails.OPS_Area_Id = item.OPS_Area_Id;
                            this.MappingLocationDetails.Served_By_Locn = item.Served_By_Locn;
                            this.MappingLocationDetails.Turnaround_Hours = item.Turnaround_Hours;

                        }
                        SessionHandler.MappingLocationDefaultMode = (int)App.BLL.Mappings.Mode.Edit;
                        SessionHandler.MappingLocationValidationGroup = "LocationEdit";
                        this.MappingLocationDetails.LoadDetails();
                        this.MappingLocationDetails.ModalExtenderMapping.Show();
                        this.UpdatePanelMappingGridview.Update();
                    }

                    break;
                case "DeleteLocation":
                    rowIndex = Convert.ToInt32(e.CommandArgument);
                    location = Convert.ToString(this.GridviewLocations.DataKeys[rowIndex].Values[0]);

                    int result = MappingsLocations.DeleteLocation(location);

                    if (result == 0)
                    {
                        this.GridviewSortingAndPaging(null);
                        this.LabelMessage.Text = Resources.lang.MessageDeleteLocation;

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

        protected void GridviewLocations_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {


            string sortExpression = string.Empty;
            if ((SessionHandler.MappingLocationSortOrder == " ASC"))
            {
                SessionHandler.MappingLocationSortOrder = " DESC";
                SessionHandler.MappingLocationSortDirection = (int)App.BLL.Mappings.SortDirection.Descending;
                sortExpression = e.SortExpression.ToString() + SessionHandler.MappingLocationSortOrder;
            }
            else
            {
                SessionHandler.MappingLocationSortOrder = " ASC";
                SessionHandler.MappingLocationSortDirection = (int)App.BLL.Mappings.SortDirection.Ascending;
                sortExpression = e.SortExpression.ToString();
            }

            SessionHandler.MappingLocationSortExpression = sortExpression.ToString();

            //Load Data
            GridviewSortingAndPaging(sortExpression);



        }

        protected void GridviewLocations_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
                            

                            string sortExpression = SessionHandler.MappingLocationSortExpression;
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
                                    if (SessionHandler.MappingLocationSortDirection == (int)App.BLL.Mappings.SortDirection.Ascending)
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

        protected void GridviewLocations_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {


            if ((e.Row != null) && e.Row.RowType == DataControlRowType.DataRow)
            {
                int cellCount = this.GridviewLocations.Columns.Count;
                int lastColumnIndex = cellCount - 1;

                LinkButton linkButtonDelete = (LinkButton)e.Row.Cells[lastColumnIndex].FindControl("LinkButtonDelete");
                string uniqueId = linkButtonDelete.UniqueID;
                string location = Convert.ToString(e.Row.Cells[2].Text);
                string message = "Are you sure you want to delete the Location " + location + " ?";
                linkButtonDelete.Attributes.Add("onclick", "return ShowConfirm('" + uniqueId + "','" + message + "')");

                CheckBox checkBoxActive = (CheckBox)e.Row.Cells[5].FindControl("CheckBoxActive");
                Image imageActive = (Image)e.Row.Cells[5].FindControl("ImageActive");

                if (checkBoxActive.Checked)
                {
                    imageActive.ImageUrl = "~/App.Images/chk_checked.png";
                }
                else
                {
                    imageActive.ImageUrl = "~/App.Images/chk_unchecked.png";
                }

            }


        }

        protected void GetPageIndex(object sender, System.EventArgs e)
        {

            //Load Data
            GridviewSortingAndPaging(SessionHandler.MappingLocationSortExpression);

        }

        protected void DropDownListRows_SelectedIndexChanged(object sender, System.EventArgs e)
        {

            int totalRecords = Convert.ToInt32(this.LabelTotalRecordsDisplay.Text);
            if (totalRecords > 10)
            {
                //Load Data
                GridviewSortingAndPaging(SessionHandler.MappingLocationSortExpression);
            }

        }

        protected void DropDownListPage_SelectedIndexChanged(object sender, System.EventArgs e)
        {

            //Load Data
            GridviewSortingAndPaging(SessionHandler.MappingLocationSortExpression);

        }

        protected void GridviewSortingAndPaging(string sortExpression)
        {

            if (sortExpression == null)
            {
                sortExpression = SessionHandler.MappingLocationSortExpression;
            }
            this.LoadData(sortExpression);
            this.UpdatePanelMappingGridview.Update();



        }

        #endregion

        #region "Click Events"
        protected void ButtonAddLocation_Click(object sender, System.EventArgs e)
        {
            SessionHandler.MappingLocationDefaultMode = (int)App.BLL.Mappings.Mode.Insert;
            SessionHandler.MappingLocationValidationGroup = "LocationInsert";
            this.MappingLocationDetails.LoadDetails();
            this.MappingLocationDetails.ModalExtenderMapping.Show();
            this.UpdatePanelMappingGridview.Update();

        }

        protected void MappingDetailsSave_Click(object sender, System.EventArgs e)
        {

            this.LabelMessage.Text = this.MappingLocationDetails.ErrorMessage;
            this.GridviewSortingAndPaging(null);
            this.MappingLocationDetails.ModalExtenderMapping.Hide();
            this.UpdatePanelMappingGridview.Update();

        }
        #endregion
    }
}