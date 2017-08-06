//=================================================================
//  File:		
//
//  Namespace:	
//
//	Classes:	
//
//  Purpose:	
//
//===================================================================
// 
//===================================================================

using System;
using App.BLL;

namespace App.MasterPages
{
    public partial class Site : MasterBase
    {

        protected override void OnInit(EventArgs e)
        {

            base.OnInit(e);

            //Add Style Sheets
            base.AddStyleSheet("Site.css", "Site", false);

            //Add Javascript Files             
            base.IncludeScripts("jquery-1.5.1.js", "Jquery", false);

        }

        protected void Page_Load(object sender, EventArgs e)
        {


        }
    }
}