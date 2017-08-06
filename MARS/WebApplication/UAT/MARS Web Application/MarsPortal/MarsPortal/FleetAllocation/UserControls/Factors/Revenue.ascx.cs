using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.FleetAllocation.BusinessLogic.DropdownLists;
using Mars.FleetAllocation.DataAccess;
using Mars.FleetAllocation.DataAccess.Entities;

namespace Mars.FleetAllocation.UserControls.Factors
{
    public partial class Revenue : UserControl
    {
        public const string FaoParameterSessionName = "FaoRevenueParameterSessionName";

        public Dictionary<DictionaryParameter, string> SessionStoredParameters
        {
            get { return (Dictionary<DictionaryParameter, string>)Session[FaoParameterSessionName]; }
            set { Session[FaoParameterSessionName] = value; }
        }

        private const string FaoSessionDataGrid = "FaoRevenueSessionDataGrid";

        private const string FaoRevenueToUploadSessionName = "FaoRevenueToUploadSessionName";

        private List<DataContext.RevenueByCommercialCarSegment> EntitiesToUpload
        {
            get
            {
                return (List<DataContext.RevenueByCommercialCarSegment>)Session[FaoRevenueToUploadSessionName];
            }
            set { Session[FaoRevenueToUploadSessionName] = value; }
        }

        public string GetUpdatePanelId { get { return ucParameters.UpdatePanelClientId; } }

        protected int RowCount { get { return int.Parse(hfRecordCount.Value); } }

        public int SelectedPanel
        {
            get { return int.Parse(hfSelectedPanel.Value); }
            set { hfSelectedPanel.Value = value.ToString(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ucMaxFactors.GridItemType = typeof(RevenueRow);
            ucMaxFactors.SessionNameForGridData = FaoSessionDataGrid;
            ucMaxFactors.ColumnHeaders = RevenueRow.HeaderRows;
            ucMaxFactors.ColumnFormats = RevenueRow.Formats;
            ucParameters.SessionStoredFaoMinCommSegParameters = SessionStoredParameters;

            if (IsPostBack)
            {
                if (fuFaoRevenueFile.PostedFile != null)
                {
                    if (hfFaoUploadRevFile.Value != string.Empty)
                    {
                        var file = fuFaoRevenueFile.PostedFile;
                        SelectedPanel = 3; //Bring the panel pack here after a full page postback
                        ParseUploadedFile(file);
                        btnUpload.Visible = true;
                        hfFaoUploadRevFile.Value = string.Empty;
                    }

                }
                else
                {
                    SelectedPanel = 0;
                }
            }
            else
            {
                ListGenerator.FillDropdownWithFaoCountries(ddlCountry);
            }
        }


        private void ParseUploadedFile(HttpPostedFile file)
        {
            try
            {
                var fileData = new byte[file.ContentLength];
                file.InputStream.Read(fileData, 0, file.ContentLength);

                var parsedData = new List<RevenueDatabaseRow>();
                using (var ms = new MemoryStream(fileData))
                {
                    var sr = new StreamReader(ms);
                    string dataRow;
                    if (!sr.EndOfStream)
                    {
                        dataRow = sr.ReadLine(); //Skip Header Row
                    }

                    while (!sr.EndOfStream)
                    {
                        dataRow = sr.ReadLine();
                        if (dataRow == null) continue;
                        var splitDataRow = dataRow.Split(',');

                        var reportingDate = DateTime.Parse(splitDataRow[0]);
                        var dwCountry = splitDataRow[1].Trim();
                        var locationCode = splitDataRow[2].Trim();
                        var carGroupCode = splitDataRow[3].Trim();
                        var commercialCarSegmentCode = splitDataRow[4].Trim();
                        var rentalCount = int.Parse(splitDataRow[5]);
                        var daysDriven = double.Parse(splitDataRow[6]);
                        var rtDays = int.Parse(splitDataRow[7]);
                        var tranDays = int.Parse(splitDataRow[8]);
                        var grossRev = double.Parse(splitDataRow[9]);
                        var performanceRev = double.Parse(splitDataRow[10]);


                        var rawData = new RevenueDatabaseRow
                        {
                            CarGroupCode = carGroupCode,
                            CommercialCarSegmentCode = commercialCarSegmentCode,
                            DaysDriven = daysDriven,
                            DwCountry = dwCountry,
                            LocationCode = locationCode,
                            GrossRev = grossRev,
                            PerformanceRevenue = performanceRev,
                            RentalCount = rentalCount,
                            ReportingDate = reportingDate,
                            RtDays = rtDays,
                            TranDays = tranDays
                        };
                        parsedData.Add(rawData);
                    }
                }
                List<DataContext.RevenueByCommercialCarSegment> dbEntities;
                using (var dataAccess = new RevenueDataAccess(null))
                {
                    dbEntities = dataAccess.MatchUploadToDatabaseEntities(parsedData);
                }
                var minDate = dbEntities.Min(d => d.MonthDate);
                var maxDate = dbEntities.Max(d => d.MonthDate);

                cbeUploadWarning.ConfirmText = string.Format("Confirming will wipe all Revenue entries from {0:MMM yyyy} onwards and replace them with Data from the selected file.", minDate);
                lblFileUploadSummary1.Text = string.Format("Date Range: {0:MMM-yyyy} - {1:MMM-yyyy}", minDate, maxDate);
                lblFileUploadSummary2.Text = string.Format("Rows Parsed: {0} Rows Matched: {1}", parsedData.Count,
                    dbEntities.Count);
                EntitiesToUpload = dbEntities;
            }
            catch (Exception ex)
            {
                lblFileUploadSummary1.Text = ex.ToString();
            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            var parameters = ucParameters.GetParameters();

            
            using (var dataAccess = new RevenueDataAccess(parameters))
            {
                var data = dataAccess.GetRevenueData();
                hfRecordCount.Value = data.Count.ToString(CultureInfo.InvariantCulture);
                ucMaxFactors.GridData = data;
            }
            
            upGrid.Update();
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                using (var dataAccess = new RevenueDataAccess(null))
                {
                    dataAccess.UploadDatabaseEntities(EntitiesToUpload);
                }
                lblFileUploadSummary1.Text = "File Successfully Uploaded";
                lblFileUploadSummary2.Text = string.Empty;

            }
            catch (Exception ex)
            {
                lblFileUploadSummary1.Text = ex.ToString();
            }

        }
    }
}