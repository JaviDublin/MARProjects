using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.Phase4Bll.Availability;
using Mars.App.Classes.Phase4Dal;
using Mars.App.Classes.Phase4Dal.Availability;
using Mars.App.Classes.Phase4Dal.Availability.Entities;

using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.Site.ForeignVehicles;
using Mars.App.UserControls.Phase4.Availability;

namespace Mars.App.Site.Availability.FleetStatus
{
    public partial class FleetStatus : Page
    {
        public const string FleetStatusSessionParameters = "FleetStatusSessionParameters";
        
        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                using (var dataAccess = new AvailabilityDataAccess(null))
                {
                    lblLastUpdate.Text = LastUpdatedFromFleetNow.GetLastUpdatedDateTime(dataAccess);
                }
                
            }
        }



        protected void btnLoad_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void RefreshData()
        {
            FleetStatusRow fleetData;
            var mergedParameters = generalParams.GetParameterDictionary();
            var startDate = mergedParameters.GetDateFromDictionary(DictionaryParameter.StartDate);
            var endDate = mergedParameters.GetDateFromDictionary(DictionaryParameter.EndDate);
            var todaysData = startDate.Date == DateTime.Now.Date && endDate == DateTime.Now.Date;

            using (var dataAccess = new FleetStatusDataAccess(mergedParameters))
            {
                lblLastUpdate.Text = LastUpdatedFromFleetNow.GetLastUpdatedDateTime(dataAccess);
                fleetData = dataAccess.GetFleetStatus();
            }
            upnlUpdatedTime.Update();
            ucFleetStatusChart.LoadChart(fleetData);
            ucFleetStatusChart.TodaysData = todaysData;
            ucFleetStatusGrid.LoadGrid(fleetData, mergedParameters);

        }

        protected override bool OnBubbleEvent(object sender, EventArgs args)
        {
            var handled = false;
            if (args is CommandEventArgs)
            {
                var commandArgs = args as CommandEventArgs;
                if (commandArgs.CommandName == FleetStatusChart.RedirectToCarSearch)
                {
                    var parametersToTransfer = generalParams.GetParameterDictionary();
                    var barClicked = TopicTranslation.GetAvailabilityTopicFromDescription(commandArgs.CommandArgument.ToString());
                    List<string> moveTypes;
                    List<string> opStatuses;
                    using (var dataAccess = new BaseDataAccess())
                    {
                        moveTypes = dataAccess.GetMovementTypesList().Select(d => d.Value).ToList();
                        opStatuses = dataAccess.GetOperationalStatusList().Select(d => d.Value).ToList();

                    }
                    var opStatusString = string.Join(",", opStatuses);
                    var moveTypeString = string.Join(",", moveTypes);
                    var excludeOverdue = string.Empty;
                    switch (barClicked)
                    {
                        case AvailabilityTopic.TotalFleet:
                            break;
                        case AvailabilityTopic.Cu:
                            opStatusString = "2";
                            break;
                        case AvailabilityTopic.Ha:
                            opStatusString = "4";
                            break;
                        case AvailabilityTopic.Hl:
                            opStatusString = "5";
                            break;
                        case AvailabilityTopic.Ll:
                            opStatusString = "6";
                            break;
                        case AvailabilityTopic.Nc:
                            opStatusString = "8";
                            break;
                        case AvailabilityTopic.Pl:
                            opStatusString = "9";
                            break;
                        case AvailabilityTopic.Tc:
                            opStatusString = "16";
                            break;
                        case AvailabilityTopic.Sv:
                            opStatusString = "14";
                            break;
                        case AvailabilityTopic.Ws:
                            opStatusString = "19";
                            break;
                        case AvailabilityTopic.OperationalFleet:
                            opStatusString = "1,7,18,15,3,10,11,17,12,13";
                            break;
                        case AvailabilityTopic.Bd:
                            opStatusString = "1";
                            break;
                        case AvailabilityTopic.Mm:
                            opStatusString = "7";
                            break;
                        case AvailabilityTopic.Tw:
                            opStatusString = "18";
                            break;
                        case AvailabilityTopic.Tb:
                            opStatusString = "15";
                            break;
                        case AvailabilityTopic.Fs:
                            opStatusString = "3";
                            break;
                        case AvailabilityTopic.Rl:
                            opStatusString = "10";
                            break;
                        case AvailabilityTopic.Rp:
                            opStatusString = "11";
                            break;
                        case AvailabilityTopic.Tn:
                            opStatusString = "17";
                            break;
                        case AvailabilityTopic.AvailableFleet:
                            opStatusString = "12,13";
                            break;
                        case AvailabilityTopic.Idle:
                            opStatusString = "12";
                            moveTypeString = string.Join(",", moveTypes.Where(d => d != "10"));
                            break;
                        case AvailabilityTopic.Su:
                            opStatusString = "13";
                            break;
                        case AvailabilityTopic.Overdue:
                            opStatusString = "12";
                            moveTypeString = "10";
                            excludeOverdue = "-1";
                            break;
                        case AvailabilityTopic.OnRent:
                            opStatusString = "12";
                            moveTypeString = "10";
                            excludeOverdue = "1";
                            break;
                        case AvailabilityTopic.Utilization:
                            break;
                        case AvailabilityTopic.UtilizationInclOverdue:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    parametersToTransfer[DictionaryParameter.OperationalStatuses] = opStatusString;

                    parametersToTransfer[DictionaryParameter.MovementTypes] = moveTypeString;

                    parametersToTransfer[DictionaryParameter.ExcludeOverdue] = excludeOverdue;

                    Session[FleetStatusSessionParameters] = parametersToTransfer;
                    Response.Redirect("~/App.Site/Availability/CarSearch/CarSearch.aspx");
                }
                handled = true;
            }
            return handled;


        }

        
    }
}