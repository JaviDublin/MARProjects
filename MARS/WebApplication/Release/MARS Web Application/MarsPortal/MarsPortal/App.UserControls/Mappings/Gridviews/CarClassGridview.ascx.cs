using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL;

namespace App.UserControls.Mappings.Gridviews
{
    public partial class CarClassGridview : System.Web.UI.UserControl
    {
        #region "Properties and Fields"
        private string _country;
        private int _car_class_id;

        private int _selection;
        public string Country
        {
            get { return _country; }
        }

        public int Car_Class_Id
        {
            get { return _car_class_id; }
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
            this.PagerControlCarClasses.GridviewToPage = this.GridviewCarClasses;
            this.PagerControlCarClasses.GridviewSessionValues = (int)App.BLL.Gridviews.GridviewToPage.MappingCarClass;
        }

        public void LoadCarClassControl()
        {



            //Settings for pager control
            //Set Default Sort Order
            SessionHandler.MappingCarClassSortOrder = "ASC";
            //Set current Page number and size
            SessionHandler.MappingCarClassCurrentPageNumber = 1;
            SessionHandler.MappingCarClassPageSize = 10;
            this.PagerControlCarClasses.PagerDropDownListRows.SelectedValue = "10";


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

            MappingsCarClass.SelectCarClasses(Convert.ToInt32(SessionHandler.MappingCarClassPageSize), Convert.ToInt32(SessionHandler.MappingCarClassCurrentPageNumber), sortExpression, this.PanelCarClasses, this.PagerControlCarClasses.PagerButtonFirst, this.PagerControlCarClasses.PagerButtonNext, this.PagerControlCarClasses.PagerButtonPrevious, this.PagerControlCarClasses.PagerButtonLast, this.PagerControlCarClasses.PagerLabelTotalPages, this.PagerControlCarClasses.PagerDropDownListPage,
            this.GridviewCarClasses, this.LabelTotalRecordsDisplay, this.EmptyDataTemplateCarClass, SessionHandler.MappingSelectedCountry, Convert.ToInt32(SessionHandler.MappingSelectedCarSegmentId));



        }
        #endregion

        #region "Gridview Events"

        public event GridViewCommandEventHandler GridviewCommand;

        protected void GridviewCarClasses_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {


            int rowIndex = -1;
            int car_class_id = -1;

            switch (e.CommandName)
            {

                case "SelectCarGroups":

                    rowIndex = Convert.ToInt32(e.CommandArgument);
                    _car_class_id = Convert.ToInt32(this.GridviewCarClasses.DataKeys[rowIndex].Values[0]);
                    _country = Convert.ToString(this.GridviewCarClasses.DataKeys[rowIndex].Values[1]);
                    _selection = (int)App.BLL.Mappings.Type.CarGroup;

                    //Raise custom event from parent page
                    if (GridviewCommand != null)
                    {
                        GridviewCommand(this, e);
                    }


                    break;
                case "EditCarClass":

                    rowIndex = Convert.ToInt32(e.CommandArgument);
                    car_class_id = Convert.ToInt32(this.GridviewCarClasses.DataKeys[rowIndex].Values[0]);

                    List<MappingsCarClass.CarClass> results = MappingsCarClass.SelectCarClassById(car_class_id);


                    if ((results != null))
                    {

                        foreach (MappingsCarClass.CarClass item in results)
                        {
                            this.MappingCarClassDetails.Car_Class_Id = item.Car_Class_Id;
                            this.MappingCarClassDetails.Car_Class = item.Car_Class;
                            this.MappingCarClassDetails.Car_Segment_Id = item.Car_Segment_Id;
                            this.MappingCarClassDetails.Sort_Car_Class = item.Sort_Car_Class;

                        }

                        SessionHandler.MappingCarClassDefaultMode = (int)App.BLL.Mappings.Mode.Edit;
                        SessionHandler.MappingCarClassValidationGroup = "CarClassEdit";
                        this.MappingCarClassDetails.LoadDetails();
                        this.MappingCarClassDetails.ModalExtenderMapping.Show();
                        this.UpdatePanelMappingGridview.Update();

                    }

                    break;

                case "DeleteCarClass":
                    rowIndex = Convert.ToInt32(e.CommandArgument);
                    car_class_id = Convert.ToInt32(this.GridviewCarClasses.DataKeys[rowIndex].Values[0]);

                    int result = MappingsCarClass.DeleteCarClass(car_class_id);

                    if (result == 0)
                    {
                        this.GridviewSortingAndPaging(null);
                        this.LabelMessage.Text = Resources.lang.MessageDeleteCarClass;

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

        protected void GridviewCarClasses_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {

            string sortExpression = string.Empty;
            if ((SessionHandler.MappingCarClassSortOrder == " ASC"))
            {
                SessionHandler.MappingCarClassSortOrder = " DESC";
                SessionHandler.MappingCarClassSortDirection = (int)App.BLL.Mappings.SortDirection.Descending;
                sortExpression = e.SortExpression.ToString() + SessionHandler.MappingCarClassSortOrder;
            }
            else
            {
                SessionHandler.MappingCarClassSortOrder = " ASC";
                SessionHandler.MappingCarClassSortDirection = (int)App.BLL.Mappings.SortDirection.Ascending;
                sortExpression = e.SortExpression.ToString();
            }

            SessionHandler.MappingCarClassSortExpression = sortExpression.ToString();

            //Load Data
            GridviewSortingAndPaging(sortExpression);




        }

        protected void GridviewCarClasses_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
                            

                            string sortExpression = SessionHandler.MappingCarClassSortExpression;
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
                                    if (SessionHandler.MappingCarClassSortDirection == (int)App.BLL.Mappings.SortDirection.Ascending)
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

        protected void GridviewCarClasses_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {


            if ((e.Row != null) && e.Row.RowType == DataControlRowType.DataRow)
            {
                int cellCount = this.GridviewCarClasses.Columns.Count;
                int lastColumnIndex = cellCount - 1;

                LinkButton linkButtonDelete = (LinkButton)e.Row.Cells[lastColumnIndex].FindControl("LinkButtonDelete");
                string uniqueId = linkButtonDelete.UniqueID;
                string carclass = Convert.ToString(e.Row.Cells[0].Text);
                string message = "Are you sure you want to delete the Car Class " + carclass + " ?";
                linkButtonDelete.Attributes.Add("onclick", "return ShowConfirm('" + uniqueId + "','" + message + "')");

            }


        }

        protected void GetPageIndex(object sender, System.EventArgs e)
        {

            //Load Data
            GridviewSortingAndPaging(SessionHandler.MappingCarClassSortExpression);

        }

        protected void DropDownListRows_SelectedIndexChanged(object sender, System.EventArgs e)
        {

            int totalRecords = Convert.ToInt32(this.LabelTotalRecordsDisplay.Text);
            if (totalRecords > 10)
            {
                //Load Data
                GridviewSortingAndPaging(SessionHandler.MappingCarClassSortExpression);
            }

        }

        protected void DropDownListPage_SelectedIndexChanged(object sender, System.EventArgs e)
        {

            //Load Data
            GridviewSortingAndPaging(SessionHandler.MappingCarClassSortExpression);

        }

        protected void GridviewSortingAndPaging(string sortExpression)
        {


            if (sortExpression == null)
            {
                sortExpression = SessionHandler.MappingCarClassSortExpression;
            }
            this.LoadData(sortExpression);
            this.UpdatePanelMappingGridview.Update();


        }

        #endregion

        #region "Click Events"
        protected void ButtonAddCarClass_Click(object sender, System.EventArgs e)
        {

            SessionHandler.MappingCarClassDefaultMode = (int)App.BLL.Mappings.Mode.Insert;
            SessionHandler.MappingCarClassValidationGroup = "CarClassInsert";
            this.MappingCarClassDetails.LoadDetails();
            this.MappingCarClassDetails.ModalExtenderMapping.Show();
            this.UpdatePanelMappingGridview.Update();

        }

        protected void MappingDetailsSave_Click(object sender, System.EventArgs e)
        {

            this.LabelMessage.Text = this.MappingCarClassDetails.ErrorMessage;
            this.GridviewSortingAndPaging(null);
            this.MappingCarClassDetails.ModalExtenderMapping.Hide();
            this.UpdatePanelMappingGridview.Update();

        }
        #endregion
    }
}