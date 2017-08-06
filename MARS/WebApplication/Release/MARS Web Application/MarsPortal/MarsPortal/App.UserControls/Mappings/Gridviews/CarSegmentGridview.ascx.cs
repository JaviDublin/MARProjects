using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL;

namespace App.UserControls.Mappings.Gridviews
{
    public partial class CarSegmentGridview : System.Web.UI.UserControl
    {
        #region "Properties and Fields"

        private string _country;
        private int _car_segment_id;

        private int _selection;
        public string Country
        {
            get { return _country; }
        }

        public int Car_Segment_Id
        {
            get { return _car_segment_id; }
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
            this.PagerControlCarSegments.GridviewToPage = this.GridviewCarSegments;
            this.PagerControlCarSegments.GridviewSessionValues = (int)App.BLL.Gridviews.GridviewToPage.MappingCarSegment;
        }

        public void LoadCarSegmentControl()
        {



            //Settings for pager control
            //Set Default Sort Order
            SessionHandler.MappingCarSegmentSortOrder = "ASC";
            //Set current Page number and size
            SessionHandler.MappingCarSegmentCurrentPageNumber = 1;
            SessionHandler.MappingCarSegmentPageSize = 10;
            this.PagerControlCarSegments.PagerDropDownListRows.SelectedValue = "10";


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


            MappingsCarSegment.SelectCarSegments(Convert.ToInt32(SessionHandler.MappingCarSegmentPageSize), Convert.ToInt32(SessionHandler.MappingCarSegmentCurrentPageNumber),
                    sortExpression, this.PanelCarSegments, this.PagerControlCarSegments.PagerButtonFirst, this.PagerControlCarSegments.PagerButtonNext,
                    this.PagerControlCarSegments.PagerButtonPrevious, this.PagerControlCarSegments.PagerButtonLast, this.PagerControlCarSegments.PagerLabelTotalPages,
                    this.PagerControlCarSegments.PagerDropDownListPage, this.GridviewCarSegments, this.LabelTotalRecordsDisplay, this.EmptyDataTemplateCarSegment, SessionHandler.MappingSelectedCountry);


        }
        #endregion

        #region "Gridview Events"

        public event GridViewCommandEventHandler GridviewCommand;

        protected void GridviewCarSegments_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {


            int rowIndex = -1;
            int car_segment_id = -1;

            switch (e.CommandName)
            {

                case "SelectCarClasses":

                    rowIndex = Convert.ToInt32(e.CommandArgument);
                    _car_segment_id = Convert.ToInt32(this.GridviewCarSegments.DataKeys[rowIndex].Values[0]);
                    _country = Convert.ToString(this.GridviewCarSegments.DataKeys[rowIndex].Values[1]);
                    _selection = (int)App.BLL.Mappings.Type.CarClass;

                    //Raise custom event from parent page
                    if (GridviewCommand != null)
                    {
                        GridviewCommand(this, e);
                    }


                    break;
                case "EditCarSegment":
                    rowIndex = Convert.ToInt32(e.CommandArgument);
                    car_segment_id = Convert.ToInt32(this.GridviewCarSegments.DataKeys[rowIndex].Values[0]);

                    List<MappingsCarSegment.CarSegment> results = MappingsCarSegment.SelectCarSegmentById(car_segment_id);


                    if ((results != null))
                    {

                        foreach (MappingsCarSegment.CarSegment item in results)
                        {
                            this.MappingCarSegmentDetails.Car_Segment_Id = item.Car_Segment_Id;
                            this.MappingCarSegmentDetails.Car_Segment = item.Car_Segment;
                            this.MappingCarSegmentDetails.Country = item.Country;
                            this.MappingCarSegmentDetails.Sort_Car_Segment = item.Sort_Car_Segment;


                        }

                        SessionHandler.MappingCarSegmentDefaultMode = (int)App.BLL.Mappings.Mode.Edit;
                        SessionHandler.MappingCarSegmentValidationGroup = "CarSegmentEdit";
                        this.MappingCarSegmentDetails.LoadDetails();
                        this.MappingCarSegmentDetails.ModalExtenderMapping.Show();
                        this.UpdatePanelMappingGridview.Update();
                    }

                    break;
                case "DeleteCarSegment":

                    rowIndex = Convert.ToInt32(e.CommandArgument);
                    car_segment_id = Convert.ToInt32(this.GridviewCarSegments.DataKeys[rowIndex].Values[0]);

                    int result = MappingsCarSegment.DeleteCarSegment(car_segment_id);

                    if (result == 0)
                    {
                        this.GridviewSortingAndPaging(null);
                        this.LabelMessage.Text = Resources.lang.MessageDeleteCarSegment;

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

        protected void GridviewCarSegments_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {


            string sortExpression = string.Empty;
            if ((SessionHandler.MappingCarSegmentSortOrder == " ASC"))
            {
                SessionHandler.MappingCarSegmentSortOrder = " DESC";
                SessionHandler.MappingCarSegmentSortDirection = (int)App.BLL.Mappings.SortDirection.Descending;
                sortExpression = e.SortExpression.ToString() + SessionHandler.MappingCarSegmentSortOrder;
            }
            else
            {
                SessionHandler.MappingCarSegmentSortOrder = " ASC";
                SessionHandler.MappingCarSegmentSortDirection = (int)App.BLL.Mappings.SortDirection.Ascending;
                sortExpression = e.SortExpression.ToString();
            }

            SessionHandler.MappingCarSegmentSortExpression = sortExpression.ToString();

            //Load Data
            GridviewSortingAndPaging(sortExpression);

        }

        protected void GridviewCarSegments_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
                            

                            string sortExpression = SessionHandler.MappingCarSegmentSortExpression;
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
                                    if (SessionHandler.MappingCarSegmentSortDirection == (int)App.BLL.Mappings.SortDirection.Ascending)
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

        protected void GridviewCarSegments_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {



            if ((e.Row != null) && e.Row.RowType == DataControlRowType.DataRow)
            {
                int cellCount = this.GridviewCarSegments.Columns.Count;
                int lastColumnIndex = cellCount - 1;

                LinkButton linkButtonDelete = (LinkButton)e.Row.Cells[lastColumnIndex].FindControl("LinkButtonDelete");
                string uniqueId = linkButtonDelete.UniqueID;
                string carSegment = Convert.ToString(e.Row.Cells[0].Text);
                string message = "Are you sure you want to delete the car segment " + carSegment + " ?";
                linkButtonDelete.Attributes.Add("onclick", "return ShowConfirm('" + uniqueId + "','" + message + "')");

            }


        }

        protected void GetPageIndex(object sender, System.EventArgs e)
        {

            //Load Data
            GridviewSortingAndPaging(SessionHandler.MappingCarSegmentSortExpression);

        }

        protected void DropDownListRows_SelectedIndexChanged(object sender, System.EventArgs e)
        {

            int totalRecords = Convert.ToInt32(this.LabelTotalRecordsDisplay.Text);
            if (totalRecords > 10)
            {
                //Load Data
                GridviewSortingAndPaging(SessionHandler.MappingCarSegmentSortExpression);
            }

        }

        protected void DropDownListPage_SelectedIndexChanged(object sender, System.EventArgs e)
        {

            //Load Data
            GridviewSortingAndPaging(SessionHandler.MappingCarSegmentSortExpression);

        }

        protected void GridviewSortingAndPaging(string sortExpression)
        {


            if (sortExpression == null)
            {
                sortExpression = SessionHandler.MappingCarSegmentSortExpression;
            }
            this.LoadData(sortExpression);
            this.UpdatePanelMappingGridview.Update();


        }

        #endregion

        #region "Click Events"
        protected void ButtonAddCarSegment_Click(object sender, System.EventArgs e)
        {

            SessionHandler.MappingCarSegmentDefaultMode = (int)App.BLL.Mappings.Mode.Insert;
            SessionHandler.MappingCarSegmentValidationGroup = "CarSegmentInsert";
            this.MappingCarSegmentDetails.LoadDetails();
            this.MappingCarSegmentDetails.ModalExtenderMapping.Show();
            this.UpdatePanelMappingGridview.Update();

        }

        protected void MappingDetailsSave_Click(object sender, System.EventArgs e)
        {
            this.LabelMessage.Text = this.MappingCarSegmentDetails.ErrorMessage;
            this.GridviewSortingAndPaging(null);
            this.MappingCarSegmentDetails.ModalExtenderMapping.Hide();
            this.UpdatePanelMappingGridview.Update();

        }
        #endregion
    }
}