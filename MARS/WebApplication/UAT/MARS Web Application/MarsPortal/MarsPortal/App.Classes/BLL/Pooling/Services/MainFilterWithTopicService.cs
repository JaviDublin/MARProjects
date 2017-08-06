using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.Pooling.Models.Abstract;
using App.Classes.BLL.Pooling.Models.Abstract;

namespace Mars.Pooling.Services
{
    public class MainFilterWithTopicService<t> : MainFilterService<t>
    {
        protected readonly IFilterModel2 _topicFilter;
        public MainFilterWithTopicService(t gridviewModel, ICmsOpsLogicModel cmsOpsModel, IThreeFilterCascadeModel carModel, IFilterModel2 topicFilter)
            : base(gridviewModel, cmsOpsModel, carModel)
        {
            if (topicFilter == null) throw new ArgumentNullException("The topicFilter has to have a value");
            _topicFilter = topicFilter;
        }
    }
}