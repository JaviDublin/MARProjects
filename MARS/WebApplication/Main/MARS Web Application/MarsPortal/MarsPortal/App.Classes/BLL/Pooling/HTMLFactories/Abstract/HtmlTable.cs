using System;
using App.Classes.Entities.Pooling.Abstract;
using Mars.Entities.Pooling;
namespace Mars.Pooling.HTMLFactories.Abstract {

    public abstract class HtmlTable {

        IMainFilterEntity _filter;
        public IMainFilterEntity Filter {
            get {
                if (_filter==null) throw new NotImplementedException("Please instantiate the Filter in abstract class Mars.Pooling.HTMLFactories.Abstract.HtmlTable to a class that implements App.Classes.Entities.Pooling.Abstract.IMainFilterEntity.");
                return _filter;
            }
            set { _filter=value; } 
        }
        public virtual string GetTable() { 
            throw new NotImplementedException("Please overload the GetTable method in abstract class Mars.Pooling.HTMLFactories.Abstract.HtmlTable. This class is part of the HtmlFactory (IHtmlFactory)."); 
        }
    }
}
