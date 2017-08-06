using App.Classes.BLL.Pooling.Models.Abstract;
using System.Web.UI.WebControls;
using Mars.DAL.Pooling.Abstract;

namespace Mars.Pooling.Models {
    public class BrowserParamsModel : IBrowserParamsModel {

        private IBrowserJavascriptRepository _repository;
        public BrowserParamsModel(IBrowserJavascriptRepository r) { _repository = r; }

        public HiddenField BrowserHeight { get; set; }
        public HiddenField BrowserWidth { get; set; }

        public void SetJavaScript(System.Web.UI.Page p) {
            p.ClientScript.RegisterStartupScript(this.GetType(), "BrowserParams", _repository.getJavascript(BrowserWidth.ID, BrowserHeight.ID));
        }
        public int getBrowserHeight() {
            int i = 0;
            int.TryParse(BrowserHeight.Value, out i);
            return i;
        }
        public int getBrowserWidth() {
            int i = 0;
            int.TryParse(BrowserWidth.Value, out i);
            return i;
        }
    }
}