using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Classes.Entities.Pooling.Abstract;

namespace Mars.DAL.Pooling.Abstract
{
    public abstract class HtmlTableRepository<T> : IHtmlTableRepository<T>
    {
        public enum queryValues { NEXTHOUR, FOLLOW4HOURS, RESTOFDAY };
        public IMainFilterEntity Filter { get; set; }
        public DateTime DateSelected { get; set; }

        public virtual IList<T> GetTable(params String[] s)
        {
            throw new NotImplementedException("Please override the virtual method GetTable in the abstract class Mars.DAL.Pooling.Abstract.HtmlTableRepository");
        }
    }
}