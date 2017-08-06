using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.Pooling.Services.Abstract;
using Mars.Pooling.Models.Abstract;
using System.Web.UI.WebControls;
using App.Classes.BLL.Pooling.Models.Abstract;

namespace Mars.Pooling.Services {
    public class ResTopicFilterService:IResTopicFilterService {
        private IFilterModel2 _filterModel;
        public ResTopicFilterService(IFilterModel2 filterModel) {
            if(filterModel==null) throw new ArgumentNullException("The parameter filterModel can't be null.");
            _filterModel=filterModel;
        }
        public void SetTopic(string queryString) {
            _filterModel.FilterDropDownList.SelectedValue=queryString=="res"?"***All***":queryString.Replace("res","").Replace('_',' ');
        }
    }
}