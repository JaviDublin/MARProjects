using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Classes.BLL.Pooling.Models.Abstract;
using App.DAL.VehiclesAbroad.Abstract;
using App.Classes.BLL.Pooling.Models;
using Mars.DAL.Pooling.Abstract;
using Mars.Pooling.Models.Abstract;
using Mars.Pooling.Models;

namespace App.Classes.BLL.Pooling.Models {
    public class ThreeFilterCascadeModel : IThreeFilterCascadeModel {

        public IFilterModel3 TopModel { get; set; }
        public IFilterModel3 BottomModel { get; set; }
        public IFilterModel3 MiddleModel { get; set; }

        public ThreeFilterCascadeModel(IFilterRepository3 t,IFilterRepository3 m,IFilterRepository3 b) {
            TopModel = new FilterModel3(t);
            MiddleModel = new FilterModel3(m);
            BottomModel = new FilterModel3(b);
        }
        public void SetRepositories(IThreeFilterRepository r) {
            SetRepositories(r.TopRepository,r.MiddleRepository,r.BottomRepository);
        }
        public void SetRepositories(IFilterRepository3 t,IFilterRepository3 m,IFilterRepository3 b) {
            TopModel.Repository3=t;
            MiddleModel.Repository3=m;
            BottomModel.Repository3=b;
        }
        public void bind(params string[] dependants) {
            clearAll();
            setFeedback();
        }
        public void SuperSelected(params string[] dependants) {
            clearAll();
            TopModel.bind(dependants);
            setFeedback();
        }
        public void TopSelected(params string[] dependants) {
            if (TopModel.SelectedIndex <= 0) MiddleModel.clear();
            else MiddleModel.bind(dependants);
            BottomModel.clear();
            setFeedback();
        }
        public void MiddleSelected(params string[] dependants) {
            if (MiddleModel.SelectedIndex <= 0) BottomModel.clear();
            else BottomModel.bind(dependants);
            setFeedback();
        }
        public void rebind(int it, int im, int ib, string st) {
            TopModel.rebind(it, st);
            MiddleModel.rebind(im,TopModel.SelectedValue,st);
            BottomModel.rebind(ib, MiddleModel.SelectedValue, TopModel.SelectedValue, st);
            setFeedback();
        }
        public void CascadeRebind(int it,int im,int ib,string st) {
            TopModel.rebind(it,st);
            MiddleModel.rebind(im,st,TopModel.SelectedValue);
            BottomModel.rebind(ib,st,TopModel.SelectedValue, MiddleModel.SelectedValue);
            setFeedback();
        }
        private void setFeedback() {
            TopModel.SetFeedback();
            MiddleModel.SetFeedback();
            BottomModel.SetFeedback();
        }
        private void clearAll() {
            TopModel.clear();
            MiddleModel.clear();
            BottomModel.clear();
        }
    }
}