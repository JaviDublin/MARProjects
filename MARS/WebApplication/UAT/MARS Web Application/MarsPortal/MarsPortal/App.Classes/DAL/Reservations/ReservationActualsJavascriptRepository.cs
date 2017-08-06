using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Classes.DAL.Reservations.Abstract;
using System.Text;

namespace App.Classes.DAL.Reservations {
    public class ReservationActualsJavascriptRepository : IJavaScriptRepository {
        public string getJavascript(params string[] s) {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<script type='text/javascript'> ");
            sb.AppendLine("function collapseId(id, val) { ");
            sb.AppendLine("var docId = document.getElementById(id); var x = parseInt(document.getElementById('chartviewHidden').value); ");
            sb.AppendLine("if (docId.style.display == 'none')  { docId.style.display = ''; if (val > 0) x |= val; } else  { docId.style.display = 'none'; if (val > 0) x ^= val; }; ");
            sb.AppendLine("document.getElementById('chartviewHidden').value = x; ");
            sb.AppendLine("}; ");

            sb.AppendLine("</script>");
            return sb.ToString();
        }
    }
}