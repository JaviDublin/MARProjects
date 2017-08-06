using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace Mars.App.Classes.Phase4Dal.Administration.MappingEntities
{
    public interface IMappingEntity
    {
        int Id { set; get; }

        List<string> GetRowNames();
    }
}
