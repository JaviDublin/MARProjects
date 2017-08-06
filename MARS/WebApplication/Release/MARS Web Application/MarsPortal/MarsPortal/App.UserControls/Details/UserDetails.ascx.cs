using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL.Utilities;
using App.BLL;

namespace App.UserControls.Details 
{
    public partial class UserDetails : System.Web.UI.UserControl {

        #region Properties and Fields

        private string _racfid;
        private string _name;
        private string _errorMessage;

        private string _validationGroup;
        public string RACFID {
            get { return _racfid; }
            set { _racfid = value; }
        }
        public string Name {
            get { return _name; }
            set { _name = value; }
        }
        public AjaxControlToolkit.ModalPopupExtender ModalExtenderUserDetails {
            get { return this.ModalPopupExtenderUserDetails; }
        }
        public string ErrorMessage {
            get { return _errorMessage; }
        }
        #endregion

        #region Page Events
        public void LoadUserDetails() {
            this.LabelMessageRoles.Visible = false;
            switch (SessionHandler.MaintanenceUsersDefaultMode) {
                case (int)Users.Mode.Insert:
                    this.LoadInsertMode();

                    break;
                case (int)Users.Mode.Edit:
                    this.LoadEditMode();

                    break;
            }
        }
        protected void LoadInsertMode() {
            _validationGroup = SessionHandler.MaintanenceUsersValidationGroup;
            this.CustomValidatorRACFID.ValidationGroup = _validationGroup;
            this.RequiredFieldValidatorName.ValidationGroup = _validationGroup;
            this.ButtonOk.ValidationGroup = _validationGroup;
            this.TextBoxRACFID.Enabled = true;
            this.CustomValidatorRACFID.Enabled = true;
            this.TextBoxRACFID.Text = string.Empty;
            this.TextBoxName.Text = string.Empty;
            this.RebindRoles();
            this.RebindRoleDescriptions();
        }
        protected void LoadEditMode() {
            _validationGroup = SessionHandler.MaintanenceUsersValidationGroup;
            this.RequiredFieldValidatorName.ValidationGroup = _validationGroup;
            this.ButtonOk.ValidationGroup = _validationGroup;
            this.TextBoxRACFID.Enabled = false;
            this.CustomValidatorRACFID.Enabled = false;
            //Fill data with user details
            this.RebindRoles();
            this.RebindRoleDescriptions();

            this.TextBoxRACFID.Text = _racfid;
            this.TextBoxName.Text = _name;

            CarSearchWorker csq = new CarSearchWorker(_racfid);
            TextBoxCarRows.Text = csq.getRowCount();

            List<Roles.RoleDetails> userRoles = Roles.SelectUserRoles(_racfid);
            System.Web.UI.WebControls.CheckBoxList dropDownListRoles = (System.Web.UI.WebControls.CheckBoxList)this.MultiSelectDropDownListRoles.FindControl("ListBoxMultiSelect");


            foreach (Roles.RoleDetails item in userRoles) {
                ListItem role = dropDownListRoles.Items.FindByValue(item.RoleId);
                if ((role != null)) { dropDownListRoles.Items[dropDownListRoles.Items.IndexOf(role)].Selected = true; }
            }
        }
        #endregion

        #region DropDown Events

        protected void RebindRoleDescriptions() {
            this.GridviewRoleDetails.DataSource = Roles.SelectRoleDescriptions();
            this.GridviewRoleDetails.DataBind();
        }


        protected void RebindRoles() {
            var dropDownListRoles = (System.Web.UI.WebControls.CheckBoxList)this.MultiSelectDropDownListRoles.FindControl("ListBoxMultiSelect");

            dropDownListRoles.DataTextField = "rolename";
            dropDownListRoles.DataValueField = "roleId";
            dropDownListRoles.Items.Clear();

            List<Roles.RoleDetails> roles = Roles.SelectRoles();

            dropDownListRoles.DataSource = roles;
            dropDownListRoles.DataBind();

            //this.AddInitialValueToListBox(dropDownListRoles, false);
        }



        private void AddInitialValueToListBox(System.Web.UI.WebControls.CheckBoxList dropDownListOption, bool selected) {
            ListItem item = new ListItem();
            item.Text = Resources.lang.ReportSettingsALL;
            item.Selected = selected;
            item.Value = "-1";
            dropDownListOption.Items.Insert(0, item);


        }
        #endregion

        #region Validation

        protected void RACFID_Validate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args) {

            if (args.Value.Length >= 1) {
                string racfid = Convert.ToString(args.Value);
                //Check if racfid already exists

                int result = Users.CheckRACFIDExits(racfid);

                if (result == 0) {
                    //Does not exist continue
                    args.IsValid = true;

                }
                else {
                    //Does exist cancel
                    args.IsValid = false;
                    this.CustomValidatorRACFID.Text = Resources.lang.ErrorMessageUniqueRACFID;
                }

            }
            else {
                //Not valid
                args.IsValid = false;
                this.CustomValidatorRACFID.Text = Resources.lang.ErrorMessageRACFID;
            }

        }

        protected bool ValidateRoleSelection() {


            var dropDownListRoles = (System.Web.UI.WebControls.CheckBoxList)this.MultiSelectDropDownListRoles.FindControl("ListBoxMultiSelect");
            var selectedCount = dropDownListRoles.Items.Cast<ListItem>().Count(item => item.Selected == true);

            return selectedCount >= 1;

        }

        #endregion

        #region Click Events

        public event EventHandler SaveUserDetails;

        protected void ButtonOk_Click(object sender, System.EventArgs e) {
            //Check page is valid
            if (Page.IsValid) {
                //Check user has selected role
                if (this.ValidateRoleSelection()) {
                    this.LabelMessageRoles.Visible = false;

                    string racfid = Server.HtmlEncode(this.TextBoxRACFID.Text.Trim().ToUpper());
                    string name = Server.HtmlEncode(this.TextBoxName.Text.Trim());
                    int result = 0;

                    switch (SessionHandler.MaintanenceUsersDefaultMode) {
                        case (int)Users.Mode.Insert:
                            //Insert user 
                            result = Users.InsertUser(racfid, name);
                            if (result == 0) {
                                //User was inserted successfully
                                // Insert roles for user
                                if (this.UpdateUserRoles(racfid) == 0) {
                                    _errorMessage = Resources.lang.MessageUserInsertedSuccessfully;
                                    break;
                                }
                                else {
                                    //Set Error message
                                    _errorMessage = Resources.lang.ErrorMessageAdministrator;
                                }
                            }
                            else {
                                //Set Error message
                                _errorMessage = Resources.lang.ErrorMessageAdministrator;
                            }

                            break;

                        case (int)Users.Mode.Edit:
                            //Update User
                            result = Users.UpdateUser(racfid, name);
                            if (result == 0) {
                                //User was updated successfully
                                // Insert roles for user
                                if (this.UpdateUserRoles(racfid) == 0) {
                                    _errorMessage = Resources.lang.MessageUserUpdatedSuccessfully;
                                    break;
                                }
                                else {
                                    //Set Error message
                                    _errorMessage = Resources.lang.ErrorMessageAdministrator;
                                }
                            }
                            else {
                                //Set Error message
                                _errorMessage = Resources.lang.ErrorMessageAdministrator;
                            }

                            break;
                    }

                    //Altered by Gavin 4/4/12
                    CarSearchWorker csw = new CarSearchWorker(racfid);
                    TextBoxCarRows.Text = csw.setRowCount(TextBoxCarRows.Text);

                    //Raise custom event from parent page
                    if (SaveUserDetails != null) {
                        SaveUserDetails(this, EventArgs.Empty);
                    }

                }
                else {
                    this.LabelMessageRoles.Visible = true;
                    this.ModalPopupExtenderUserDetails.Show();
                }

            }
            else {
                this.ModalPopupExtenderUserDetails.Show();
            }


        }

        protected int UpdateUserRoles(string racfid) {

            int result = -1;
            System.Web.UI.WebControls.CheckBoxList dropDownListRoles = (System.Web.UI.WebControls.CheckBoxList)this.MultiSelectDropDownListRoles.FindControl("ListBoxMultiSelect");
            string roleId = null;
            string countryValue = null;
            int delimiterIndex = -1;

            //Delete Current roles for user
            Roles.DeleteUserRoles(racfid);

            // Insert roles for user
            foreach (var item in dropDownListRoles.Items.Cast<ListItem>().Where(d => d.Selected)) {
                if (Convert.ToString(item.Value) == "1") {
                    //Administrator is selected
                    Users.UpdateUserRoles(racfid, 1, null);
                }
                else {
                    //Get role id and country from listbox 
                    roleId = Convert.ToString(item.Value);
                    delimiterIndex = roleId.IndexOf("|");
                    countryValue = roleId.Remove(0, delimiterIndex + 1);
                    roleId = roleId.Remove(delimiterIndex, roleId.Length - delimiterIndex);
                    //Update user roles
                    Users.UpdateUserRoles(racfid, Convert.ToInt32(roleId), countryValue);
                }
            }
            result = 0;

            //clear old cached roles and accessible countries
            string cacheKey = racfid + "Roles";
            MarsV2Cache.RemoveObjectFromCache(cacheKey);
            cacheKey = racfid + "Country";
            MarsV2Cache.RemoveObjectFromCache(cacheKey);

            return result;
        }
        #endregion
    }
}