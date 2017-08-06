using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Bll;
using Mars.App.Classes.Phase4Bll.Administration;
using Mars.App.Classes.Phase4Bll.CustomGridViewItems;
using Mars.App.Classes.Phase4Dal.Administration.MappingEntities;

namespace Mars.App.UserControls.Phase4.Administration.Mapping
{
    public partial class EntityGrid : UserControl
    {
        public string SessionStringName { get; set; }

        public AdminMappingEnum EntityType
        {
            get
            {
                return
                    (AdminMappingEnum)Enum.Parse(typeof(AdminMappingEnum), hfEntityType.Value);
            }
            set
            {
                if(value == AdminMappingEnum.CarGroup || value == AdminMappingEnum.Location)
                {
                    ucExportToExcel.Visible = true;
                }
                hfEntityType.Value = value.ToString();
            }
        }

        public IEnumerable<IMappingEntity> GridData
        {
            get { return (IEnumerable<IMappingEntity>)Session[SessionStringName]; }
            set { Session[SessionStringName] = value; }
        }


        private SortDirection OverviewSortDirection
        {
            get { return (SortDirection)Session["SortDirection" + SessionStringName]; }
            set { Session["SortDirection" + SessionStringName] = value; }
        }

        private PropertyInfo OverviewSortColumn
        {
            get { return Session["SortColumn" + SessionStringName] as PropertyInfo; }
            set { Session["SortColumn" + SessionStringName] = value; }
        }

        private List<string> ColumnNames
        {
            set { hfColumnNames.Value = string.Join("¬", value.ToArray()); }
            get 
            { 
                var returned = hfColumnNames.Value.Split('¬').ToList();
                return returned;
            }
        }

        private int PageSize
        {
            get
            {
                return int.Parse(ddlPageSize.SelectedValue);
            }
            set
            {
                ddlPageSize.SelectedValue = value.ToString(CultureInfo.InvariantCulture);
            }
        }

        private int CurrentGvPage
        {
            get { return int.Parse(string.IsNullOrEmpty(hfCurrentGvPage.Value) ? "0" : hfCurrentGvPage.Value); }
            set { hfCurrentGvPage.Value = value.ToString(CultureInfo.InvariantCulture); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                OverviewSortDirection = SortDirection.Descending;
            }
            if (GridData != null)
            {
                BindGridView(PageSize);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            gvEntityGrid.PageIndex = CurrentGvPage - 1;
            pnlPager.Visible = GridData != null;
            if (GridData != null)
            {
                BindGridView(PageSize);
                SetGridviewPage();
            }
        }


        private void BindGridView(int pageSize)
        {
            var gd = GridData.FirstOrDefault();
            var columNames = gd == null ? ColumnNames : gd.GetRowNames();

            if(gd != null)
            {
                ColumnNames = gd.GetRowNames();
            }

            gvEntityGrid.Columns.Clear();
            foreach (var c in columNames)
            {
                if (c == "Id") continue;
                var templateField = new TemplateField
                {
                    HeaderTemplate =
                        new MappingGridViewTemplate(ListItemType.Header, c),
                    ItemTemplate =
                        new MappingGridViewTemplate(ListItemType.Item, c)
                };

                gvEntityGrid.Columns.Add(templateField);
            }

            var templateField2 = new TemplateField
            {
                HeaderTemplate =
                    new MappingGridViewTemplate(ListItemType.Header, string.Empty),
                ItemTemplate =
                    new MappingGridViewTemplate(ListItemType.Item, "Edit", EntityType.ToString())
            };
            gvEntityGrid.Columns.Add(templateField2);

            gvEntityGrid.DataSource = GridData;
            gvEntityGrid.DataBind();
            gvEntityGrid.PageSize = pageSize;
            PageSize = pageSize;
        }
        


        protected void ddlPageSize_SizeChange(object sender, EventArgs e)
        {
            var ddlPageSize = sender as DropDownList;
            if (ddlPageSize == null) return;
            var pageSize = int.Parse(ddlPageSize.SelectedValue);
            gvEntityGrid.PageSize = pageSize;
            BindGridView(pageSize);
            CurrentGvPage = 1;
        }

        protected void Gridview_Sorting(object sender, GridViewSortEventArgs e)
        {
            switch (EntityType)
            {
                case AdminMappingEnum.Country:
                    OverviewSortColumn = typeof(CountryEntity).GetProperty(e.SortExpression);
                    break;
                case AdminMappingEnum.CmsPool:
                    OverviewSortColumn = typeof(PoolEntity).GetProperty(e.SortExpression);
                    break;
                case AdminMappingEnum.CmsLocationGroup:
                    OverviewSortColumn = typeof(LocationGroupEntity).GetProperty(e.SortExpression);
                    break;
                case AdminMappingEnum.OpsRegion:
                    OverviewSortColumn = typeof(RegionEntity).GetProperty(e.SortExpression);
                    break;
                case AdminMappingEnum.OpsArea:
                    OverviewSortColumn = typeof(AreaEntity).GetProperty(e.SortExpression);
                    break;
                case AdminMappingEnum.Location:
                    OverviewSortColumn = typeof(LocationEntity).GetProperty(e.SortExpression);
                    break;
                case AdminMappingEnum.CarSegment:
                    OverviewSortColumn = typeof(CarSegmentEntity).GetProperty(e.SortExpression);
                    break;
                case AdminMappingEnum.CarClass:
                    OverviewSortColumn = typeof(CarClassEntity).GetProperty(e.SortExpression);
                    break;
                case AdminMappingEnum.CarGroup:
                    OverviewSortColumn = typeof(CarGroupEntity).GetProperty(e.SortExpression);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            

            OverviewSortDirection = OverviewSortDirection == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
            SortGrid(OverviewSortDirection, OverviewSortColumn);
        }

        private void SortGrid(SortDirection direction, PropertyInfo sortByColumnName)
        {
            var isInt = sortByColumnName.PropertyType == typeof(int);
            var isDateTime = sortByColumnName.PropertyType == typeof(DateTime?);

            var sortedGridData = GridData.ToList();
            if (direction == SortDirection.Ascending)
            {
                sortedGridData.Sort((x, y) => isDateTime ? (int)(((DateTime)(sortByColumnName.GetValue(x, null) ?? DateTime.MinValue)).Date
                                                - ((DateTime)(sortByColumnName.GetValue(y, null) ?? DateTime.MinValue)).Date
                                        ).TotalDays
                                    : isInt ? (int)sortByColumnName.GetValue(x, null) - (int)sortByColumnName.GetValue(y, null)
                                    : String.CompareOrdinal(sortByColumnName.GetValue(x, null).ToString().ToLower()
                                    , sortByColumnName.GetValue(y, null).ToString().ToLower())
                            );
            }
            else
            {
                sortedGridData.Sort(
                    (x, y) =>
                        isDateTime
                            ? (int)
                                (((DateTime)(sortByColumnName.GetValue(y, null) ?? DateTime.MinValue)).Date -
                                 ((DateTime)(sortByColumnName.GetValue(x, null) ?? DateTime.MinValue)).Date).TotalDays
                            : isInt
                                ? (int)sortByColumnName.GetValue(y, null) - (int)sortByColumnName.GetValue(x, null)
                                : String.CompareOrdinal(sortByColumnName.GetValue(y, null).ToString().ToLower()
                                    , sortByColumnName.GetValue(x, null).ToString().ToLower()));

            }
            GridData = sortedGridData;
        }

        public void BindGrid(IEnumerable<IMappingEntity> data)
        {
            CurrentGvPage = 1;
            GridData = data;
        }

        private void SetGridviewPage()
        {
            var totalCount = GridData.Count();

            
            var firstPage = CurrentGvPage == 1;
            var lastPage = GridData != null && CurrentGvPage == ((totalCount + PageSize - 1) / PageSize);
            lbgvFirst.Enabled = !firstPage;
            lbgvFirst.ImageUrl = firstPage ? "~/App.Images/pager-first-dis.png" : "~/App.Images/pager-first.png";
            lbgvPrevious.Enabled = !firstPage;
            lbgvPrevious.ImageUrl = firstPage ? "~/App.Images/pager-previous-dis.png" : "~/App.Images/pager-previous.png";
            lbgvNext.Enabled = !lastPage;
            lbgvNext.ImageUrl = lastPage ? "~/App.Images/pager-next-dis.png" : "~/App.Images/pager-next.png";
            lbgvLast.Enabled = !lastPage;
            lbgvLast.ImageUrl = lastPage ? "~/App.Images/pager-last-dis.png" : "~/App.Images/pager-last.png";

            if (GridData == null) return;

            lblRowCount.Text = string.Format("Total {0}: {1:##,##0}", GetRecordType(), totalCount);

            lblPageAt.Text = string.Format("Page {0} of {1}", CurrentGvPage, (totalCount + PageSize - 1) / PageSize);
        }

        private string GetRecordType()
        {
            switch (EntityType)
            {
                case AdminMappingEnum.Country:
                    return "Countries";
                case AdminMappingEnum.CmsPool:
                    return "Pools";
                case AdminMappingEnum.CmsLocationGroup:
                    return "Location Groups";
                case AdminMappingEnum.OpsRegion:
                    return "Regions";
                case AdminMappingEnum.OpsArea:
                    return "Areas";
                case AdminMappingEnum.Location:
                    return "Locations";
                case AdminMappingEnum.CarSegment:
                    return "Car Segments";
                case AdminMappingEnum.CarClass:
                    return "Car Classes";
                case AdminMappingEnum.CarGroup:
                    return "Car Groups";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        

        protected void gvFirstButton_Click(object sender, EventArgs e)
        {
            CurrentGvPage = 1;

        }

        protected void gvPreviousButton_Click(object sender, EventArgs e)
        {
            CurrentGvPage--;
        }

        protected void gvNextButton_Click(object sender, EventArgs e)
        {
            CurrentGvPage++;
        }

        protected void gvLastButton_Click(object sender, EventArgs e)
        {
            CurrentGvPage = (GridData.Count() + PageSize - 1) / PageSize;
        }

        protected override bool OnBubbleEvent(object sender, EventArgs args)
        {
            var handled = false;
            if (args is CommandEventArgs)
            {
                var commandArgs = args as CommandEventArgs;
                if (commandArgs.CommandName == HelpIcons.ExportToExcel.ExportString)
                {
                    ExportToExcel();
                    handled = true;
                }
                
            }
            return handled;
        }

        private void ExportToExcel()
        {
            var stringToExport = EntityType == AdminMappingEnum.Location ? ExportLocationData() : ExportCarGroupData();

            Session["ExportData"] = stringToExport;
            Session["ExportFileType"] = "csv";
            Session["ExportFileName"] = string.Format("{1} Export {0}"
                    , DateTime.Now.ToShortDateString()
                    , EntityType == AdminMappingEnum.Location ? "Location" : "Car Group");
        }

        private string ExportLocationData()
        {
            var sw = new StringBuilder();
            sw.AppendLine("Location, LocationName,  Country Name, Pool, Location Group, Region, Area, ADR, CAL, Served By, Turnaround Hours, Active");
            var castGridData = (List<LocationEntity>) GridData;
            foreach (var gd in castGridData)
            {
                sw.AppendLine(string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}", gd.LocationCode
                                            , gd.LocationFullName.Replace(",", ""), gd.CountryName, gd.PoolName,
                                            gd.LocationGroupName, gd.RegionName, gd.AreaName, gd.AirportDowntownRailroad,
                                            gd.CorporateAgencyLicencee, gd.ServedBy, gd.TurnaroundHours, gd.ActiveYesNo ));
            }
            return sw.ToString();
        }

        private string ExportCarGroupData()
        {
            var sw = new StringBuilder();
            sw.AppendLine("Country Name, Car Group, Car Segment, Car Class, Gold, Five Star, President Circle, Platinum, Active");
            var castGridData = (List<CarGroupEntity>)GridData;
            foreach (var gd in castGridData)
            {
                sw.AppendLine(string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}", gd.CountryName, gd.CarGroupName, 
                                            gd.CarSegmentName,
                                            gd.CarClassName,  gd.CarGroupGold, gd.CarGroupFiveStar,
                                            gd.CarGroupPresidentCircle, gd.CarGroupPlatinum, gd.ActiveYesNo));
            }
            
            return sw.ToString();
        }
    }
}
