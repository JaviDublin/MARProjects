using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SqlServer.Dts.Runtime;
using System.Text;
using Mars.App.Classes.DAL.FleetDemand;
using App.BLL.ExtensionMethods;

namespace Mars.App.Site.Administration.FleetDemand
{
    public partial class OpsFleetUpload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                ddlOWRDayOfWeek.Items.Add(new ListItem("Select", "0"));
                ddlCMSDayOfWeek.Items.Add(new ListItem("Select", "0"));
                using (var dc = new Mars.App.Classes.DAL.FleetDemand.DataAccess())
                {
                    foreach (var day in dc.GetDimDays())
                    {
                        ddlOWRDayOfWeek.Items.Add(new ListItem(day.DayOfWeek, day.DayKey.ToString()));
                        ddlCMSDayOfWeek.Items.Add(new ListItem(day.DayOfWeek, day.DayKey.ToString()));
                    }
                }
            }
        }

        public int SelectedTab { get; set; }    

        protected void ExecuteSSISPackage(string filePath, byte packageId, ref long? executionId)
        {
            //throw new IOException(String.Format("File saved as {0}. Please contact RAD Team to run import job {1}", filePath, packageId));

            using (FleetDemandDataContext dc = new FleetDemandDataContext())
            {

                int? resultCode = -1;

                dc.UploadParameterFile(
                    packageId: packageId,
                    addedby: Rad.Security.ApplicationAuthentication.GetGlobalId(),
                    csvFilePath: filePath,
                    uploadedDate: DateTime.Today.ToString("yyyyMMdd"),
                    validfrom: DateTime.Today.ToString("yyyyMMdd"),
                    executionId: ref executionId,
                    resultCode: ref resultCode
                    );

                if (resultCode != 0)
                {
                    throw new IOException("SSIS asynchronous job initialization failed. Please contact RAD team.");
                }
            }              

        }

        protected void btnCMSUpload_Click(object sender, EventArgs e)
        {
            SelectedTab = 0;
            try
            {
                if (fuCMSFileUpload.HasFile)
                {
                    string filePath = Mars.Properties.Settings.Default.ParameterFilesUploadFolder;
                    string fileName = DateTime.Now.ToString("yyyyMMdd HHmmss") + "CMS -" + fuCMSFileUpload.FileName;
                    
                    if (fileName.Substring(fileName.Length - 4, 4).ToUpper() != ".CSV")
                    {
                        throw new IOException("Please select csv file.");
                    }
                    
                    string fullFilePath = Path.Combine(filePath, fileName);
                    using (FileStream sw = new FileStream(fullFilePath, FileMode.Create))
                    {
                        fuCMSFileUpload.PostedFile.InputStream.CopyTo(sw);
                    }

                    byte SSISpackageID = Mars.Properties.Settings.Default.CMSOpFleetSSIS;
                    long? executionId = null;
                    ExecuteSSISPackage(fullFilePath, SSISpackageID, ref executionId);

                    lblCMSUploadStatus.Text = "The file has been successfully uploaded. The import job number is " + executionId.ToString() + " and the job started at " + DateTime.Now.ToString() + "<br />" +
                        "\nThe Logistics Dashboard is processed on a daily basis each morning. The newly uploaded data will be used in tomorrow\'s calculations. " + "<br />" +
                        "\nThe changes you have made now will not be visible till the Dashboard is processed tomorrow.";
                }
            }
            catch (IOException ex)
            {
                lblCMSUploadStatus.Text = "Error " + DateTime.Now.ToString() + " - " + ex.Message;
                lblCMSUploadStatus.ForeColor = System.Drawing.Color.Red;
                return;
            }
        }


        protected void btnCMSSave_Click(object sender, EventArgs e)
        {
            SelectedTab = 0; 

            int selectedLocationKey = lcCMS.LocationKey;

            if (selectedLocationKey == 0)
            {
                lblCMSIndividualUploadStatus.Text = "Error - please select Location";
                lblCMSIndividualUploadStatus.ForeColor = System.Drawing.Color.Red;
                return;
            }

            try
            {
                byte selectedDayOfWeekKey = Byte.Parse(ddlCMSDayOfWeek.SelectedValue);
                if (selectedDayOfWeekKey == 0)
                {
                    lblCMSIndividualUploadStatus.Text = "Error - please select Day Of Week";
                    lblCMSIndividualUploadStatus.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                decimal parameterValue = Decimal.Parse(tbCMSParameter.Text);

                int? result = 0;

                using (FleetDemandDataContext dc = new FleetDemandDataContext())
                {

                    dc.OpFleetRatioInsert(validFrom: DateTime.Today,
                                            validFromDateKey: Int32.Parse(DateTime.Today.ToString("yyyyMMdd")),
                                            countryKey: lcCMS.CountryKey,
                                            locationKey: selectedLocationKey,
                                            dayKey: selectedDayOfWeekKey,
                                            fleetRatio: parameterValue,
                                            addedBy: Rad.Security.ApplicationAuthentication.GetGlobalId(),
                                            uploadedDate: DateTime.Today,
                                            uploadedDateKey: Int32.Parse(DateTime.Today.ToString("yyyyMMdd")),
                                            resultCode: ref result);

                    if (result == 0)
                    {
                        lblCMSIndividualUploadStatus.Text = "Success - Parameter Updated"; 
                    }
                    else
                    {
                        lblCMSIndividualUploadStatus.Text = "Error";
                        lblCMSIndividualUploadStatus.ForeColor = System.Drawing.Color.Red;
                    }

                    
                }

                CMSLogDataBind();
            }
            catch (Exception ex)
            {
                lblCMSIndividualUploadStatus.Text = "Error - " + ex.Message;
                lblCMSIndividualUploadStatus.ForeColor = System.Drawing.Color.Red;
                return;
            }
        }

        protected void rptCMSLog_PreRender(object sender, EventArgs e)
        {
            CMSLogDataBind();
        }

        protected void CMSLogDataBind()
        {
            using (var dc = new Mars.App.Classes.DAL.FleetDemand.DataAccess())
            {
                rptCMSLog.DataSource = dc.GetOpFleetRatioLog(countryKey: null
                    , cmsPoolKey: null
                    , locationGroupKey: null
                    , locationKey: lcCMS.LocationKey
                    , dayKey: null 
                    ); 
                rptCMSLog.DataBind();
            }
        }

        // psd

        protected void btnPSDUpload_Click(object sender, EventArgs e)
        {
            SelectedTab = 1;
            try
            {
                if (fuPSDFileUpload.HasFile)
                {
                    string filePath = Mars.Properties.Settings.Default.ParameterFilesUploadFolder;
                    string fileName = DateTime.Now.ToString("yyyyMMdd HHmmss") + "PSD -" + fuPSDFileUpload.FileName;

                    if (fileName.Substring(fileName.Length - 4, 4).ToUpper() != ".CSV")
                    {
                        throw new IOException("Please select csv file.");
                    }
                    
                    string fullFilePath = Path.Combine(filePath, fileName);
                    using (FileStream sw = new FileStream(fullFilePath, FileMode.Create))
                    {
                        fuPSDFileUpload.PostedFile.InputStream.CopyTo(sw);
                    }

                    byte SSISpackageID = Mars.Properties.Settings.Default.PriceablePercentSSIS;
                    long? executionId = null;
                    ExecuteSSISPackage(fullFilePath, SSISpackageID, ref executionId);

                    lblPSDUploadStatus.Text = "The file has been successfully uploaded. The import job number is " + executionId.ToString() + " and the job started at " + DateTime.Now.ToString() + "<br />" +
                        "\nThe Logistics Dashboard is processed on a daily basis each morning. The newly uploaded data will be used in tomorrow\'s calculations. " + "<br />" +
                        "\nThe changes you have made now will not be visible till the Dashboard is processed tomorrow.";
                }
            }
            catch (IOException ex)
            {
                lblPSDUploadStatus.Text = "Error " + DateTime.Now.ToString() + " - " + ex.Message;
                lblPSDUploadStatus.ForeColor = System.Drawing.Color.Red;
                return;
            }
        }

        protected void btnPSDSave_Click(object sender, EventArgs e)
        {
            SelectedTab = 1;

            int selectedLocationKey = lcPSD.LocationKey;
            int selectedCarClassKey = lcPSD.CarClassKey;

            if (selectedLocationKey == 0)
            {
                lblPSDIndividualUploadStatus.Text = "Error - please select Location";
                lblPSDIndividualUploadStatus.ForeColor = System.Drawing.Color.Red;
                return;
            }

            if (selectedCarClassKey == 0)
            {
                lblPSDIndividualUploadStatus.Text = "Error - please select Car Class";
                lblPSDIndividualUploadStatus.ForeColor = System.Drawing.Color.Red;
                return;
            }


            try
            {
                string monthYear = PSDDate.Text;
                DateTime dateTime = DateTime.ParseExact(monthYear, "MMMM yyyy", System.Globalization.CultureInfo.InvariantCulture);

                decimal parameterValue = Decimal.Parse(tbPSDParameter.Text);

                int? result = 0;

                using (FleetDemandDataContext dc = new FleetDemandDataContext())
                {
                    dc.PriceablePercentInsert(validFrom: DateTime.Today,
                                            validFromDateKey: Int32.Parse(DateTime.Today.ToString("yyyyMMdd")),
                                            countryKey: lcPSD.CountryKey,
                                            locationKey: selectedLocationKey,
                                            reportDateKey: Int32.Parse(dateTime.ToString("yyyyMMdd")),
                                            carClassKey: selectedCarClassKey,
                                            priceablePercent: parameterValue,
                                            addedBy: Rad.Security.ApplicationAuthentication.GetGlobalId(),
                                            uploadedDate: DateTime.Today,
                                            uploadedDateKey: Int32.Parse(DateTime.Today.ToString("yyyyMMdd")),
                                            resultCode: ref result);

                    if (result == 0)
                    {
                        lblPSDIndividualUploadStatus.Text = "Success - Parameter Updated";
                    }
                    else
                    {
                        lblPSDIndividualUploadStatus.Text = "Error";
                        lblPSDIndividualUploadStatus.ForeColor = System.Drawing.Color.Red;
                    }


                }

                PSDLogDataBind();
            }
            catch (Exception ex)
            {
                lblPSDIndividualUploadStatus.Text = "Error - " + ex.Message;
                lblPSDIndividualUploadStatus.ForeColor = System.Drawing.Color.Red;
                return;
            }
        }

        protected void rptPSDLog_PreRender(object sender, EventArgs e)
        {
            PSDLogDataBind();
        }

        protected void PSDLogDataBind()
        {
            using (var dc = new Mars.App.Classes.DAL.FleetDemand.DataAccess())
            {

                rptPSDLog.DataSource = dc.GetPriceablePercentLog(countryKey: null
                    , cmsPoolKey: null
                    , locationGroupKey: null
                    , locationKey: lcPSD.LocationKey
                    , carClassKey: lcPSD.CarClassKey
                    , reportDateKey: null
                    );
                rptPSDLog.DataBind();
            }
        }

        // rpd

        protected void btnRPDUpload_Click(object sender, EventArgs e)
        {
            SelectedTab = 2;
            try
            {
                if (fuRPDFileUpload.HasFile)
                {
                    string filePath = Mars.Properties.Settings.Default.ParameterFilesUploadFolder;
                    string fileName = DateTime.Now.ToString("yyyyMMdd HHmmss") + "RPD -" + fuRPDFileUpload.FileName;

                    if (fileName.Substring(fileName.Length - 4, 4).ToUpper() != ".CSV")
                    {
                        throw new IOException("Please select csv file.");
                    }                    

                    string fullFilePath = Path.Combine(filePath, fileName);
                    using (FileStream sw = new FileStream(fullFilePath, FileMode.Create))
                    {
                        fuRPDFileUpload.PostedFile.InputStream.CopyTo(sw);
                    }

                    byte SSISpackageID = Mars.Properties.Settings.Default.AvgRpdSSIS;
                    long? executionId = null;
                    ExecuteSSISPackage(fullFilePath, SSISpackageID, ref executionId);

                    lblRPDUploadStatus.Text = "The file has been successfully uploaded. The import job number is " + executionId.ToString() + " and the job started at " + DateTime.Now.ToString() + "<br />" +
                        "\nThe Logistics Dashboard is processed on a daily basis each morning. The newly uploaded data will be used in tomorrow\'s calculations. " + "<br />" +
                        "\nThe changes you have made now will not be visible till the Dashboard is processed tomorrow.";

                }
            }
            catch (IOException ex)
            {
                lblRPDUploadStatus.Text = "Error " + DateTime.Now.ToString() + " - " + ex.Message;
                lblRPDUploadStatus.ForeColor = System.Drawing.Color.Red;
                return;
            }
        }

        protected void btnRPDSave_Click(object sender, EventArgs e)
        {
            SelectedTab = 2;

            int selectedLocationKey = lcRPD.LocationKey;
            int selectedCarClassKey = lcRPD.CarClassKey;

            if (selectedLocationKey == 0)
            {
                lblRPDIndividualUploadStatus.Text = "Error - please select Location";
                lblRPDIndividualUploadStatus.ForeColor = System.Drawing.Color.Red;
                return;
            }

            if (selectedCarClassKey == 0)
            {
                lblRPDIndividualUploadStatus.Text = "Error - please select Car Class";
                lblRPDIndividualUploadStatus.ForeColor = System.Drawing.Color.Red;
                return;
            }


            try
            {
                string monthYear = RPDDate.Text;
                DateTime dateTime = DateTime.ParseExact(monthYear, "MMMM yyyy", System.Globalization.CultureInfo.InvariantCulture);

                decimal parameterValue = Decimal.Parse(tbRPDParameter.Text);

                int? result = 0;

                using (FleetDemandDataContext dc = new FleetDemandDataContext())
                {
                    dc.AvgRpdInsert(validFrom: DateTime.Today,
                                            validFromDateKey: Int32.Parse(DateTime.Today.ToString("yyyyMMdd")),
                                            countryKey: lcRPD.CountryKey,
                                            locationKey: selectedLocationKey,
                                            reportDateKey: Int32.Parse(dateTime.ToString("yyyyMMdd")),
                                            carClassKey: selectedCarClassKey,
                                            avgRpd: parameterValue,
                                            addedBy: Rad.Security.ApplicationAuthentication.GetGlobalId(),
                                            uploadedDate: DateTime.Today,
                                            uploadedDateKey: Int32.Parse(DateTime.Today.ToString("yyyyMMdd")),
                                            resultCode: ref result);

                    if (result == 0)
                    {
                        lblRPDIndividualUploadStatus.Text = "Success - Parameter Updated";
                    }
                    else
                    {
                        lblRPDIndividualUploadStatus.Text = "Error"; 
                        lblRPDIndividualUploadStatus.ForeColor = System.Drawing.Color.Red;
                    }


                }

                RPDLogDataBind();
            }
            catch (Exception ex)
            {
                lblRPDIndividualUploadStatus.Text = "Error - " + ex.Message;
                lblRPDIndividualUploadStatus.ForeColor = System.Drawing.Color.Red;
                return;
            }
        }

        protected void rptRPDLog_PreRender(object sender, EventArgs e)
        {
            RPDLogDataBind();
        }

        protected void RPDLogDataBind()
        {
            using (var dc = new Mars.App.Classes.DAL.FleetDemand.DataAccess())
            {                

                rptRPDLog.DataSource = dc.GetAvgRpdLog(countryKey: null
                    , cmsPoolKey: null
                    , locationGroupKey: null
                    , locationKey: lcRPD.LocationKey
                    , carClassKey: lcRPD.CarClassKey
                    , reportDateKey: null  //reportDateKey
                    );
                rptRPDLog.DataBind();
            }
        }

        // One Way Rentals

        protected void btnOWRUpload_Click(object sender, EventArgs e)
        {
            SelectedTab = 3;
            try
            {
                if (fuOWRFileUpload.HasFile)
                {
                    string filePath = Mars.Properties.Settings.Default.ParameterFilesUploadFolder;
                    string fileName = DateTime.Now.ToString("yyyyMMdd HHmmss") + "OWR -" + fuOWRFileUpload.FileName;

                    if (fileName.Substring(fileName.Length - 4, 4).ToUpper() != ".CSV")
                    {
                        throw new IOException("Please select csv file.");
                    }
                    
                    string fullFilePath = Path.Combine(filePath, fileName);
                    using (FileStream sw = new FileStream(fullFilePath, FileMode.Create))
                    {
                        fuOWRFileUpload.PostedFile.InputStream.CopyTo(sw);
                    }

                    byte SSISpackageID = Mars.Properties.Settings.Default.OneWayRentalsSSIS;
                    long? executionId = null;
                    ExecuteSSISPackage(fullFilePath, SSISpackageID, ref executionId);

                    lblOWRUploadStatus.Text = "The file has been successfully uploaded. The import job number is " + executionId.ToString() + " and the job started at " + DateTime.Now.ToString() + "<br />" +
                        "\nThe Logistics Dashboard is processed on a daily basis each morning. The newly uploaded data will be used in tomorrow\'s calculations. " + "<br />" +
                        "\nThe changes you have made now will not be visible till the Dashboard is processed tomorrow.";

                }
            }
            catch (IOException ex)
            {
                lblOWRUploadStatus.Text = "Error " + DateTime.Now.ToString() + " - " + ex.Message;
                lblOWRUploadStatus.ForeColor = System.Drawing.Color.Red;
                return;
            }
        }

        protected void btnOWRSave_Click(object sender, EventArgs e)
        {
            SelectedTab = 3;

            int selectedLocationKey = lcOWR.LocationKey;
            if (selectedLocationKey == 0)
            {
                lblOWRIndividualUploadStatus.Text = "Error - please select Location";
                lblOWRIndividualUploadStatus.ForeColor = System.Drawing.Color.Red;
                return;
            }

            try
            {
                byte selectedDayOfWeekKey = Byte.Parse(ddlOWRDayOfWeek.SelectedValue);
                if (selectedDayOfWeekKey == 0)
                {
                    lblOWRIndividualUploadStatus.Text = "Error - please select Day Of Week";
                    lblOWRIndividualUploadStatus.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                decimal parameterValue = Decimal.Parse(tbOWRParameter.Text);

                int? result = 0;

                using (FleetDemandDataContext dc = new FleetDemandDataContext())
                {
                    dc.OneWayInsert(validFrom: DateTime.Today,
                                            validFromDateKey: Int32.Parse(DateTime.Today.ToString("yyyyMMdd")),
                                            countryKey: lcOWR.CountryKey,
                                            locationKey: selectedLocationKey,
                                            dayKey: selectedDayOfWeekKey,
                                            oneWay: parameterValue,
                                            addedBy: Rad.Security.ApplicationAuthentication.GetGlobalId(),
                                            uploadedDate: DateTime.Today,
                                            uploadedDateKey: Int32.Parse(DateTime.Today.ToString("yyyyMMdd")),
                                            resultCode: ref result);

                    if (result == 0)
                    {
                        lblOWRIndividualUploadStatus.Text = "Success - Parameter Updated";
                    }
                    else
                    {
                        lblOWRIndividualUploadStatus.Text = "Error";
                        lblOWRIndividualUploadStatus.ForeColor = System.Drawing.Color.Red;
                    }

                }

                OWRLogDataBind();
            }
            catch (Exception ex)
            {
                lblOWRIndividualUploadStatus.Text = "Error - " + ex.Message;
                lblOWRIndividualUploadStatus.ForeColor = System.Drawing.Color.Red;
                return;
            }
        }

        protected void rptOWRLog_PreRender(object sender, EventArgs e)
        {
            OWRLogDataBind();
        }

        protected void OWRLogDataBind()
        {
            using (var dc = new Mars.App.Classes.DAL.FleetDemand.DataAccess())
            {
                rptOWRLog.DataSource = dc.GetOneWayLog(countryKey: null
                    , cmsPoolKey: null
                    , locationGroupKey: null
                    , locationKey: lcOWR.LocationKey
                    , dayKey: null
                    );
                rptOWRLog.DataBind();
            }

        }

    }
}