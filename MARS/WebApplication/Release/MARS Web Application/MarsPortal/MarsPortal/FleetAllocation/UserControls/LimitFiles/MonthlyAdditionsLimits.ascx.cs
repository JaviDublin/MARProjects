using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.FleetAllocation.DataAccess.AdditionsLimits;
using Mars.FleetAllocation.DataAccess.AdditionsLimits.Entities;

namespace Mars.FleetAllocation.UserControls.AdditionsLimits
{
    public partial class MonthlyAdditionsLimits : UserControl
    {
        private const string FaoMonthlyAdditionsFileRow = "FaoMonthlyAdditionsFileRow";

        

        protected void Page_Load(object sender, EventArgs e)
        {
            
            ucMonthlyAdds.GridItemType = typeof(MonthlyFileLimitRow);
            ucMonthlyAdds.SessionNameForGridData = FaoMonthlyAdditionsFileRow;
            ucMonthlyAdds.ColumnHeaders = MonthlyFileLimitRow.HeaderRows;
            ucMonthlyAdds.ColumnFormats = MonthlyFileLimitRow.Formats;
            
            

            if (!IsPostBack)
            {
                using (var dataAccess = new MonthlyAddLimitDataAccess())
                {
                    var data = dataAccess.GetMonthlyAdditionsFileRows();
                    ucMonthlyAdds.GridData = data;
                }
            }
        }

        protected override bool OnBubbleEvent(object source, EventArgs args)
        {
            var handled = false;

            if (args is CommandEventArgs)
            {
                var commandArgs = args as CommandEventArgs;
                if (commandArgs.CommandName == AutoGrid.ViewKeyword)
                {
                    var fileUploadId = int.Parse(commandArgs.CommandArgument.ToString());
                    
                }
                

                handled = true;
            }

            return handled;
        }

        
    }
}