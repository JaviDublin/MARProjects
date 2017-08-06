using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL.Cache;
using App.BLL.EventArgs;
using App.BLL.ExtensionMethods;
using App.BLL.Utilities;
using App.DAL.MarsDataAccess.ParameterAccess;
using App.Entities.Graphing;
using App.Entities.Graphing.Parameters;
using Castle.Core.Internal;
using Dundas.Charting.WebControl;
using Mars.App.Classes.BLL.EventArgs;
using Mars.App.Classes.BLL.Pooling;
using Mars.App.Classes.BLL.Pooling.AdditionDeletion;
using Mars.App.Classes.DAL.NonRevReWrite.ParameterHolders;
using Mars.App.UserControls.DatePicker;
using Label = System.Web.UI.WebControls.Label;
using Mars.App.Classes.BLL.ExtensionMethods;

namespace Mars.App.Site.Pooling
{
    public partial class AdditionsDeletions : Page
    {
        private AdditionDeletionBusinessLogic _addDelBl;

        private AdditionDeletionFileSummary FileSummary
        {
            get
            {
                return (AdditionDeletionFileSummary)Session["AdditionDeletionSummary"];
            }
            set
            {
                Session["AdditionDeletionSummary"] = value;
            }
        }

        private GraphData AddDelInformation
        {
            get
            {
                return (GraphData)Session["AdditionDeletionParameters"];
            }
            set
            {
                Session["AdditionDeletionParameters"] = value;
            }
        }

        private string CountrySelected
        {
            get
            {
                return AddDelInformation.SelectedParameters.Count == 0 ? string.Empty : AddDelInformation.SelectedParameters[ParameterNames.Country];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
            _addDelBl = new AdditionDeletionBusinessLogic();
            adManualChanges.AddDelBl = _addDelBl;
            lblUploadMessage.Text = string.Empty;
            ShowParseButton(true);
            if (!IsPostBack)
            {
                cpeBody.Collapsed = true;
                
                AddDelInformation = new GraphData {ReportParameters = this.GetReportParameters(LocationLogic.Cms)};
            }
            else
            {
                cpeBody.Collapsed = false;
                cpeBody.ClientState = "false";
            }
            DynamicParameters.ParamsHolder = AddDelInformation.ReportParameters;
            DynamicParameters.SelectedParameters = AddDelInformation.SelectedParameters;
            
            Page.LoadComplete += PageLoadComplete;
        }


        protected void PageLoadComplete(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DynamicParameters.SimpleDateSelected = string.Empty;
            }

            if (!string.IsNullOrEmpty(DynamicParameters.QuickSelectedValue))
            {
                var branches = ParameterCache.GetAllBranches();
                this.CheckQuickBranchSelected(branches, DynamicParameters.QuickSelectedValue
                            , DynamicParameters.SelectedParameters, AddDelInformation.ReportParameters, AddDelInformation.CmsOpsLogic);

            }

        }

        

        protected override bool OnBubbleEvent(object source, EventArgs args)
        {
            var handled = false;

            var commandEvent = args as CommandEventArgs;
            if (commandEvent != null)
            {
                if (commandEvent.CommandName == "RefreshData")
                {
                    PopulateDataGrid();
                }
            }

            if (args is OpsParameterLogicChangedArgs)
            {
                var isOps = ((OpsParameterLogicChangedArgs)args).OpsSelected;
                AddDelInformation.CmsOpsLogic = isOps ? LocationLogic.Ops : LocationLogic.Cms;
                AddDelInformation.ReportParameters = this.GetReportParameters(AddDelInformation.CmsOpsLogic);
                AddDelInformation.SelectedParameters = new Dictionary<string, string>();
                DynamicParameters.ParamsHolder = AddDelInformation.ReportParameters;
                DynamicParameters.SelectedParameters = AddDelInformation.SelectedParameters;

                DynamicParameters.ShowOpsSelector = true;
                handled = true;
            }

            return handled;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!CountrySelected.IsNullOrEmpty())
            {
                adManualChanges.CarGroupCountry = CountrySelected;
                adManualChanges.ClearFields();
                PopulateDataGrid();    
            }
            else
            {
                cpeBody.ClientState = "true";
                cpeBody.Collapsed = true;
                AddDelInformation = new GraphData { ReportParameters = this.GetReportParameters(LocationLogic.Cms) };
            }
        }

        protected void btnExport_Clicked(object sender, EventArgs e)
        {
            BuildExportData();
        }



        protected void btnSaveManualChanges_Click(object sender, EventArgs e)
        {
            var additionDeletions = ExtractGridViewItems();
            _addDelBl.UpdateAdditionDeletions(additionDeletions);
            PopulateDataGrid();

        }

        protected void btnDeleteManualEntries_Click(object sender, EventArgs e)
        {
            var additionDeletions = ExtractGridViewItems();
            var itemsToDelete = additionDeletions.Where(d => d.MarkedForDeletion).ToList();
            _addDelBl.DeleteAdditionDeletions(itemsToDelete);
            PopulateDataGrid();
        }

        private List<AdditionDeletionGridViewHolder> ExtractGridViewItems()
        {
            var returned = new List<AdditionDeletionGridViewHolder>();
            foreach (RepeaterItem item in rptAddDel.Items)
            {
                var ad = new AdditionDeletionGridViewHolder();

                var cbDeleteRecord = (CheckBox)item.FindControl("cbDeleteRecord");
                var identifier = (HiddenField)item.FindControl("hfAddDelId");
                var wwd = (Label)item.FindControl("lblWwd");
                var wwdId = (HiddenField)item.FindControl("hfLocationWwdId");
                var carGroup = (Label)item.FindControl("lblCarGroup");
                var carGroupId = (HiddenField)item.FindControl("hfCarGroupId");
                var repDate = (SingleDateTimePicker)item.FindControl("sdpRepDate");
                var addition = (Label)item.FindControl("lblAddition");
                var value = (TextBox)item.FindControl("tbValue");

                ad.Identifier = int.Parse(identifier.Value);
                ad.LocationWwd = wwd.Text;
                ad.LocationWwdId = int.Parse(wwdId.Value);
                ad.CarGroup = carGroup.Text;
                ad.CarGroupId = int.Parse(carGroupId.Value);
                var inputDate = repDate.SelectedDateTime;
                if (inputDate == null) continue;
                ad.RepDate = inputDate.Value;
                ad.Addition = addition.Visible;
                int inputValue;
                var succeeded = int.TryParse(value.Text, out inputValue);
                if(!succeeded) continue;
                ad.Value = inputValue;

                if (cbDeleteRecord.Checked)
                {
                    ad.MarkedForDeletion = true;
                }

                returned.Add(ad);
            }
            return returned;
        }

        protected void btnParse_Click(object sender, EventArgs e)
        {
            if (!fuAdditionsDeletions.HasFile) return;
            var processedDataMessage = _addDelBl.ProcessFile(CountrySelected, fuAdditionsDeletions.PostedFile);
            FileSummary = processedDataMessage;
            lblUploadMessage.Text = string.Format("{0} valid entries, {1} invalid entries", processedDataMessage.ValidRows, processedDataMessage.RowsSkipped);
            
            ShowParseButton(false);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            FileSummary = null;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var fs = FileSummary;
            if (fs == null) return;
            var attachMessage = _addDelBl.InsertNewAdditionsDeletions(fs.Additions, fs.Deletions, CountrySelected);
            FileSummary = null;
            lblUploadMessage.Text = attachMessage;
            PopulateDataGrid();
        }

        private List<AdditionDeletionGridViewHolder> GetCountryGridData()
        {
            
            if (!DynamicParameters.SelectedParameters.ContainsKey(ParameterNames.Country))
            {
                return null;
            }
            var country = DynamicParameters.SelectedParameters[ParameterNames.Country];
            var repParams = new ReportsParameters
                            {
                                Country = country
                                    
                            };
            var gridData = _addDelBl.GetAdditionDeletionGridViewHolders(repParams, EnumAdditionDeletion.Both);
            return gridData;
        }


        private List<AdditionDeletionGridViewHolder> GetGridData()
        {
            var repParams = new ReportsParameters();


            DynamicParameters.SelectedParameters.AddOrUpdateInDictionary(ParameterNames.FromDate, DynamicParameters.SimpleDateSelected);
            repParams.SetParametersFromDictionary(AddDelInformation.SelectedParameters);

            EnumAdditionDeletion additionOrDeletion;
            var succeeded = Enum.TryParse(ddlAddDelSelection.SelectedValue, out additionOrDeletion);

            if (succeeded == false || string.IsNullOrEmpty(repParams.Country))
            {
                rptAddDel.DataSource = null;
                rptAddDel.DataBind();
                return null;
            }
            if (repParams.GroupId == null && string.IsNullOrEmpty(repParams.Branch))
            {
                return null;
            }


            var gridData = _addDelBl.GetAdditionDeletionGridViewHolders(repParams, additionOrDeletion);
            return gridData;
        }

        private void BuildExportData()
        {
            var sb = new StringBuilder();

            var gridData = GetCountryGridData();
            if (gridData == null) return;

            //sb.AppendLine("<html>");
            //sb.AppendLine("<body>");
            //sb.AppendLine("<table>");
            //sb.Append("<tr>");
            //sb.AppendFormat("{0},{1},{2},{3},{4},{5}" 
            //                ,  "Location", "Car Group", "Day", "Time", "Number", "AddDel");
            sb.AppendLine("Location,Car Group,Day,Time,Number,AddDel");
            //sb.AppendLine("</tr>");


            foreach (var gd in gridData)
            {
                //sb.Append("<tr>");


                //sb.AppendFormat("<td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td>"
                sb.AppendFormat("{0},{1},{2},{3},{4},{5}"
                    , gd.LocationWwd, gd.CarGroup, gd.RepDate.ToString("yyyy-MM-dd"), gd.RepDate.ToShortTimeString(), gd.Value, gd.Addition ? "ADD" : "DEL");
                sb.AppendLine();

                //sb.AppendLine("</tr>");
            }

            //sb.AppendLine("</table>");
            //sb.AppendLine("</body>");
            //sb.AppendLine("</html>");



            var s = sb.ToString();
            Session["ExportData"] = s;
            Session["ExportFileName"] = "AdditionDeletionExport";
            Session["ExportFileType"] = "csv";
        }

        private void PopulateDataGrid()
        {
            var gridData = GetGridData();
            
            rptAddDel.DataSource = gridData;
            rptAddDel.DataBind();
        }

        private void ShowParseButton(bool show)
        {
            btnParse.Visible = show;
            fuAdditionsDeletions.Enabled = show;
            btnSubmit.Visible = !show;
            btnCancel.Visible = !show;
        }


    }
}