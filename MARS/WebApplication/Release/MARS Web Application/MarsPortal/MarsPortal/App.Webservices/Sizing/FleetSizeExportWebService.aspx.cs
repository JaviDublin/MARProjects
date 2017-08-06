using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Mars.DAL.Sizing.Management.Abstract;
using Mars.DAL.Sizing.Management;
using System.Security.Principal;
using System.Web.Security;

namespace Mars.Webservices.Sizing {
    public partial class FleetSizeExportWebService:System.Web.UI.Page {

        public String AddTable { get; set; }
        public String DelTable { get; set; }
        public String InsertCmd { get; set; }
        public String AddCommit { get; set; }
        public String DelCommit { get; set; }
        public String Disabled { get; set; }
        public String PageUser { get; set; }

        protected void Page_Load(object sender,EventArgs e) {
            IFleetSizeExportRepository r=new FleetSizeExportRepository();

            FormsIdentity fi = User.Identity as FormsIdentity;
            PageUser=fi.Ticket.UserData.Split('|')[1];
            Disabled=r.IsUserAdmin(PageUser)?String.Empty:"disabled";
            if(Request.QueryString["saveAll"]=="yes") {
                try {
                    r.SetCmd(ManualMovementCommands.addTable.ToString(),Request.QueryString["AddTable"]);
                    r.SetCmd(ManualMovementCommands.delTable.ToString(),Request.QueryString["DelTable"]);
                    r.SetCmd(ManualMovementCommands.insertCmd.ToString(),Request.QueryString["InsertTable"]);
                    r.SetCmd(ManualMovementCommands.addCommit.ToString(),Request.QueryString["AddCommit"]);
                    r.SetCmd(ManualMovementCommands.delCommit.ToString(),Request.QueryString["DelCommit"]);
                    Response.Write("Save successful.,");
                }
                catch { Response.Write("Save unsuccessful.,"); }
                return; 
            }            
            AddTable=r.GetCmd(ManualMovementCommands.addTable.ToString());
            DelTable=r.GetCmd(ManualMovementCommands.delTable.ToString());
            InsertCmd=r.GetCmd(ManualMovementCommands.insertCmd.ToString());
            AddCommit=r.GetCmd(ManualMovementCommands.addCommit.ToString());
            DelCommit=r.GetCmd(ManualMovementCommands.delCommit.ToString());
        }
    }
}