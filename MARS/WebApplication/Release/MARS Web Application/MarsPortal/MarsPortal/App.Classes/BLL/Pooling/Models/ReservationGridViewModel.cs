using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using App.Classes.BLL.Pooling.Models.Abstract;
using App.Entities.VehiclesAbroad;
using System.Web.UI.WebControls;
using App.Classes.DAL.Reservations.Abstract;
using Mars.App.Classes.DAL.MarsDBContext;
using App.Classes.Entities.Pooling.Abstract;
using Mars.App.Classes.DAL.Pooling.Queryables;
using Mars.Entities.Reservations.Abstract;
using System.Web.DynamicData;
using System.Data;
using System.Web.UI;
using Mars.DAL.Pooling.Abstract;
using Mars.Entities.Pooling;
using Microsoft.SqlServer.Dts.Runtime;

namespace App.Classes.BLL.Pooling.Models
{
    public class ReservationGridViewModel : IReservationGridViewModel
    {

        private readonly string S1 = "<a style='color:White;' href=\"javascript:__doPostBack('ctl00$ContentPlaceholderMainContent$GridViewDetails','Sort$",
            S2 = "')\">", S3 = "</a>";
        private GridView _gridview;
        private readonly IReservationRepository _repository;
        private string _sortExpression;
        private IDictionary<string, string> _sortDic;
        private readonly IDoPostBackRepository _postBackRepository;
        public int DataItemsCount { get; private set; }



        private string SortDirection
        {
            get { return _sortExpression == null ? "asc" : _sortDic[_sortExpression]; }
        }
        public GridView GridViewer
        {
            get
            {
                if (_gridview == null) throw new NullReferenceException("No value had been assigned to GridViewer");
                return _gridview;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("The value can't be null"); //guard
                //if(_gridview!=null) throw new InvalidOperationException("You can't reassign this GridViewer"); //once only
                _gridview = value;
            }
        }
        public IMainFilterEntity MainFilters { get; set; }
        public IReservationDetailsFilterEntity ResFilters { get; set; }

        public ReservationGridViewModel(IReservationRepository ir, IDoPostBackRepository postBackRespository)
        {
            if (ir == null) throw new ArgumentNullException("Constructor cannot have a null value passed to it, inject a version of IReservationRepository");
            if (postBackRespository == null) throw new ArgumentNullException("Constructor cannot have a null value passed to it, inject a version of IDoPostBackRepository");
            _repository = ir;
            _postBackRepository = postBackRespository;
            _sortDic = new Dictionary<string, string>();
        }

        public List<ResGridDisplay> GetGridDataToBind()
        {
            var data = (from p in _repository.getList(MainFilters, ResFilters, _sortExpression, SortDirection)
                        select new ResGridDisplay
                        {
                            ResIdNumber = p.RES_ID_NBR,
                            CarType = p.GR_INCL_GOLDUPGR,
                            LocationOut = p.RES_LOC,
                            DateOut = p.RS_ARRIVAL_DATE == null ? "" : p.RS_ARRIVAL_DATE.Value.ToShortDateString(),
                            TimeOut = p.RS_ARRIVAL_TIME == null ? "" : p.RS_ARRIVAL_TIME.Value.ToShortTimeString(),
                            LocationIn = p.RTRN_LOC,
                            DateIn = p.RTRN_DATE == null ? "" : p.RTRN_DATE.Value.ToShortDateString(),
                            TimeIn = p.RTRN_TIME == null ? "" : p.RTRN_TIME.Value.ToShortTimeString(),
                            ReservationDays = p.RES_DAYS.ToString(),
                            Tariff = p.RATE_QUOTED,
                            CustomerName = p.CUST_NAME,
                            Cdp = p.CDPID_NBR,
                            No1 = p.NO1_CLUB_GOLD,
                            FlightNumber = p.FLIGHT_NBR,
                            Gold = p.GS,
                            PrePaid = p.PREPAID
                        }).ToList();
            return data;
        }

        public void bind(params string[] dependants)
        {

            var data = GetGridDataToBind();
            GridViewer.DataSource = data;
            GridViewer.DataBind();
            foreach (ResDetailsHeadingEntity item in _postBackRepository.GetList())
            {
                for (int i = 0; i < GridViewer.Rows.Count; i++)
                {
                    GridViewer.Rows[i].Cells[item.index].HorizontalAlign = item.Alignment;
                }
                    
                if (GridViewer.HeaderRow != null) GridViewer.HeaderRow.Cells[item.index].Text = S1 + item.SortExpression + S2 + item.Text + S3;
            }
            DataItemsCount = data.Count;

        }
        public IReservationDetailsEntity GetModalDetails(int rowIndex)
        {
            return _repository.getItem(GridViewer.Rows[rowIndex].Cells[1].Text);
        }
        public string SortExpression(string sortString)
        {
            if (string.IsNullOrEmpty(sortString)) return string.Empty;
            _sortExpression = sortString;
            if (_sortDic.ContainsKey(sortString)) _sortDic[sortString] = _sortDic[sortString] == "asc" ? "desc" : "asc";
            else _sortDic[sortString] = "asc";
            return _sortExpression;
        }
    }
}
