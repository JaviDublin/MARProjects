using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mars.App.Classes.BLL.ExtensionMethods
{
    public static class UserControlExtentions
    {
        internal static void SortGrid<T>(this UserControl uc, SortDirection direction, PropertyInfo sortByColumnName
            , List<T> gridData)
        {
            var isInt = sortByColumnName.PropertyType == typeof(int);
            var isDateTime = sortByColumnName.PropertyType == typeof(DateTime?);

            if (direction == SortDirection.Ascending)
            {
                gridData.Sort((x, y) => isDateTime ? (int)(((DateTime)(sortByColumnName.GetValue(x, null) ?? DateTime.MinValue)).Date
                                                - ((DateTime)(sortByColumnName.GetValue(y, null) ?? DateTime.MinValue)).Date
                                        ).TotalDays
                                    : isInt ? (int)sortByColumnName.GetValue(x, null) - (int)sortByColumnName.GetValue(y, null)
                                    : string.CompareOrdinal(sortByColumnName.GetValue(x, null).ToString().ToLower()
                                    , sortByColumnName.GetValue(y, null).ToString().ToLower().ToLower())
                            );
            }
            else
            {
                gridData.Sort(
                    (x, y) =>
                        isDateTime
                            ? (int)
                                (((DateTime)(sortByColumnName.GetValue(y, null) ?? DateTime.MinValue)).Date -
                                 ((DateTime)(sortByColumnName.GetValue(x, null) ?? DateTime.MinValue)).Date).TotalDays
                            : isInt
                                ? (int)sortByColumnName.GetValue(y, null) - (int)sortByColumnName.GetValue(x, null)
                                : string.CompareOrdinal(sortByColumnName.GetValue(y, null).ToString().ToLower()
                                    , sortByColumnName.GetValue(x, null).ToString().ToLower()));

            }
        }

    }
}