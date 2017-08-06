using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Bll;
using Mars.App.Classes.Phase4Bll.Availability;
using Mars.App.Classes.Phase4Dal.Availability.Entities;
using Mars.App.Classes.Phase4Dal.Enumerators;

namespace Mars.App.UserControls.Phase4.Availability
{
    public partial class HistoricalTrendGrid : UserControl
    {
        public bool HourlySeries
        {
            get { return bool.Parse(hfHourlySeries.Value); }
            set { hfHourlySeries.Value = value.ToString(); }

        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void LoadGrid(List<FleetStatusRow> fsd, PercentageDivisorType type)
        {
            if (type != PercentageDivisorType.Values)
            {
                var dataToChart = fsd.Select(f => new FleetStatusGridPercentRow(f, true, HourlySeries)).ToList();
                gvHistoricalTrend.DataSource = dataToChart;
            }
            else
            {
                var dataToChart = fsd.Select(f => new FleetStatusGridRow(f, true, HourlySeries)).ToList();
                gvHistoricalTrend.DataSource = dataToChart;
            }
            gvHistoricalTrend.DataBind();
        }

        protected void gvHistoricalTrend_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.Header) return;
            e.Row.Cells[0].Text = "Date";
            e.Row.Cells[1].Text = TopicTranslation.GetAvailabilityTopicShortDescription(AvailabilityTopic.TotalFleet);
            e.Row.Cells[2].Text = TopicTranslation.GetAvailabilityTopicShortDescription(AvailabilityTopic.Cu);
            e.Row.Cells[3].Text = TopicTranslation.GetAvailabilityTopicShortDescription(AvailabilityTopic.Ha);
            e.Row.Cells[4].Text = TopicTranslation.GetAvailabilityTopicShortDescription(AvailabilityTopic.Hl);
            e.Row.Cells[5].Text = TopicTranslation.GetAvailabilityTopicShortDescription(AvailabilityTopic.Ll);
            e.Row.Cells[6].Text = TopicTranslation.GetAvailabilityTopicShortDescription(AvailabilityTopic.Nc);
            e.Row.Cells[7].Text = TopicTranslation.GetAvailabilityTopicShortDescription(AvailabilityTopic.Pl);
            e.Row.Cells[8].Text = TopicTranslation.GetAvailabilityTopicShortDescription(AvailabilityTopic.Tc);
            e.Row.Cells[9].Text = TopicTranslation.GetAvailabilityTopicShortDescription(AvailabilityTopic.Sv);
            e.Row.Cells[10].Text = TopicTranslation.GetAvailabilityTopicShortDescription(AvailabilityTopic.Ws);
            e.Row.Cells[11].Text = TopicTranslation.GetAvailabilityTopicShortDescription(AvailabilityTopic.OperationalFleet);
            e.Row.Cells[12].Text = TopicTranslation.GetAvailabilityTopicShortDescription(AvailabilityTopic.Bd);
            e.Row.Cells[13].Text = TopicTranslation.GetAvailabilityTopicShortDescription(AvailabilityTopic.Mm);
            e.Row.Cells[14].Text = TopicTranslation.GetAvailabilityTopicShortDescription(AvailabilityTopic.Tw);
            e.Row.Cells[15].Text = TopicTranslation.GetAvailabilityTopicShortDescription(AvailabilityTopic.Tb);
            e.Row.Cells[16].Text = TopicTranslation.GetAvailabilityTopicShortDescription(AvailabilityTopic.Fs);
            e.Row.Cells[17].Text = TopicTranslation.GetAvailabilityTopicShortDescription(AvailabilityTopic.Rl);
            e.Row.Cells[18].Text = TopicTranslation.GetAvailabilityTopicShortDescription(AvailabilityTopic.Rp);
            e.Row.Cells[19].Text = TopicTranslation.GetAvailabilityTopicShortDescription(AvailabilityTopic.Tn);
            e.Row.Cells[20].Text = TopicTranslation.GetAvailabilityTopicShortDescription(AvailabilityTopic.AvailableFleet);
            e.Row.Cells[21].Text = TopicTranslation.GetAvailabilityTopicShortDescription(AvailabilityTopic.Idle);
            e.Row.Cells[22].Text = TopicTranslation.GetAvailabilityTopicShortDescription(AvailabilityTopic.Su);
            e.Row.Cells[23].Text = TopicTranslation.GetAvailabilityTopicShortDescription(AvailabilityTopic.Overdue);
            e.Row.Cells[24].Text = TopicTranslation.GetAvailabilityTopicShortDescription(AvailabilityTopic.OnRent);
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
                }
                handled = true;
            }
            return handled;
        }

        private void ExportToExcel()
        {
            var dataTable = HtmlTableGenerator.GenerateCsvFromGridview(gvHistoricalTrend);
            Session["ExportData"] = dataTable;
            Session["ExportFileType"] = "csv";
            Session["ExportFileName"] = string.Format("Availability HistoricalTrend Export {0}", DateTime.Now.ToShortDateString());
        }
    }
}