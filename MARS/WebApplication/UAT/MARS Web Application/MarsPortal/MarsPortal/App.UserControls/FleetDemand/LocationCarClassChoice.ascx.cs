using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mars.App.UserControls.FleetDemand
{
    public partial class LocationCarClassChoice : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlCountry.Items.Add(new ListItem("Select", "0"));
                using (var dc = new Mars.App.Classes.DAL.FleetDemand.DataAccess())
                {
                    foreach (var country in dc.GetCountries())
                    {
                        ddlCountry.Items.Add(new ListItem(country.Country, country.CountryKey.ToString()));
                    }
                }

                ddlPool.Items.Clear();
                ddlLocationGroup.Items.Clear();
                ddlLocation.Items.Clear();
                ddlCarClass.Items.Clear();
            }
        }

        public int LocationKey
        {
            get
            {
                int selectedKey;
                try
                {
                    selectedKey = Int32.Parse(ddlLocation.SelectedValue);
                }
                catch( SystemException)
                {
                    selectedKey = 0;
                }

                return selectedKey;               
            }
        }

        public int CountryKey
        {
            get
            {
                int selectedKey;
                try
                {
                    selectedKey = Int32.Parse(ddlCountry.SelectedValue);
                }
                catch( SystemException)
                {
                    selectedKey = 0;
                }

                return selectedKey;               
            }
        }

        public int CarClassKey
        {
            get
            {
                int selectedKey;
                try
                {
                    selectedKey = Int32.Parse(ddlCarClass.SelectedValue);
                }
                catch (SystemException)
                {
                    selectedKey = 0;
                }

                return selectedKey;
            }
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedCountryKey = Int32.Parse(ddlCountry.SelectedValue);

            ddlPool.Items.Clear();
            ddlLocationGroup.Items.Clear();
            ddlLocation.Items.Clear();
            ddlCarClass.Items.Clear();

            using (var dc = new Mars.App.Classes.DAL.FleetDemand.DataAccess())
            {
                var pools = dc.GetPoolsForCountry(selectedCountryKey);
                if (pools.Count > 0)
                {
                    ddlPool.Items.Add(new ListItem("Select", "0"));
                    foreach (var pool in pools)
                    {
                        ddlPool.Items.Add(new ListItem(pool.CmsPool, pool.CmsPoolKey.ToString()));
                    }
                }

                var cars = dc.GetCarClassForCountry(selectedCountryKey);
                if (cars.Count > 0)
                {
                    ddlCarClass.Items.Add(new ListItem("Select", "0"));
                    foreach (var car in cars)
                    {
                        ddlCarClass.Items.Add(new ListItem(car.CarClass, car.CarClassKey.ToString()));
                    }
                }
            }

        }

        protected void ddlPool_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedPoolKey = Int32.Parse(ddlPool.SelectedValue);

            ddlLocationGroup.Items.Clear();
            using (var dc = new Mars.App.Classes.DAL.FleetDemand.DataAccess())
            {
                var locationGroups = dc.GetLocationGroupForPool(selectedPoolKey);
                if (locationGroups.Count > 0)
                {
                    ddlLocationGroup.Items.Add(new ListItem("Select", "0"));
                    foreach (var locationGroup in locationGroups)
                    {
                        ddlLocationGroup.Items.Add(new ListItem(locationGroup.LocationGroupCode, locationGroup.LocationGroupKey.ToString()));
                    }
                }
            }
            ddlLocation.Items.Clear();
        }

        protected void ddlLocationGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedLocationGroupKey = Int32.Parse(ddlLocationGroup.SelectedValue);

            ddlLocation.Items.Clear();
            using (var dc = new Mars.App.Classes.DAL.FleetDemand.DataAccess())
            {
                var locations = dc.GetLocationsForLocationGroup(selectedLocationGroupKey);
                if (locations.Count > 0)
                {
                    ddlLocation.Items.Add(new ListItem("Select", "0"));
                    foreach (var location in locations)
                    {
                        ddlLocation.Items.Add(new ListItem(location.LocationCode + " - " + location.LocationName, location.LocationKey.ToString()));
                    }
                }
            }
        }    
    
    }


}