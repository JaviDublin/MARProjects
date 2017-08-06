using System;
using System.Web.UI.HtmlControls;
using Mars.Pooling.HTMLFactories;
using App.Classes.DAL.Pooling.Abstract;
namespace Mars.Pooling.Models.Abstract {
    public interface ICompGridModel {

        void Bind(params string[] param);
        HtmlGenericControl DataTable { get; set; }
        CompHtmlTable _HtmlTable { get; set; }
        Enums.Headers Mode { get; set; }
    }
}
