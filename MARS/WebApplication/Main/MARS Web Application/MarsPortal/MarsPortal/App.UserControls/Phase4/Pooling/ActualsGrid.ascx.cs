using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Classes.Phase4Dal.Pooling;
using Mars.App.Classes.Phase4Dal.Pooling.Entities;

namespace Mars.App.UserControls.Phase4.Pooling
{
    public partial class ActualsGrid : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        public void PopulateGrid(List<DayActualsRow> data)
        {
            var topics = data.Select(d => d.RowName);
            rptTopics.DataSource = topics;
            rptTopics.DataBind();

            rptActualsGrid.DataSource = data;
            rptActualsGrid.DataBind();

            PopulateHeaderHours();
            PopulateHeaderDays();
        }

        private void PopulateHeaderDays()
        {
            var dayHeaderCells = new List<DayActualDayHeaderCell>
                                 {
                                     new DayActualDayHeaderCell
                                     {
                                         CellName = DateTime.Now.DayOfWeek.ToString(),
                                         ColSpan = 24 - DateTime.Now.Hour
                                     },
                                     new DayActualDayHeaderCell
                                     {
                                         CellName = DateTime.Now.AddDays(1).DayOfWeek.ToString(),
                                         ColSpan = 24
                                     },
                                     new DayActualDayHeaderCell
                                     {
                                         CellName = DateTime.Now.AddDays(2).DayOfWeek.ToString(),
                                         ColSpan = 24
                                     },
                                     new DayActualDayHeaderCell
                                     {
                                         CellName = DateTime.Now.AddDays(3).DayOfWeek.ToString(),
                                         ColSpan = 24
                                     },
                                 };

            rptHeaderDaysRow.DataSource = dayHeaderCells;
            rptHeaderDaysRow.DataBind();
        }

        private void PopulateHeaderHours()
        {
            var dates = new List<string>();
            for (int i = 0; i < DayActualsDataAccess.HoursForActuals; i++)
            {
                var hourRange = string.Format("{0}-{1}", DateTime.Now.AddHours(i).Hour.ToString("00")
                                        , DateTime.Now.AddHours(i + 1).Hour.ToString("00"));
                dates.Add(hourRange);

            }
            rptHeaderHoursRow.DataSource = dates;
            rptHeaderHoursRow.DataBind();
        }


    }
}