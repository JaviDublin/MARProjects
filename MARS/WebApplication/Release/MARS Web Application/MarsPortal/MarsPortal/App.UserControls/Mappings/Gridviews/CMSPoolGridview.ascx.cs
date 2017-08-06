using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL;

namespace App.UserControls.Mappings.Gridviews
{
    public partial class CMSPoolGridview : System.Web.UI.UserControl
    {
        #region "Properties and Fields"

        private string _country;
        private int _cms_pool_id;

        private int _selection;
        public string Country
        {
            get { return _country; }
        }
        public int CMS_Pool_Id
        {
            get { return _cms_pool_id; }
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
            this.PagerControlCMSPools.GridviewToPage = this.GridviewCMSPools;
            this.PagerControlCMSPools.GridviewSessionValues = (int)App.BLL.Gridviews.GridviewToPage.MappingCMSPool;
        }

        public void LoadCMSPoolsControl()
        {



            //Settings for pager control
            //Set Default Sort Order
            SessionHandler.MappingCMSPoolSortOrder = "ASC";
            //Set current Page number and size
            SessionHandler.MappingCMSPoolCurrentPageNumber = 1;
            SessionHandler.MappingCMSPoolPageSize = 10;
            this.PagerControlCMSPools.PagerDropDownListRows.SelectedValue = "10";


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


            MappingsCMSPools.SelectCMSPools(Convert.ToInt32(SessionHandler.MappingCMSPoolPageSize), Convert.ToInt32(SessionHandler.MappingCMSPoolCurrentPageNumber), sortExpression,
                    this.PanelCMSPools, this.PagerControlCMSPools.PagerButtonFirst, this.PagerControlCMSPools.PagerButtonNext, this.PagerControlCMSPools.PagerButtonPrevious,
                        this.PagerControlCMSPools.PagerButtonLast, this.PagerControlCMSPools.PagerLabelTotalPages, this.PagerControlCMSPools.PagerDropDownListPage,
                            this.GridviewCMSPools, this.LabelTotalRecordsDisplay, this.EmptyDataTemplateCMSPools, SessionHandler.MappingSelectedCountry);


        }
        #endregion

        #region "Gridview Events"

        public event GridViewCommandEventHandler GridviewCommand;

        protected void GridviewCMSPools_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {


            int rowIndex = -1;
            int cms_pool_id = -1;

            switch (e.CommandName)
            {

                case "SelectCMSLocationGroups":

                    rowIndex = Convert.ToInt32(e.CommandArgument);
                    _cms_pool_id = Convert.ToInt32(this.GridviewCMSPools.DataKeys[rowIndex].Values[0]);
                    _country = Convert.ToString(this.GridviewCMSPools.DataKeys[rowIndex].Values[1]);
                    _selection = (int)App.BLL.Mappings.Type.CMSLocationGroups;

                    //Raise custom event from parent page
                    if (GridviewCommand != null)
                    {
                        GridviewCommand(this, e);
                    }


                    break;
                case "EditCMSPool":
                    rowIndex = Convert.ToInt32(e.CommandArgument);
                    cms_pool_id = Convert.ToInt32(this.GridviewCMSPools.DataKeys[rowIndex].Values[0]);

                    List<MappingsCMSPools.CMSPools> results = MappingsCMSPools.SelectCMSPoolById(cms_pool_id);


                    if ((results != null))
                    {
                        foreach (MappingsCMSPools.CMSPools item in results)
                        {
                            this.MappingCMSPoolDetails.CMS_Pool_Id = item.CMS_Pool_Id;
                            this.MappingCMSPoolDetails.CMS_Pool = item.CMS_Pool;
                            this.MappingCMSPoolDetails.Country = item.Country;

                        }
                        SessionHandler.MappingCMSPoolDefaultMode = (int)App.BLL.Mappings.Mode.Edit;
                        SessionHandler.MappingCMSPoolValidationGroup = "CMSPoolEdit";
                        this.MappingCMSPoolDetails.LoadDetails();
                        this.MappingCMSPoolDetails.ModalExtenderMapping.Show();
                        this.UpdatePanelMappingGridview.Update();
                    }

                    break;


                case "DeleteCMSPool":
                    rowIndex = Convert.ToInt32(e.CommandArgument);
                    cms_pool_id = Convert.ToInt32(this.GridviewCMSPools.DataKeys[rowIndex].Values[0]);

                    int result = MappingsCMSPools.DeleteCMSPool(cms_pool_id);

                    if (result == 0)
                    {
                        this.GridviewSortingAndPaging(null);
                        this.LabelMessage.Text = Resources.lang.MessageDeleteCMSPool;

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

        protected void GridviewCMSPools_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {

            string sortExpression = string.Empty;
            if ((SessionHandler.MappingCMSPoolSortOrder == " ASC"))
            {
                SessionHandler.MappingCMSPoolSortOrder = " DESC";
                SessionHandler.MappingCMSPoolSortDirection = (int)App.BLL.Mappings.SortDirection.Descending;
                sortExpression = e.SortExpression.ToString() + SessionHandler.MappingCMSPoolSortOrder;
            }
            else
            {
                SessionHandler.MappingCMSPoolSortOrder = " ASC";
                SessionHandler.MappingCMSPoolSortDirection = (int)App.BLL.Mappings.SortDirection.Ascending;
                sortExpression = e.SortExpression.ToString();
            }

            SessionHandler.MappingCMSPoolSortExpression = sortExpression.ToString();

            //Load Data
            GridviewSortingAndPaging(sortExpression);



        }

        protected void GridviewCMSPools_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
                            

                            string sortExpression = SessionHandler.MappingCMSPoolSortExpression;
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
                                    if (SessionHandler.MappingCMSPoolSortDirection == (int)App.BLL.Mappings.SortDirection.Ascending)
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

        protected void GridviewCMSPools_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {



            if ((e.Row != null) && e.Row.RowType == DataControlRowType.DataRow)
            {
                int cellCount = this.GridviewCMSPools.Columns.Count;
                int lastColumnIndex = cellCount - 1;

                LinkButton linkButtonDelete = (LinkButton)e.Row.Cells[lastColumnIndex].FindControl("LinkButtonDelete");
                string uniqueId = linkButtonDelete.UniqueID;
                string cmsPool = Convert.ToString(e.Row.Cells[0].Text);
                string message = "Are you sure you want to delete the CMS Pool " + cmsPool + " ?";
                linkButtonDelete.Attributes.Add("onclick", "return ShowConfirm('" + uniqueId + "','" + message + "')");

            }


        }

        protected void GetPageIndex(object sender, System.EventArgs e)
        {

            //Load Data
            GridviewSortingAndPaging(SessionHandler.MappingCMSPoolSortExpression);

        }

        protected void DropDownListRows_SelectedIndexChanged(object sender, System.EventArgs e)
        {

            int totalRecords = Convert.ToInt32(this.LabelTotalRecordsDisplay.Text);
            if (totalRecords > 10)
            {
                //Load Data
                GridviewSortingAndPaging(SessionHandler.MappingCMSPoolSortExpression);
            }

        }

        protected void DropDownListPage_SelectedIndexChanged(object sender, System.EventArgs e)
        {

            //Load Data
            GridviewSortingAndPaging(SessionHandler.MappingCMSPoolSortExpression);

        }

        protected void GridviewSortingAndPaging(string sortExpression)
        {


            if (sortExpression == null)
            {
                sortExpression = SessionHandler.MappingCMSPoolSortExpression;
            }
            this.LoadData(sortExpression);
            this.UpdatePanelMappingGridview.Update();


        }

        #endregion

        #region "Click Events"
        protected void ButtonAddCMSPool_Click(object sender, System.EventArgs e)
        {

            SessionHandler.MappingCMSPoolDefaultMode = (int)App.BLL.Mappings.Mode.Insert;
            SessionHandler.MappingCMSPoolValidationGroup = "CMSPoolInsert";
            this.MappingCMSPoolDetails.LoadDetails();
            this.MappingCMSPoolDetails.ModalExtenderMapping.Show();
            this.UpdatePanelMappingGridview.Update();

        }

        protected void MappingDetailsSave_Click(object sender, System.EventArgs e)
        {

            this.LabelMessage.Text = this.MappingCMSPoolDetails.ErrorMessage;
            this.GridviewSortingAndPaging(null);
            this.MappingCMSPoolDetails.ModalExtenderMapping.Hide();
            this.UpdatePanelMappingGridview.Update();

        }
        #endregion
    }
}