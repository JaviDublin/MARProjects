using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.Pooling.Services.Abstract
{
    public abstract class FilterService<t> : IFilterService
    {
        protected readonly t _gridviewModel;
        public FilterService(t gridviewModel)
        {
            if (gridviewModel == null) throw new ArgumentNullException("The gridviewModel needs to be injected and can't be null"); _gridviewModel = gridviewModel;
        }

        public virtual void FillFilter()
        {
            throw new NotImplementedException("Implement a version of this method.");
        }
    }
}