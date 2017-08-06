using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL;
using App.DAL.MarsDataAccess.ParameterAccess;

namespace App.UserControls.Mappings.Details
{
    public partial class CarGroupDetails : System.Web.UI.UserControl
    {
        #region Properties and Fields
        private string _validationGroup;
        private int _car_group_id;
        private string _car_group;
        private string _car_group_gold;
        private int _car_class_id;
        private int _sort_car_group;


        private string _errorMessage;
        public int Car_Group_Id
        {
            get { return _car_group_id; }
            set { _car_group_id = value; }
        }

        public string Car_Group
        {
            get { return _car_group; }
            set { _car_group = value; }
        }

        public string Car_Group_Gold
        {
            get { return _car_group_gold; }
            set { _car_group_gold = value; }
        }

        public string Car_Group_FiveStar { get; set; }
        public string Car_Group_PresidentCircle { get; set; }
        public string Car_Group_Platinum { get; set; }


        public int Car_Class_Id
        {
            get { return _car_class_id; }
            set { _car_class_id = value; }
        }

        public int Sort_Car_Group
        {
            get { return _sort_car_group; }
            set { _sort_car_group = value; }
        }



        public AjaxControlToolkit.ModalPopupExtender ModalExtenderMapping
        {
            get { return this.ModalPopupExtenderMappingDetails; }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
        }
        #endregion

        #region Page Events

        public void Page_Load(object sender, EventArgs e)
        {
            tbCarGroupError.Text = string.Empty;
            tbCarGroupGoldError.Text = string.Empty;
            tbCarGroupFiveStarError.Text = string.Empty;
            tbCarGroupPresidentCircleError.Text = string.Empty;
            tbCarGroupPlatinumError.Text = string.Empty;

            acCarGroup.ContextKey = SessionHandler.MappingSelectedCountry;
            acCarGroupGold.ContextKey = SessionHandler.MappingSelectedCountry;
            acCarGroupFiveStar.ContextKey = SessionHandler.MappingSelectedCountry;
            acCarGroupPresidentCircle.ContextKey = SessionHandler.MappingSelectedCountry;
            acCarGroupPlatinum.ContextKey = SessionHandler.MappingSelectedCountry;
        }

        public void LoadDetails()
        {

            _validationGroup = SessionHandler.MappingCarGroupValidationGroup;
            this.ButtonSave.ValidationGroup = _validationGroup;


            foreach (Control cntrl in this.PanelPopupMappingDetails.Controls)
            {
                if (cntrl is TextBox)
                {
                    TextBox txtBox = (TextBox)cntrl;
                    txtBox.ValidationGroup = _validationGroup;
                }
                if (cntrl is RequiredFieldValidator)
                {
                    RequiredFieldValidator reqFieldValidator = (RequiredFieldValidator)cntrl;
                    reqFieldValidator.ValidationGroup = _validationGroup;
                }
                if (cntrl is CustomValidator)
                {
                    CustomValidator custValidator = (CustomValidator)cntrl;
                    custValidator.ValidationGroup = _validationGroup;
                }
                if (cntrl is System.Web.UI.WebControls.DropDownList)
                {
                    System.Web.UI.WebControls.DropDownList ddList = (System.Web.UI.WebControls.DropDownList)cntrl;
                    ddList.ValidationGroup = _validationGroup;
                }

            }


            switch (SessionHandler.MappingCarGroupDefaultMode)
            {
                case (int)App.BLL.Mappings.Mode.Insert:
                    this.LoadInsertMode();

                    break;
                case (int)App.BLL.Mappings.Mode.Edit:
                    this.LoadEditMode();

                    break;
            }

        }


        protected void LoadInsertMode()
        {
            
            TextBoxCarGroup.Text = string.Empty;
            TextBoxCarGroupGold.Text = string.Empty;
            TextBoxCarGroupFiveStar.Text = string.Empty;
            TextBoxCarGroupPresidentCircle.Text = string.Empty;
            TextBoxCarGroupPlatinum.Text = string.Empty;

            TextBoxSortOrder.Text = "0";

            DatabindCarClass(true);
            TextBoxCarGroup.Focus();


        }


        protected void LoadEditMode()
        {
            DatabindCarClass(false);
            

            LabelCarGroupId.Text = _car_group_id.ToString();
            TextBoxCarGroup.Text = _car_group;
            TextBoxCarGroupGold.Text = _car_group_gold;
            TextBoxCarGroupFiveStar.Text = Car_Group_FiveStar;
            TextBoxCarGroupPresidentCircle.Text = Car_Group_PresidentCircle;
            TextBoxCarGroupPlatinum.Text = Car_Group_Platinum;

            TextBoxSortOrder.Text = _sort_car_group.ToString();
            DropDownListCarClass.SelectedValue = Convert.ToString(_car_class_id);

            TextBoxCarGroup.Focus();

        }


        protected void DatabindCarClass(bool allSelected)
        {
            DropDownListCarClass.Items.Clear();
            DropDownListCarClass.DataSource = ReportLookups.GetCarClassesWithCarSegments();
            DropDownListCarClass.DataBind();
            AddInitialValueToDropDown(this.DropDownListCarClass, allSelected);

        }


        /// <summary>
        /// Add "ALL" item to drop down lists
        /// </summary>
        /// <param name="dropDownListOption"></param>
        /// <remarks></remarks>

        private void AddInitialValueToDropDown(System.Web.UI.WebControls.DropDownList dropDownListOption, bool selected)
        {
            ListItem item = new ListItem();
            item.Text = Resources.lang.MappingDropDownListInitialValue;
            item.Selected = selected;
            item.Value = "-1";
            dropDownListOption.Items.Insert(0, item);



        }
        #endregion

        #region Validation
        protected void SortOrder_Validate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            if (args.Value.Length >= 0)
            {
                //Is valid
                args.IsValid = true;
            }
            else
            {
                //Not valid
                args.IsValid = false;

            }

        }
        #endregion

        #region Click Events
        public event EventHandler SaveMappingDetails;


        protected void ButtonSave_Click(object sender, System.EventArgs e)
        {



            if (Page.IsValid)
            {
                int car_group_id = -1;
                string car_group = this.TextBoxCarGroup.Text;
                string car_group_gold = this.TextBoxCarGroupGold.Text;
                string car_group_fiveStar = this.TextBoxCarGroupFiveStar.Text;
                string car_group_PresidentCircle = this.TextBoxCarGroupPresidentCircle.Text;
                string car_group_Platinum = this.TextBoxCarGroupPlatinum.Text;
                int sort_car_group = Convert.ToInt32(TextBoxSortOrder.Text.Trim());
                int car_class_id = Convert.ToInt32(this.DropDownListCarClass.SelectedValue);





                int result = -1;

                switch (SessionHandler.MappingCarGroupDefaultMode)
                {
                    case (int)App.BLL.Mappings.Mode.Insert:
                        result = MappingsCarGroup.InsertCarGroup(car_group, car_group_gold
                                                , car_group_fiveStar
                                                , car_group_PresidentCircle
                                                , car_group_Platinum
                                                , car_class_id
                                                , sort_car_group);

                        break;
                    case (int)App.BLL.Mappings.Mode.Edit:

                        var validCarGroups = ParameterDataAccess.GetCarGroups().Where(d => d.Country == SessionHandler.MappingSelectedCountry).Select(d => d.CarGroup).ToList();

                        if (!validCarGroups.Contains(TextBoxCarGroup.Text.Trim()))
                        {
                            tbCarGroupError.Text = "Invalid Car Group";
                            ModalPopupExtenderMappingDetails.Show();
                            return;
                        }

                        if (!validCarGroups.Contains(TextBoxCarGroupGold.Text.Trim()))
                        {
                            tbCarGroupGoldError.Text = "Invalid Car Group";
                            ModalPopupExtenderMappingDetails.Show();
                            return;
                        }

                        if (!validCarGroups.Contains(TextBoxCarGroupFiveStar.Text.Trim()))
                        {
                            tbCarGroupFiveStarError.Text = "Invalid Car Group";
                            ModalPopupExtenderMappingDetails.Show();
                            return;
                        }

                        if (!validCarGroups.Contains(TextBoxCarGroupPresidentCircle.Text.Trim()))
                        {
                            tbCarGroupPresidentCircleError.Text = "Invalid Car Group";
                            ModalPopupExtenderMappingDetails.Show();
                            return;
                        }

                        if (!validCarGroups.Contains(TextBoxCarGroupPlatinum.Text.Trim()))
                        {
                            tbCarGroupPlatinumError.Text = "Invalid Car Group";
                            ModalPopupExtenderMappingDetails.Show();
                            return;
                        }


                        car_group_id = Convert.ToInt32(this.LabelCarGroupId.Text);
                        result = MappingsCarGroup.UpdateCarGroup(car_group_id, car_group, car_group_gold
                                                , car_group_fiveStar
                                                , car_group_PresidentCircle
                                                , car_group_Platinum
                                                , car_class_id, sort_car_group);

                        break;
                }

                _errorMessage = result == 0 ? Resources.lang.MessageCarGroupSaved : Resources.lang.ErrorMessageAdministrator;

                //Raise custom event from parent page
                if (SaveMappingDetails != null)
                {
                    SaveMappingDetails(this, EventArgs.Empty);
                }
            }
            else
            {
                //Keep the modal popup form show
                ModalPopupExtenderMappingDetails.Show();
            }


        }
        #endregion
    }
}