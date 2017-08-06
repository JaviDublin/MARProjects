using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL;

namespace App.UserControls.Mappings.Details
{
    public partial class CarSegmentDetails : System.Web.UI.UserControl
    {
        #region Properties and Fields
        private string _validationGroup;
        private int _car_segment_id;
        private string _car_segment;
        private string _country;
        private int _sort_car_segment;

        private string _errorMessage;

        public int Car_Segment_Id
        {
            get { return _car_segment_id; }
            set { _car_segment_id = value; }
        }

        public string Car_Segment
        {
            get { return _car_segment; }
            set { _car_segment = value; }
        }

        public string Country
        {
            get { return _country; }
            set { _country = value; }
        }

        public int Sort_Car_Segment
        {
            get { return _sort_car_segment; }
            set { _sort_car_segment = value; }
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
            _validationGroup = SessionHandler.MappingCarSegmentValidationGroup;

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


            switch (SessionHandler.MappingCarSegmentDefaultMode)
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

            this.TextBoxCarSegment.Text = string.Empty;
            this.TextBoxSortOrder.Text = "0";

            this.DatabindCountries(true);
            this.TextBoxCarSegment.Focus();

        }


        protected void LoadEditMode()
        {

            this.DatabindCountries(false);
            this.LabelCarSegmentId.Text = _car_segment_id.ToString();
            this.TextBoxCarSegment.Text = _car_segment;
            this.TextBoxSortOrder.Text = _sort_car_segment.ToString();
            this.DropDownListCountries.SelectedValue = _country;

            this.TextBoxCarSegment.Focus();


        }


        protected void DatabindCountries(bool allSelected)
        {
            this.DropDownListCountries.Items.Clear();
            this.DropDownListCountries.DataSource = ReportLookups.GetCountriesAll();
            this.DropDownListCountries.DataBind();
            this.AddInitialValueToDropDown(this.DropDownListCountries, allSelected);

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
                int car_segment_id = 0;
                string car_segment = this.TextBoxCarSegment.Text;
                int sort_car_segment = Convert.ToInt32(this.TextBoxSortOrder.Text.Trim());
                string country = Convert.ToString(this.DropDownListCountries.SelectedValue);

                int result = -1;

                switch (SessionHandler.MappingCarSegmentDefaultMode)
                {
                    case (int)App.BLL.Mappings.Mode.Insert:
                        result = MappingsCarSegment.InsertCarSegment(car_segment, country, sort_car_segment);

                        break;
                    case (int)App.BLL.Mappings.Mode.Edit:
                        car_segment_id = Convert.ToInt32(this.LabelCarSegmentId.Text);
                        result = MappingsCarSegment.UpdateCarSegment(car_segment_id, car_segment, country, sort_car_segment);

                        break;
                }

                if (result == 0)
                {
                    //Success
                    _errorMessage = Resources.lang.MessageCarSegmentSaved;
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