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

using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Rad.Common;

namespace App.BLL
{
    /// <summary>
    /// Base class for all master pages
    /// </summary>
    public class MasterBase : MasterPage
    {

        #region Declarations

        private HtmlMeta _compatibleMeta;
        private HtmlMeta _contentMeta;
        private HtmlLink _faviconLink;
        private HtmlLink _faviconType;
        private HtmlLink _siteStyleSheet;

        #endregion

        #region Page Methods

        /// <summary>
        /// Add title and meta tags / favicon to head section of page
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(System.EventArgs e)
        {
            //Set Application Title
            this.Page.Title = Mars.Properties.Settings.Default.ApplicationName;

      
            //Add Favicon to Head Section
            this._faviconLink = new HtmlLink();
            this._faviconLink.Attributes.Add("rel", "shortcut icon");
            this._faviconLink.Attributes.Add("href", "~/App.Images/favicon.ico");
            this._faviconLink.Attributes.Add("type", "image/x-icon");

            this._faviconType = new HtmlLink();
            this._faviconType.Attributes.Add("rel", "icon");
            this._faviconType.Attributes.Add("href", "~/App.Images/favicon.ico");
            this._faviconType.Attributes.Add("type", "image/x-icon");

            this.Page.Header.Controls.Add(this._faviconLink);
            this.Page.Header.Controls.Add(this._faviconType);


            base.OnInit(e);
        }


        //Regex values to strip white space
        private static readonly Regex RegexBetweenTags = new Regex(@">(?! )\s+", RegexOptions.Compiled);
        private static readonly Regex RegexLineBreaks = new Regex(@"([\n\s])+?(?<= {2,})<", RegexOptions.Compiled);


        /// <summary>
        /// Strip unwanted white spaces etc from html
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {

            //Removes all white space from html output
            using (HtmlTextWriter htmlwriter = new HtmlTextWriter(new System.IO.StringWriter()))
            {

                base.Render(htmlwriter);
                string html = htmlwriter.InnerWriter.ToString();

                //Match results with Regex values
                html = RegexBetweenTags.Replace(html, ">");
                html = RegexLineBreaks.Replace(html, "<");

                //Output the Html
                writer.Write(html);

            }

        }

        #endregion

        #region Methods


        /// <summary>
        /// Add style sheets required to page
        /// </summary>
        /// <param name="styleSheet">name of style sheet</param>
        /// <param name="key">a unique key for the style sheet eg: clientId of page</param>
        /// <param name="useMinified">Specifiy whether or not to use the minified version in release mode</param>
        protected void AddStyleSheet(string styleSheet, string key, bool useMinified)
        {
            //Declare a variable to hold style sheet url
            string styleSheetUrl = @"~/App.Styles/";

            //Create a new html link
            this._siteStyleSheet = new HtmlLink();

            //Depending on whether or not we are in debug mode
            //Create the url of the html link

#if DEBUG
            styleSheetUrl += styleSheet;
#else

            if (useMinified)
            {
                styleSheet = styleSheet.Substring(0, styleSheet.Length - 4);
                styleSheet += @".min.css";
            }

            styleSheetUrl += styleSheet;


#endif

            //Check to see if the style sheet is already added to the page
            if (!Page.ClientScript.IsClientScriptBlockRegistered(key))
            {
                //Create the link
                styleSheetUrl = Page.ResolveUrl(styleSheetUrl);
                this._siteStyleSheet.Attributes.Add("href", styleSheetUrl);
                this._siteStyleSheet.Attributes.Add("rel", "stylesheet");
                this._siteStyleSheet.Attributes.Add("type", "text/css");

                //Add the style sheet to the page
                this.Page.Header.Controls.Add(this._siteStyleSheet);

                //Register to say style sheet is loaded
                this.Page.ClientScript.RegisterClientScriptBlock(typeof(System.Web.UI.Page), key, "");
            }
        }

        /// <summary>
        /// Load client scripts that are required for the application
        /// </summary>
        /// <param name="script">name of file</param>
        /// <param name="key">unique key for script</param>
        /// <param name="useMinified">use minified when in release mode</param>
        protected void IncludeScripts(string script, string key, bool useMinified)
        {
            //Define a variable for the script url
            string scriptUrl = @"~/App.Scripts/";

            //Check whether or not we are in debug / release

#if DEBUG
            scriptUrl += script;

#else
            if (useMinified)
            {
                script = script.Substring(0, script.Length - 3);
                script += @".min.js";
            }

            scriptUrl += script;
#endif

            //Get the resolved url for the script file
            scriptUrl = Page.ResolveUrl(scriptUrl);

            //Register the included scripts
            Scripts.RegisterIncludeClientScriptPage(this.Page, key, (scriptUrl));
        }

        /// <summary>
        /// Check the session timeout
        /// </summary>
        protected void CheckSessionTimeout()
        {

            //Time to refresh page
            int milliSecondsTimeOut = (this.Session.Timeout * 60000) - 5;

            StringBuilder timeoutScript = new StringBuilder();
            timeoutScript.Append("$(document).ready(function() {");
            //Parameters expected
            //milliSecondsTimeOut
            timeoutScript.Append("CheckSessionTimeout('" + milliSecondsTimeOut + "')");
            timeoutScript.Append("});");

            //Register the scripts
            //Register the script
            Scripts.RegisterStartupClientScript(Page, timeoutScript.ToString(), "CheckSessionTimeout" + this.ClientID);

        }

        #endregion


    }
}