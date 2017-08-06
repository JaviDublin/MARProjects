using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.DAL.MarsDataAccess;
using Mars.App.Classes.Phase4Bll.Administration;
using Mars.App.Classes.Phase4Dal.Administration.Users;
using Mars.App.Classes.Phase4Dal.Administration.UserTraining;
using Rad.Security;

namespace Mars.App.UserControls.Panel
{
    public partial class CommunicationsPanel : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var comData = CommunicationDataAccess.GetAllActiveNewsItems();

                rptComm.DataSource = comData;
                rptComm.DataBind();

                var documents = CommunicationDataAccess.GetAllDocuments().ToList();
                rptDocs.DataSource = documents;
                rptDocs.DataBind();

                using (var dataAccess = new UsersAndRolesDataAccess())
                {
                     var employeeId = ApplicationAuthentication.GetEmployeeId();
                    var userType = dataAccess.GetUserType(employeeId);
                    var countryAdmins = dataAccess.GetCountryAdmin(userType);
                    gvCountryAdmins.DataSource = countryAdmins.OrderBy(d=> d.Country).ThenBy(d => d.Name);
                }
                using (var dataAccess = new UserTrainingDataAccess())
                {
                    var trainingEntities = dataAccess.GetActiveEntries();
                    rptTraining.DataSource = trainingEntities;
                    
                }
                gvCountryAdmins.DataBind();
                rptTraining.DataBind();
            }
        }

        protected void rptDocs_Command(object sendere, CommandEventArgs e)
        {
            if (e.CommandName == "OpenFile")
            {
                var fileUrl = e.CommandArgument.ToString();

                var fileType = Path.GetExtension(fileUrl).Replace(".", string.Empty);
                Response.Clear();
                Response.ContentType = string.Format("application/{0}", fileType);
                Response.AddHeader("Content-Type", string.Format("application/{0}", fileType));
                Response.AddHeader("Content-Disposition", string.Format("inline;filename={0}", Path.GetFileName(fileUrl)));

                
                var ffs = File.OpenRead(fileUrl);
                var buf = new byte[ffs.Length];
                ffs.Read(buf, 0, buf.Length);
                Response.BinaryWrite(buf);

                Response.End();
            }
        }
    }
}