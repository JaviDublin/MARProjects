
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using Mars.App.Classes.DAL.MarsDBContext;

namespace Mars.App.Classes.Phase4Dal.NonRev.Parameters
{
    internal static class NonRevParameterDataAccess
    {
        internal static List<ListItem> GetFleetTypes(MarsDBDataContext dataContext)
        {
            var returned = (from ft in dataContext.VehicleFleetTypes
                            orderby ft.FleetTypeName
                            select new ListItem(ft.FleetTypeName, ft.VehicleFleetTypeId.ToString(CultureInfo.InvariantCulture))).ToList();
            return returned;
        }

        internal static List<ListItem> GetOperationalStatuses(MarsDBDataContext dataContext)
        {
            var returned = (from os in dataContext.Operational_Status
                            orderby os.OperationalStatusCode
                            select new ListItem(" " + os.OperationalStatusCode //+ " - " + os.OperationalStatusName 
                                        , os.OperationalStatusId.ToString(CultureInfo.InvariantCulture))).ToList();
            return returned;
        }

        internal static List<ListItem> GetOwningAreas(MarsDBDataContext dataContext)
        {
            var returned = (from ac in dataContext.AREACODEs
                                    join fea in dataContext.FLEET_EUROPE_ACTUALs.Select(d=> d.OWNAREA).Distinct() on ac.ownarea equals fea
                            orderby ac.ownarea
                            select new ListItem(ac.ownarea + " - " + ac.area_name
                                        , ac.ownarea)).ToList();
            return returned;
        }


        internal static List<ListItem> GetMovementTypes(MarsDBDataContext dataContext)
        {
            var returned = (from mt in dataContext.Movement_Types
                            orderby mt.MovementTypeCode
                            select new ListItem(" " + mt.MovementTypeCode// + " - " + mt.MovementTypeName
                                        , mt.MovementTypeId.ToString(CultureInfo.InvariantCulture))).ToList();
            return returned;
        }

        internal static List<ListItem> GetRemarkReasons(MarsDBDataContext dataContext)
        {
            var returned = (from rem in dataContext.NonRev_Remarks_Lists
                            orderby rem.RemarkId != 1, rem.RemarkText
                            select new ListItem(rem.RemarkText
                                        , rem.RemarkId.ToString(CultureInfo.InvariantCulture))).ToList();
            return returned;
        }

        internal static List<string> GetLicecePlates(MarsDBDataContext dataContext)
        {
            var returned = (from rem in dataContext.Vehicles
                            select rem.LicensePlate).Distinct().ToList();
            return returned;
        }

        internal static List<string> GetVins(MarsDBDataContext dataContext)
        {
            var returned = (from rem in dataContext.Vehicles
                            select rem.Vin).Distinct().ToList();
            return returned;
        }

        internal static List<string> GetUnitNumbers(MarsDBDataContext dataContext)
        {
            var returned = (from rem in dataContext.Vehicles
                            select rem.UnitNumber.ToString()).Distinct().ToList();
            return returned;
        }

        internal static List<string> GetDriverNames(MarsDBDataContext dataContext)
        {
            var returned = (from rem in dataContext.Vehicles
                            select rem.LastDriverName).Distinct().ToList();
            return returned;
        }

        internal static List<string> GetVehicleColours(MarsDBDataContext dataContext)
        {
            var returned = (from rem in dataContext.Vehicles
                            select rem.Colour).Distinct().ToList();
            return returned;
        }

        internal static List<string> GetModelDescription(MarsDBDataContext dataContext)
        {
            var returned = (from rem in dataContext.Vehicles
                            select rem.ModelDescription).Distinct().ToList();
            return returned;
        }

    }
}