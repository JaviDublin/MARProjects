using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Classes.BLL.Pooling.Models.Abstract;
using App.BLL.VehiclesAbroad.Models;
using App.Classes.DAL.Pooling.Abstract;

namespace App.Classes.BLL.Pooling.Models {

    public class ButtonModel2 : ButtonModel, IButtonModel2 {

        public IButtonModelRepository Repository { get; set; }
        public ButtonModel2() { }
        public ButtonModel2(IButtonModelRepository r) {
            Repository = r;
        }
        public string getRedirectUrl() {
            return Repository.getUri();
        }
        public void setLabel() {
            _Text = Repository.getLabel();
        }
    }
}