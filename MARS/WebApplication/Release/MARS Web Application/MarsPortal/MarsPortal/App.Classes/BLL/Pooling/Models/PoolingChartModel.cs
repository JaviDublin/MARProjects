using System.Web.UI.DataVisualization.Charting;
using App.Classes.Entities.Pooling.Abstract;
using App.Classes.DAL.Reservations.Abstract;
using Mars.Pooling.Models.Abstract;
using App.Classes.DAL.Pooling.Abstract;
using System.Data;
using System;
using System.Drawing;

namespace Mars.BLL.Pooling.Models
{
    public class PoolingChartModel : IPoolingChartModel
    {

        public IReservationDayActualsRepository Repository { get; set; }
        public Chart _Chart { get; set; }
        public IMainFilterEntity Filter { get; set; }
        public DataTable _DataTable { get; set; }
        public int NoOfPoints { get; set; }
        bool[] check;
        System.Drawing.Color[] colors = { Color.Cyan, Color.Purple, Color.Green, Color.DarkOrange, Color.Black, Color.DeepPink, Color.DarkBlue, Color.RosyBrown, Color.Violet, Color.Gray, Color.LawnGreen, Color.Teal , Color.Tomato};
        string CHECKEDIMG = @"..\..\App.Images\checked.jpg", UNCHECKEDIMG = @"..\..\App.Images\unchecked.jpg";

        public PoolingChartModel()
        {
            check = new bool[13];
            check[0] = true;
            for (int i = 1; i < check.Length; i++) check[i] = false;
        }
        public void bind()
        {
            _Chart.BorderSkin.SkinStyle = System.Web.UI.DataVisualization.Charting.BorderSkinStyle.Emboss;
            _Chart.BorderSkin.PageColor = Color.FromArgb(255, 252, 197);
            Legend l = new Legend("Default");
            
            foreach (Enums.ActualsRows i in Enum.GetValues(typeof(Enums.ActualsRows)))
            {
                _Chart.Series.Add(i.ToString());
                if (i == Enums.ActualsRows.Balance) _Chart.Series[i.ToString()].ChartType = SeriesChartType.Column;
                else _Chart.Series[i.ToString()].ChartType = SeriesChartType.Spline;
                _Chart.Series[i.ToString()].Color = colors[(int)i];
                _Chart.Series[i.ToString()].IsVisibleInLegend = false;
                LegendItem li = new LegendItem();
                li.Cells.Add(getLegCell(i.ToString(), colors[(int)i], "c1"));
                li.Cells.Add(getLegCell(i.ToString(), colors[(int)i], "c2", check[(int)i] ? CHECKEDIMG : UNCHECKEDIMG, "check:" + i.ToString(), LegendCellType.Image, "Click to Show/Hide"));
                l.CustomItems.Add(li);
            }
            _Chart.Legends.Add(l);
            _Chart.Width = 1000;
            _Chart.Height = 500;
            _Chart.ChartAreas[0].Position.Y = 4;
            _Chart.ChartAreas[0].Position.Width = 80;
            _Chart.ChartAreas[0].Position.Height = 90;
            _Chart.ChartAreas[0].AxisX.Interval = 1;
            _Chart.ChartAreas[0].AxisX.TextOrientation = TextOrientation.Rotated90;
            _Chart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            DateTime dt = DateTime.Now;
            for (int i = 0; i < _Chart.Series.Count; i++)
            {
                Object[] a = _DataTable.Rows[i].ItemArray;
                for (int x = 0; x < NoOfPoints; x++)
                {
                    if (!String.IsNullOrEmpty(a[x] as String))
                        if (check[(int)i])
                        {
                            String sDate = NoOfPoints == 72 ? dt.AddHours(x).ToString("dd-MM hh:mm:ss") : dt.AddDays(x).ToString("dd-MM hh:mm:ss");
                            _Chart.Series[i].Points.AddXY(sDate, Convert.ToInt32(a[x]));
                            _Chart.Series[i].Points[x].ToolTip = (a[x] as string) + " at " + sDate;
                        }
                }
            }
            _Chart.DataBind();
        }
        public void ChartClick(object o, System.Web.UI.WebControls.ImageMapEventArgs e)
        {
            if (e.PostBackValue.Contains("check"))
            {
                String s = e.PostBackValue.Replace("check:", "");
                for (Enums.ActualsRows i = Enums.ActualsRows.Balance; i <= Enums.ActualsRows.Local; i++)
                {
                    if (s == i.ToString()) check[(int)i] = !check[(int)i];
                }
            }
        }
        LegendCell getLegCell(string text, Color col, string cellName, string image, string postback, LegendCellType cellType, string tooltip)
        {
            return new LegendCell
            {
                ForeColor = col,
                Text = text,
                Image = image,
                PostBackValue = postback,
                CellType = cellType,
                Margins = { Left = 5, Right = 5 },
                ToolTip = tooltip
            };
        }
        LegendCell getLegCell(string text, Color col, string cellName)
        {
            return getLegCell(text, col, cellName, "", "", LegendCellType.Text, "");
        }
    }
}