using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.FleetAllocation.BusinessLogic.DropdownLists;
using Mars.FleetAllocation.DataAccess.CurrencyConversion;
using Mars.FleetAllocation.DataAccess.CurrencyConversion.Entities;


namespace Mars.FleetAllocation.UserControls.Factors
{
    public partial class UsdConversionRate : UserControl
    {

        public const string FaoSessionStoredConversionRatesString = "FaoSessionStoredConversionRatesString";

        protected void Page_Load(object sender, EventArgs e)
        {
            ucConversionRates.GridItemType = typeof(UsdEuroConversionRow);
            ucConversionRates.SessionNameForGridData = FaoSessionStoredConversionRatesString;
            ucConversionRates.ColumnHeaders = UsdEuroConversionRow.HeaderRows;
            ucConversionRates.ColumnFormats = UsdEuroConversionRow.Formats;

            if (!IsPostBack)
            {
                
                FillRates();
            }
        }

        protected override bool OnBubbleEvent(object source, EventArgs args)
        {
            var handled = false;

            if (args is CommandEventArgs)
            {
                var commandArgs = args as CommandEventArgs;
                if (commandArgs.CommandName == AutoGrid.EditCommand)
                {

                    var yearParameter = int.Parse(commandArgs.CommandArgument.ToString());
                    lblYear.Text = commandArgs.CommandArgument.ToString();
                    UsdEuroConversionRow rateRow;
                    using (var dataAccess = new CurrencyConversionDataAccess())
                    {
                        rateRow = dataAccess.GetRateByYear(yearParameter);
                        
                    }
                    tbEuroRate.Text = rateRow.EuroRate.ToString("0.000");
                    tbGbpRate.Text = rateRow.GbpRate.ToString("0.000");
                    
                    mpeCurrencyRateLimit.Show();


                    handled = true;
                }
            }
            return handled;
        }


        public void FillRates()
        {
            using (var dataAccess = new CurrencyConversionDataAccess())
            {
                var data = dataAccess.GetConversionRates(3, 1);
                ucConversionRates.GridData = data;
            }
        }

        protected void btnSaveRate_Click(object sender, EventArgs e)
        {
            var year = int.Parse(lblYear.Text);
            var eurRate = double.Parse(tbEuroRate.Text);
            var gbpRate = double.Parse(tbGbpRate.Text);
            using (var dataAccess = new CurrencyConversionDataAccess())
            {
                dataAccess.UpdateRateForYear(year, eurRate, gbpRate);
            }
            FillRates();
        }
    }
}