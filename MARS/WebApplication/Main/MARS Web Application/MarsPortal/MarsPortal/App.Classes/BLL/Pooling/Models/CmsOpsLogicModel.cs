using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Classes.BLL.Pooling.Models.Abstract;
using System.Web.UI.WebControls;
using App.DAL.VehiclesAbroad.Abstract;
using Mars.DAL.Pooling.Abstract;
using System.Web.UI;
using Mars.Pooling.Models.Abstract;
using Mars.Pooling.Models;

namespace App.Classes.BLL.Pooling.Models {
    public class CmsOpsLogicModel : ICmsOpsLogicModel {

        static string[] CMSLabels = { "Pool:", "Location Group:" };
        static string[] OPSLabels = { "Region:", "Area:" };
        bool _isOPS;
        IThreeFilterRepository _cmsRepository,_opsRepository;
        public bool isOPS { get { return _isOPS; } }
        public bool isCMS { get { return !_isOPS; } }
        public Label TopLabel0 { get; set; }
        public Label TopLabel1 { get; set; }
        public Label BottomLabel0 { get; set; }
        public Label BottomLabel1 { get; set; }
        public RadioButton CMSRadioButton { get; set; }
        public RadioButton OPSRadioButton { get; set; }
        public IThreeFilterCascadeModel GeneralThreeFilterModel { get; set; }
        public IFilterModel3 CountryFilterModel { get; set; }

        public CmsOpsLogicModel(IFilterRepository3 countryRepository,IThreeFilterRepository cmsRepository,IThreeFilterRepository opsRepository) {
            CountryFilterModel = new FilterModel3(countryRepository);
            _cmsRepository=cmsRepository;
            _opsRepository=opsRepository;
            GeneralThreeFilterModel=new ThreeFilterCascadeModel(cmsRepository.TopRepository,cmsRepository.MiddleRepository,cmsRepository.BottomRepository);
        }
        public void Initialise(Page p) {
            if(p.IsPostBack) return;
            CountryFilterModel.bind();
            SetToCms();
        }
        public void SetToOps() {
            _isOPS=true;
            CMSRadioButton.Checked=false;
            OPSRadioButton.Checked=true;
            TopLabel0.Text = OPSLabels[0];
            TopLabel1.Text = OPSLabels[1];
            BottomLabel0.Text = OPSLabels[0];
            BottomLabel1.Text = OPSLabels[1];
            GeneralThreeFilterModel.SetRepositories(_opsRepository);
            GeneralThreeFilterModel.SuperSelected(CountryFilterModel.SelectedValue);
        }
        public void SetToCms() {
            _isOPS=false;
            CMSRadioButton.Checked=true;
            OPSRadioButton.Checked=false;
            TopLabel0.Text = CMSLabels[0];
            TopLabel1.Text = CMSLabels[1];
            BottomLabel0.Text = CMSLabels[0];
            BottomLabel1.Text = CMSLabels[1];
            GeneralThreeFilterModel.SetRepositories(_cmsRepository);
            GeneralThreeFilterModel.SuperSelected(CountryFilterModel.SelectedValue);
        }
        void bind() {
            GeneralThreeFilterModel.bind(CountryFilterModel.SelectedValue);
        }
    }
}