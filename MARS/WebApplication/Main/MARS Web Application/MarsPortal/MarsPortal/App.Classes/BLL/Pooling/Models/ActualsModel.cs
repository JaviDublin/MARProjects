using System;
using System.Web.UI.HtmlControls;
using App.Classes.Entities.Pooling.Abstract;
using App.Classes.DAL.Reservations.Abstract;
using System.Web.UI.WebControls;
using App.BLL.VehiclesAbroad.Models.Abstract;
using App.Classes.DAL.Pooling.Abstract;
using Mars.Pooling.Models.Abstract;
using Mars.Pooling.HTMLFactories.Abstract;
using Mars.Pooling.HTMLFactories;
using App.Classes.DAL.Reservations;
using Mars.DAL.Pooling;

namespace Mars.Pooling.Models
{

    public class ActualsModel
    {

        public ActualHtmlTable _htmlTable;
        IHtmlFactory _factory = new HtmlFactory(Enums.HtmlTable.Actuals);
        static string JSNAME = "ActualsModelScript";
        public HtmlGenericControl _HtmlControl { get; set; }
        public IMainFilterEntity MainFilters { get; set; }
        public IFilterModel ColumnMaster { get; set; }
        public HiddenField GridChartHidden { get; set; }
        Enums.Headers _mode;
        DayActualRepository _repositoryThree, _repositoryThirty;
        public Enums.Headers Mode
        {
            get { return _mode; }
            set
            {
                _mode = value;
                if (value == Enums.Headers.threeDayActualStatus) _htmlTable.Repository = _repositoryThree;
                else _htmlTable.Repository = _repositoryThirty;
            }
        }
        public ActualsModel()
        {
            _htmlTable = _factory.HtmlTable as ActualHtmlTable;
            _repositoryThree = new DayActualRepository(new ReservationActualsJavascriptRepository());
            _repositoryThirty = new DayActualRepository(new ReservationActualsJavascriptRepository(), Enums.DayActualTime.THIRTY);
        }
        public void bind(params string[] dependants)
        {
            int cond = 0;
            int.TryParse(GridChartHidden.Value, out cond);
            _htmlTable.Condition = cond;
            _htmlTable.Mode = Mode;
            Int32 cols = 0;
            if (!Int32.TryParse(dependants[1], out cols)) cols = 72;
            _htmlTable.NumberColumns = cols;
            int width = 0;
            if (!int.TryParse(dependants[0], out width)) width = 1000;
            width = (int)(width * 0.80);
            _htmlTable.Width = width.ToString();
            _htmlTable.Filter = MainFilters;
            _HtmlControl.InnerHtml = _htmlTable.GetTable();
        }



        public void findClicked()
        {
            throw new NotImplementedException();
        }
        public void setJavascript(System.Web.UI.Page p)
        {
            p.ClientScript.RegisterStartupScript(this.GetType(), JSNAME, _htmlTable.Repository.GetJavascript());
        }
    }
}