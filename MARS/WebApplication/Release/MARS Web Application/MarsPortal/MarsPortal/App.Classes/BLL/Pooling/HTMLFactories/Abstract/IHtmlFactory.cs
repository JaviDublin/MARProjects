using System;
using App.Classes.DAL.Pooling.Abstract;
using App.Classes.Entities.Pooling.Abstract;

namespace Mars.Pooling.HTMLFactories.Abstract {

    public interface IHtmlFactory {
        HtmlTable HtmlTable { get; set; }
        string GetHTML();
    }
}
