using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Castle.Core.Internal;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.DAL.FleetDemand;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.Phase4Bll;
using Mars.App.Classes.Phase4Bll.Availability;
using Mars.App.Classes.Phase4Dal.Availability;
using Mars.App.Classes.Phase4Dal.Availability.Entities;
using Mars.App.Classes.Phase4Dal.Enumerators;
using App.BLL.ExtensionMethods;

namespace Mars.App.UserControls.Phase4.Availability
{
    public partial class FleetStatusGrid : UserControl
    {
        private const string AllString = "All";

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void LoadGrid(FleetStatusRow fsd, Dictionary<DictionaryParameter, string> parameters )
        {
            //var data = new List<FleetStatusGridRow> {new FleetStatusGridRow(fsd, false)};
            //gvFleetStatus.DataSource = data;
            //gvFleetStatus.DataBind();
            
            
            lblTotalFleet.Text = string.Format("{0:#,0}", fsd.TotalFleet);
            lblOperationalFleet.Text = string.Format("{0:#,0}", fsd.OperationalFleet);
            lblAvailableFleet.Text = string.Format("{0:#,0}", fsd.AvailableFleet);
            lblBd.Text = string.Format("{0:#,0}", fsd.Bd);
            lblWs.Text = string.Format("{0:#,0}", fsd.Ws);
            lblMm.Text = string.Format("{0:#,0}", fsd.Mm);
            lblTb.Text = string.Format("{0:#,0}", fsd.Tb);
            lblTw.Text = string.Format("{0:#,0}", fsd.Tw);
            lblFs.Text = string.Format("{0:#,0}", fsd.Fs);
            lblShop.Text = string.Format("{0:#,0}", fsd.InShop);
            lblRl.Text = string.Format("{0:#,0}", fsd.Rl);
            lblOverdue.Text = string.Format("{0:#,0} ", fsd.Overdue);


            lblOnRent.Text = string.Format("{0:#,0}", fsd.OnRent);
            lblShop.Text = string.Format("{0:#,0}", fsd.InShop);
            fsd.SetDivisorType(PercentageDivisorType.OperationalFleet);
            lblShopTotalOverOperFleet.Text = string.Format("{0:P} ", fsd.InShopPercent);
            lblRp.Text = string.Format("{0:#,0}", fsd.Rp);

            fsd.SetDivisorType(PercentageDivisorType.AvailableFleet);
            lblOverdueOverAvailable.Text = string.Format("{0:P} ", fsd.OverduePercent);
            lblTn.Text = string.Format("{0:#,0}", fsd.Tn);

            lblIdle.Text = string.Format("{0:#,0} ", fsd.Idle);
            lblSv.Text = string.Format("{0:#,0} ", fsd.Sv);

            lblOnRent.Text = string.Format("{0:#,0} ", fsd.OnRent);
            lblSu.Text = string.Format("{0:#,0} ", fsd.Su);
            lblTc.Text = string.Format("{0:#,0} ", fsd.Tc);

            fsd.SetDivisorType(PercentageDivisorType.OperationalFleet);
            lblUtiOnRentOperFleetInclOverdue.Text = string.Format("{0:P} ", fsd.UtilInclOverduePercent);
            lblIdleAndSu.Text = string.Format("{0:#,0} ", fsd.Idle + fsd.Su);

            lblIdleAndSuOverAvailableFleet.Text = string.Format("{0:P} ", ((double)(fsd.Idle + fsd.Su)) / fsd.OperationalFleet);
            lblHl.Text = string.Format("{0:#,0} ", fsd.Hl);
            lblUtiOnRentOperFleet.Text = string.Format("{0:P} ", fsd.UtilizationPercent);
            lblHa.Text = string.Format("{0:#,0} ", fsd.Ha);


            PopulateGridParameters(parameters);
        }

        private void PopulateGridParameters(Dictionary<DictionaryParameter, string> parameters)
        {
            var locationCountry = AllString;
            var poolRegion = AllString;
            var locationArea = AllString;
            var location = AllString;
            var owningCountry = AllString;
            var carSegment = AllString;
            var carClass = AllString;
            var carGroup = AllString;
            var fleetTypes = string.Empty;
            var dateRange = string.Empty;
            
            using (var dataAccess = new FleetStatusDataAccess(parameters))
            {
                if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationCountry))
                {
                    locationCountry = dataAccess.GetCountryFromId(parameters[DictionaryParameter.LocationCountry]);    
                }
                
                if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Pool))
                {
                    int poolId = int.Parse(parameters[DictionaryParameter.Pool]);
                    poolRegion = dataAccess.GetPoolFromId(poolId);
                }
                else
                {
                    if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Region))
                    {
                        int regionId = int.Parse(parameters[DictionaryParameter.Region]);
                        poolRegion = dataAccess.GetRegionFromId(regionId);
                    }
                }
                if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.LocationGroup))
                {
                    int locationGroupId = int.Parse(parameters[DictionaryParameter.LocationGroup]);
                    locationArea = dataAccess.GetLocationGroupFromId(locationGroupId);
                }
                else
                {
                    if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Area))
                    {
                        int areaId = int.Parse(parameters[DictionaryParameter.Area]);
                        locationArea = dataAccess.GetAreaFromId(areaId);
                    }
                }

                if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Location))
                {
                    int locationId = int.Parse(parameters[DictionaryParameter.Location]);
                    location = dataAccess.GetLocationFromId(locationId);
                }

                if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.OwningCountry))
                {
                    owningCountry = dataAccess.GetCountryFromId(parameters[DictionaryParameter.OwningCountry]);
                }

                if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarSegment))
                {
                    int carSegmentId = int.Parse(parameters[DictionaryParameter.CarSegment]);
                    carSegment = dataAccess.GetCarSegmentFromId(carSegmentId);
                }

                if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarClass))
                {
                    int carClassId = int.Parse(parameters[DictionaryParameter.CarClass]);
                    carClass = dataAccess.GetCarClassFromId(carClassId);
                }

                if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarGroup))
                {
                    int carGroupId = int.Parse(parameters[DictionaryParameter.CarGroup]);
                    carGroup = dataAccess.GetCarGroupFromId(carGroupId);
                }
                if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.FleetTypes))
                {
                    var fleetTypeIds = parameters[DictionaryParameter.FleetTypes].Split(',');
                    var fleetTypeIdsHolder = new List<int>();
                    fleetTypeIds.ForEach(d => fleetTypeIdsHolder.Add(int.Parse(d)));
                    fleetTypes = string.Join(", ", dataAccess.GetFleetTypenamesFromIds(fleetTypeIdsHolder));
                }
            }

            if (parameters[DictionaryParameter.StartDate] == DateTime.Now.ToShortDateString()
                || parameters[DictionaryParameter.DayOfWeek] == "0" || parameters[DictionaryParameter.DayOfWeek] == string.Empty)
            {
                lblDayOfWeek.Text = AllString;
            }
            else
            {
                lblDayOfWeek.Text = Enum.Parse(typeof (DayOfWeek), parameters[DictionaryParameter.DayOfWeek]).ToString();
            }

            lblDate.Text = string.Format("From {0} to {1}", parameters[DictionaryParameter.StartDate], parameters[DictionaryParameter.EndDate]);
            lblValues.Text = parameters[DictionaryParameter.AvailabilityKeyGrouping];
            lblDisplay.Text = parameters[DictionaryParameter.AvailabilityDayGrouping];
            lblLocationCountry.Text = locationCountry;
            lblPoolRegion.Text = poolRegion;
            lblLocationGroupArea.Text = locationArea;
            lblLocation.Text = location;
            lblOwningCountry.Text = owningCountry;
            lblCarSegment.Text = carSegment;
            lblCarClass.Text = carClass;
            lblCarGroup.Text = carGroup;
            lblFleetTypes.Text = fleetTypes;
        }


        protected void gvFleetStatus_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        


            if (e.Row.RowType != DataControlRowType.Header) return;
            e.Row.Cells[0].Text = "Fleet Status";
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
            e.Row.Cells[23].Text = TopicTranslation.GetAvailabilityTopicShortDescription(AvailabilityTopic.OnRent);
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
            //var dataTable = HtmlTableGenerator.GenerateCsvFromGridview(gvFleetStatus);
            var sw = new StringWriter();
            var htextw = new HtmlTextWriter(sw);
            tblExportTable.RenderControl(htextw);
            Session["ExportData"] = sw.ToString();
            Session["ExportFileType"] = "xls";
            Session["ExportFileName"] = string.Format("Availability Fleet status Export {0}", DateTime.Now.ToShortDateString());
        }
    }
}