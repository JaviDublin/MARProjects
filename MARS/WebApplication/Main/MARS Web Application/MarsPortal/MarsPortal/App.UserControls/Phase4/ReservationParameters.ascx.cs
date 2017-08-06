using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Dal.Enumerators;

namespace Mars.App.UserControls.Phase4
{
    public partial class ReservationParameters : UserControl
    {
        private const string ReservationParameterSessionName = "ReservationParameterSessionName";

        public bool ShowAdditionalIndividualReservationParameters
        {
            set { pnlIndividualReservationParameters.Visible = value; }
        }

        public Dictionary<DictionaryParameter, string> SessionStoredReservationParameters
        {
            get { return (Dictionary<DictionaryParameter, string>)Session[ReservationParameterSessionName]; }
            set { Session[ReservationParameterSessionName] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ucSingleParameters.SessionStoredReservationParameters = SessionStoredReservationParameters;
            if (!IsPostBack)
            {
                Page.DataBind();
                GetDefaultParameters();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            
        }

        private void GetDefaultParameters()
        {
            tbFromDate.Text = DateTime.Now.ToShortDateString();
            tbToDate.Text = DateTime.Now.AddDays(2).ToShortDateString();
            if (SessionStoredReservationParameters != null)
            {
                ExtractParametersFromSession();
            }
        }

        public void cbBasicParams_Checked(object sender, EventArgs e)
        {
            ucMultiParameters.Visible = !cbBasicParams.Checked;
            ucSingleParameters.Visible = cbBasicParams.Checked;
            upnlParams.Update();
        }

        public Dictionary<DictionaryParameter, string> GetParameterDictionary()
        {
            var parameters = ucSingleParameters.Visible 
                ? ucSingleParameters.BuildParameterDictionary() 
                : ucMultiParameters.BuildParameterDictionary();

            AddDateRestrictions(parameters);
            SessionStoredReservationParameters = parameters;
            return parameters;
        }

        private void AddDateRestrictions(Dictionary<DictionaryParameter, string> parameters)
        {
            parameters.Add(DictionaryParameter.ReservationCheckOutInDateLogic, rblCheckOutSelection.SelectedValue);
            parameters.Add(DictionaryParameter.StartDate, tbFromDate.Text);
            parameters.Add(DictionaryParameter.EndDate, tbToDate.Text);
            
            parameters.Add(DictionaryParameter.ReservationCustomerName, acCustomerName.SelectedText);
            parameters.Add(DictionaryParameter.ReservationExternalId, acExternalId.SelectedText);
            parameters.Add(DictionaryParameter.ReservationFlightNumber, acFlightNumber.SelectedText);

        }


        protected override bool OnBubbleEvent(object sender, EventArgs args)
        {
            var handled = false;

            if (args is CommandEventArgs)
            {
                var commandArgs = args as CommandEventArgs;

                if (commandArgs.CommandName == "ParametersChanged")
                {
                    upnlParams.Update();
                }
            }
            return handled;
        }

        private void ExtractParametersFromSession()
        {
            var savedParams = SessionStoredReservationParameters;

            if (!string.IsNullOrEmpty(savedParams[DictionaryParameter.StartDate]))
            {
                tbFromDate.Text = savedParams[DictionaryParameter.StartDate];
            }

            if (!string.IsNullOrEmpty(savedParams[DictionaryParameter.EndDate]))
            {
                tbToDate.Text = savedParams[DictionaryParameter.EndDate];
            }


        }

    }
}