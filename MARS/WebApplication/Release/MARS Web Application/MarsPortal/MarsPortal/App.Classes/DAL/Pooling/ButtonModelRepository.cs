using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Classes.DAL.Pooling.Abstract;
using System.Text;

namespace App.Classes.DAL.Pooling {
    public class ButtonModelRepository : IButtonModelRepository {

        Enums.buttons _enumeration;
        public ButtonModelRepository(Enums.buttons e) {
            _enumeration = e;
        }
        public string getUri() {
            return getUrlDictionary()[_enumeration];
        }
        public string getLabel() {
            return getTextDictionary()[_enumeration];
        }
        public string getLabel(Enums.buttons b) {
            return getTextDictionary()[b];
        }
        // == could be replaced with XML ==
        // could be loaded as routes in the global.asx 
        IDictionary<Enums.buttons, string> getUrlDictionary() {
            IDictionary<Enums.buttons, string> dic = new Dictionary<Enums.buttons, string>();
            dic.Add(Enums.buttons.ThirtyDayActual, @"/Pooling/ThirtyDayActuals");
            dic.Add(Enums.buttons.ThreeDayActual, @"/Pooling/ThreeDayActuals");
            return dic;
        }
        IDictionary<Enums.buttons, string> getTextDictionary() {
            IDictionary<Enums.buttons, string> dic = new Dictionary<Enums.buttons, string>();
            dic.Add(Enums.buttons.ThirtyDayActual, @"Switch to 30 Day Actuals");
            dic.Add(Enums.buttons.ThreeDayActual, @"Switch to 3 Day Actuals");
            dic.Add(Enums.buttons.SwitchToChart, @"Switch to Chart");
            dic.Add(Enums.buttons.SwitchToGrid, @"Switch to Grid");
            return dic;
        }
    }
}