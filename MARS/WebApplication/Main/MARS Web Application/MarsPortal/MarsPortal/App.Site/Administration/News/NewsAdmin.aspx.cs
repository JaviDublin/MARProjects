using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.DAL.MarsDataAccess;
using App.BLL.ExtensionMethods;

namespace Mars.App.Site.Administration.News
{
    public partial class NewsAdmin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                NewsGrid();
                pnlAddNews.Visible = false;
                ClearItems();
            }
        }


        #region Functions
        protected void NewsGrid()
        {
            var comData = CommunicationDataAccess.GetAllNewsItems();
            grdNews.DataSource = comData;
            grdNews.DataBind();
        }


        protected void ClearItems()
        {
            txtDetails.Text = "";
            txtHeading.Text = "";
            rblIsActive.Items [0].Selected  = true;
            rblIsPriority.Items[1].Selected = true;

            pnlAddNews.GroupingText = "Add News";
            hfNewsID.Value = "";
            lblErrorMessage.Text = "";
            btnAddNews.Visible = true;
        }

        private void EditNewsItem(int intNewsID)
        {
            pnlAddNews .Visible = true;
           
            var comDataRow = CommunicationDataAccess.SelectNewsItem(intNewsID);
            txtDetails.Text = comDataRow.Details;
            txtHeading.Text = comDataRow.Heading;
          

            if ((comDataRow .IsActive  == true))
            {
                rblIsActive.Items[0].Selected = true;
            }
            else
            {
                rblIsActive.Items[1].Selected = true;
            }

            if ((comDataRow.Priority == true))
            {
                rblIsPriority.Items[0].Selected = true;
            }
            else
            {
                rblIsPriority.Items[1].Selected = true;
            }

        }

        private void DeleteNewsItem(int intNewsID)
        {
            CommunicationDataAccess.DeleteNewsItem(intNewsID);
            ClearItems();
            NewsGrid();
 
        }

        #endregion

        #region News Panel Controls
        protected void btnAddNews_Click(object sender, EventArgs e)
        {
            ClearItems();
            pnlAddNews.Visible = true;
            btnAddNews.Visible = false;
        }

        protected void btnCancelNews_Click(object sender, EventArgs e)
        {
            pnlAddNews.Visible = false;
            ClearItems();
            
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strUserID = "";
            string strHeading = "";
            string strDetails = "";
            bool isActive = false;
            bool isPriority = false;

            strHeading = txtHeading.Text;
            strDetails = txtDetails.Text;
            //Validation
            if ((txtHeading.Text == ""))
            {
                lblErrorMessage.Text = "Please enter in a Heading";
                return;
            }

         
            if ((txtDetails.Text == ""))
            {
                lblErrorMessage.Text = "Please enter in Details";
                return;
            }

            strUserID = this.Page.RadUserId();

            if (( rblIsActive.Items[0].Selected == true))
            {
                isActive = true;
            }
            else
            {
                isActive = false;
            }

            if ((rblIsPriority.Items[0].Selected == true))
            {
                isPriority = true;
            }
            else
            {
                isPriority = false;
            }

            
            if (!String.IsNullOrEmpty(hfNewsID.Value) ) //Update

            {
                int newsID;
                newsID = int.Parse (hfNewsID.Value);

                CommunicationDataAccess.UpdateNews (newsID,strUserID, strHeading, strDetails, isActive, isPriority);
            }
            else //Save
            { 
                CommunicationDataAccess.SaveNews(strUserID, strHeading, strDetails, isActive, isPriority);
            }
            
            pnlAddNews.Visible = false;
            NewsGrid();
            ClearItems();
          

        }

#endregion

        #region Grid Controls
        protected void grdNewsEdit(object sender, CommandEventArgs e)
        {
            if ((e.CommandName == "EditItem"))
            {
                int intNewsItemID = int.Parse(e.CommandArgument.ToString());
                pnlAddNews.GroupingText = "Edit News";
                hfNewsID.Value = e.CommandArgument.ToString();
                EditNewsItem(intNewsItemID);
                btnAddNews.Visible = false;
            }

            if ((e.CommandName == "DeleteItem"))
            {
                int intNewsItemID = int.Parse(e.CommandArgument.ToString());
                
                hfNewsID.Value = e.CommandArgument.ToString();
                DeleteNewsItem(intNewsItemID);
                
            }
            
        }





        #endregion
     

    }
}