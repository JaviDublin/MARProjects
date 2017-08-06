using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.DAL.Pooling.Abstract;
using App.Classes.Entities.Pooling.Abstract;

namespace Mars.DAL.Pooling {
    public class SiteComparisonRepositoryTest:IComparisonRepository {

        readonly int NoInArray=73, NoOfRows=20;
        public IMainFilterEntity Filter { get; set; }

        public IList<String[]> GetList() {
            IList<String[]> ABagOfData=new List<String[]>();
            for(int o=0;o<NoOfRows;o++) {
                String[] s= new String[NoInArray];
                s[0]="Row Header "+o;
                for(Int32 i=1;i<NoInArray;i++) s[i]="0"+o+"0"+i;
                ABagOfData.Add(s);
            }
            return ABagOfData;
        }


        public global::App.Classes.DAL.Pooling.Abstract.Enums.DayActualTime Tme {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }


        public IList<string[]> GetList(global::App.Classes.DAL.Pooling.Abstract.Enums.DayActualTime Tme) {
            throw new NotImplementedException();
        }
    }
}