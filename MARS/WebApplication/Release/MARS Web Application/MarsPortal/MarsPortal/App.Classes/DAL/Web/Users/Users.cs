using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.DAL.Data;

namespace App.BLL
{
    public class Users
    {

        #region "Enums"
        public enum SortDirection : int
        {
            Ascending = 1,
            Descending = 2
        }

        public enum Mode : int
        {
            Insert = 1,
            Edit = 2
        }
        #endregion

        #region "Functions"
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetUserRACFID()
        {

            if ((HttpContext.Current.User.Identity.IsAuthenticated))
            {
                //Return RACFID
                return HttpContext.Current.User.Identity.Name.ToString();
            }
            else
            {
                return null;
            }
        }


        public static int CheckRACFIDExits(string racfId)
        {

            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_UserCheckExists, con);

            //Set Parameters
            cmd.Parameters.AddWithValue("@racfId", racfId);

            //Execute Command
            int result = 0;
            using (con)
            {
                con.Open();
                result = Convert.ToInt32(cmd.ExecuteScalar());
            }
            con.Close();
            return result;
        }



        // Select users by search criteria
        public static void SelectUsersbyFilter(Panel usersPanel, Button buttonFirst, Button buttonNext,
            Button buttonPrevious, Button buttonLast, Label labelTotalPages, DropDownList dropDownListPage,
            GridView gridviewUsers, Label labelTotalRecords, UserControl emptyDataTemplate, string searchValue)
        {
            int rowCount = 0;
            var results = new List<UserDetails>();

            //Initialise Connection
            using (var con = DBManager.CreateConnection())
            {
                var cmd = DBManager.CreateProcedure(StoredProcedures.Portal_UserSelectFilter, con);
                cmd.Parameters.AddWithValue("@searchValue", searchValue);
                con.Open();
                
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new UserDetails(reader));
                    rowCount++;
                }
            }


            //Show reservation panel
            usersPanel.Visible = true;
            //Hide Empty Data Template
            emptyDataTemplate.Visible = false;
            //Databind the gridview
            gridviewUsers.DataSource = results;
            gridviewUsers.DataBind();
            //Display results
            labelTotalPages.Text = "1";
            //Show totals label
            labelTotalRecords.Text = rowCount.ToString();
            //Clear items in page drop down list
            dropDownListPage.Items.Clear();

            // Set Paging objects to default, not needed for filter search 
            buttonPrevious.Enabled = false;
            buttonPrevious.CssClass = "PagerPreviousInactive";
            buttonFirst.Enabled = false;
            buttonFirst.CssClass = "PagerFirstInactive";

            if ((Convert.ToInt32(labelTotalPages.Text) > 1))
            {
                buttonNext.Enabled = true;
                buttonNext.CssClass = "PagerNextActive";
                buttonLast.Enabled = true;
                buttonLast.CssClass = "PagerLastActive";
            }
            else
            {
                buttonNext.Enabled = false;
                buttonNext.CssClass = "PagerNextInactive";
                buttonLast.Enabled = false;
                buttonLast.CssClass = "PagerLastInactive";
            }
        }


        public static void SelectUsers(int pageSize, int currentPageNumber, string sortExpression, Panel usersPanel, Button buttonFirst, Button buttonNext, Button buttonPrevious, Button buttonLast, Label labelTotalPages, DropDownList dropDownListPage,
        GridView gridviewUsers, Label labelTotalRecords, UserControl emptyDataTemplate)
        {

            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_UserSelectAll, con);

            int startRowIndex = ((currentPageNumber - 1) * pageSize) + 1;
            int maximumRows = (currentPageNumber * pageSize);


            cmd.Parameters.AddWithValue("@startRowIndex", startRowIndex);
            cmd.Parameters.AddWithValue("@maximumRows", maximumRows);

            if (sortExpression == null)
            {
                cmd.Parameters.AddWithValue("@sortExpression", System.DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@sortExpression", sortExpression);
            }

            //Execute Command
            List<UserDetails> results = new List<UserDetails>();
            int rowCount = 0;
            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(new UserDetails(reader));
                }

                reader.NextResult();

                while (reader.Read())
                {
                    rowCount = Convert.ToInt32(reader["totalCount"]);
                }

            }
            con.Close();



            if (rowCount >= 1)
            {
                //Show reservation panel
                usersPanel.Visible = true;
                //Hide Empty Data Template
                emptyDataTemplate.Visible = false;

                //Databind the gridview
                gridviewUsers.DataSource = results;
                gridviewUsers.DataBind();

                //Display results
                int totalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(rowCount) / Convert.ToDouble(pageSize)));
                labelTotalPages.Text = totalPages.ToString();

                //Show totals label
                labelTotalRecords.Text = rowCount.ToString();

                //Clear items in page drop down list
                dropDownListPage.Items.Clear();
                //Add list of pages to drop down list
                for (int i = 1; i <= Convert.ToInt32(labelTotalPages.Text); i++)
                {
                    dropDownListPage.Items.Add(new ListItem(i.ToString()));
                }
                //Set the selected page
                dropDownListPage.SelectedValue = currentPageNumber.ToString();

                //Set pager buttons depending on page selected

                if (currentPageNumber == 1)
                {
                    buttonPrevious.Enabled = false;
                    buttonPrevious.CssClass = "PagerPreviousInactive";
                    buttonFirst.Enabled = false;
                    buttonFirst.CssClass = "PagerFirstInactive";

                    if ((Convert.ToInt32(labelTotalPages.Text) > 1))
                    {
                        buttonNext.Enabled = true;
                        buttonNext.CssClass = "PagerNextActive";
                        buttonLast.Enabled = true;
                        buttonLast.CssClass = "PagerLastActive";
                    }
                    else
                    {
                        buttonNext.Enabled = false;
                        buttonNext.CssClass = "PagerNextInactive";
                        buttonLast.Enabled = false;
                        buttonLast.CssClass = "PagerLastInactive";
                    }
                }
                else
                {
                    buttonPrevious.Enabled = true;
                    buttonPrevious.CssClass = "PagerPreviousActive";
                    buttonFirst.Enabled = true;
                    buttonFirst.CssClass = "PagerFirstActive";

                    if ((currentPageNumber == Convert.ToInt32(labelTotalPages.Text)))
                    {
                        buttonNext.Enabled = false;
                        buttonNext.CssClass = "PagerNextInactive";
                        buttonLast.Enabled = false;
                        buttonLast.CssClass = "PagerLastInactive";
                    }
                    else
                    {
                        buttonNext.Enabled = true;
                        buttonNext.CssClass = "PagerNextActive";
                        buttonLast.Enabled = true;
                        buttonLast.CssClass = "PagerLastActive";

                    }
                }

            }
            else
            {
                //Hide reservation panel
                usersPanel.Visible = false;
                //Show Empty Data Template
                emptyDataTemplate.Visible = true;
            }


        }

        public static int InsertUser(string racfId, string name)
        {

            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_UserInsert, con);

            //Set Parameters
            cmd.Parameters.AddWithValue("@racfId", racfId);
            cmd.Parameters.AddWithValue("@name", name);
            //Execute Command
            using (con)
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            con.Close();
            //Success
            return 0;

        }

        public static int UpdateUser(string racfId, string name)
        {

            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_UserUpdate, con);

            //Set Parameters
            cmd.Parameters.AddWithValue("@racfId", racfId);
            cmd.Parameters.AddWithValue("@name", name);
            //Execute Command
            using (con)
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            con.Close();
            //Success
            return 0;

        }

        public static int UpdateUserRoles(string racfId, int roleId, string country)
        {

            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_UserUpdateRoles, con);

            //Set Parameters
            cmd.Parameters.AddWithValue("@racfId", racfId);
            cmd.Parameters.AddWithValue("@roleId", roleId);
            if (country == null)
            {
                cmd.Parameters.AddWithValue("@country", System.DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@country", country);
            }

            //Execute Command
            using (con)
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            con.Close();
            //Success
            return 0;

        }

        public static int DeleteUser(string racfId)
        {

            //Initialise Connection
            SqlConnection con = DBManager.CreateConnection();
            SqlCommand cmd = DBManager.CreateProcedure(StoredProcedures.Portal_UserDelete, con);

            //Set Parameters
            cmd.Parameters.AddWithValue("@racfId", racfId);
            //Execute Command
            using (con)
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            con.Close();
            //Success
            return 0;

        }


        #endregion

        #region Classes

        public class UserDetails
        {

            #region Properties and Fields
            private string _racfId;
            private string _name;

            private string _roles;
            public string Racfid
            {
                get { return _racfId; }
            }

            public string Name
            {
                get { return _name; }
            }

            public string Roles
            {
                get { return _roles; }
            }

            #endregion

            #region Constructors

            public UserDetails(SqlDataReader reader)
            {
                if (reader["racfid"] != DBNull.Value)
                {
                    _racfId = Convert.ToString(reader["racfid"]);
                }
                if (reader["name"] != DBNull.Value)
                {
                    _name = Convert.ToString(reader["name"]);
                }
                if (reader["roles"] != DBNull.Value)
                {
                    _roles = Convert.ToString(reader["roles"]);
                }

            }

            #endregion

        }

        #endregion

    }
}