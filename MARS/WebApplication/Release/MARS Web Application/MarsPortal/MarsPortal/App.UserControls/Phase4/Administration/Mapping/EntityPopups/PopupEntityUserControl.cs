using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Bll.Administration;
using Mars.App.Classes.Phase4Dal.Enumerators;

namespace Mars.App.UserControls.Phase4.Administration.Mapping.EntityPopups
{
    public abstract class PopupEntityUserControl : UserControl
    {
        protected const string UpdateCountrySuccess = "Country Saved";
        protected const string UpdatePoolSuccess = "Pool Saved";
        protected const string UpdateLocationGroupSuccess = "Location Group Saved";
        protected const string UpdateRegionSuccess = "Region Saved";
        protected const string UpdateAreaSuccess = "Area Saved";
        protected const string UpdateLocationSuccess = "Location Saved";
        protected const string UpdateCarSegmentSuccess = "Car Segment Saved";
        protected const string UpdateCarClassSuccess = "Car Class Saved";
        protected const string UpdateCarGroupSuccess = "Car Group Saved";


        protected const string UpdateCountryDeleted = "Country Deleted";
        protected const string UpdatePoolDeleted = "Pool Deleted";
        protected const string UpdateLocationGroupDeleted = "Location Group Deleted";
        protected const string UpdateRegionDeleted = "Region Deleted";
        protected const string UpdateAreaDeleted = "Area Deleted";
        protected const string UpdateLocationDeleted = "Location Deleted";
        protected const string UpdateCarSegmentDeleted = "Car Segment Deleted";
        protected const string UpdateCarClassDeleted = "Car Class Deleted";
        protected const string UpdateCarGroupDeleted = "Car Group Deleted";

        protected Dictionary<DictionaryParameter, string> GetEntityParameters(AdminMappingEnum type)
        {
            return  (Dictionary<DictionaryParameter, string>)
                Session[EntityParameter.SessionSelectedFiltersInEntityParameter + type];
        }

        public virtual void ShowPopup()
        {
            
        }

        /// <summary>
        /// Calling with Id 0 acts as a new Entity Popup
        /// </summary>
        /// <param name="id"></param>
        public virtual void SetValues(int id)
        {

        }

        protected void SetDropDownList(DropDownList ddl, List<ListItem> items, int selectedValue )
        {
            ddl.Items.Clear();
            ddl.Items.AddRange(items.ToArray());
            if(selectedValue == -1)
            {
                ddl.SelectedIndex = 0;
            }
            else
            {
                try
                {
                    ddl.SelectedValue = selectedValue.ToString(CultureInfo.InvariantCulture);
                }
                catch (ArgumentOutOfRangeException)
                {
                    ddl.SelectedIndex = 0;
                }
                
            }   
        }

        protected void SetDropDownListByText(DropDownList ddl, List<ListItem> items, string selectedText)
        {
            ddl.Items.Clear();
            ddl.Items.AddRange(items.ToArray());
            var selectedTextItem = items.FirstOrDefault(d => d.Text == selectedText);
            if(selectedTextItem == null)
            {
                ddl.SelectedIndex = 0;
                return;
            }
            var firstOrDefault = selectedTextItem.Value;
            ddl.SelectedValue = firstOrDefault;
        }


        /// <summary>
        /// If the message is empty, bubbles the bubbledMessage to the owning page, otherwise keeps the popup open
        /// and displays the message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="bubbledMessage"></param>
        /// <param name="type"></param>
        /// <param name="lbl"></param>
        protected void ProcessDatabaseReply(string message, string bubbledMessage, AdminMappingEnum type, Label lbl)
        {
            if (message == string.Empty)
            {
                var parameters = new List<string> { type.ToString(), bubbledMessage };
                RaiseBubbleEvent(this, new CommandEventArgs(App.Site.Administration.Mappings.Mappings.MappingUpdate, parameters));
            }
            else
            {
                lbl.Text = message;
                ShowPopup();
            }
        }

    }
}
