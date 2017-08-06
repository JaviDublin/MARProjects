using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI.WebControls;

namespace App.BLL.Utilities
{
    public class GenericSorter<T> : IComparer<T>
    {
        public GenericSorter(string sortExpression, SortDirection sortDirection)
        {
            this.SortExpression = sortExpression;
            this.SortDirection = sortDirection;
        }

        public SortDirection SortDirection
        {
            get;
            set;
        }

        public string SortExpression
        {
            get;
            set;
        }

        private static object GetPropertyValue(object o, string property)
        {
            PropertyInfo pi = o.GetType().GetProperty(property);
            object val = pi.GetValue(o, null);
            return val;
        }

        public int Compare(T x, T y)
        {
            string[] fieldParts = SortExpression.Split('.');

            object compareValX = x;
            object compareValY = y;

            //get the value of each field in the list.
            foreach (string field in fieldParts)
            {
                compareValX = GetPropertyValue(compareValX, field);
                compareValY = GetPropertyValue(compareValY, field);
            }

            compareValX = compareValX ?? "";
            compareValY = compareValY ?? "";

            if (SortDirection == SortDirection.Ascending)
                return ((IComparable)compareValX).CompareTo(compareValY);
            return ((IComparable)compareValY).CompareTo(compareValX);
        }
    }
}