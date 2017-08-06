using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.UserControls.Details
{
    public partial class AvailabilityCarSearchDetails : System.Web.UI.UserControl
    {

        #region Properties and Fields

        private string _remark;
        private DateTime _nextOnRentdate;
        private string _group;
        private string _modelCode;
        private string _model;
        private string _ownarea;
        private string _driverName;
        private string _blockDate;
        private string _license;
        private string _unit;
        private string _vehicleIdentNbr;
        private int _kilometers;
        private string _regDate;
        private string _lstwwd;
        private string _lstdate;
        private string _duewwd;
        private string _duedate;
        private string _movetype;
        private string _lastDocument;
        private string _operstat;
        private int _nonRev;
        private string _bdDays;
        private string _prevwwd;
        private string _mmDays;
        private string _groupCharged;
        private string _lsttime;
        private string _duetime;

        private string _carhold;
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }

        public DateTime NextOnRentDate
        {
            get { return _nextOnRentdate; }
            set { _nextOnRentdate = value; }
        }

        public string Group
        {
            get { return _group; }
            set { _group = value; }
        }
        public string ModelCode
        {
            get { return _modelCode; }
            set { _modelCode = value; }
        }
        public string Model
        {
            get { return _model; }
            set { _model = value; }
        }

        public string OwnArea
        {
            get { return _ownarea; }

            set { _ownarea = value; }
        }
        public string DriverName
        {
            get { return _driverName; }
            set { _driverName = value; }
        }
        public string BlockDate
        {
            get { return _blockDate.ToString(); }
            set { _blockDate = value; }
        }
        public string License
        {
            get { return _license; }
            set { _license = value; }
        }
        public string Unit
        {
            get { return _unit; }
            set { _unit = value; }
        }
        public string VehicleIdentNbr
        {
            get { return _vehicleIdentNbr; }
            set { _vehicleIdentNbr = value; }
        }
        public string Kilometers
        {
            get { return _kilometers.ToString(); }
            set { _kilometers = Convert.ToInt32(value); }
        }
        public string RegDate
        {
            get { return _regDate; }
            set { _regDate = value; }
        }
        public string Lstwwd
        {
            get { return _lstwwd; }
            set { _lstwwd = value; }
        }
        public string LstDate
        {
            get { return _lstdate; }
            set { _lstdate = value; }
        }
        public string Duewwd
        {
            get { return _duewwd; }
            set { _duewwd = value; }
        }
        public string DueDate
        {
            get { return _duedate; }
            set { _duedate = value; }
        }
        public string Movetype
        {
            get { return _movetype; }
            set { _movetype = value; }
        }
        public string LastDocument
        {
            get { return _lastDocument; }
            set { _lastDocument = value; }
        }
        public string Operstat
        {
            get { return _operstat; }
            set { _operstat = value; }
        }
        public string NonRev
        {
            get { return _nonRev.ToString(); }
            set { _nonRev = Convert.ToInt32(value); }
        }
        public string BDDays
        {
            get { return _bdDays; }
            set { _bdDays = value; }
        }
        public string Prevwwd
        {
            get { return _prevwwd; }
            set { _prevwwd = value; }
        }
        public string MMDays
        {
            get { return _mmDays; }
            set { _mmDays = value; }
        }
        public string GroupCharged
        {
            get { return _groupCharged; }
            set { _groupCharged = value; }
        }
        public string Lsttime
        {
            get { return _lsttime; }
            set { _lsttime = value; }
        }
        public string Duetime
        {
            get { return _duetime; }
            set { _duetime = value; }
        }
        public string Carhold
        {
            get { return _carhold; }
            set { _carhold = value; }
        }

        public Label LabelSerial
        {
            get { return this.LabelSerialTitle; }
        }

        public AjaxControlToolkit.ModalPopupExtender ModalExtenderCarDetails
        {
            get { return this.ModalPopupExtenderCarSearch; }
        }

        #endregion

        #region Page Methods

        public void LoadDetails()
        {

            //Set values for labels / textboxes
            LabelDisplayBDDays.Text = _bdDays;
            LabelDisplayBlockdate.Text = _blockDate;
            LabelDisplayCarhold.Text = _carhold;
            LabelDisplayCharged.Text = _groupCharged;
            LabelDisplayDriverName.Text = _driverName;
            LabelDisplayDuedate.Text = _duedate;
            LabelDisplayDuetime.Text = _duetime;
            LabelDisplayDueWWD.Text = _duewwd;
            LabelDisplayGroup.Text = _group;
            LabelDisplayKilometers.Text = _kilometers.ToString();
            LabelDisplayLastDocument.Text = _lastDocument;
            LabelDisplayLicense.Text = _license;
            LabelDisplayLstdate.Text = _lstdate;
            LabelDisplayLsttime.Text = _lsttime;
            LabelDisplayLstwwd.Text = _lstwwd;
            LabelDisplayMMDays.Text = _mmDays;
            LabelDisplayModel.Text = _model;
            LabelDisplayModelcode.Text = _modelCode;
            LabelDisplayMovetype.Text = _movetype;
            LabelDisplayNonrev.Text = _nonRev.ToString();
            LabelDisplayOperstat.Text = _operstat;
            LabelDisplayOwnarea.Text = _ownarea;
            LabelDisplayPrevWWD.Text = _prevwwd;
            LabelDisplayRegDate.Text = _regDate;
            LabelDisplayUnit.Text = _unit;
            LabelDisplayVIdNo.Text = _vehicleIdentNbr;
            TextAreaRemarks.Text = _remark;

            DatePickerNextOnRent.DefaultDate = _nextOnRentdate.Year == 1 ? DateTime.Now : _nextOnRentdate;
            
            DatePickerNextOnRent.SetDateDefault();
        }

        #endregion

        #region Control Methods

        public event EventHandler SaveRemarks;

        protected void ButtonOk_Click(object sender, System.EventArgs e)
        {

            if (Page.IsValid)
            {
                _remark = Server.HtmlEncode(this.TextAreaRemarks.Text.Trim());
                _vehicleIdentNbr = this.LabelDisplayVIdNo.Text.Trim();

                TextBox expectedResolutionDate = (TextBox)this.DatePickerNextOnRent.FindControl("TextBoxDatePicker");
                _nextOnRentdate = Convert.ToDateTime(expectedResolutionDate.Text);

                //Raise custom event from parent page
                if (SaveRemarks != null)
                {
                    SaveRemarks(this, EventArgs.Empty);
                }

            }
        }

        #endregion
    }
}