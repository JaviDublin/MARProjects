using App.BLL.VehiclesAbroad.Models.Filters;
using System.Web.UI.WebControls;
using App.DAL.VehiclesAbroad.Abstract;
using App.Classes.BLL.Pooling.Models.Abstract;
using System;

namespace App.Classes.BLL.Pooling.Models {
    public class FilterModel2 : FilterModel, IFilterModel2 {         

        protected readonly String FEEDBACKARGUMENT = "All";

        protected Label _errorLabel;
        public Label ErrorLabel {
            get { return _errorLabel; }
            set { _errorLabel = value; _errorLabel.ForeColor = System.Drawing.Color.Red; }
        }
        protected Label _feedbackLabel;
        public Label FeedbackLabel {
            get { return _feedbackLabel; }
            set { _feedbackLabel = value; _feedbackLabel.Font.Bold = true; }
        }

        public FilterModel2(IFilterRepository filterRepository) : base(filterRepository) { }

        public override void bind(params string[] dependants) {
            var q = Repository.getList(dependants);
            clear();
            foreach (var item in q)
                FilterDropDownList.Items.Add(item);
            FilterDropDownList.DataBind();
            SetFeedback();
        }
        public override void rebind(int selectedIndex, params string[] dependants) {
            var q = Repository.getList(dependants);
            clear();
            foreach (var item in q)
                FilterDropDownList.Items.Add(item);
            FilterDropDownList.SelectedIndex = selectedIndex;
            FilterDropDownList.DataBind();
            SetFeedback();
        }
        public void SetFeedback() {
            if (FeedbackLabel == null) return;
            FeedbackLabel.Text = SelectedValue == String.Empty ? FEEDBACKARGUMENT : SelectedValue;
        }
        //public int GetIndex(string s)
        //{
        //    if (string.IsNullOrEmpty(s)) return -1;
        //    for (int i = 0; i < FilterDropDownList.Items.Count; i++)
        //    {
        //        if (FilterDropDownList.Items[i].Value == s) return i;
        //    }
        //    return -1;
        //}
    }
}