using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.Classes.BLL.Pooling.Models.Abstract;
using App.Classes.DAL.Pooling.Abstract;
using App.Classes.DAL.Pooling;

namespace App.Classes.BLL.Pooling.Models {
    public class HeadingModel : IHeadingModel {

        IHeaderRepository _repository;
        public HeadingModel() { _repository = new HeaderRepository(); }
        public HeadingModel(IHeaderRepository r) { _repository = r; }
        public System.Web.UI.WebControls.Label HeadingLabel { get; set; }

        public string Text {
            get { return HeadingLabel.Text; }
        }
        public void setText(Enums.Headers h) {
            HeadingLabel.Text = _repository.getHeader(h);
        }
    }
}
