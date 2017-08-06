using System.Collections.Generic;


namespace App.BLL.TrendLine
{
    public static class TrendLineCalculator
    {
        internal static Trendline CalculateLinearRegression(double[] values)
        {
            var yAxisValues = new List<double>();
            var xAxisValues = new List<double>();

            for (var i = 0; i < values.Length; i++)
            {
                yAxisValues.Add(values[i]);
                xAxisValues.Add(i + 1);
            }

            return new Trendline(yAxisValues, xAxisValues);
        }
    }
}