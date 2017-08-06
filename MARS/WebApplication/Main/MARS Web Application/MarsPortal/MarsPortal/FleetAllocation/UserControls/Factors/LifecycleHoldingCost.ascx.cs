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
    public partial class LifecycleHoldingCost : UserControl
    {

        public const string FaoParameterSessionName = "FaoLifecycleHoldingCostParameterSessionName";

        public Dictionary<DictionaryParameter, string> SessionStoredFaoLifecycleHoldingCostParameters
        {
            get { return (Dictionary<DictionaryParameter, string>)Session[FaoParameterSessionName]; }
            set { Session[FaoParameterSessionName] = value; }
        }

        private const string FaoSessionDataGrid = "FaoLifeCycleHoldingCostSessionDataGrid";

        private const string FaoLifecycleHoldingCostToUploadSessionName = "FaoLifecycleHoldingCostToUploadSessionName";

        private List<DataContext.LifecycleHoldingCost> EntitiesToUpload
        {
            get
            {
                return (List<DataContext.LifecycleHoldingCost>)Session[FaoLifecycleHoldingCostToUploadSessionName];
            }
            set { Session[FaoLifecycleHoldingCostToUploadSessionName] = value; }
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
            ucMaxFactors.GridItemType = typeof(LifecycleHoldingCostRow);
            ucMaxFactors.SessionNameForGridData = FaoSessionDataGrid;
            ucMaxFactors.ColumnHeaders = LifecycleHoldingCostRow.HeaderRows;
            ucMaxFactors.ColumnFormats = LifecycleHoldingCostRow.Formats;
            ucParameters.SessionStoredFaoMinCommSegParameters = SessionStoredFaoLifecycleHoldingCostParameters;

            if (IsPostBack)
            {
                if (fuFaoHoldingCostFile.PostedFile != null)
                {
                    if (hfFaoUploadHoldingCostFile.Value != string.Empty)
                    {
                        var file = fuFaoHoldingCostFile.PostedFile;
                        SelectedPanel = 2; //Bring the panel pack here after a full page postback
                        ParseUploadedFile(file);
                        btnUpload.Visible = true;
                        hfFaoUploadHoldingCostFile.Value = string.Empty;
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

                var parsedData = new List<LifecycleHoldingCostRow>();
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

                        var year = int.Parse(splitDataRow[0].Trim());
                        var month = DateTime.ParseExact(splitDataRow[1].Trim(), "MMM",
                            CultureInfo.CurrentCulture).Month;
                        var carGroup = splitDataRow[2].Trim();
                        var holdingCost = double.Parse(splitDataRow[3].Trim());

                        var monthDate = new DateTime(year, month, 1);

                        var rawData = new LifecycleHoldingCostRow(monthDate)
                        {
                            CarGroup = carGroup,
                            Cost = holdingCost,
                        };
                        parsedData.Add(rawData);
                    }
                }
                List<DataContext.LifecycleHoldingCost> dbEntities;
                using (var dataAccess = new LifecycleHoldingCostDataAccess(null))
                {
                    dbEntities = dataAccess.MatchUploadToDatabaseEntities(parsedData);
                }
                var minDate = dbEntities.Min(d => d.MonthDate);
                var maxDate = dbEntities.Max(d => d.MonthDate);

                cbeUploadWarning.ConfirmText = string.Format("Confirming will wipe all Holding Costs from {0:MMM yyyy} onwards and replace them with Data from the selected file.", minDate);
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
            
            using (var dataAccess = new LifecycleHoldingCostDataAccess(parameters))
            {
                var data = dataAccess.GetLifecycleHoldingCosts();
                hfRecordCount.Value = data.Count.ToString(CultureInfo.InvariantCulture);
                ucMaxFactors.GridData = data;
            }
            
            upGrid.Update();
        }


        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                using (var dataAccess = new LifecycleHoldingCostDataAccess(null))
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