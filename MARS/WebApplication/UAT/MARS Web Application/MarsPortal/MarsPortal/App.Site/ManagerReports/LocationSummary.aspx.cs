using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Bll.Availability;
using Mars.App.Classes.Phase4Dal.Availability;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Classes.Phase4Dal.ManagerReports;
using Mars.App.Classes.Phase4Dal.ManagerReports.Entities;
using Mars.App.Classes.Phase4Dal.Reservations;
using Mars.App.UserControls.Phase4;

namespace Mars.App.Site.ManagerReports
{
    public partial class LocationSummary : Page
    {
        private const string LocationSummarySessionString = "LocationSummarySessionString";
        private const string LocationSummaryForeignSessionString = "LocationSummaryForeignSessionString";

        private const string TransferToFleetStatusLinkButton = "TransferToFleetStatusLinkButton";

        public List<LocationSummaryRow> GridData
        {
            private get { return Session[LocationSummarySessionString] as List<LocationSummaryRow>; }
            set { Session[LocationSummarySessionString] = value; }
        }

        private List<LocationSummaryRow> _restrictedGridData;

        public List<LocationSummaryForeignRow> ForiegnGridData
        {
            private get { return Session[LocationSummaryForeignSessionString] as List<LocationSummaryForeignRow>; }
            set { Session[LocationSummaryForeignSessionString] = value; }
        }

        private SortDirection OverviewSortDirection
        {
            get { return (SortDirection)Session["SortDirection" + LocationSummarySessionString]; }
            set { Session["SortDirection" + LocationSummarySessionString] = value; }
        }

        private PropertyInfo OverviewSortColumn
        {
            get { return Session["SortColumn" + LocationSummarySessionString] as PropertyInfo; }
            set { Session["SortColumn" + LocationSummarySessionString] = value; }
        }

        private bool _exportClicked;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                tbQuickLocation.Text = string.Empty;
                OverviewSortDirection = SortDirection.Descending;

                using (var dataAccess = new AvailabilityDataAccess(null))
                {
                    lbFleet.Items.AddRange(dataAccess.GetFleetTypesList(ModuleType.Availability).ToArray());
                    lblLastUpdate.Text = LastUpdatedFromFleetNow.GetLastUpdatedDateTime(dataAccess);
                }

                using (var dataAccess = new ReservationDataAccess(null))
                {
                    lblReservationUpdate.Text = dataAccess.GetLastGwdRequest();
                }
            }

            var eventTarget = Request["__EVENTTARGET"];
            if (eventTarget == null) return;
            if (eventTarget == upnlLocationSummary.ClientID)
            {
                CheckLocationCode();
            }

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            _restrictedGridData = ddlCarSegments.SelectedValue == "" 
                                ? GridData 
                                : GridData.Where(d => d.CarSegmentName == ddlCarSegments.SelectedValue).ToList();

            gvSummary.DataSource = _restrictedGridData;
            gvSummary.DataBind();

            gvForeignVehicles.DataSource = ForiegnGridData;
            gvForeignVehicles.DataBind();
            
            if (GridData != null)
            {
                AddHeadersToGrid();
            }

            if (_exportClicked)
            {
                ExportToExcel();
            }
        }

        protected void gvSummary_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[2].Text = _restrictedGridData.Sum(d => d.AvailabilityOp).ToString(CultureInfo.InvariantCulture);
                e.Row.Cells[3].Text = _restrictedGridData.Sum(d => d.AvailabilityShop).ToString(CultureInfo.InvariantCulture);
                e.Row.Cells[4].Text = _restrictedGridData.Sum(d => d.AvailabilityAvailable).ToString(CultureInfo.InvariantCulture);
                e.Row.Cells[5].Text = _restrictedGridData.Sum(d => d.AvailabilityOnRent).ToString(CultureInfo.InvariantCulture);
                e.Row.Cells[6].Text = _restrictedGridData.Sum(d => d.AvailabilityIdle).ToString(CultureInfo.InvariantCulture);
                e.Row.Cells[7].Text = _restrictedGridData.Sum(d => d.AvailabilityOverdue).ToString(CultureInfo.InvariantCulture);
                e.Row.Cells[8].Text = (_restrictedGridData.Sum(d => d.AvailabilityOnRent) / _restrictedGridData.Sum(d => d.AvailabilityOp)).ToString("P");
                e.Row.Cells[9].Text = _restrictedGridData.Sum(d => d.NonRevGreaterThanThree).ToString(CultureInfo.InvariantCulture);
                e.Row.Cells[10].Text = _restrictedGridData.Sum(d => d.NonRevGreaterThanSeven).ToString(CultureInfo.InvariantCulture);
                e.Row.Cells[11].Text = _restrictedGridData.Sum(d => d.ReservationCheckInToday).ToString(CultureInfo.InvariantCulture);
                e.Row.Cells[12].Text = _restrictedGridData.Sum(d => d.ReservationCheckInRemaining).ToString(CultureInfo.InvariantCulture);
                e.Row.Cells[13].Text = _restrictedGridData.Sum(d => d.ReservationCheckOutToday).ToString(CultureInfo.InvariantCulture);
                e.Row.Cells[14].Text = _restrictedGridData.Sum(d => d.ReservationCheckOutRemaining).ToString(CultureInfo.InvariantCulture);
                
                e.Row.Cells.RemoveAt(1);
                e.Row.Cells[0].ColumnSpan = 2;
            }
        }


  
        private void CheckLocationCode()
        {
            upnlLocationSummary.Update();
            pnlResults.Visible = false;
            pnlNoData.Visible = true;
            var locationCode = tbQuickLocation.Text;
            if (locationCode.Length < 7)
                return;
            using (var dataAccess = new LocationSummaryDataAccess())
            {
                var locationId = dataAccess.GetLocationIdFromCode(locationCode);
                if (locationId == 0)
                {
                    GridData = null;
                    return;
                }

                pnlResults.Visible = true;
                pnlNoData.Visible = false;
                lblLocationName.Text = dataAccess.GetLocationName(locationId);

                GetLocationGrid(dataAccess, locationId);
                AddCarSegments();
            }   

        }

        private void GetLocationGrid(LocationSummaryDataAccess dataAccess, int locationId)
        {
            var nowDate = DateTime.Now.ToShortDateString();
            lblDate.Text = nowDate;
            var parameters = new Dictionary<DictionaryParameter, string>();

            parameters[DictionaryParameter.StartDate] = nowDate;
            parameters[DictionaryParameter.PercentageCalculation] = PercentageDivisorType.OperationalFleet.ToString();
            parameters[DictionaryParameter.Location] = locationId.ToString(CultureInfo.InvariantCulture);
            parameters[DictionaryParameter.FleetTypes] = AvailabilityParameters.GetSelectedKeys(lbFleet.Items);


            hfLocationId.Value = locationId.ToString(CultureInfo.InvariantCulture);

            BuildLocalVehiclesGrid(dataAccess, parameters);
            
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            var locationId = int.Parse(hfLocationId.Value);
            using (var dataAccess = new LocationSummaryDataAccess())
            {
                GetLocationGrid(dataAccess, locationId);
            }
        }

        protected void Gridview_Sorting(object sender, GridViewSortEventArgs e)
        {
            OverviewSortColumn = typeof(LocationSummaryRow).GetProperty(e.SortExpression);

            OverviewSortDirection = OverviewSortDirection == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
            SortGrid(OverviewSortDirection, OverviewSortColumn);
        }

        private void SortGrid(SortDirection direction, PropertyInfo sortByColumnName)
        {
            var isInt = sortByColumnName.PropertyType == typeof(int);
            var isDouble = sortByColumnName.PropertyType == typeof(double);
            var isDateTime = sortByColumnName.PropertyType == typeof(DateTime?);

            var sortedGridData = GridData.ToList();


            if (direction == SortDirection.Ascending)
            {


                sortedGridData.Sort((x, y) =>
                {
                    if (isDateTime)
                    {
                        var dt1 =
                            (DateTime)(sortByColumnName.GetValue(x, null) ?? DateTime.MinValue);
                        var dt2 =
                            (DateTime)(sortByColumnName.GetValue(y, null) ?? DateTime.MinValue);

                        return dt1.CompareTo(dt2);
                    }

                    if (isInt)
                    {
                        var int1 = (int)sortByColumnName.GetValue(x, null);
                        var int2 = (int)sortByColumnName.GetValue(y, null);
                        return int1.CompareTo(int2);
                    }
                    if (isDouble)
                    {
                        var dbl1 = (double)sortByColumnName.GetValue(x, null);
                        var dbl2 = (double)sortByColumnName.GetValue(y, null);
                        return dbl1.CompareTo(dbl2);
                    }
                    return String.Compare(sortByColumnName.GetValue(x, null).ToString()
                        , sortByColumnName.GetValue(y, null).ToString());
                }
                    );

            }
            else
            {
                sortedGridData.Sort(
                    (x, y) =>
                    {
                        if (isDateTime)
                        {
                            var dt1 =
                                (DateTime)(sortByColumnName.GetValue(y, null) ?? DateTime.MinValue);
                            var dt2 =
                                (DateTime)(sortByColumnName.GetValue(x, null) ?? DateTime.MinValue);

                            return dt1.CompareTo(dt2);
                        }

                        if (isInt)
                        {
                            var int1 = (int)sortByColumnName.GetValue(y, null);
                            var int2 = (int)sortByColumnName.GetValue(x, null);
                            return int1.CompareTo(int2);
                        }
                        if (isDouble)
                        {
                            var dbl1 = (double)sortByColumnName.GetValue(y, null);
                            var dbl2 = (double)sortByColumnName.GetValue(x, null);
                            return dbl1.CompareTo(dbl2);
                        }
                        return String.Compare(sortByColumnName.GetValue(y, null).ToString()
                            , sortByColumnName.GetValue(x, null).ToString());
                    }
                    );

            }
            GridData = sortedGridData;
        }

        private void AddHeadersToGrid()
        {
            var row = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);

            
            var left = new TableHeaderCell {ColumnSpan = 2};
            row.Cells.Add(left);

            var availability = new TableHeaderCell {ColumnSpan = 7};
            var lb = new LinkButton
                     {
                         Text = "Availability",
                         PostBackUrl = "~/App.Site/Availability/FleetStatus/FleetStatus.aspx"
                     };
            availability.Controls.Add(lb);
            row.Cells.Add(availability);

            var nonRev = new TableHeaderCell { ColumnSpan = 2};
            lb = new LinkButton
            {
                Text = "Non Rev",
                PostBackUrl = "~/App.Site/NonRevenue/Overview/Overview.aspx"
            };
            nonRev.Controls.Add(lb);
            row.Cells.Add(nonRev);

            var reservations = new TableHeaderCell { ColumnSpan = 4};
            lb = new LinkButton
            {
                Text = "Reservations",
                PostBackUrl = "~/App.Site/ForeignVehicles/ReservationDetails.aspx"
            };
            reservations.Controls.Add(lb);
            row.Cells.Add(reservations);

            ((Table)gvSummary.Controls[0]).Rows.AddAt(0, row);
        }


        private void BuildLocalVehiclesGrid(LocationSummaryDataAccess dataAccess, Dictionary<DictionaryParameter, string> parameters)
        {
            GridData = dataAccess.GetFleetStatusData(parameters);
            ForiegnGridData = dataAccess.GetForeignFleetStatusData(parameters);
            
        }

        private void AddCarSegments()
        {
            ddlCarSegments.Items.Clear();
            ddlCarSegments.Items.Add(new ListItem("***All***", ""));
            var distinctCarSegments = GridData.Select(d => d.CarSegmentName).Distinct();
            var listItems = distinctCarSegments.Select(d => new ListItem(d));
            ddlCarSegments.Items.AddRange(listItems.ToArray());
        }

        protected override bool OnBubbleEvent(object sender, EventArgs args)
        {
            var handled = false;
            if (args is CommandEventArgs)
            {
                var commandArgs = args as CommandEventArgs;
                if (commandArgs.CommandName == UserControls.Phase4.HelpIcons.ExportToExcel.ExportString)
                {
                    _exportClicked = true;   
                }

                handled = false;
            }
            return handled;
        }

        private void ExportToExcel()
        {
            //var dataTable = HtmlTableGenerator.GenerateCsvFromGridview(gvFleetStatus);
            var sw = new StringWriter();
            var htextw = new HtmlTextWriter(sw);
            gvSummary.RenderControl(htextw);
            Session["ExportData"] = sw.ToString();
            Session["ExportFileType"] = "xls";
            Session["ExportFileName"] = string.Format("Availability Fleet status Export {0}", DateTime.Now.ToShortDateString());
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET server control at run time. */
        }

    }
}