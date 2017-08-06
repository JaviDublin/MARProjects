using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.FleetAllocation.BusinessLogic.DropdownLists;
using Mars.FleetAllocation.DataAccess;
using Mars.FleetAllocation.DataAccess.Entities.Ranking;

namespace Mars.FleetAllocation.UserControls.Factors
{
    public partial class RankingDisplay : UserControl
    {

        public const string FaoRankingDisplaySessionString = "FaoRankingDisplaySessionString";
        public List<RankingOrderEntitiy> SessionStoredRanking
        {
            get { return (List<RankingOrderEntitiy>) Session[FaoRankingDisplaySessionString]; }
            set { Session[FaoRankingDisplaySessionString] = value; }
        }

        public string GetUpdatePanelId { get { return ucFaoParameter.UpdatePanelClientId; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            agLimits.GridItemType = typeof(RankingOrderEntitiy);
            agLimits.SessionNameForGridData = "FaoRankingLimits";
            agLimits.ColumnHeaders = RankingOrderEntitiy.HeaderRows;
            agLimits.ColumnFormats = RankingOrderEntitiy.Formats;

            if (!IsPostBack)
            {
                ListGenerator.FillDropdownWithFaoCountries(ddlCountry);
            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            using (var dataAccess = new DemandGapDataAccess())
            {
                var data = dataAccess.GetFinanceEntities();
                SessionStoredRanking = data;
                agLimits.GridData = data.ToList();
            }
            ucFaoParameter.Visible = true;
            btnFilter.Visible = true;
        }

        

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            var sessionData = SessionStoredRanking;
            var parameters = ucFaoParameter.GetParameters();
            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.Location))
            {
                var locationId = int.Parse(parameters[DictionaryParameter.Location]);
                sessionData = sessionData.Where(d => d.GetLocationId() == locationId).ToList();
            }

            if (parameters.ContainsValueAndIsntEmpty(DictionaryParameter.CarGroup))
            {
                var carGroupId = int.Parse(parameters[DictionaryParameter.CarGroup]);
                sessionData = sessionData.Where(d => d.GetCarGroupId() == carGroupId).ToList();
            }

            agLimits.GridData = sessionData;

        }
    }
}