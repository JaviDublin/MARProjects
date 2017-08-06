using System;
using System.Diagnostics;
using Mars.App.Classes.DAL.NonRevReWrite;
using Mars.App.Classes.DAL.NonRevReWrite.Enums;
using Mars.App.Classes.DAL.NonRevReWrite.ParameterHolders;

namespace MarsPortal
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.ControlPanelRibbon.SetControlTitle("Application Home");
        }


    }
}