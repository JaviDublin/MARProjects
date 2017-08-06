using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.FleetAllocation.BusinessLogic.DropdownLists;
using Mars.FleetAllocation.DataAccess;
using Mars.FleetAllocation.DataAccess.AdditionsLimits;
using Mars.FleetAllocation.DataAccess.AdditionsLimits.Entities;
using Mars.FleetAllocation.DataContext;

namespace Mars.FleetAllocation.UserControls.LimitFiles
{
    public partial class LimitUpload : UserControl
    {
        private const string MonthlyLimitSession = "FaoMontlyLimitForMonth";
        private const string WeeklyLimitSession = "FaoWeeklyLimitForMonth";
        private const string FaoWeeklyDataToUploadSessionName = "FaoWeeklyDataToUploadSessionName";

        protected int WeeklyIdToUpdate
        {
            get { return int.Parse(hfWeeklyIdToUpdate.Value); }
            set { hfWeeklyIdToUpdate.Value = value.ToString(); }
        }

        private List<MonthlyLimitOnCarGroup> EntitiesToUpload
        {
            get {
                return (List<MonthlyLimitOnCarGroup>) Session[FaoWeeklyDataToUploadSessionName];
            }
            set { Session[FaoWeeklyDataToUploadSessionName] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ucMonthlyLimit.GridItemType = typeof(MonthlyLimitRow);
            ucMonthlyLimit.SessionNameForGridData = MonthlyLimitSession;
            ucMonthlyLimit.ColumnHeaders = MonthlyLimitRow.HeaderRows;

            ucWeeklyLimit.GridItemType = typeof(WeeklyEditRow);
            ucWeeklyLimit.SessionNameForGridData = WeeklyLimitSession;
            ucWeeklyLimit.ColumnHeaders = WeeklyEditRow.HeaderRows;

            btnUpload.Visible = false;

            if (!IsPostBack)
            {
                FillDropdowns();
                PopulateGrids();
            }
            else
            {
                if (fuFaoMonthlyAdditionFile.PostedFile != null)
                {
                    var file = fuFaoMonthlyAdditionFile.PostedFile;
                    
                    ParseUploadedFile(file);
                    btnUpload.Visible = true;
                }
            }
        }

        private void ParseUploadedFile(HttpPostedFile file)
        {
            try
            {
                var fileData = new byte[file.ContentLength];
                file.InputStream.Read(fileData, 0, file.ContentLength);

                var parsedData = new List<MonthlyLimitUploadRow>();
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


                        var rawData = new MonthlyLimitUploadRow
                                      {
                                          Year = int.Parse(splitDataRow[0].Trim()),
                                          Month =
                                              DateTime.ParseExact(splitDataRow[1].Trim(), "MMM",
                                                  CultureInfo.CurrentCulture).Month,
                                          CarGroup = splitDataRow[2].Trim(),
                                          Additions = int.Parse(splitDataRow[3].Trim()),
                                      };
                        parsedData.Add(rawData);
                    }
                }
                List<MonthlyLimitOnCarGroup> dbEntities;
                using (var dataAccess = new MonthlyAddLimitDataAccess())
                {
                    dbEntities = dataAccess.MatchUploadToDatabaseEntities(parsedData);
                }
                var minDate = dbEntities.Min(d => d.MonthDate);
                var maxDate = dbEntities.Max(d => d.MonthDate);

                cbeUploadWarning.ConfirmText = string.Format("Confirming will wipe all Monthly Additions from {0:MMM yyyy} onwards and replace it with Data from the selected file.", minDate);
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


        private void FillDropdowns()
        {
            var now = DateTime.Now;
            
            for (int i = 1; i > -4; i--)
            {
                ddlYearSelection.Items.Add(new ListItem(now.AddYears(i).Year.ToString(CultureInfo.InvariantCulture)));
            }
            ddlYearSelection.SelectedValue = now.Year.ToString(CultureInfo.InvariantCulture);

            var months = new List<ListItem>();
            for (int i = 0; i < 12; i++)
            {
                months.Add(new ListItem(CultureInfo.CurrentUICulture.DateTimeFormat.MonthNames[i], (i + 1).ToString(CultureInfo.InvariantCulture)));
            }
            ddlMonthSelection.Items.AddRange(months.ToArray());

            ddlCarSegment.Items.Add(new ListItem("Car", "4"));
            ddlCarSegment.Items.Add(new ListItem("Van", "6"));

            ListGenerator.FillDropdownWithFaoCountries(ddlCountry);
            

            //ddlCountry.Items
        }

        private void PopulateGrids()
        {
            var yearSelected = int.Parse(ddlYearSelection.SelectedValue);
            var monthSelected = int.Parse(ddlMonthSelection.SelectedValue);
            var dateSelected = new DateTime(yearSelected, monthSelected, 1);
            var carSegmentSelected = int.Parse(ddlCarSegment.SelectedValue);
            using (var dataAccess = new MonthlyAddLimitDataAccess())
            {
                var monthlyData = dataAccess.GetMonthlyLimits(dateSelected, carSegmentSelected);
                ucMonthlyLimit.GridData = monthlyData;

                lblMonthlySummary.Text = string.Format("{3:#,#} {0} {1} {2}", hfSummaryText.Value
                            , ddlMonthSelection.SelectedItem.Text, yearSelected
                            , monthlyData.Sum(d=> d.AdditionsLimit));

                var weeklyData = dataAccess.GetWeekLyLimits(dateSelected, carSegmentSelected);
                ucWeeklyLimit.GridData = weeklyData;

                lblWeeklySummary.Text = string.Format("{3:#,#} {0} {1} {2}", hfSummaryText.Value
                            , ddlMonthSelection.SelectedItem.Text, yearSelected
                            , weeklyData.Sum(d => d.AdditionsLimit));

                var difference = monthlyData.Sum(d => d.AdditionsLimit) - weeklyData.Sum(d => d.AdditionsLimit);
                lblDifference.Text = difference.ToString("#,#");
                lblDifference.ForeColor = difference == 0 ? Color.Black : difference < 0 ? Color.Red : Color.Green;
            }
        }


        protected override bool OnBubbleEvent(object source, EventArgs args)
        {
            var handled = false;

            if (args is CommandEventArgs)
            {
                var commandArgs = args as CommandEventArgs;
                if (commandArgs.CommandName == AutoGrid.EditCommand)
                {
                    
                    WeeklyIdToUpdate = int.Parse(commandArgs.CommandArgument.ToString());
                    using (var dataAccess = new MonthlyAddLimitDataAccess())
                    {
                        var weeklyEntity = dataAccess.GetWeeklyLimitEntiy(WeeklyIdToUpdate);
                        lblWeeklyInfo.Text = string.Format(hfWeeklyInfo.Value, weeklyEntity.Year, weeklyEntity.Week);
                        tbNewWeeklyAdditionLimit.Text = weeklyEntity.Additions.ToString();
                        
                    }
                    mpeEditWeeklyLimit2.Show();
                    
                    
                    handled = true;
                }
            }
            return handled;
        }

        protected void RefreshGrids(object sender, EventArgs e)
        {
            PopulateGrids();
        }

        protected void btnSaveWeeklyAdditionChange_Click(object sender, EventArgs e)
        {
            var newFigure = int.Parse(tbNewWeeklyAdditionLimit.Text);
            using (var dataAccess = new MonthlyAddLimitDataAccess())
            {
                dataAccess.UpdateWeeklyLimit(WeeklyIdToUpdate, newFigure);
            }
            PopulateGrids();
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                using (var dataAccess = new MonthlyAddLimitDataAccess())
                {
                    dataAccess.UploadDatabaseEntities(EntitiesToUpload);
                }
                lblFileUploadSummary1.Text = "File Successfully Uploaded";
                lblFileUploadSummary2.Text = string.Empty;
                PopulateGrids();
            }
            catch (Exception ex)
            {
                lblFileUploadSummary1.Text = ex.ToString();
            }
            
        }
    }
}