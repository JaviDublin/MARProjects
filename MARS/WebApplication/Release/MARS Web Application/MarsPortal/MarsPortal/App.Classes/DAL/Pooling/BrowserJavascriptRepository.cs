using System.Text;
using Mars.DAL.Pooling.Abstract;

namespace Mars.DAL.Pooling {
    public class BrowserJavascriptRepository : IBrowserJavascriptRepository {

        // s[0] is width & s[1] is height element names
        public string getJavascript(params string[] s) {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<script type='text/javascript'>");
            sb.AppendLine("var winW = 630, winH = 460;");
            sb.AppendLine("if (document.body && document.body.offsetWidth) {");
            sb.AppendLine("winW = document.body.offsetWidth;");
            sb.AppendLine("winH = document.body.offsetHeight;");
            sb.AppendLine("}");
            sb.AppendLine("if (document.compatMode=='CSS1Compat' && document.documentElement && document.documentElement.offsetWidth ) {");
            sb.AppendLine("winW = document.documentElement.offsetWidth;");
            sb.AppendLine("winH = document.documentElement.offsetHeight;");
            sb.AppendLine("}");
            sb.AppendLine("if (window.innerWidth && window.innerHeight) {");
            sb.AppendLine("winW = window.innerWidth;");
            sb.AppendLine("winH = window.innerHeight;");
            sb.AppendLine("}");
            sb.AppendLine("document.getElementById('" + s[0] + "').value = winW;");
            sb.AppendLine("document.getElementById('" + s[1] + "').value = winH;");
            sb.AppendLine("document.getElementById('ActualsScroller_Id').style.width = winW*0.80+'px';");
            //sb.AppendLine("alert('test, width='+winW + ', height='+winH);");
            sb.AppendLine("</script>");
            return sb.ToString();
        }
    }
}