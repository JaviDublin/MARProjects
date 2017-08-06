using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using App.BLL.ExtensionMethods;
using App.Entities.Graphing.Parameters;
using App.UserControls.Parameters;
using App.UserControls.DatePicker;
using Mars.App.Classes.BLL.EventArgs;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.UserControls.Parameters;

namespace App.UserControls.Parameters
{
    public partial class GeneralReportParameters : System.Web.UI.UserControl
    {
        internal event MenuEventHandler TabChanged;

        public bool IsNonRevReport { get; set; }
        
        public bool HideReportTypeControl
        {
            set
            {
                tcSelectionReport.Visible = !value;
                tblGeneralParams.Width = "850px";
            }
        }

        public bool ShowQuickLocationGroupBox
        {
            set { DynamicReportParametersControl.ShowQuickLocationGroupBox = value; }
        }

        public bool HideDatePicker
        {
            set { DynamicReportParametersControl.HideDatePicker = value; }  
        }

        public bool RootParameterPostback
        {
            set { DynamicReportParametersControl.RootParameterPostback = value; }
        }

        public bool GridviewDatePicker
        {
            set { DynamicReportParametersControl.GridviewDatePicker = value; }
        }

        internal int ExportType
        {
            set { eeExcelExport.ExportType = value; }
            get { return eeExcelExport.ExportType; }
        }

        public bool DisplayExcelFilteringParameters
        {
            set
            { 
                eeExcelExport.DisplayParameters = value;
                if(value)
                    pnlChart.CssClass = "adjustmentdynamicParamsNarrow";
            }
            get { return eeExcelExport.DisplayParameters; }
        }
   
        public bool SingleDateSelection
        {
            set { DynamicReportParametersControl.SingleDateSelection = value; }
   
        }

        public bool ShowSimpleDatePicker
        {
            set { DynamicReportParametersControl.ShowSimpleDateSelector = value; }
        }

        public bool NextNinetyDayOnly
        {
            get
            {
                return DynamicReportParametersControl.NextNinetyDayOnly;
            }

            set
            {
                DynamicReportParametersControl.NextNinetyDayOnly = value;
            }
        }

        public bool FutureDatesOnly
        {
            get
            {
                return DynamicReportParametersControl.FutureDatesOnly;
            }

            set
            {
                DynamicReportParametersControl.FutureDatesOnly = value;
            }
        }

        internal ReportTypeParameters ReportTypeControl { get { return rtSelectionReport; } }
        internal DynamicReportParameters DynamicReportParametersControl { get { return DynamicParameters; } }
        internal ExportToExcel ExcelExportControl { get { return eeExcelExport; } }
        internal NonRevenueParameters NonRevParams { get { return ucNonRevParams; } }

        internal List<ReportParameter> ParamsHolder
        {
            get { return DynamicParameters.ParamsHolder; }
            set { DynamicParameters.ParamsHolder = value; }
        }

        internal Dictionary<string, string> SelectedParameters
        {
            get
            {
                if(DynamicParameters.ShowSimpleDateSelector)
                {
                    DynamicParameters.SelectedParameters.AddOrUpdateInDictionary(ParameterNames.FromDate, DynamicParameters.SimpleDateSelected);
                    DynamicParameters.SelectedParameters.AddOrUpdateInDictionary(ParameterNames.ToDate, DynamicParameters.SimpleDateSelected);
                    
                }
                else
                {
                    if (DynamicParameters.SingleDateSelection)
                    {
                        var datePicker = (DatePicker.DatePicker)DynamicParameters.FindControl("ucDatePicker");
                        DynamicParameters.SelectedParameters.AddOrUpdateInDictionary(ParameterNames.FromDate, datePicker.FromDate);
                        DynamicParameters.SelectedParameters.AddOrUpdateInDictionary(ParameterNames.ToDate, datePicker.ToDate);
                    }
                    else
                    {
                        var datePicker = (DateRangePicker)DynamicParameters.FindControl("ucDateRange");
                        DynamicParameters.SelectedParameters.AddOrUpdateInDictionary(ParameterNames.FromDate, datePicker.FromDate);
                        DynamicParameters.SelectedParameters.AddOrUpdateInDictionary(ParameterNames.ToDate, datePicker.ToDate);
                    }
                }

                
                return DynamicParameters.SelectedParameters;
            }
            set 
            { 
                DynamicParameters.SelectedParameters = value;
            }   
        }

        private void SetNonRevParameterOptions(bool isNonRev)
        {
            ucNonRevParams.Visible = isNonRev;
            rtSelectionReport.Visible = !isNonRev;
            DynamicParameters.ShowOpsSelector = isNonRev;
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.LoadComplete += PageLoadComplete;
            if(!IsPostBack)
            {
                mvReportsSelection.ActiveViewIndex = 0;
                RaiseTabChangedEvent(this, new MenuEventArgs(new MenuItem("0", "0")));
            }
            

        }

        protected void PageLoadComplete(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetDisplay("0");
            }
            SetNonRevParameterOptions(IsNonRevReport);
        }

        protected void menuReportsSelection_MenuItemClick(object sender, MenuEventArgs e)
        {
            SetDisplay(e.Item.Value);
            RaiseTabChangedEvent(sender, e);
        }

        private void SetDisplay(string tabIndex)
        {
            switch (tabIndex)
            {
                case "0":
                    tcExcelParams.Visible = false;                    
                    tcDynamParams.Width = 1050;
                    ParamsHolder.ShowAllParamsNotInRoot(true);
                    
                    break;
                case "2":
                    tcExcelParams.Visible = false;
                    tcDynamParams.Width = 1050;
                    ParamsHolder.ShowAllParamsNotInRoot(true);

                    break;

                case "1":
                    tcExcelParams.Visible = true;                    
                    tcExcelParams.Width = 350;
                    tcExcelParams.VerticalAlign = VerticalAlign.Top;
                    switch (ExportType)
                    {
                        case 1: //groupings only
                            ParamsHolder.ShowAllParamsNotInRoot(false);
                            DynamicReportParametersControl.HideDatePicker = false;
                            tcDynamParams.Width = 700;
                            break;

                        case 2: //filtering site only 
                            ParamsHolder.ShowAllParamsInBranch(2, false);
                            DynamicReportParametersControl.HideDatePicker = false;
                            tcDynamParams.Width = 700;
                            break;

                        case 3: //grouping site only, filtering fleet only
                            ParamsHolder.ShowAllParamsInBranch(1, false);
                            DynamicReportParametersControl.HideDatePicker = false;
                            tcDynamParams.Width = 700;
                            break;

                        case 4: //groupings only, no date, no chart  
                            ParamsHolder.ShowAllParamsNotInRoot(false);
                            DynamicReportParametersControl.HideDatePicker = true;
                            tcDynamParams.Width = 200;

                            break;

                        default://filtering only, no change required
                            tcExcelParams.VerticalAlign = VerticalAlign.Middle;
                            tcDynamParams.Width = 700;
                            break;
                    }

                    break;
            }
            
        }

        private void RaiseTabChangedEvent(object sender, MenuEventArgs e)
        {
            if (TabChanged != null)
                TabChanged(this, e);
        }
    }
}