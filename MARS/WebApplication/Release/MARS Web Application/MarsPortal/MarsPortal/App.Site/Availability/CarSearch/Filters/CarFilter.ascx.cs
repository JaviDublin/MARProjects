using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using App.BLL.Workers;
using App.BLL; //--Added--
using App.BLL.Utilities;

namespace App.AvailabilityTool.CarSearch.Filters
{
    public partial class CarFilter : System.Web.UI.UserControl
    {
        // created: 28/3/12 by Gavin
        // handles CarFilter User Control
        // Uses CSVWorker to generate reports in the app_classes/DAL folder

        #region delegates
        
        public delegate void FilterClickEventHandler(object o, EventArgs e);
        public delegate void DownloadClickEventHandler(object o, EventArgs e);
        
        #endregion

        #region properties
        
        public TextBox TextBoxLicense;
        public TextBox TextBoxUnit;
        public TextBox TextBoxVin;
        public TextBox TextBoxName;
        public TextBox TextBoxColour;
        public TextBox TextBoxMiles;
        public TextBox TextBoxModel;
        public TextBox TextBoxModDesc;
        
        #endregion

        #region public methods
        public void generateExcel(List<App.BLL.AvailabilityCarSearch.CarSearchDetails> l)
        {

            //Set directory variable
            string tempDirectory = Mars.Properties.Settings.Default.TempDirectory;

            //Clear Files from directory
            Helper.DeleteTempFiles(tempDirectory);

            //Create csv and download
            CSVWorker _csvWorker = new CSVWorker("MarsReport", Server.MapPath(tempDirectory), l);
            downloadFile(_csvWorker.FileName, tempDirectory);

            //CSVWorker _csvWorker = new CSVWorker("MarsReport", Server.MapPath("~/Downloads/"), l);
            //downloadFile(_csvWorker.FileName, "~/Downloads/");
        }
        #endregion

        #region protected
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                clearTextBoxes();
            }
        }
        protected void Page_Unload(object o, EventArgs e)
        {
            clearTextBoxes();
        }
        protected void ButtonCarFilter_Click(object sender, EventArgs e)
        {
            onFilterClick(EventArgs.Empty);
        }
        protected void ButtonClear_Click(object sender, EventArgs e)
        {
            clearTextBoxes();
            onFilterClick(EventArgs.Empty);
        }
        protected void ButtonDownload_Click(object sender, EventArgs e)
        {
            onDownloadClick(EventArgs.Empty);
        }
        #endregion

        #region events
        public event FilterClickEventHandler _filterClickEventHandler;
        public virtual void onFilterClick(EventArgs e)
        {
            if (_filterClickEventHandler != null) _filterClickEventHandler(this, e);
        }
        public event DownloadClickEventHandler _downloadClickEventHandler;
        public virtual void onDownloadClick(EventArgs e)
        {
            if (_downloadClickEventHandler != null) _downloadClickEventHandler(this, e);
        }
        #endregion

        #region worker methods
        private void clearTextBoxes()
        {
            TextBoxLicense.Text = "";
            TextBoxMiles.Text = "";
            TextBoxName.Text = "";
            TextBoxUnit.Text = "";
            TextBoxVin.Text = "";
            TextBoxModel.Text = "";
            TextBoxModDesc.Text = "";
            TextBoxColour.Text = "";
        }
        public void downloadFile(string fileName, string filePath)
        { // save fileName to filePath
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ";");
            Response.Redirect(filePath + fileName, false);
            Response.End();
        }
        #endregion
    }
}