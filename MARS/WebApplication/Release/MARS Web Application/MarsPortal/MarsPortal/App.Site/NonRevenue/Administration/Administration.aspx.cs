using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AjaxControlToolkit;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.Phase4Dal.NonRev;
using Mars.App.Classes.Phase4Dal.NonRev.Entities.FileUploadEntities;

namespace Mars.App.Site.NonRevenue.Administration
{
    public partial class Administration : Page
    {
        private const string DataToUploadSessionName = "DataToUploadSessionName";

        public int SelectedTab { get; set; }

        private UploadSummary DataToUpload
        {
            get { return (UploadSummary) Session[DataToUploadSessionName]; }
            set { Session[DataToUploadSessionName] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblConfirmMessage.Text = string.Empty;
            lblErrorMessage.Text = string.Empty;
            if (!IsPostBack)
            {
                PopulateReasonGrid();
            }
        }

   


        protected void btnNewReason_Click(object sender, EventArgs e)
        {
            SelectedTab = 0;
            tbReason.Text = string.Empty;
            cbActive.Checked = true;
            mpeNonRevOverview.Show();
        }

        protected void btnSaveReason_Click(object sender, EventArgs e)
        {
            SelectedTab = 0;
            
            using (var dataAccess = new AdminDataAccess(null))
            {
                if (hfSelectedReason.Value != string.Empty)
                {
                    var reasonId = int.Parse(hfSelectedReason.Value);
                    dataAccess.UpdateReason(reasonId, tbReason.Text, cbActive.Checked);    
                }
                else
                {
                    var result = dataAccess.AddNewReason(tbReason.Text, cbActive.Checked);
                    if (result != string.Empty)
                    {
                        lblErrorMessage.Text = result;
                        mpeNonRevOverview.Show();
                    }
                }
            }
            hfSelectedReason.Value = string.Empty;
            PopulateReasonGrid();
        }

        protected void gvReasons_Edit(object sender, CommandEventArgs e)
        {
            SelectedTab = 0;
            if ((e.CommandName == "EditItem"))
            {
                var reasonId = int.Parse(e.CommandArgument.ToString());
                hfSelectedReason.Value = reasonId.ToString(CultureInfo.InvariantCulture);
                using (var dataAccess = new AdminDataAccess(null))
                {
                    var reason = dataAccess.GetReason(reasonId);
                    tbReason.Text = reason.RemarkText;
                    cbActive.Checked = reason.isActive.HasValue && reason.isActive.Value;
                }
                mpeNonRevOverview.Show();
            }
        }

        protected void btnParse_Click(object sender, EventArgs e)
        {
            SelectedTab = 1;
            tbUploadResults.Text = string.Empty;
            if (!fuReasonsUpload.HasFile) return;
            try
            {
                
                var uploadSummary = ParseHttpUploadedFile(fuReasonsUpload.PostedFile);

                using (var dataAccess = new AdminDataAccess(null))
                {
                    DataToUpload = dataAccess.FillUploadSummary(uploadSummary);
                    
                    tbUploadResults.Text = uploadSummary.ErrorList.ToString();
                }
            }
            catch (Exception ex)
            {
                tbUploadResults.Text = "Unable to upload the selected file.";    
            }
        }

        protected void btnSubmitUpload_Click(object sender, EventArgs e)
        {
            SelectedTab = 1;
            var dataToBeUploaded = DataToUpload.DataToBeUploaded;
            using (var dataAccess = new AdminDataAccess(null))
            {
                var result = dataAccess.UploadRows(dataToBeUploaded, Rad.Security.ApplicationAuthentication.GetGlobalId());
                lblConfirmMessage.Text = result;
                mpeConfirmWindow.Show();

                tbUploadResults.Text = string.Empty;
            }         
        }

        private void PopulateReasonGrid()
        {
            List<NonRev_Remarks_List> reasons;
            using (var dataAccess = new AdminDataAccess(null))
            {
                reasons = dataAccess.GetReasonEntries();
            }
            reasons = reasons.OrderBy(d => d.RemarkText).ToList();

            //lbReasons.Items.Clear();
            //lbReasons.Items.AddRange(reasons.ToArray());
            gvReasons.DataSource = reasons;
            gvReasons.DataBind();

        }

        private UploadSummary ParseHttpUploadedFile(HttpPostedFile file)
        {
            var returned = new UploadSummary();
            var fileData = new byte[file.ContentLength];
            file.InputStream.Read(fileData, 0, file.ContentLength);

            using (var ms = new MemoryStream(fileData))
            {
                var sr = new StreamReader(ms);


                while (!sr.EndOfStream)
                {
                    var dataRow = sr.ReadLine();
                    if (dataRow == null) continue;
                    var splitDataRow = dataRow.Split(',');

                    var rawData = new UploadRow
                                     {
                                         OwningCountry = splitDataRow[0].Trim().ToUpper(),
                                         Vin = splitDataRow[1].Trim(),
                                         EstimatedResolved = splitDataRow[2].Trim(),
                                         ReasonName = splitDataRow[3].Trim(),
                                         RemarkText = splitDataRow[4].Trim(),
                                     };
                    returned.DataToBeUploaded.Add(rawData);
                }
            }
            return returned;
        }
        
    }
}