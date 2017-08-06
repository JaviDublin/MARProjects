using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Classes.DAL.Reservations.Abstract {
    public interface IJavaScriptRepository {
        string getJavascript(params string[] s);
    }
}