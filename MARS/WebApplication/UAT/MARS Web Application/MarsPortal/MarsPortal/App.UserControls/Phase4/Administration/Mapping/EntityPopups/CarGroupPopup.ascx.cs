using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Bll.Administration;
using Mars.App.Classes.Phase4Dal.Administration;
using Mars.App.Classes.Phase4Dal.Administration.MappingEntities;
using Mars.App.Classes.Phase4Dal.Enumerators;

namespace Mars.App.UserControls.Phase4.Administration.Mapping.EntityPopups
{
    public partial class CarGroupPopup : PopupEntityUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;
        }

        protected void bnSavePopup_Click(object sender, EventArgs e)
        {
            string message;
            var cge = new CarGroupEntity
            {
                Id = int.Parse(hfCarGroupId.Value),
                CarClassId = int.Parse(ddlCarClass.SelectedValue),
                //Using the Selected Text here, one day the Car Group table will change to use Ids then this can change to selectedValue
                CarGroupGold = ddlGoldUpgrade.SelectedItem.Text,
                CarGroupFiveStar = ddlFiveStarUpgrade.SelectedItem.Text,
                CarGroupPlatinum = ddlPlatinum.SelectedItem.Text,
                CarGroupPresidentCircle = ddlPresidentCircle.SelectedItem.Text,
                Active = cbActive.Checked
            };
            using (var dataAccess = new MappingSingleUpdate())
            {
                message = dataAccess.UpdateCarGroup(cge);
            }
            if (message == string.Empty)
            {
                var parameters = new List<string> { AdminMappingEnum.CarGroup.ToString(), UpdateCarGroupSuccess };
                RaiseBubbleEvent(this, new CommandEventArgs(App.Site.Administration.Mappings.Mappings.MappingUpdate, parameters));
            }
            else
            {
                lblMessage.Text = message;
                ShowPopup();
            }
        }

        public override void ShowPopup()
        {
            mpCarGroup.Show();
        }

        public override void SetValues(int id)
        {
            CarGroupEntity pe;
            List<ListItem> carClasses, carSegments, carGroupsGold;

            

    
            var parameters = new Dictionary<DictionaryParameter, string>();

            using (var dataAccess = new MappingSingleSelect())
            {
                pe = dataAccess.GetCarGroupEntity(id);
                parameters.Add(DictionaryParameter.CarSegment, pe.CarSegmentId.ToString(CultureInfo.InvariantCulture));
                parameters.Add(DictionaryParameter.LocationCountry, pe.CountryId.ToString(CultureInfo.InvariantCulture));
                carClasses = AdminParameterDataAccess.GetAdminCarClassListItems(
                    parameters, dataAccess.DataContext, string.Empty, false);
                carSegments = AdminParameterDataAccess.GetAdminCarSegmentListItems(
                    parameters, dataAccess.DataContext, string.Empty, false);
                carGroupsGold = AdminParameterDataAccess.GetAdminCarGroupsWithinSegmentListItems(
                    parameters, dataAccess.DataContext, string.Empty, false);

            }

            //Need to clong this list as Selected is contained in the List Item and sharing it overrides Selected
            var carGroupsPlatinum = new List<ListItem>(carGroupsGold.Count);
            var fiveStar = new List<ListItem>(carGroupsGold.Count);
            var presidentCircle = new List<ListItem>(carGroupsGold.Count);
            foreach(var cg in carGroupsGold)
            {
                carGroupsPlatinum.Add(new ListItem(cg.Text, cg.Value));
                fiveStar.Add(new ListItem(cg.Text, cg.Value));
                presidentCircle.Add(new ListItem(cg.Text, cg.Value));
            }
            

            lblCountryName.Text = pe.CountryName;
            
            lblCarGroup.Text = pe.CarGroupName;
            cbActive.Checked = pe.Active;
            hfCarGroupId.Value = id.ToString(CultureInfo.InvariantCulture);

            SetDropDownList(ddlCarClass, carClasses, pe.CarClassId);
            SetDropDownList(ddlCarSegment, carSegments, pe.CarSegmentId);

            SetDropDownListByText(ddlGoldUpgrade, carGroupsGold, pe.CarGroupGold);
            SetDropDownListByText(ddlPlatinum, carGroupsPlatinum, pe.CarGroupPlatinum);
            SetDropDownListByText(ddlFiveStarUpgrade, fiveStar, pe.CarGroupFiveStar);
            SetDropDownListByText(ddlPresidentCircle, presidentCircle, pe.CarGroupPresidentCircle);
            
        }



        protected void ddlCarSegment_SelectionChanged(object sender, EventArgs e)
        {
            var parameters = new Dictionary<DictionaryParameter, string>();
            List<ListItem> carClasses;
            parameters.Add(DictionaryParameter.CarSegment, ddlCarSegment.SelectedValue);
            using (var dataAccess = new MappingSingleSelect())
            {
                carClasses = AdminParameterDataAccess.GetAdminCarClassListItems(
                    parameters, dataAccess.DataContext, string.Empty, false);
            }
            SetDropDownList(ddlCarClass, carClasses, -1);
            ShowPopup();
        }
    }
}