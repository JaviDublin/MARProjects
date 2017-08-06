using System;

namespace App.BLL.ExtensionMethods
{
    internal static class DecimalExtensions
    {
        public static decimal Round(this decimal d)
        {
            return Math.Round(d, 0, MidpointRounding.AwayFromZero);
        }


    }
}