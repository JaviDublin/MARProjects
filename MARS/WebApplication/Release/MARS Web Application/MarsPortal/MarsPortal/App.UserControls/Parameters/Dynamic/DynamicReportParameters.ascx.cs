using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using App.BLL.ExtensionMethods;
using App.BLL.EventArgs;
using App.Entities.Graphing.Parameters;
using App.UserControls.DatePicker;
using Mars.App.Classes.BLL.EventArgs;

//Note: In the below code and comments, the root is considered the lowest point in the tree structure, and branches extend up from it
//      This might be confusing as graphically the root element is rendered on top and the branches extend downwards.
using Mars.App.Classes.BLL.ExtensionMethods;

namespace App.UserControls.Parameters
{
    /// <summary>
    /// This class allows you to build a two branched tree of parameters with one root. Any change in a lower (closer to the root)
    /// parameter will reset all parameters higher than it.
    /// The exception is if this change comes from the Quick / Autocomplete function
    /// </summary>
    public partial class DynamicReportParameters : System.Web.UI.UserControl
    {
        internal List<ReportParameter> ParamsHolder;

        private bool _raiseChangeNotification;
        private bool _hideGroupingText;

        private bool _rebuildTable;
        public bool ShowOpsSelector
        {
            set
            {
                rblOpsSelector.Visible = value;
                _rebuildTable = true;
            }
        }

        public bool HideGroupingText
        {
            set
            {
                _hideGroupingText = value;
                if (_hideGroupingText)
                    pnlDynamicParameters.GroupingText = "";
            }
        }


        public bool FlatTable { get; set; }

        public bool RootParameterPostback { get; set; }


        public bool HideDatePicker { get; set; }


        internal DateRangePicker DateRangePickerControl
        {
            get { return ucDateRange; }
        }

        public bool GridviewDatePicker
        {
            get
            {
                return ucDatePicker.GridviewDatePicker;
            }

            set
            {
                ucDatePicker.GridviewDatePicker = value;
            }
        }

        public bool NextNinetyDayOnly
        {
            get
            {
                return ucDatePicker.NextNinetyDayOnly;
            }

            set
            {
                ucDatePicker.NextNinetyDayOnly = value;
            }
        }


        public bool FutureDatesOnly
        {
            get
            {
                return ucDatePicker.FutureDatesOnly;
            }

            set
            {
                ucDatePicker.FutureDatesOnly = value;
            }
        }

        internal Dictionary<string, string> SelectedParameters = new Dictionary<string, string>();

        private bool _singleDateSelection;
        internal string QuickSelectedValue
        {
            get { return quickSelectLocationGroup.QuickSelectedValue; }
            set { quickSelectLocationGroup.QuickSelectedValue = value; }
        }

        public bool ShowQuickLocationGroupBox
        {
            set { quickSelectLocationGroup.Visible = value; }
        }


        public bool ShowNoDateSelector
        {
            set
            {
                ucDateRange.Visible = !value;
                ucDatePicker.Visible = !value;
                pnlSingleDate.Visible = !value;    
            }
            
        }

        public bool ShowSimpleDateSelector
        {
            set
            {
                ucDateRange.Visible = !value;
                ucDatePicker.Visible = !value;
                pnlSingleDate.Visible = value;
                
            }
            get { return pnlSingleDate.Visible; }
        }

        public string SimpleDateSelected
        {
            get { return tbDate.Text; }
            set { tbDate.Text = value; }
        }
        
        public bool SingleDateSelection
        {
            set
            {
                if (HideDatePicker)
                {
                    ucDateRange.Visible = false;
                    ucDatePicker.Visible = false;
                }
                else
                {
                    ucDateRange.Visible = !value;
                    ucDatePicker.Visible = value;
                    _singleDateSelection = value;
                }
            }
            get { return _singleDateSelection; }
        }


        protected void OpsLogicChanged(object sender, EventArgs e)
        {
            RaiseBubbleEvent(this, new OpsParameterLogicChangedArgs {OpsSelected = rblOpsSelector.SelectedIndex != 0});
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                tbDate.Text = DateTime.Now.ToShortDateString();
            }
            SetupTable();
            Page.LoadComplete += PageLoadComplete;
        }

        private void SetupTable()
        {
            if (!FlatTable)
                BuildTable();
            else
                BuildFlatTable();

            
            var root = ParamsHolder.GetRootParameter();
            if (root.ParameterDropDownList.Items.Count == 1)
            {
                root.SetParameterOptions(SelectedParameters);
            }
        }


        protected void PageLoadComplete(object sender, EventArgs e)
        {
            if(_rebuildTable)
            {
                SetupTable();    
            }

            //The root parameter is a special case as it is the only one where a change will effect more than one branch.
            var root = ParamsHolder.GetRootParameter();
            SelectedParameters.AddOrUpdateInDictionary(root.Name, root.SelectedValue);
            
            if (!string.IsNullOrEmpty(QuickSelectedValue))
            {
                if (root.HasValueChanged)
                {
                    ParamsHolder.ClearBelowHighestChangedParameter(2);
                    UpdateParametersInBranch(ParamsHolder.GetFirstParameterInBranch(2));
                }
                //Set all previous values to the current ones, so the next postback won't think anything was changed.
                ParamsHolder.ForEach(p => p.PreviousValue = p.SelectedValue);

                UpdateParametersInBranch(ParamsHolder.GetFirstParameterInBranch(1));
                UpdateParametersInBranch(ParamsHolder.GetFirstParameterInBranch(2));
                QuickSelectedValue = "";
                return;
            }

            //No Quickset, check if the root parameter was changed. If it was we need to clear all parameters above the root

            if (root.HasValueChanged)
            {
                ParamsHolder.ClearAllAboveRoot();
            }
            else
            {
                ParamsHolder.ClearBelowHighestChangedParameter(1);
                ParamsHolder.ClearBelowHighestChangedParameter(2);
            }

            //Goes through each branch, looking for changes the user has made since the last postback
            UpdateParametersInBranch(ParamsHolder.GetFirstParameterInBranch(1));
            UpdateParametersInBranch(ParamsHolder.GetFirstParameterInBranch(2));


            //Once we're done, set all previous values to the current, so the next post back doesn't think anything has changed
            ParamsHolder.ForEach(p => p.PreviousValue = p.SelectedValue);

            upnlParameters.Update();


            if (_raiseChangeNotification)
                RaiseBubbleEvent(sender, new ParameterChangeEventArgs());

            quickSelectLocationGroup.AccessMethod = rblOpsSelector.Visible ? "GetBranchList" : "GetLocationPoolList";
        }

        

        

        /// <summary>
        /// Recursive method! Is started at the first parameter above the root. Whatever it's current value is added or updated in the dictionary
        /// Then checks if the parameter needs updating. If the value below it has changed, it needs to reload it's drop down list.
        /// Only if there is a parameter higher than it, does the method recursivly call itself
        /// </summary>
        /// <param name="currentParameter"></param>
        private void UpdateParametersInBranch(ReportParameter currentParameter)
        {
            if (currentParameter == null) return;
            SelectedParameters.AddOrUpdateInDictionary(currentParameter.Name, currentParameter.SelectedValue);
            if (ParamsHolder.DoesParameterNeedUpdating(currentParameter))
            {
                currentParameter.SetParameterOptions(SelectedParameters);
            }

            var paramAbove = ParamsHolder.GetNextHighestParameter(currentParameter);

            if (paramAbove == null) return;
            UpdateParametersInBranch(paramAbove);
        }

        /// <summary>
        /// Generates a table from the Parameters passed in. The root sits on top of the 4 cell table
        /// Then a Label Cell | Label Cell layout with branch 1 on the left and branch 2 on the right
        /// </summary>
        
        private void BuildTable()
        {
            phParameterTable.Controls.Clear();
            var table = new Table();

            //Add the root level
            var tableRow = new TableRow();
            AddParameterCellToRow(tableRow, 0, 0, false);
            table.Rows.Add(tableRow);

            for (var i = 1; i <= ParamsHolder.Max(p => p.OptionIndex); i++)
            {
                tableRow = new TableRow();

                AddParameterCellToRow(tableRow, 1, i, false);
                AddParameterCellToRow(tableRow, 2, i, false);

                table.Rows.Add(tableRow);
            }

            phParameterTable.Controls.Add(table);
        }

        private void BuildFlatTable()
        {            
            pnlDynamicParameters.Height = 30;
            phParameterTable.Controls.Clear();
            var table = new Table();
            var tableRow = new TableRow();

            AddParameterCellToRow(tableRow, 0, 0, true);
            
            for (var i = 1; i <= ParamsHolder.Max(p => p.OptionIndex); i++)
                AddParameterCellToRow(tableRow, 1, i, true);               
            
            for (var i = 1; i <= ParamsHolder.Max(p => p.OptionIndex); i++)
                AddParameterCellToRow(tableRow, 2, i, true);

            table.Rows.Add(tableRow);
            phParameterTable.Controls.Add(table);
        }

        private void AddParameterCellToRow(TableRow tableRow, int branchIndex, int optionIndex, bool isFlatTable)
        {
            
            var dropDownTableCell = new TableCell();
            var labelTableCell = new TableCell();

            var param = ParamsHolder.Where(p => p.BranchIndex == branchIndex && p.OptionIndex == optionIndex).FirstOrDefault();
            
            if (param == null)
            {
                if(!isFlatTable)
                    tableRow.Cells.Add(new TableCell() { ColumnSpan = 2 });
                return;
            }

            param.ParameterLabel.ID = param.LabelName;
            param.ParameterLabel.Text = param.Name + ": ";
            

            //No need to post back if it's the last parameter in a branch
            //if (optionIndex > 0 && !ParamsHolder.Exists(p => p.BranchIndex == branchIndex && p.OptionIndex == optionIndex + 1)
            //    && !isFlatTable && !RootParameterPostback)
            //{
            //    param.ParameterDropDownList.AutoPostBack = false;
            //}

            param.ParameterDropDownList.SelectedIndexChanged += new EventHandler(ParameterDropDownList_SelectedIndexChanged);

            if (optionIndex == 0 && !isFlatTable)
            {
                dropDownTableCell.ColumnSpan = 4;
                dropDownTableCell.Controls.Add(param.ParameterLabel);
            }
            else
            {
                labelTableCell.Controls.Add(param.ParameterLabel);
                tableRow.Cells.Add(labelTableCell);
                tableRow.CssClass = "Left-Align";    
            }
        
            dropDownTableCell.Controls.Add(param.ParameterDropDownList);
            tableRow.Cells.Add(dropDownTableCell);

            
        }

        void ParameterDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            _raiseChangeNotification = true;
        }
    }
}