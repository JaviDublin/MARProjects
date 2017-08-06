using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.Pooling.Models.Abstract;
using Mars.DAL.Pooling.Abstract;
using Mars.Entities.Pooling;
using Mars.DAL.Pooling;
using App.Classes.Entities.Pooling.Abstract;

namespace Mars.Pooling.Models {
    public class LabelStatusModel:LabelModel {

        public IMainFilterEntity Filter{get;set;}
        public Int32 NoOfDays { get; set; }
        public IOverdueActualRepository _repository;
        public LabelStatusModel(IOverdueActualRepository repository) {
            if(repository==null) throw new ArgumentNullException("The repository can't be null.");
            _repository=repository;
            NoOfDays=3;
        }
        public override void Update() {
            if(Filter==null) throw new NullReferenceException("The Update method in the LabelStatusModel is used, set the Filter property to a reference to MainFilterEntity");
            string s =_repository.GetItem(Filter,NoOfDays);
            Text=string.IsNullOrEmpty(s)?"0":s;
        }
    }
}