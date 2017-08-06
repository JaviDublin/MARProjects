using System;
using System.Text;
using System.Web.UI;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using App.DAL.Data;
using Rad.Common;

namespace App.CustomControls
{

    public class TextArea : System.Web.UI.WebControls.TextBox
    {
        /// <summary> 
        /// Override PreRender to include custom javascript 
        /// </summary> 
        /// <param name="e"></param> 
        protected override void OnPreRender(EventArgs e)
        {
            if (MaxLength > 0 && TextMode == System.Web.UI.WebControls.TextBoxMode.MultiLine)
            {
                // Add javascript handlers for paste and keypress 
                Attributes.Add("onkeypress", "doKeypress(this);");
                Attributes.Add("onbeforepaste", "doBeforePaste(this);");
                Attributes.Add("onpaste", "doPaste(this);");

                // Add attribute for access of maxlength property on client-side 
                Attributes.Add("maxLength", this.MaxLength.ToString());

            }
            base.OnPreRender(e);
        }
    }

    public class ExcelGrid : System.Web.UI.WebControls.GridView
    {
        protected override void OnDataBinding(EventArgs e)
        {
            this.Font.Size = System.Web.UI.WebControls.FontUnit.Small;
            this.ForeColor = System.Drawing.ColorTranslator.FromHtml("#333333");
            this.GridLines = GridLines.None;
            this.AlternatingRowStyle.BackColor = System.Drawing.Color.White;
            this.AlternatingRowStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("#284775");
            this.EditRowStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#999999");
            this.FooterStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#5D7B9D");
            this.FooterStyle.Font.Bold = true;
            this.FooterStyle.ForeColor = System.Drawing.Color.White;
            this.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#5D7B9D");
            this.HeaderStyle.Font.Bold = true;
            this.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.PagerStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#284775");
            this.PagerStyle.HorizontalAlign = HorizontalAlign.Center;
            this.PagerStyle.ForeColor = System.Drawing.Color.White;
            this.RowStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#F7F6F3");
            this.RowStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("#333333");
            this.SelectedRowStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#E2DED6");
            this.SelectedRowStyle.Font.Bold = true;
            this.SelectedRowStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("#E2DED6");
            this.SortedAscendingCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#E9E7E2");
            this.SortedAscendingHeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#506C8C");
            this.SortedDescendingCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFDF8");
            this.SortedDescendingHeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#6F8DAE");
            base.OnDataBinding(e);
        }
    }
}