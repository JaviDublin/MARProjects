using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.DAL.Pooling.Abstract;
using Mars.Entities.Pooling;

namespace Mars.DAL.Pooling {
    public class DoPostBackRepository:IDoPostBackRepository {

        private readonly IList<ResDetailsHeadingEntity> _list;
        public DoPostBackRepository() {
            //very basic way of doing it!!!!
            _list=new List<ResDetailsHeadingEntity>();
            _list.Add(new ResDetailsHeadingEntity { index=1,Alignment=System.Web.UI.WebControls.HorizontalAlign.Left,SortExpression="RES_ID",Text="RES-ID" });
            _list.Add(new ResDetailsHeadingEntity { index=2,Alignment=System.Web.UI.WebControls.HorizontalAlign.Left,SortExpression="GR",Text="GR" });
            _list.Add(new ResDetailsHeadingEntity { index=3,Alignment=System.Web.UI.WebControls.HorizontalAlign.Left,SortExpression="LOC_OUT",Text="LOC OUT" });
            _list.Add(new ResDetailsHeadingEntity { index=4,Alignment=System.Web.UI.WebControls.HorizontalAlign.Center ,SortExpression="DATE_OUT",Text="DATE" });
            _list.Add(new ResDetailsHeadingEntity { index=5,Alignment=System.Web.UI.WebControls.HorizontalAlign.Center,SortExpression="OUT",Text="OUT" });
            _list.Add(new ResDetailsHeadingEntity { index=6,Alignment=System.Web.UI.WebControls.HorizontalAlign.Left,SortExpression="LOC_IN",Text="LOC IN" });
            _list.Add(new ResDetailsHeadingEntity { index=7,Alignment=System.Web.UI.WebControls.HorizontalAlign.Center,SortExpression="DATE_IN",Text="DATE" });
            _list.Add(new ResDetailsHeadingEntity { index=8,Alignment=System.Web.UI.WebControls.HorizontalAlign.Center,SortExpression="IN",Text="IN" });
            _list.Add(new ResDetailsHeadingEntity { index=9,Alignment=System.Web.UI.WebControls.HorizontalAlign.Right,SortExpression="LN",Text="LN" });
            _list.Add(new ResDetailsHeadingEntity { index=10,Alignment=System.Web.UI.WebControls.HorizontalAlign.Left,SortExpression="TARIFF",Text="TARIFF" });
            _list.Add(new ResDetailsHeadingEntity { index=11,Alignment=System.Web.UI.WebControls.HorizontalAlign.Left,SortExpression="CUSTOMER",Text="CUSTOMER" });
            _list.Add(new ResDetailsHeadingEntity { index=12,Alignment=System.Web.UI.WebControls.HorizontalAlign.Right,SortExpression="CDP",Text="CDP" });
            _list.Add(new ResDetailsHeadingEntity { index=13,Alignment=System.Web.UI.WebControls.HorizontalAlign.Left,SortExpression="NO1",Text="#1" });
            _list.Add(new ResDetailsHeadingEntity { index=14,Alignment=System.Web.UI.WebControls.HorizontalAlign.Left,SortExpression="FLT_NBR",Text="FLT-NBR" });
            _list.Add(new ResDetailsHeadingEntity { index=15,Alignment=System.Web.UI.WebControls.HorizontalAlign.Center,SortExpression="GO",Text="GO" });
            _list.Add(new ResDetailsHeadingEntity { index=16,Alignment=System.Web.UI.WebControls.HorizontalAlign.Center,SortExpression="PP",Text="PP" });
        }

        public IList<ResDetailsHeadingEntity> GetList() {
            return _list;
        }
    }
}