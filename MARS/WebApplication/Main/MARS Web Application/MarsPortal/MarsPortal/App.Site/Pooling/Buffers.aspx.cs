using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.BLL.Cache;
using App.BLL.ExtensionMethods;
using App.BLL.Utilities;
using App.DAL.MarsDataAccess.ParameterAccess;
using App.Entities.Graphing;
using App.Entities.Graphing.Parameters;
using Castle.Core.Internal;
using Mars.App.Classes.BLL.EventArgs;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.BLL.Pooling.AdditionDeletion;
using Mars.App.Classes.DAL.NonRevReWrite.ParameterHolders;

namespace Mars.App.Site.Pooling
{
    public partial class Buffers : Page
    {
        private BufferBusinessLogic _bufferBl;

        private string CountrySelected
        {
            get
            {
                return BufferInformation.SelectedParameters.Count == 0 ? string.Empty : BufferInformation.SelectedParameters[ParameterNames.Country];
            }
        }

        private BufferFileSummary FileSummary
        {
            get
            {
                return (BufferFileSummary)Session["BufferSummary"];
            }
            set
            {
                Session["BufferSummary"] = value;
            }
        }

        private GraphData BufferInformation
        {
            get
            {
                return (GraphData)Session["BufferParameters"];
            }
            set
            {
                Session["BufferParameters"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _bufferBl = new BufferBusinessLogic();
            lblUploadMessage.Text = string.Empty;
            ShowParseButton(true);
            lblMessage.Text = string.Empty;
            if (!IsPostBack)
            {
                cpeBody.Collapsed = true;
                BufferInformation = new GraphData { ReportParameters = this.GetReportParameters(LocationLogic.Cms) };
            }
            else
            {
                cpeBody.Collapsed = false;
                cpeBody.ClientState = "false";
            }

            DynamicParameters.ParamsHolder = BufferInformation.ReportParameters;
            DynamicParameters.SelectedParameters = BufferInformation.SelectedParameters;



            Page.LoadComplete += PageLoadComplete;
        }

        protected void PageLoadComplete(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(DynamicParameters.QuickSelectedValue))
            {
                var branches = ParameterCache.GetAllBranches();
                this.CheckQuickBranchSelected(branches, DynamicParameters.QuickSelectedValue
                            , DynamicParameters.SelectedParameters, BufferInformation.ReportParameters, BufferInformation.CmsOpsLogic);
            }
        }


        protected override bool OnBubbleEvent(object source, EventArgs args)
        {
            var handled = false;


            var changedArgs = args as OpsParameterLogicChangedArgs;
            if (changedArgs != null)
            {
                var isOps = changedArgs.OpsSelected;
                BufferInformation.CmsOpsLogic = isOps ? LocationLogic.Ops : LocationLogic.Cms;
                BufferInformation.ReportParameters = this.GetReportParameters(BufferInformation.CmsOpsLogic);
                BufferInformation.SelectedParameters = new Dictionary<string, string>();
                DynamicParameters.ParamsHolder = BufferInformation.ReportParameters;
                DynamicParameters.SelectedParameters = BufferInformation.SelectedParameters;

                DynamicParameters.ShowOpsSelector = true;
                handled = true;
            }

            return handled;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!CountrySelected.IsNullOrEmpty())
            {
                acCarGroup.ContextKey = BufferInformation.SelectedParameters[ParameterNames.Country];
                PopulateDataGrid();
            }
            else
            {
                cpeBody.ClientState = "true";
                cpeBody.Collapsed = true;
                BufferInformation = new GraphData { ReportParameters = this.GetReportParameters(LocationLogic.Cms) };
            }
        }


        protected void btnSaveManualChanges_Click(object sender, EventArgs e)
        {
            var additionDeletions = ExtractGridViewItems();
            _bufferBl.UpdateBuffers(additionDeletions);
            PopulateDataGrid();
        }


        protected void btnAdd_Click(object sender, EventArgs e)
        {
            var message = AddBuffer();
            tbWwd.Text = string.Empty;
            tbCarGroup.Text = string.Empty;
            tbValue.Text = string.Empty;
            lblMessage.Text = message;
            PopulateDataGrid();
        }

        private string AddBuffer()
        {
            int number;
            if (CountrySelected == string.Empty)
            {
                return "Select a Country to Map Car Group Codes from";
            }

            if (tbWwd.Text.Substring(0, 2).ToLower() != CountrySelected.ToLower())
            {
                return "The WWD code must match the Country selected in the Parameter Selection";
            }

            var succeeded = int.TryParse(tbValue.Text, out number);
            if (!succeeded)
            {
                return "Invalid Amount entered";
            }
            var bufferData = new BufferGridViewHolder
            {
                LocationWwd = tbWwd.Text,
                CarGroup = tbCarGroup.Text,
                Value = number
            };
            var message = _bufferBl.InsertManualBuffer(bufferData);
            
            return message;
        }

        protected void btnDeleteManualEntries_Click(object sender, EventArgs e)
        {
            var additionDeletions = ExtractGridViewItems();
            var itemsToDelete = additionDeletions.Where(d => d.MarkedForDeletion).ToList();
            _bufferBl.DeleteBuffers(itemsToDelete);
            PopulateDataGrid();
        }

        private List<BufferGridViewHolder> ExtractGridViewItems()
        {
            var returned = new List<BufferGridViewHolder>();
            foreach (RepeaterItem item in rptBuffers.Items)
            {
                var bgvh = new BufferGridViewHolder();

                var cbDeleteRecord = (CheckBox)item.FindControl("cbDeleteRecord");
                var identifier = (HiddenField)item.FindControl("hfAddDelId");
                var wwd = (Label)item.FindControl("lblWwd");
                var wwdId = (HiddenField)item.FindControl("hfLocationWwdId");
                var carGroup = (Label)item.FindControl("lblCarGroup");
                var carGroupId = (HiddenField)item.FindControl("hfCarGroupId");
                               
                var value = (TextBox)item.FindControl("tbValue");

                bgvh.Identifier = int.Parse(identifier.Value);
                bgvh.LocationWwd = wwd.Text;
                bgvh.LocationWwdId = int.Parse(wwdId.Value);
                bgvh.CarGroup = carGroup.Text;
                bgvh.CarGroupId = int.Parse(carGroupId.Value);

                int inputValue;
                var succeeded = int.TryParse(value.Text, out inputValue);
                if (!succeeded) continue;
                bgvh.Value = inputValue;

                if (cbDeleteRecord.Checked)
                {
                    bgvh.MarkedForDeletion = true;
                }

                returned.Add(bgvh);
            }
            return returned;
        }

        protected void btnParse_Click(object sender, EventArgs e)
        {
            if (!fuBuffers.HasFile) return;
            var processedDataMessage = _bufferBl.ProcessFile(CountrySelected, fuBuffers.PostedFile);
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
            var attachMessage = _bufferBl.InsertNewBuffers(fs.Buffers, CountrySelected);
            FileSummary = null;
            lblUploadMessage.Text = attachMessage;
            PopulateDataGrid();
        }

        private List<BufferGridViewHolder> GetBufferGridData()
        {
            var repParams = new ReportsParameters();
            DynamicParameters.SelectedParameters.AddOrUpdateInDictionary(ParameterNames.FromDate, DynamicParameters.SimpleDateSelected);
            repParams.SetParametersFromDictionary(BufferInformation.SelectedParameters);

            if (string.IsNullOrEmpty(repParams.Country))
            {
                rptBuffers.DataSource = null;
                rptBuffers.DataBind();
                return null;
            }

            var gridData = _bufferBl.GetBufferGridViewHolders(repParams);
            return gridData;
        }

        protected void btnExport_Clicked(object sender, EventArgs e)
        {
            BuildExportData();
        }

        private void BuildExportData()
        {
            var sb = new StringBuilder();

            var gridData = GetBufferGridData();
            if (gridData == null) return;

            sb.AppendLine("<html>");
            sb.AppendLine("<body>");
            sb.AppendLine("<table>");
            sb.Append("<tr>");
            sb.AppendFormat("<td>{0}</td><td>{1}</td><td>{2}</td>"
                            , "Location", "Car Group", "Amount");
            sb.AppendLine("</tr>");


            foreach (var gd in gridData)
            {
                sb.Append("<tr>");


                sb.AppendFormat("<td>{0}</td><td>{1}</td><td>{2}</td>"
                    , gd.LocationWwd, gd.CarGroup, gd.Value);


                sb.AppendLine("</tr>");
            }

            sb.AppendLine("</table>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");



            var s = sb.ToString();
            Session["ExportData"] = s;
            Session["ExportFileName"] = "BufferExport";
            Session["ExportFileType"] = "xls";
        }

        private void PopulateDataGrid()
        {
            var gridData = GetBufferGridData();
            if (gridData == null)
            {
                return;
            }
            rptBuffers.DataSource = gridData;
            rptBuffers.DataBind();
        }

        private void ShowParseButton(bool show)
        {
            btnParse.Visible = show;
            fuBuffers.Enabled = show;
            btnSubmit.Visible = !show;
            btnCancel.Visible = !show;
        }

    }
}