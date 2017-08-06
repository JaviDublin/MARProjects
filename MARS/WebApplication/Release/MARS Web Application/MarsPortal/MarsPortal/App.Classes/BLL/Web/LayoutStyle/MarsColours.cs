using System.Drawing;

/* As anything inside the chartcontol is rendered to a JPEG fonts can not be set via Style Sheets */

namespace App.Styles
{
    internal static class MarsColours
    {
        internal static readonly Color MarsYellow = Color.FromArgb(255, 249, 133);

        internal static readonly Color ChartLegendValuesShown = Color.Black;
        internal static readonly Color ChartLegendValuesHidden = Color.Gray;
        internal static readonly Color ChartLineBreakColour = Color.Black;
        internal static readonly Color ChartNoDataMessageColour = Color.Red;
        internal static readonly Color ChartHighlightWeekendColour = Color.LightCyan;

    }
}