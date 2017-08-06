using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL;

namespace App.UserControls.Mappings.Gridviews
{
    public partial class CarGroupGridview : System.Web.UI.UserControl
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
            this.PagerControlCarGroups.GridviewToPage = this.GridviewCarGroups;
            this.PagerControlCarGroups.GridviewSessionValues = (int)App.BLL.Gridviews.GridviewToPage.MappingCarGroup;
        }

        public void LoadCarGroupControl()
        {
            //Settings for pager control
            //Set Default Sort Order
            SessionHandler.MappingCarGroupSortOrder = "ASC";
            //Set current Page number and size
            SessionHandler.MappingCarGroupCurrentPageNumber = 1;
            SessionHandler.MappingCarGroupPageSize = 10;
            this.PagerControlCarGroups.PagerDropDownListRows.SelectedValue = "10";


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


            MappingsCarGroup.SelectCarGroups(Convert.ToInt32(SessionHandler.MappingCarGroupPageSize), Convert.ToInt32(SessionHandler.MappingCarGroupCurrentPageNumber),
                    sortExpression, this.PanelCarGroups, this.PagerControlCarGroups.PagerButtonFirst, this.PagerControlCarGroups.PagerButtonNext,
                        this.PagerControlCarGroups.PagerButtonPrevious, this.PagerControlCarGroups.PagerButtonLast, this.PagerControlCarGroups.PagerLabelTotalPages,
                        this.PagerControlCarGroups.PagerDropDownListPage, this.GridviewCarGroups, this.LabelTotalRecordsDisplay, this.EmptyDataTemplateCarGroup,
                            SessionHandler.MappingSelectedCountry, Convert.ToInt32(SessionHandler.MappingSelectedCarClassId));


        }
        #endregion

        #region "Gridview Events"

        protected void GridviewCarGroups_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {

            int rowIndex = -1;
            int car_group_id = -1;

            switch (e.CommandName)
            {

                case "EditCarGroup":
                    rowIndex = Convert.ToInt32(e.CommandArgument);
                    car_group_id = Convert.ToInt32(GridviewCarGroups.DataKeys[rowIndex].Values[0]);

                    var results = MappingsCarGroup.SelectCarGroupById(car_group_id);
                    

                    if ((results != null))
                    {
                        foreach (MappingsCarGroup.CarGroup item in results)
                        {
                            MappingCarGroupDetails.Car_Group_Id = item.Car_Group_Id;
                            MappingCarGroupDetails.Car_Group = item.Car_Group;
                            MappingCarGroupDetails.Car_Group_Gold = item.Car_Group_Gold;
                            MappingCarGroupDetails.Car_Group_FiveStar = item.Car_Group_Fivestar;
                            MappingCarGroupDetails.Car_Group_PresidentCircle = item.Car_Group_PresidentCircle;
                            MappingCarGroupDetails.Car_Group_Platinum = item.Car_Group_Platinum;
                            MappingCarGroupDetails.Car_Class_Id = item.Car_Class_Id;
                            MappingCarGroupDetails.Sort_Car_Group = item.Sort_Car_Group;
                            

                        }

                        SessionHandler.MappingCarGroupDefaultMode = (int)App.BLL.Mappings.Mode.Edit;
                        SessionHandler.MappingCarGroupValidationGroup = "CarGroupEdit";
                        MappingCarGroupDetails.LoadDetails();
                        MappingCarGroupDetails.ModalExtenderMapping.Show();
                        UpdatePanelMappingGridview.Update();

                    }

                    break;
                case "DeleteCarGroup":
                    rowIndex = Convert.ToInt32(e.CommandArgument);
                    car_group_id = Convert.ToInt32(this.GridviewCarGroups.DataKeys[rowIndex].Values[0]);

                    int result = MappingsCarGroup.DeleteCarGroup(car_group_id);

                    if (result == 0)
                    {
                        this.GridviewSortingAndPaging(null);
                        this.LabelMessage.Text = Resources.lang.MessageDeleteCarGroup;

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

        protected void GridviewCarGroups_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {



            string sortExpression = string.Empty;
            if ((SessionHandler.MappingCarGroupSortOrder == " ASC"))
            {
                SessionHandler.MappingCarGroupSortOrder = " DESC";
                SessionHandler.MappingCarGroupSortDirection = (int)App.BLL.Mappings.SortDirection.Descending;
                sortExpression = e.SortExpression.ToString() + SessionHandler.MappingCarGroupSortOrder;
            }
            else
            {
                SessionHandler.MappingCarGroupSortOrder = " ASC";
                SessionHandler.MappingCarGroupSortDirection = (int)App.BLL.Mappings.SortDirection.Ascending;
                sortExpression = e.SortExpression.ToString();
            }

            SessionHandler.MappingCarGroupSortExpression = sortExpression.ToString();

            //Load Data
            GridviewSortingAndPaging(sortExpression);


        }

        protected void GridviewCarGroups_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
                            

                            string sortExpression = SessionHandler.MappingCarGroupSortExpression;
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
                                    if (SessionHandler.MappingCarGroupSortDirection == (int)App.BLL.Mappings.SortDirection.Ascending)
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

        protected void GridviewCarGroups_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {




            if ((e.Row != null) && e.Row.RowType == DataControlRowType.DataRow)
            {
                int cellCount = this.GridviewCarGroups.Columns.Count;
                int lastColumnIndex = cellCount - 1;

                LinkButton linkButtonDelete = (LinkButton)e.Row.Cells[lastColumnIndex].FindControl("LinkButtonDelete");
                string uniqueId = linkButtonDelete.UniqueID;
                string cargroup = Convert.ToString(e.Row.Cells[0].Text);
                string message = "Are you sure you want to delete the Car Group " + cargroup + " ?";
                linkButtonDelete.Attributes.Add("onclick", "return ShowConfirm('" + uniqueId + "','" + message + "')");

            }


        }

        protected void GetPageIndex(object sender, System.EventArgs e)
        {

            //Load Data
            GridviewSortingAndPaging(SessionHandler.MappingCarGroupSortExpression);

        }

        protected void DropDownListRows_SelectedIndexChanged(object sender, System.EventArgs e)
        {

            int totalRecords = Convert.ToInt32(this.LabelTotalRecordsDisplay.Text);
            if (totalRecords > 10)
            {
                //Load Data
                GridviewSortingAndPaging(SessionHandler.MappingCarGroupSortExpression);
            }

        }

        protected void DropDownListPage_SelectedIndexChanged(object sender, System.EventArgs e)
        {

            //Load Data
            GridviewSortingAndPaging(SessionHandler.MappingCarGroupSortExpression);

        }

        protected void GridviewSortingAndPaging(string sortExpression)
        {


            if (sortExpression == null)
            {
                sortExpression = SessionHandler.MappingCarGroupSortExpression;
            }
            this.LoadData(sortExpression);
            this.UpdatePanelMappingGridview.Update();


        }

        #endregion

        #region "Click Events"
        protected void ButtonAddCarGroup_Click(object sender, System.EventArgs e)
        {

            SessionHandler.MappingCarGroupDefaultMode = (int)App.BLL.Mappings.Mode.Insert;
            SessionHandler.MappingCarGroupValidationGroup = "CarGroupInsert";
            this.MappingCarGroupDetails.LoadDetails();
            this.MappingCarGroupDetails.ModalExtenderMapping.Show();
            this.UpdatePanelMappingGridview.Update();

        }

        protected void MappingDetailsSave_Click(object sender, System.EventArgs e)
        {

            this.LabelMessage.Text = this.MappingCarGroupDetails.ErrorMessage;
            this.GridviewSortingAndPaging(null);
            this.MappingCarGroupDetails.ModalExtenderMapping.Hide();
            this.UpdatePanelMappingGridview.Update();

        }
        #endregion
    }
}