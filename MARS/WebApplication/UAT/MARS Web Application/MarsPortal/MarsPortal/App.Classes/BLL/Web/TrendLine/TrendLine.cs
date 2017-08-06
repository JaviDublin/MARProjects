using System;
using System.Collections.Generic;
using System.Linq;

namespace App.BLL.TrendLine
{
    internal class Trendline
    {
        private readonly IList<double> _xAxisValues;
        private readonly IList<double> _yAxisValues;
        private int _count;
        private double _xAxisValuesSum;
        private double _xxSum;
        private double _xySum;
        private double _yAxisValuesSum;

        internal Trendline(IList<double> yAxisValues, IList<double> xAxisValues)
        {
            _yAxisValues = yAxisValues;
            _xAxisValues = xAxisValues;

            Initialize();
        }

        internal double Slope { get; private set; }
        internal double Intercept { get; private set; }
        internal double Start { get; private set; }
        internal double End { get; private set; }

        private void Initialize()
        {
            _count = _yAxisValues.Count;
            _yAxisValuesSum = _yAxisValues.Sum();
            _xAxisValuesSum = _xAxisValues.Sum();
            _xxSum = 0;
            _xySum = 0;

            for (int i = 0; i < _count; i++)
            {
                _xySum += (_xAxisValues[i] * _yAxisValues[i]);
                _xxSum += (_xAxisValues[i] * _xAxisValues[i]);
            }

            Slope = CalculateSlope();
            Intercept = CalculateIntercept();
            Start = CalculateStart();
            End = CalculateEnd();
        }

        internal List<double> GetTrendLineYPoints()
        {
            return _xAxisValues.Select(x => (Slope * x) + Intercept).ToList();
        }

        private double CalculateSlope()
        {
            try
            {
                return ((_count * _xySum) - (_xAxisValuesSum * _yAxisValuesSum)) / ((_count * _xxSum) - (_xAxisValuesSum * _xAxisValuesSum));
            }
            catch (DivideByZeroException)
            {
                return 0;
            }
        }

        private double CalculateIntercept()
        {
            return (_yAxisValuesSum - (Slope * _xAxisValuesSum)) / _count;
        }

        private double CalculateStart()
        {
            return (Slope * _xAxisValues.First()) + Intercept;
        }

        private double CalculateEnd()
        {
            return (Slope * _xAxisValues.Last()) + Intercept;
        }


    }
}