using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.DAL.Pooling.Abstract {
    public interface IBrowserJavascriptRepository {
        string getJavascript(params string[] s);
    }
}
