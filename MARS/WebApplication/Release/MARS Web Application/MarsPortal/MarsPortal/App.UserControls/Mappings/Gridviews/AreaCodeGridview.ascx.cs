using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL;

namespace App.UserControls.Mappings.Gridviews
{
    public partial class AreaCodeGridview : System.Web.UI.UserControl
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
            this.PagerControlAreaCodes.GridviewToPage = this.GridviewAreaCodes;
            this.PagerControlAreaCodes.GridviewSessionValues = (int)App.BLL.Gridviews.GridviewToPage.MappingAreaCode;
        }

        public void LoadAreaCodesControl()
        {



            //Settings for pager control
            //Set Default Sort Order
            SessionHandler.MappingAreaCodeSortOrder = "ASC";
            //Set current Page number and size
            SessionHandler.MappingAreaCodeCurrentPageNumber = 1;
            SessionHandler.MappingAreaCodePageSize = 10;
            this.PagerControlAreaCodes.PagerDropDownListRows.SelectedValue = "10";

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


            MappingsAreaCodes.SelectAreaCodes(Convert.ToInt32 (SessionHandler.MappingAreaCodePageSize), Convert.ToInt32 (SessionHandler.MappingAreaCodeCurrentPageNumber), 
                                        sortExpression, this.PanelAreaCodes, this.PagerControlAreaCodes.PagerButtonFirst, this.PagerControlAreaCodes.PagerButtonNext, this.PagerControlAreaCodes.PagerButtonPrevious, this.PagerControlAreaCodes.PagerButtonLast, this.PagerControlAreaCodes.PagerLabelTotalPages, this.PagerControlAreaCodes.PagerDropDownListPage,
            this.GridviewAreaCodes, this.LabelTotalRecordsDisplay, this.EmptyDataTemplateAreaCodes, SessionHandler.MappingSelectedCountry);


        }
        #endregion

        #region "Gridview Events"

        protected void GridviewAreaCodes_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {


            int rowIndex = -1;
            string ownarea = null;

            switch (e.CommandName)
            {

                case "EditAreaCode":
                    rowIndex = Convert.ToInt32(e.CommandArgument);
                    ownarea = Convert.ToString(this.GridviewAreaCodes.DataKeys[rowIndex].Values[0]);

                    List<MappingsAreaCodes.AreaCodes> results = MappingsAreaCodes.SelectAreaCodesByOwnArea(ownarea);

                    if ((results != null))
                    {
                        //There should be only 1 row

                        foreach (MappingsAreaCodes.AreaCodes item in results)
                        {
                           
                            this.MappingAreaCodeDetails.OwnArea = item.OwnArea;
                            this.MappingAreaCodeDetails.Country = item.Country;
                            this.MappingAreaCodeDetails.Area_Name = item.Area_Name;
                            this.MappingAreaCodeDetails.OPCO = item.OPCO;
                            this.MappingAreaCodeDetails.FleetCo = item.FleetCo;
                            this.MappingAreaCodeDetails.CarSales = item.CarSales;
                            this.MappingAreaCodeDetails.Licensee = item.Licensee;

                        }

                        SessionHandler.MappingAreaCodeDefaultMode = (int)App.BLL.Mappings.Mode.Edit;
                        SessionHandler.MappingAreaCodeValidationGroup = "AreaCodeEdit";
                        this.MappingAreaCodeDetails.LoadDetails();
                        this.MappingAreaCodeDetails.ModalExtenderMapping.Show();
                        this.UpdatePanelMappingGridview.Update();

                    }

                    break;
                case "DeleteAreaCode":
                    rowIndex = Convert.ToInt32(e.CommandArgument);
                    ownarea = Convert.ToString(this.GridviewAreaCodes.DataKeys[rowIndex].Values[0]);

                    int result = MappingsAreaCodes.DeleteAreaCode(ownarea);

                    if (result == 0)
                    {
                        this.GridviewSortingAndPaging(null);
                        this.LabelMessage.Text = Resources.lang.MessageDeleteAreaCode;

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

        protected void GridviewAreaCodes_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {

            string sortExpression = string.Empty;
            if ((SessionHandler.MappingAreaCodeSortOrder == " ASC"))
            {
                SessionHandler.MappingAreaCodeSortOrder = " DESC";
                SessionHandler.MappingAreaCodeSortDirection = (int)App.BLL.Mappings.SortDirection.Descending;
                sortExpression = e.SortExpression.ToString() + SessionHandler.MappingAreaCodeSortOrder;
            }
            else
            {
                SessionHandler.MappingAreaCodeSortOrder = " ASC";
                SessionHandler.MappingAreaCodeSortDirection = (int)App.BLL.Mappings.SortDirection.Ascending;
                sortExpression = e.SortExpression.ToString();
            }

            SessionHandler.MappingAreaCodeSortExpression = sortExpression.ToString();

            //Load Data
            GridviewSortingAndPaging(sortExpression);


        }

        protected void GridviewAreaCodes_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
                            

                            string sortExpression = SessionHandler.MappingAreaCodeSortExpression;
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
                                    if (SessionHandler.MappingAreaCodeSortDirection == (int)App.BLL.Mappings.SortDirection.Ascending)
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

        protected void GridviewAreaCodes_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {



            if ((e.Row != null) && e.Row.RowType == DataControlRowType.DataRow)
            {
                int cellCount = this.GridviewAreaCodes.Columns.Count;
                int lastColumnIndex = cellCount - 1;

                LinkButton linkButtonDelete = (LinkButton)e.Row.Cells[lastColumnIndex].FindControl("LinkButtonDelete");
                string uniqueId = linkButtonDelete.UniqueID;
                string areaCode = Convert.ToString(e.Row.Cells[2].Text);
                string message = "Are you sure you want to delete the area code " + areaCode + " ?";
                linkButtonDelete.Attributes.Add("onclick", "return ShowConfirm('" + uniqueId + "','" + message + "')");

               

            }


        }

        protected void GetPageIndex(object sender, System.EventArgs e)
        {

            //Load Data
            GridviewSortingAndPaging(SessionHandler.MappingAreaCodeSortExpression);

        }

        protected void DropDownListRows_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            int totalRecords = Convert.ToInt32(this.LabelTotalRecordsDisplay.Text);
            if (totalRecords > 10)
            {
                //Load Data
                GridviewSortingAndPaging(SessionHandler.MappingAreaCodeSortExpression);
            }

        }

        protected void DropDownListPage_SelectedIndexChanged(object sender, System.EventArgs e)
        {

            //Load Data
            GridviewSortingAndPaging(SessionHandler.MappingAreaCodeSortExpression);

        }

        protected void GridviewSortingAndPaging(string sortExpression)
        {


            if (sortExpression == null)
            {
                sortExpression = SessionHandler.MappingAreaCodeSortExpression;
            }
            this.LoadData(sortExpression);
            this.UpdatePanelMappingGridview.Update();


        }

        #endregion

        #region "Click Events"
        protected void ButtonAddAreaCode_Click(object sender, System.EventArgs e)
        {

            SessionHandler.MappingAreaCodeDefaultMode = (int)App.BLL.Mappings.Mode.Insert;
            SessionHandler.MappingAreaCodeValidationGroup = "AreaCodeInsert";
            this.MappingAreaCodeDetails.LoadDetails();
            this.MappingAreaCodeDetails.ModalExtenderMapping.Show();
            this.UpdatePanelMappingGridview.Update();

        }

        protected void MappingDetailsSave_Click(object sender, System.EventArgs e)
        {

            this.LabelMessage.Text = this.MappingAreaCodeDetails.ErrorMessage;
            this.GridviewSortingAndPaging(null);
            this.MappingAreaCodeDetails.ModalExtenderMapping.Hide();
            this.UpdatePanelMappingGridview.Update();

        }
        #endregion
    }
}