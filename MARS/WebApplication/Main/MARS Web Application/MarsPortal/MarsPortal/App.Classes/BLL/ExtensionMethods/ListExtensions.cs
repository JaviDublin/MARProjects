using System;
using System.Collections.Generic;

namespace App.BLL.ExtensionMethods
{
    internal static class ListExtensions
    {
        internal static void AddOrRemoveString(this List<string> list, string inString)
        {
            if(list.Contains(inString))
            {
                list.Remove(inString);
            }
            else
            {
                list.Add(inString);
            }

        }

        internal static void AddPreviousIfZero(this List<double> list, double val)
        {
            if(list.Count > 0 && val == 0)
            {
                list.Add(list[list.Count - 1]);
                return;
            }
            list.Add(val);
        }
    }
}