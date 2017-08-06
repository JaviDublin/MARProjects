using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.Pooling.Services.Abstract;
using App.Classes.BLL.Pooling.Models.Abstract;

namespace Mars.Pooling.Services
{
    public class MainFilterService<t> : FilterService<t>, IFilterService
    {
        protected readonly ICmsOpsLogicModel _cmsOpsModel;
        protected readonly IThreeFilterCascadeModel _carModel;
        public MainFilterService(t gridviewModel, ICmsOpsLogicModel cmsOpsModel, IThreeFilterCascadeModel carModel)
            : base(gridviewModel)
        {
            if (cmsOpsModel == null) throw new ArgumentNullException("The cmsopsModel can't be null");
            if (carModel == null) throw new ArgumentNullException("The carModel can't be null");
            _cmsOpsModel = cmsOpsModel;
            _carModel = carModel;
        }
    }
}