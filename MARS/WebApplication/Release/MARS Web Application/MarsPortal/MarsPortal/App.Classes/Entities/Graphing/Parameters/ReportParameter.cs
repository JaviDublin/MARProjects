using System.Collections.Generic;
using System.Web.UI.WebControls;
using Resources;
using System;

namespace App.Entities.Graphing.Parameters
{
    public class ReportParameter
    {
        public string Name { get; private set; }
        public string LabelName { get; private set; }

        public string SelectedValue { get { return ParameterDropDownList.SelectedValue == LocalizedParameterControl.AllParameterSelection ? "" : ParameterDropDownList.SelectedValue; } }

        private string _previousValue;
        public string PreviousValue 
        {
            get { return _previousValue; }
            set 
            {
                if (value != "")
                    LastNonEmptyValue = value;
                _previousValue = value;
            } 
        }
        public string LastNonEmptyValue { get; set; }

        public bool IsVisible { 
            set { 
                ParameterDropDownList.Visible = value;
                ParameterLabel.Visible = value;
            }
            
        }
        
        public DropDownList ParameterDropDownList { get; set; }
        public Label ParameterLabel { get; set; }

        public bool HasValueChanged
        {
            get { return PreviousValue != SelectedValue ? true : false; }
        }

        public int OptionIndex { get; set; }
        public int BranchIndex { get; set; }

         public Func<Dictionary<string, string>, List<ListItem>> GetData { get; set; }

         public ReportParameter(int branchIndex, int optionIndex, Func<Dictionary<string, string>, List<ListItem>> accessMethod, string name)
        {
            Name = name;
            LabelName = "lbl" + name;
            BranchIndex = branchIndex;
            OptionIndex = optionIndex;
            GetData = accessMethod;
            ParameterDropDownList = new DropDownList() { AutoPostBack = true, ID="ddl" + name, Width = 120};
            ParameterDropDownList.Items.Add(new ListItem(LocalizedParameterControl.AllParameterSelection, LocalizedParameterControl.AllParameterSelection));
            ParameterLabel = new Label();
            PreviousValue = "";
            IsVisible = true;
        }

        public List<ListItem> GetParameterOptions(Dictionary<string,string> sqlParams)
        {
            return GetData(sqlParams);
        }

        public void SetParameterOptions(Dictionary<string, string> sqlParams)
        {
            ClearDropdownList();
            if (GetData != null)
            {
                var data = GetData(sqlParams);
                if (data != null)
                {
                    ParameterDropDownList.Items.AddRange(data.ToArray());
                }
            }
        }
        
        public void ClearDropdownList()
        {
            if (ParameterDropDownList.Items.Count > 1)
            {
                ParameterDropDownList.Items.Clear();
                ParameterDropDownList.Items.Add(new ListItem(LocalizedParameterControl.AllParameterSelection, LocalizedParameterControl.AllParameterSelection));
            }
        }

        public string GetParameterTitle()
        {
            return string.Format("{0}: {1}   ", Name, ParameterDropDownList.SelectedItem.Text);
        }
    }
}