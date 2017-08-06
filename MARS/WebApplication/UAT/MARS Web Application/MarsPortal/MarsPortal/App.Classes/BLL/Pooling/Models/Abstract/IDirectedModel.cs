using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Pooling.Models.Abstract {
    public interface IDirectedModel {
        IDictionary <string,string>GetQueries(string queryString);
    }
}
