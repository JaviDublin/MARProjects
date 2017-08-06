using System;
using System.Web.UI.WebControls;

namespace App.BLL.ExtensionMethods
{
    internal static class GridViewExtensions
    {
        public static int FindColumnIndex(this GridView gridView, string accessibleHeaderText)
        {
            for (var index = 0; index < gridView.Columns.Count; index++)
            {
                if (String.Compare(gridView.Columns[index].AccessibleHeaderText, accessibleHeaderText, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return index;
                }
            }
            return -1;
        }
    }
}