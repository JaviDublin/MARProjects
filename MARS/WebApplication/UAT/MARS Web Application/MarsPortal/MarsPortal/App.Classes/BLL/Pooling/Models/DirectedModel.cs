using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.Pooling.Models.Abstract;

namespace Mars.Pooling.Models {
    public class DirectedModel:IDirectedModel {
        private readonly char[] QUERYSPLITCHARS= { '?','&' },SPLITEQUALS= { '=' };
        public DirectedModel() {}
        public IDictionary<string,string> GetQueries(string queryString) {
            IDictionary<string,string> dic=new Dictionary<string,string>();
            string[] sa=queryString.Split(QUERYSPLITCHARS);
            if(sa.Length<2) return null;
            for(int i=1;i<sa.Length;i++) {
                string[] s=sa[i].Split(SPLITEQUALS);
                dic.Add(s[0],s[1]);
            }
            return dic;
        }
    }
}