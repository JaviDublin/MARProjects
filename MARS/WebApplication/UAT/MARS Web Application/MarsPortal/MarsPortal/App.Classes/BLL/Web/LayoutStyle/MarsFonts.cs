using System.Drawing;

/* As anything inside the chartcontol is rendered to a JPEG fonts can not be set via Style Sheets */

namespace App.Styles
{
    internal static class MarsFonts
    {
        internal static readonly Font ChartingNoDataMessage = new Font("Tahoma", 16);
        internal static readonly Font ChartingChartNameTitle = new Font("Tahoma", 12);
        internal static readonly Font ChartingShowLegend = new Font("Tahoma", 10);
        internal static readonly Font ChartingLinkingMessage = new Font("Tahoma", 10);
        
    }
}