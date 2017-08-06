using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.Phase4Bll.Administration;
using Mars.App.Classes.Phase4Dal;
using Mars.App.Classes.Phase4Dal.Administration;
using Mars.App.Classes.Phase4Dal.Enumerators;
using Mars.App.UserControls.Phase4.Administration.Mapping.EntityPopups;

namespace Mars.App.UserControls.Phase4.Administration.Mapping
{
    public partial class EntityParameter : UserControl
    {
        public const string ParamChangedString = "ParamChangedString";
        public const string QuickSelectChangedString = "QuickSelectChangedString";
        public const string NewEntityString = "NewEntity";

        public const string SessionSelectedFiltersInEntityParameter = "SessionSelectedFiltersInEntityParameter";



        /// <summary>
        /// Accecssed by the PopupEntityUserControl
        /// </summary>
        protected Dictionary<DictionaryParameter, string> SelectedSessionParameters
        {
            set { Session[SessionSelectedFiltersInEntityParameter + ParameterType] = value; }
        }

        
        public AdminMappingEnum ParameterType
        {
            get { return (AdminMappingEnum)Enum.Parse(typeof(AdminMappingEnum), hfParameterType.Value); }
            set { hfParameterType.Value = value.ToString(); }
        }

        private string ActiveOnlySelected
        {
            get
            {
                return rbActive.SelectedValue == "2"
                                 ? string.Empty
                                 : rbActive.SelectedValue == "0" ? "true" : "false";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;
            if (!IsPostBack)
            {
                SetLabels();

                using (var dataAccess = new MappingListSelect())
                {
                    PopulateCountryParameters(dataAccess.DataContext,
                        ParameterType == AdminMappingEnum.Country
                        ? true.ToString()
                        : (cbActiveCountry.Visible && cbActiveCountry.Checked).ToString());
                    ddlPool.Items.Add(ParameterDataAccess.EmptyItem);
                    ddlRegion.Items.Add(ParameterDataAccess.EmptyItem);
                    ddlArea.Items.Add(ParameterDataAccess.EmptyItem);
                    ddlLocationGroup.Items.Add(ParameterDataAccess.EmptyItem);
                    ddlCarSegment.Items.Add(ParameterDataAccess.EmptyItem);
                    ddlCarClass.Items.Add(ParameterDataAccess.EmptyItem);
                }
                PopulateCompanyDropdown(0, 0);
            }

        }

        private void PopulateCompanyDropdown(int countryId, int companyTypeId)
        {
            LocationPopup.AddCompanyDropDownItems(countryId, companyTypeId, ddlCompany, true);
        }

        public void Page_PreRender(object sender, EventArgs e)
        {
            SelectedSessionParameters = GetParameters();
        }

        protected void cbActiveCountry_Changed(object sender, EventArgs e)
        {
            using (var dataAccess = new MappingListSelect())
            {
                PopulateCountryParameters(dataAccess.DataContext, (cbActiveCountry.Visible && cbActiveCountry.Checked).ToString());

                ClearCmsDropDowns();
                ClearOpsDropDowns();
                ddlCarSegment.Items.Clear();
                ddlCarSegment.Items.Add(ParameterDataAccess.EmptyItem);
                ddlCarClass.Items.Clear();
                ddlCarClass.Items.Add(ParameterDataAccess.EmptyItem);
            }
        }

        public void SetMessage(string message)
        {
            lblMessage.Text = message;
        }

        private void ClearCmsDropDowns()
        {
            ddlPool.Items.Clear();
            ddlPool.Items.Add(ParameterDataAccess.EmptyItem);
            ddlLocationGroup.Items.Clear();
            ddlLocationGroup.Items.Add(ParameterDataAccess.EmptyItem);
        }


        private void ClearOpsDropDowns()
        {
            ddlRegion.Items.Clear();
            ddlRegion.Items.Add(ParameterDataAccess.EmptyItem);
            ddlArea.Items.Clear();
            ddlArea.Items.Add(ParameterDataAccess.EmptyItem);
        }
        private void SetLabels()
        {
            var type = string.Empty;
            cbActiveCountry.Visible = true;
            
            switch (ParameterType)
            {
                case AdminMappingEnum.Country:
                    type = "Countries";
                    btnAddNew.Text = "New Country";
                    cbActiveCountry.Visible = false;
                    break;
                case AdminMappingEnum.CmsPool:
                    lblCountry.Visible = true;
                    ddlCountry.Visible = true;
                    btnAddNew.Text = "New Pool";
                    type = "Pools";
                    break;
                case AdminMappingEnum.CmsLocationGroup:
                    type = "Location Group";
                    btnAddNew.Text = "New Location Group";
                    lblCountry.Visible = true;
                    ddlCountry.Visible = true;
                    lblPool.Visible = true;
                    ddlPool.Visible = true;
                    

                    break;
                case AdminMappingEnum.OpsRegion:
                    type = "Region";
                    btnAddNew.Text = "New Region";
                    lblCountry.Visible = true;
                    ddlCountry.Visible = true;
                    break;
                case AdminMappingEnum.OpsArea:
                    type = "Area";
                    btnAddNew.Text = "New Area";
                    lblCountry.Visible = true;
                    ddlCountry.Visible = true;
                    lblRegion.Visible = true;
                    ddlRegion.Visible = true;
                    break;
                case AdminMappingEnum.Location:
                    type = "Location";
                    btnAddNew.Visible = false;
                    lblCountry.Visible = true;
                    ddlCountry.Visible = true;

                    rblCmsOps.Visible = true;
                    lblRegion.Visible = rblCmsOps.SelectedValue != "0";
                    ddlRegion.Visible = rblCmsOps.SelectedValue != "0";
                    lblArea.Visible = rblCmsOps.SelectedValue != "0";
                    ddlArea.Visible = rblCmsOps.SelectedValue != "0";
                    lblPool.Visible = rblCmsOps.SelectedValue == "0";
                    ddlPool.Visible = rblCmsOps.SelectedValue == "0";
                    lblLocationGroup.Visible = rblCmsOps.SelectedValue == "0";
                    ddlLocationGroup.Visible = rblCmsOps.SelectedValue == "0";

                    lblCompany.Visible = true;
                    ddlCompany.Visible = true;

                    break;
                case AdminMappingEnum.CarSegment:
                    type = "Car Segment";
                    btnAddNew.Text = "New Car Segment";
                    lblCountry.Visible = true;
                    ddlCountry.Visible = true;

                    break;
                case AdminMappingEnum.CarClass:
                    type = "Car Class";
                    btnAddNew.Text = "New Car Class";
                    lblCountry.Visible = true;
                    ddlCountry.Visible = true;
                    lblCarSegment.Visible = true;
                    ddlCarSegment.Visible = true;

                    break;
                case AdminMappingEnum.CarGroup:
                    type = "Car Group";
                    btnAddNew.Visible = false;
                    lblCountry.Visible = true;
                    ddlCountry.Visible = true;
                    lblCarSegment.Visible = true;
                    ddlCarSegment.Visible = true;
                    lblCarClass.Visible = true;
                    ddlCarClass.Visible = true;

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            lblActive.Text = string.Format("Active {0}:", type);
            lblQuickComplete.Text = string.Format("Quick Complete {0}:", type);

        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            RaiseBubbleEvent(this, new CommandEventArgs(NewEntityString, ParameterType));
        }

        private void PopulateCountryParameters(MarsDBDataContext dataContext, string activeOnly)
        {
            var d = AdminParameterDataAccess.GetAllCountryListItems(dataContext, activeOnly);
            ddlCountry.Items.Clear();
            ddlCountry.Items.AddRange(d.ToArray());
        }

        private void PopulatePoolParameters(MarsDBDataContext dataContext, string activeOnly)
        {
            var parameters = GetParameters();
            var d = AdminParameterDataAccess.GetAdminPoolListItems(parameters, dataContext, activeOnly);
            ddlPool.Items.Clear();
            ddlPool.Items.AddRange(d.ToArray());
        }

        private void PopulateLocationGroupParameters(MarsDBDataContext dataContext, string activeOnly)
        {
            var parameters = GetParameters();
            var d = AdminParameterDataAccess.GetAdminLocationGroupListItems(parameters, dataContext, activeOnly);
            ddlLocationGroup.Items.Clear();
            ddlLocationGroup.Items.AddRange(d.ToArray());
        }

        private void PopulateAreaParameters(MarsDBDataContext dataContext, string activeOnly)
        {
            var parameters = GetParameters();
            var d = AdminParameterDataAccess.GetAdminAreaListItems(parameters, dataContext, activeOnly);
            ddlArea.Items.Clear();
            ddlArea.Items.AddRange(d.ToArray());
        }

        private void PopulateRegionParameters(MarsDBDataContext dataContext, string activeOnly)
        {
            var parameters = GetParameters();
            var d = AdminParameterDataAccess.GetAdminRegionListItems(parameters, dataContext, activeOnly);
            ddlRegion.Items.Clear();
            ddlRegion.Items.AddRange(d.ToArray());
        }

        private void PopulateCarSegmentParameters(MarsDBDataContext dataContext, string activeOnly)
        {
            var parameters = GetParameters();
            var d = AdminParameterDataAccess.GetAdminCarSegmentListItems(parameters, dataContext, activeOnly);
            ddlCarSegment.Items.Clear();
            ddlCarSegment.Items.AddRange(d.ToArray());
        }

        private void PopulateCarClassParameters(MarsDBDataContext dataContext, string activeOnly)
        {
            var parameters = GetParameters();
            var d = AdminParameterDataAccess.GetAdminCarClassListItems(parameters, dataContext, activeOnly);
            ddlCarClass.Items.Clear();
            ddlCarClass.Items.AddRange(d.ToArray());
        }

        protected void rblCmsOps_SelectionChanged(object sender, EventArgs e)
        {
            if(rblCmsOps.SelectedValue == "0") //CMS
            {
                if (ddlCountry.SelectedIndex == 0)
                {
                    ddlRegion.Items.Clear();
                    ddlRegion.Items.Add(ParameterDataAccess.EmptyItem);
                }
                else
                {
                    ddlRegion.SelectedIndex = 0;    
                }
                
                ddlArea.Items.Clear();
                ddlArea.Items.Add(ParameterDataAccess.EmptyItem);
            }
            else
            {
                if (ddlCountry.SelectedIndex == 0)
                {
                    ddlPool.Items.Clear();
                    ddlPool.Items.Add(ParameterDataAccess.EmptyItem);
                }
                else
                {
                    ddlPool.SelectedIndex = 0;
                }
                
                ddlLocationGroup.Items.Clear();
                ddlLocationGroup.Items.Add(ParameterDataAccess.EmptyItem);
            }

            
            SetLabels();
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            RaiseBubbleEvent(this, new CommandEventArgs(ParamChangedString, ParameterType));
        }

        protected void rbActive_SelectionChanged(object sender, EventArgs e)
        {

            using (var dataAccess = new MappingListSelect())
            {
                if (ParameterType == AdminMappingEnum.Country)
                {
                    PopulateCountryParameters(dataAccess.DataContext, ActiveOnlySelected);
                }
            }
            RaiseBubbleEvent(this, new CommandEventArgs(ParamChangedString, ParameterType));

        }


        public Dictionary<DictionaryParameter, string> GetParameters()
        {
            var parameters = new Dictionary<DictionaryParameter, string>
                                 {
                                     {DictionaryParameter.LocationCountry, ddlCountry.SelectedValue},
                                     {DictionaryParameter.Pool, ddlPool.SelectedValue},
                                     {DictionaryParameter.Region, ddlRegion.SelectedValue},
                                     {DictionaryParameter.LocationGroup, ddlLocationGroup.SelectedValue},
                                     {DictionaryParameter.Area, ddlArea.SelectedValue},
                                     {DictionaryParameter.ActiveOnly, ActiveOnlySelected},
                                     {DictionaryParameter.ContainsString, tbQuickComplete.Text},
                                     {DictionaryParameter.CarSegment, ddlCarSegment.SelectedValue},
                                     {DictionaryParameter.CarClass, ddlCarClass.SelectedValue},
                                     {DictionaryParameter.CompanyId, ddlCompany.SelectedValue},
                                 };
            
            return parameters;
        }

        protected void ddlCountry_SelectionChanged(object sender, EventArgs e)
        {
            using (var dataAccess = new MappingListSelect())
            {
                if (ddlPool.Visible || ddlRegion.Visible)
                {
                    PopulatePoolParameters(dataAccess.DataContext, ActiveOnlySelected);
                    PopulateRegionParameters(dataAccess.DataContext, ActiveOnlySelected);
                    ddlArea.Items.Clear();
                    ddlArea.Items.Add(ParameterDataAccess.EmptyItem);
                    ddlLocationGroup.Items.Clear();
                    ddlLocationGroup.Items.Add(ParameterDataAccess.EmptyItem);
                }
                if (ddlCarSegment.Visible)
                {
                    PopulateCarSegmentParameters(dataAccess.DataContext, ActiveOnlySelected);
                    ddlCarClass.Items.Clear();
                    ddlCarClass.Items.Add(ParameterDataAccess.EmptyItem);
                }
            }
            var countryId = ddlCountry.SelectedValue == string.Empty ? 0 : int.Parse(ddlCountry.SelectedValue);
            PopulateCompanyDropdown(countryId, 0);
            RaiseBubbleEvent(this, new CommandEventArgs(ParamChangedString, ParameterType));

        }

        protected void ddlPool_SelectionChanged(object sender, EventArgs e)
        {

            if (ddlLocationGroup.Visible)
            {
                using (var dataAccess = new MappingListSelect())
                {
                    PopulateLocationGroupParameters(dataAccess.DataContext, ActiveOnlySelected);
                }
            }

            RaiseBubbleEvent(this, new CommandEventArgs(ParamChangedString, ParameterType));
        }

        protected void ddlRegion_SelectionChanged(object sender, EventArgs e)
        {
            if (ddlArea.Visible)
            {
                using (var dataAccess = new MappingListSelect())
                {
                    PopulateAreaParameters(dataAccess.DataContext, ActiveOnlySelected);
                }
            }

            RaiseBubbleEvent(this, new CommandEventArgs(ParamChangedString, ParameterType));
        }

        protected void ddlCarSegment_SelectionChanged(object sender, EventArgs e)
        {
            if (ddlCarClass.Visible)
            {
                using (var dataAccess = new MappingListSelect())
                {
                    PopulateCarClassParameters(dataAccess.DataContext, ActiveOnlySelected);
                }
            }
            RaiseBubbleEvent(this, new CommandEventArgs(ParamChangedString, ParameterType));
        }

        protected void ParameterChanged(object sender, EventArgs e)
        {
            RaiseBubbleEvent(this, new CommandEventArgs(ParamChangedString, ParameterType));
        }

        protected void AutoCompleteEntity(object sender, EventArgs e)
        {
            tbQuickComplete.Focus();
            var functionString = string.Format("$('#{0}').val($('#{0}').val())", tbQuickComplete.ClientID);

            //ScriptManager.RegisterStartupScript(this, typeof(Page), "MoveCursor", functionString, true);
            RaiseBubbleEvent(this, new CommandEventArgs(QuickSelectChangedString, ParameterType));

        }
    }
}