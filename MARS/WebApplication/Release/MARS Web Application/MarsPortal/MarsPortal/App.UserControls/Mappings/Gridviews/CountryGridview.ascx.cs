using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL;

namespace App.UserControls.Mappings.Gridviews
{
    public partial class CountryGridview : System.Web.UI.UserControl
    {
        #region "Properties and Fields"
        private string _country;

        private int _selection;
        public string Country
        {
            get { return _country; }
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
            this.PagerControlCountries.GridviewToPage = this.GridviewCountries;
            this.PagerControlCountries.GridviewSessionValues = (int)App.BLL.Gridviews.GridviewToPage.MappingCountry;

        }
        public void LoadCountryControl()
        {


            //Settings for pager control
            //Set Default Sort Order
            SessionHandler.MappingCountrySortOrder = "ASC";
            //Set current Page number and size
            SessionHandler.MappingCountryCurrentPageNumber = 1;
            SessionHandler.MappingCountryPageSize = 10;
            this.PagerControlCountries.PagerDropDownListRows.SelectedValue = "10";

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


            MappingsCountry.SelectCountry(Convert.ToInt32(SessionHandler.MappingCountryPageSize), Convert.ToInt32(SessionHandler.MappingCountryCurrentPageNumber), sortExpression,
                    this.PanelCountries, this.PagerControlCountries.PagerButtonFirst, this.PagerControlCountries.PagerButtonNext, this.PagerControlCountries.PagerButtonPrevious,
                        this.PagerControlCountries.PagerButtonLast, this.PagerControlCountries.PagerLabelTotalPages, this.PagerControlCountries.PagerDropDownListPage,
                            this.GridviewCountries, this.LabelTotalRecordsDisplay, this.EmptyDataTemplateCountries);


        }

        #endregion

        #region "Gridview Events"

        public event GridViewCommandEventHandler GridviewCommand;

        protected void GridviewCountries_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {

            string country = null;

            switch (e.CommandName)
            {

                case "SelectAreaCodes":

                    _country = Convert.ToString(e.CommandArgument);
                    _selection = (int)App.BLL.Mappings.Type.AreaCode;

                    //Raise custom event from parent page
                    if (GridviewCommand != null)
                    {
                        GridviewCommand(this, e);
                    }


                    break;
                case "SelectCMSPools":
                    _country = Convert.ToString(e.CommandArgument);
                    _selection = (int)App.BLL.Mappings.Type.CMSPools;

                    //Raise custom event from parent page
                    if (GridviewCommand != null)
                    {
                        GridviewCommand(this, e);
                    }


                    break;
                case "SelectOPSRegions":
                    _country = Convert.ToString(e.CommandArgument);
                    _selection = (int)App.BLL.Mappings.Type.OPSRegions;

                    //Raise custom event from parent page
                    if (GridviewCommand != null)
                    {
                        GridviewCommand(this, e);
                    }


                    break;
                case "SelectCarSegments":
                    _country = Convert.ToString(e.CommandArgument);
                    _selection = (int)App.BLL.Mappings.Type.CarSegment;

                    //Raise custom event from parent page
                    if (GridviewCommand != null)
                    {
                        GridviewCommand(this, e);
                    }


                    break;
                case "SelectModelCodes":
                    _country = Convert.ToString(e.CommandArgument);
                    _selection = (int)App.BLL.Mappings.Type.ModelCodes;

                    //Raise custom event from parent page
                    if (GridviewCommand != null)
                    {
                        GridviewCommand(this, e);
                    }


                    break;

                case "EditCountry":
                    country = Convert.ToString(e.CommandArgument);
                    List<MappingsCountry.CountryList> results = MappingsCountry.SelectCountryByCountry(country);

                    if ((results != null))
                    {
                        foreach (MappingsCountry.CountryList item in results)
                        {
                            this.MappingCountryDetails.Country = item.Country;
                            this.MappingCountryDetails.Country_Description = item.Country_Description;
                            this.MappingCountryDetails.Country_DW = item.Country_DW;
                            this.MappingCountryDetails.Active = item.Active;
                        }
                        SessionHandler.MappingCountryDefaultMode = (int)App.BLL.Mappings.Mode.Edit;
                        SessionHandler.MappingCountryValidationGroup = "CountryEdit";
                        this.MappingCountryDetails.LoadDetails();
                        this.MappingCountryDetails.ModalExtenderMapping.Show();
                        this.UpdatePanelMappingGridview.Update();
                    }

                    break;

                case "DeleteCountry":
                    country = Convert.ToString(e.CommandArgument);

                    int result = MappingsCountry.DeleteCountry(country);

                    if (result == 0)
                    {
                        this.GridviewSortingAndPaging(null);
                        this.LabelMessage.Text = Resources.lang.MessageDeleteCountry;

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

        protected void GridviewCountries_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {


            string sortExpression = string.Empty;
            if ((SessionHandler.MappingCountrySortOrder == " ASC"))
            {
                SessionHandler.MappingCountrySortOrder = " DESC";
                SessionHandler.MappingCountrySortDirection = (int)App.BLL.Mappings.SortDirection.Descending;
                sortExpression = e.SortExpression.ToString() + SessionHandler.MappingCountrySortOrder;
            }
            else
            {
                SessionHandler.MappingCountrySortOrder = " ASC";
                SessionHandler.MappingCountrySortDirection = (int)App.BLL.Mappings.SortDirection.Ascending;
                sortExpression = e.SortExpression.ToString();
            }

            SessionHandler.MappingCountrySortExpression = sortExpression.ToString();

            //Load Data
            GridviewSortingAndPaging(sortExpression);

        }

        protected void GridviewCountries_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
                            

                            string sortExpression = SessionHandler.MappingCountrySortExpression;
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
                                    if (SessionHandler.MappingCountrySortDirection == (int)App.BLL.Mappings.SortDirection.Ascending)
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

        protected void GridviewCountries_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {



            if ((e.Row != null) && e.Row.RowType == DataControlRowType.DataRow)
            {
                int cellCount = this.GridviewCountries.Columns.Count;
                int lastColumnIndex = cellCount - 1;

                LinkButton linkButtonDelete = (LinkButton)e.Row.Cells[lastColumnIndex].FindControl("LinkButtonDelete");
                string uniqueId = linkButtonDelete.UniqueID;
                string country = Convert.ToString(e.Row.Cells[2].Text);
                string message = "Are you sure you want to delete the country " + country + " ?";

                linkButtonDelete.Attributes.Add("onclick", "return ShowConfirm('" + uniqueId + "','" + message + "')");
            }


        }

        protected void GetPageIndex(object sender, System.EventArgs e)
        {

            //Load Data
            GridviewSortingAndPaging(SessionHandler.MappingCountrySortExpression);

        }

        protected void DropDownListRows_SelectedIndexChanged(object sender, System.EventArgs e)
        {

            int totalRecords = Convert.ToInt32(this.LabelTotalRecordsDisplay.Text);
            if (totalRecords > 10)
            {
                //Load Data
                GridviewSortingAndPaging(SessionHandler.MappingCountrySortExpression);
            }

        }

        protected void DropDownListPage_SelectedIndexChanged(object sender, System.EventArgs e)
        {

            //Load Data
            GridviewSortingAndPaging(SessionHandler.MappingCountrySortExpression);

        }

        protected void GridviewSortingAndPaging(string sortExpression)
        {

            if (sortExpression == null)
            {
                sortExpression = SessionHandler.MappingCountrySortExpression;
            }
            this.LoadData(sortExpression);
            this.UpdatePanelMappingGridview.Update();

        }

        #endregion

        #region "Click Events"

        protected void ButtonAddCountry_Click(object sender, System.EventArgs e)
        {

            SessionHandler.MappingCountryDefaultMode = (int)App.BLL.Mappings.Mode.Insert;
            SessionHandler.MappingCountryValidationGroup = "CountryInsert";
            this.MappingCountryDetails.LoadDetails();
            this.MappingCountryDetails.ModalExtenderMapping.Show();
            this.UpdatePanelMappingGridview.Update();

        }

        protected void MappingDetailsSave_Click(object sender, System.EventArgs e)
        {

            this.LabelMessage.Text = this.MappingCountryDetails.ErrorMessage;
            this.GridviewSortingAndPaging(null);
            this.MappingCountryDetails.ModalExtenderMapping.Hide();
            this.UpdatePanelMappingGridview.Update();

        }
        #endregion
    }
}