using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL;

namespace App.UserControls.Mappings.Details
{
    public partial class CarClassDetails : System.Web.UI.UserControl
    {
        #region Properties and Fields
        private string _validationGroup;
        private int _car_class_id;
        private string _car_class;
        private int _car_segment_id;
        private int _sort_car_class;

        private string _errorMessage;
        public int Car_Class_Id
        {
            get { return _car_class_id; }
            set { _car_class_id = value; }
        }

        public string Car_Class
        {
            get { return _car_class; }
            set { _car_class = value; }
        }

        public int Car_Segment_Id
        {
            get { return _car_segment_id; }
            set { _car_segment_id = value; }
        }

        public int Sort_Car_Class
        {
            get { return _sort_car_class; }
            set { _sort_car_class = value; }
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
        public void LoadDetails()
        {
            //Set Validation Group
            _validationGroup = SessionHandler.MappingCarClassValidationGroup;

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

            switch (SessionHandler.MappingCarClassDefaultMode)
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
            this.TextBoxCarClass.Text = string.Empty;
            this.TextBoxSortOrder.Text = "0";
            this.DatabindCarSegments(true);
            this.TextBoxCarClass.Focus();


        }


        protected void LoadEditMode()
        {
            this.DatabindCarSegments(false);
            this.LabelCarClassId.Text = _car_class_id.ToString();
            this.TextBoxCarClass.Text = _car_class;
            this.TextBoxSortOrder.Text = _sort_car_class.ToString();
            this.DropDownListCarSegment.SelectedValue = Convert.ToString(_car_segment_id);
            this.TextBoxCarClass.Focus();


        }


        protected void DatabindCarSegments(bool allSelected)
        {

            this.DropDownListCarSegment.Items.Clear();
            this.DropDownListCarSegment.DataSource = ReportLookups.GetCarSegmentsWithCountries();
            this.DropDownListCarSegment.DataBind();
            this.AddInitialValueToDropDown(this.DropDownListCarSegment, allSelected);


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
                int car_class_id = -1;
                string car_class = this.TextBoxCarClass.Text;
                int car_segment_id = Convert.ToInt32(this.DropDownListCarSegment.SelectedValue);
                int sort_car_class = Convert.ToInt32(this.TextBoxSortOrder.Text.Trim());

                int result = -1;

                switch (SessionHandler.MappingCarClassDefaultMode)
                {
                    case (int)App.BLL.Mappings.Mode.Insert:

                        result = MappingsCarClass.InsertCarClass(car_class, car_segment_id, sort_car_class);
                        break;
                    case (int)App.BLL.Mappings.Mode.Edit:
                        car_class_id = Convert.ToInt32(this.LabelCarClassId.Text);
                        result = MappingsCarClass.UpdateCarClass(car_class_id, car_class, car_segment_id, sort_car_class);

                        break;
                }

                if (result == 0)
                {
                    //Success
                    _errorMessage = Resources.lang.MessageAreaCodeSaved;
                }
                else
                {
                    //Failed
                    _errorMessage = Resources.lang.ErrorMessageAdministrator;
                }

                //Raise custom event from parent page
                if (SaveMappingDetails != null)
                {
                    SaveMappingDetails(this, EventArgs.Empty);
                }
            }
            else
            {
                //Keep the modal popup form show
                this.ModalPopupExtenderMappingDetails.Show();
            }


        }
        #endregion
    }
}