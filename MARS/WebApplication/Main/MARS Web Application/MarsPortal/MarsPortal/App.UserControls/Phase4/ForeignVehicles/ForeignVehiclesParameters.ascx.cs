using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Dal;
using Mars.App.Classes.Phase4Dal.Enumerators;

namespace Mars.App.UserControls.Phase4
{
    public partial class ForeignVehiclesParameters : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.DataBind();
                GetDefaultParameters();
            }
        }

        private void GetDefaultParameters()
        {
            using (var dataAccess = new BaseDataAccess())
            {
                lbFleet.Items.AddRange(dataAccess.GetFleetTypesList(ModuleType.NonRev).ToArray());
                var daysOfWeek = dataAccess.GetDayOfWeeks();
                daysOfWeek.Insert(0, new ListItem("All", string.Empty));

                var movTypes = dataAccess.GetMovementTypesList();
                movTypes.ForEach(d => d.Selected = d.Text != "R-O");
                var opStatuses = dataAccess.GetOperationalStatusList();
                opStatuses.ForEach(d => d.Selected = d.Text == "RT");
                lbMovementType.Items.AddRange(movTypes.ToArray());
                lbOperationalStatus.Items.AddRange(opStatuses.ToArray());
            }
        }

        public void cbBasicParams_Checked(object sender, EventArgs e)
        {
            ucMultiVehicleParams.Visible = !cbBasicParams.Checked;
            ucSingleVehicleParams.Visible = cbBasicParams.Checked;
            upnlParams.Update();
        }

        public Dictionary<DictionaryParameter, string> GetParameterDictionary()
        {
            var parameters = ucSingleVehicleParams.Visible
                ? ucSingleVehicleParams.BuildParameterDictionary()
                : ucMultiVehicleParams.BuildParameterDictionary();

            var availableParameters = BuildParameterDictionary();
            Dictionary<DictionaryParameter, string> mergedParameters;
            try
            {
                mergedParameters = parameters.Concat(availableParameters).ToDictionary(d => d.Key, d => d.Value);
            }
            catch
            {
                throw new Exception("Mixed Parameter types!");
            }
            return mergedParameters;
        }

        private Dictionary<DictionaryParameter, string> BuildParameterDictionary()
        {
            var returned = new Dictionary<DictionaryParameter, string>
                           {
                               {DictionaryParameter.FleetTypes, GetSelectedKeys(lbFleet.Items)}
                               , {DictionaryParameter.LicencePlate, acLicencePlate.SelectedText}
                               , {DictionaryParameter.Vin, acVin.SelectedText}
                               , {DictionaryParameter.UnitNumber, acUnitNumber.SelectedText}
                               , {DictionaryParameter.DriverName, acDriverName.SelectedText}
                               , {DictionaryParameter.Colour, acColour.SelectedText}
                               , {DictionaryParameter.ModelDescription, acModelDesc.SelectedText}
                               , {DictionaryParameter.OwningArea, acOwningArea.SelectedText}
                               , {DictionaryParameter.OperationalStatuses, GetSelectedKeys(lbOperationalStatus.Items)}
                               , {DictionaryParameter.MovementTypes, GetSelectedKeys(lbMovementType.Items)}
                           };
            ;
            return returned;
        }

        public static string GetSelectedKeys(ListItemCollection lic)
        {

            var selectedItems = (from ListItem li in lic where li.Selected select li.Value).ToList();
            if (selectedItems.Count == lic.Count) return string.Empty;
            var returned = string.Join(",", selectedItems);
            return returned;
        }
    }
}