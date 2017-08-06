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
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Rad.Web;

namespace App.BLL
{
    public class PageBase : Page
    {

        #region Page Methods

        /// <summary>
        /// Overide the Initialize culture event
        /// </summary>
        protected override void InitializeCulture()
        {
            base.InitializeCulture();

            //Check to see if we have a cookie value for selected culture
            if (Cookie.GetCookieString(Cookie.LanguagePreference) != null)
            {
                //Get the value for the culture
                object o = Cookie.GetCookieString(Cookie.LanguagePreference);

                //Convert the value into correct data type
                CultureInfo culture = new CultureInfo(o.ToString());

                //Apply the language setting
                ApplyLanguage(culture, false);

            }


        }

        /// <summary>
        /// Apply language culture and option to refresh page
        /// </summary>
        /// <param name="culture">Culture info en-GB</param>
        /// <param name="refresh">Refresh page or not</param>
        protected void ApplyLanguage(CultureInfo culture, bool refresh)
        {
            //Set the new culture
            Rad.Globalization.LanguageManager.ApplicationCurrentCulture = culture;

            //Add selected culture to cookie value
            if (refresh)
            {
                //Refresh the current page
                Response.Redirect(Request.Url.AbsoluteUri, false);
            }

        }

        #endregion





    }
}