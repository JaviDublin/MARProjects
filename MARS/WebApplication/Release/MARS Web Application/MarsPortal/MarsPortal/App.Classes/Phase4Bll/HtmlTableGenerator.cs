using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace Mars.App.Classes.Phase4Bll
{
    public static class HtmlTableGenerator
    {
        public static string GenerateHtmlTableFromGridview(GridView gv)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<html>");
            sb.AppendLine("<body>");
            sb.Append("<table>");


            sb.Append("<tr>");
            foreach (TableCell c in gv.HeaderRow.Cells)
            {
                sb.Append("<td>" + c.Text + "</td>");
            }
            sb.Append("</tr>");
            foreach (GridViewRow row in gv.Rows)
            {
                sb.Append("<tr>");
                for (int j = 0; j < gv.Columns.Count; j++)
                {
                    sb.Append("<td>" + row.Cells[j].Text + "</td>");
                }
                sb.Append("</tr>");
            }
            sb.Append("</table>");
            sb.AppendLine("</html>");
            sb.AppendLine("</body>");
            return sb.ToString();
        }
        public static string GenerateCsvFromGridview(GridView gv, bool includeHeader = true)
        {
            var sb = new StringBuilder();

            if (includeHeader)
            {
                foreach (TableCell c in gv.HeaderRow.Cells)
                {
                    if (c.Visible)
                    {
                        sb.Append(c.Text + ",");
                    }
                }
                sb.AppendLine();    
            }
            
            foreach (GridViewRow row in gv.Rows)
            {

                for (int j = 0; j < gv.HeaderRow.Cells.Count; j++)
                {
                    sb.Append("\"" + row.Cells[j].Text + "\",");
                }
                sb.AppendLine();
            }
            sb.AppendLine();
            return sb.ToString();
        }

    }
}