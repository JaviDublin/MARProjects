using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL;

namespace App.UserControls.Mappings.Gridviews
{
    public partial class ModelCodeGridview : System.Web.UI.UserControl
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
            this.PagerControlModelCodes.GridviewToPage = this.GridviewModelCodes;
            this.PagerControlModelCodes.GridviewSessionValues = (int)App.BLL.Gridviews.GridviewToPage.MappingModelCode;
        }

        public void LoadModelCodeControl()
        {



            //Settings for pager control
            //Set Default Sort Order
            SessionHandler.MappingModelCodeSortOrder = "ASC";
            //Set current Page number and size
            SessionHandler.MappingModelCodeCurrentPageNumber = 1;
            SessionHandler.MappingModelCodePageSize = 10;
            this.PagerControlModelCodes.PagerDropDownListRows.SelectedValue = "10";


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


            MappingsModelCodes.SelectModelCodes(Convert.ToInt32(SessionHandler.MappingModelCodePageSize), Convert.ToInt32(SessionHandler.MappingModelCodeCurrentPageNumber),
                    sortExpression, this.PanelModelCodes, this.PagerControlModelCodes.PagerButtonFirst, this.PagerControlModelCodes.PagerButtonNext,
                        this.PagerControlModelCodes.PagerButtonPrevious, this.PagerControlModelCodes.PagerButtonLast, this.PagerControlModelCodes.PagerLabelTotalPages,
                            this.PagerControlModelCodes.PagerDropDownListPage, this.GridviewModelCodes, this.LabelTotalRecordsDisplay, this.EmptyDataTemplateModelCodes,
                                SessionHandler.MappingSelectedCountry);

        }
        #endregion

        #region "Gridview Events"


        protected void GridviewModelCodes_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {


            int rowIndex = -1;
            int model_id = -1;

            switch (e.CommandName)
            {

                case "EditModelCode":
                    rowIndex = Convert.ToInt32(e.CommandArgument);
                    model_id = Convert.ToInt32(this.GridviewModelCodes.DataKeys[rowIndex].Values[0]);

                    List<MappingsModelCodes.ModelCodes> results = MappingsModelCodes.SelectModelCodeById(model_id);


                    if ((results != null))
                    {
                        foreach (MappingsModelCodes.ModelCodes item in results)
                        {

                            this.MappingModelCodeDetails.Model_Id = item.Model_Id;
                            this.MappingModelCodeDetails.Model = item.Model;
                            this.MappingModelCodeDetails.Country = item.Country;
                            this.MappingModelCodeDetails.Active = item.Active;

                        }
                        SessionHandler.MappingModelCodeDefaultMode = (int)App.BLL.Mappings.Mode.Edit;
                        SessionHandler.MappingModelCodeValidationGroup = "ModelCodeEdit";
                        this.MappingModelCodeDetails.LoadDetails();
                        this.MappingModelCodeDetails.ModalExtenderMapping.Show();
                        this.UpdatePanelMappingGridview.Update();
                    }

                    break;
                case "DeleteModelCode":
                    rowIndex = Convert.ToInt32(e.CommandArgument);
                    model_id = Convert.ToInt32(this.GridviewModelCodes.DataKeys[rowIndex].Values[0]);

                    int result = MappingsModelCodes.DeleteModelCode(model_id);
                    if (result == 0)
                    {
                        this.GridviewSortingAndPaging(null);
                        this.LabelMessage.Text = Resources.lang.MessageDeleteModelCode;


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

        protected void GridviewModelCodeSorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {


            string sortExpression = string.Empty;
            if ((SessionHandler.MappingModelCodeSortOrder == " ASC"))
            {
                SessionHandler.MappingModelCodeSortOrder = " DESC";
                SessionHandler.MappingModelCodeSortDirection = (int)App.BLL.Mappings.SortDirection.Descending;
                sortExpression = e.SortExpression.ToString() + SessionHandler.MappingModelCodeSortOrder;
            }
            else
            {
                SessionHandler.MappingModelCodeSortOrder = " ASC";
                SessionHandler.MappingModelCodeSortDirection = (int)App.BLL.Mappings.SortDirection.Ascending;
                sortExpression = e.SortExpression.ToString();
            }

            SessionHandler.MappingModelCodeSortExpression = sortExpression.ToString();

            //Load Data
            GridviewSortingAndPaging(sortExpression);


        }

        protected void GridviewOModelCodes_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
                            

                            string sortExpression = SessionHandler.MappingModelCodeSortExpression;
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
                                    if (SessionHandler.MappingModelCodeSortDirection == (int)App.BLL.Mappings.SortDirection.Ascending)
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

        protected void GridviewModelCodes_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {


            if ((e.Row != null) && e.Row.RowType == DataControlRowType.DataRow)
            {
                int cellCount = this.GridviewModelCodes.Columns.Count;
                int lastColumnIndex = cellCount - 1;

                LinkButton linkButtonDelete = (LinkButton)e.Row.Cells[lastColumnIndex].FindControl("LinkButtonDelete");
                string uniqueId = linkButtonDelete.UniqueID;
                string model = Convert.ToString(e.Row.Cells[0].Text);
                string message = "Are you sure you want to delete the Model " + model + " ?";
                linkButtonDelete.Attributes.Add("onclick", "return ShowConfirm('" + uniqueId + "','" + message + "')");


                CheckBox checkBoxActive = (CheckBox)e.Row.Cells[2].FindControl("CheckBoxActive");
                Image imageActive = (Image)e.Row.Cells[2].FindControl("ImageActive");

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
            GridviewSortingAndPaging(SessionHandler.MappingModelCodeSortExpression);

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
            GridviewSortingAndPaging(SessionHandler.MappingModelCodeSortExpression);


        }

        protected void GridviewSortingAndPaging(string sortExpression)
        {

            if (sortExpression == null)
            {
                sortExpression = SessionHandler.MappingModelCodeSortExpression;
            }
            this.LoadData(sortExpression);
            this.UpdatePanelMappingGridview.Update();



        }

        #endregion

        #region "Click Events"
        protected void ButtonModelCode_Click(object sender, System.EventArgs e)
        {

            SessionHandler.MappingModelCodeDefaultMode = (int)App.BLL.Mappings.Mode.Insert;
            SessionHandler.MappingModelCodeValidationGroup = "ModelInsert";
            this.MappingModelCodeDetails.LoadDetails();
            this.MappingModelCodeDetails.ModalExtenderMapping.Show();
            this.UpdatePanelMappingGridview.Update();

        }

        protected void MappingDetailsSave_Click(object sender, System.EventArgs e)
        {

            this.LabelMessage.Text = this.MappingModelCodeDetails.ErrorMessage;
            this.GridviewSortingAndPaging(null);
            this.MappingModelCodeDetails.ModalExtenderMapping.Hide();
            this.UpdatePanelMappingGridview.Update();

        }
        #endregion
    }
}