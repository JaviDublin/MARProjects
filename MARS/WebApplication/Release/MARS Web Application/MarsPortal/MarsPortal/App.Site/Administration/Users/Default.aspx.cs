using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL;

namespace App.Management.Users
{
    public partial class Default : PageBase
    {
        #region "Page Events"


        protected void Page_Load(object sender, System.EventArgs e)
        {

            //Set Page title
            this.Page.Title = "MARS - Maintenance Users";
            this.UserControlPageInformation.LastUpdateLabel.Visible = false;

            //Settings for pager control
            this.PagerControlUsers.GridviewToPage = this.GridviewUsers;
            this.PagerControlUsers.GridviewSessionValues = (int)Gridviews.GridviewToPage.MaintenanceUsers;

            if (!Page.IsPostBack)
            {


                //Set page informtion on usercontrol
                base.SetPageInformationTitle("UserMaintenance", this.UserControlPageInformation, false);

                this.LabelMessage.Text = string.Empty;
                SessionHandler.ClearMaintanenceUsersSessions();
                //Settings for pager control
                //Set Default Sort Order
                SessionHandler.MaintanenceUsersSortOrder = "ASC";
                //Set current Page number and size
                SessionHandler.MaintanenceUsersCurrentPageNumber = 1;
                SessionHandler.MaintanenceUsersPageSize = 10;

                //Load Data
                LoadUsersData();


            }

        }

        #endregion

        #region "Click Events"

        protected void ButtonAddUser_Click(object sender, System.EventArgs e)
        {
            SessionHandler.MaintanenceUsersValidationGroup = "UserInsert";
            SessionHandler.MaintanenceUsersDefaultMode = (int)App.BLL.Users.Mode.Insert;
            this.UserDetails.LoadUserDetails();
            this.UserDetails.ModalExtenderUserDetails.Show();
            this.UpdatePanelMaintenanceUsers.Update();


        }


        protected void UserSave_Click(object sender, System.EventArgs e)
        {

            this.LabelMessage.Text = this.UserDetails.ErrorMessage;
            this.GridviewSortingAndPaging(null);
            this.UserDetails.ModalExtenderUserDetails.Hide();
            this.UpdatePanelMaintenanceUsers.Update();

        }
        #endregion

        #region "Gridview Events"


        protected void LoadUsersData()
        {

            App.BLL.Users.SelectUsers(Convert.ToInt32(SessionHandler.MaintanenceUsersPageSize), Convert.ToInt32(SessionHandler.MaintanenceUsersCurrentPageNumber), SessionHandler.MaintanenceUsersSortExpression, this.PanelUsers, this.PagerControlUsers.PagerButtonFirst, this.PagerControlUsers.PagerButtonNext, this.PagerControlUsers.PagerButtonPrevious, this.PagerControlUsers.PagerButtonLast, this.PagerControlUsers.PagerLabelTotalPages, this.PagerControlUsers.PagerDropDownListPage,
             this.GridviewUsers, this.LabelTotalRecordsDisplay, this.EmptyDataTemplateUsers);

        }


        protected void GridviewUsers_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {

            string racfId = null;
            int index = -1;
            GridViewRow row = null;

            switch (e.CommandName)
            {

                case "EditUser":
                    index = Convert.ToInt32(e.CommandArgument);
                    row = this.GridviewUsers.Rows[index];
                    string name = null;

                    racfId = Convert.ToString(row.Cells[0].Text);
                    name = Convert.ToString(row.Cells[1].Text);

                    this.UserDetails.RACFID = racfId;
                    this.UserDetails.Name = name;

                    SessionHandler.MaintanenceUsersValidationGroup = "UserUpdate";
                    SessionHandler.MaintanenceUsersDefaultMode = (int)App.BLL.Users.Mode.Edit;
                    this.UserDetails.LoadUserDetails();
                    this.UserDetails.ModalExtenderUserDetails.Show();
                    this.UpdatePanelMaintenanceUsers.Update();

                    break;
                case "DeleteUser":

                    index = Convert.ToInt32(e.CommandArgument);
                    row = this.GridviewUsers.Rows[index];
                    racfId = Convert.ToString(row.Cells[0].Text);

                    int result = App.BLL.Users.DeleteUser(racfId);

                    if (result == 0)
                    {
                        this.GridviewSortingAndPaging(null);
                        this.LabelMessage.Text = Resources.lang.MessageDeleteUser;
                    }
                    else
                    {
                        this.LabelMessage.Text = Resources.lang.ErrorMessageAdministrator;
                    }

                    this.UpdatePanelMaintenanceUsers.Update();
                    break;
            }



        }


        protected void GridviewUsers_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
                            //image.ImageUrl = "~/App.Images/sort-blank21.gif";

                            string sortExpression = SessionHandler.MaintanenceUsersSortExpression;
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
                                    string s = SessionHandler.MaintanenceUsersSortDirection.ToString();
                                    if (SessionHandler.MaintanenceUsersSortDirection == (int)App.BLL.Users.SortDirection.Ascending)
                                    {
                                        image.ImageUrl = "~/App.Images/sort-ascending.gif";
                                    }
                                    else
                                    {
                                        image.ImageUrl = "~/App.images/sort-descending.gif";
                                    }
                                    cell.Controls.Add(image);
                                }
                            }

                            
                        }
                    }
                }
            }
            this.UpdatePanelMaintenanceUsers.Update();

        }


        protected void GridviewUsers_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {

            if ((e.Row != null) && e.Row.RowType == DataControlRowType.DataRow)
            {
                int cellCount = this.GridviewUsers.Columns.Count;
                int lastColumnIndex = cellCount - 1;

                LinkButton linkButtonDelete = (LinkButton)e.Row.Cells[lastColumnIndex].FindControl("LinkButtonUserDelete");
                string uniqueId = linkButtonDelete.UniqueID;
                string user = Convert.ToString(e.Row.Cells[1].Text);
                string message = "Are you sure you want to delete the user " + user + " ? ";

                linkButtonDelete.Attributes.Add("onclick", "return ShowConfirm('" + uniqueId + "','" + message + "')");
            }

        }


        protected void GridviewUsers_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            string sortExpression = string.Empty;
            if ((SessionHandler.MaintanenceUsersSortOrder == " ASC"))
            {
                SessionHandler.MaintanenceUsersSortOrder = " DESC";
                SessionHandler.MaintanenceUsersSortDirection = (int)App.BLL.Users.SortDirection.Descending;
                sortExpression = e.SortExpression.ToString() + SessionHandler.MaintanenceUsersSortOrder;
            }
            else
            {
                SessionHandler.MaintanenceUsersSortOrder = " ASC";
                SessionHandler.MaintanenceUsersSortDirection = (int)App.BLL.Users.SortDirection.Ascending;
                sortExpression = e.SortExpression.ToString();
            }

            SessionHandler.MaintanenceUsersSortExpression = sortExpression.ToString();

            //Load Data
            GridviewSortingAndPaging(sortExpression);

        }


        protected void GetPageIndex(object sender, System.EventArgs e)
        {
            //Load Data
            GridviewSortingAndPaging(SessionHandler.MaintanenceUsersSortExpression);

        }


        protected void DropDownListRows_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            int totalRecords = Convert.ToInt32(this.LabelTotalRecordsDisplay.Text);
            if (totalRecords > 10)
            {
                //Load Data
                GridviewSortingAndPaging(SessionHandler.MaintanenceUsersSortExpression);
            }

        }


        protected void DropDownListPage_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            //Load Data
            GridviewSortingAndPaging(SessionHandler.MaintanenceUsersSortExpression);

        }


        protected void GridviewSortingAndPaging(string sortExpression)
        {

            if (sortExpression == null)
            {
                sortExpression = SessionHandler.MaintanenceUsersSortExpression;
            }

            App.BLL.Users.SelectUsers(Convert.ToInt32(SessionHandler.MaintanenceUsersPageSize), Convert.ToInt32(SessionHandler.MaintanenceUsersCurrentPageNumber), sortExpression,
                    this.PanelUsers, this.PagerControlUsers.PagerButtonFirst, this.PagerControlUsers.PagerButtonNext, this.PagerControlUsers.PagerButtonPrevious,
                    this.PagerControlUsers.PagerButtonLast, this.PagerControlUsers.PagerLabelTotalPages, this.PagerControlUsers.PagerDropDownListPage,
            this.GridviewUsers, this.LabelTotalRecordsDisplay, this.EmptyDataTemplateUsers);


        }

        #endregion

        protected void BnSearchUserClick(object sender, EventArgs e)
        {
            // 1. Search for user by criteia 1, search type, 2 search value
            // 2. rebind gridView 
            App.BLL.Users.SelectUsersbyFilter(PanelUsers, this.PagerControlUsers.PagerButtonFirst,
                this.PagerControlUsers.PagerButtonNext, this.PagerControlUsers.PagerButtonPrevious,
                this.PagerControlUsers.PagerButtonLast, this.PagerControlUsers.PagerLabelTotalPages,
                this.PagerControlUsers.PagerDropDownListPage,
                this.GridviewUsers, this.LabelTotalRecordsDisplay, this.EmptyDataTemplateUsers, tbSearchBox.Text);

            PagerControlUsers.Visible = false;

        }
    }
}