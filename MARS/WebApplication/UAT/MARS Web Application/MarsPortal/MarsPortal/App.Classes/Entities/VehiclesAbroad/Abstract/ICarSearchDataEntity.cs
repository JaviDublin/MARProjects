using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Entities.VehiclesAbroad {
    public interface ICarSearchDataEntity {
        string Lstwwd { get; set; }
        DateTime? Lstdate { get; set; }
        string Lsttime { get; set; }
        string Vc { get; set; }
        string Unit { get; set; }
        string License { get; set; }
        string Model { get; set; }
        string Moddesc { get; set; }
        string Duewwd { get; set; }
        DateTime? Duedate { get; set; }
        string Duetime { get; set; }
        string Op { get; set; }
        string Mt { get; set; }
        string Hold { get; set; }
        string Nr { get; set; }
        string Driver { get; set; }
        string Doc { get; set; }
        int Lstmlg { get; set; }
        string Remarks { get; set; }
        string Charged { get; set; }
        int Nonrev { get; set; }
        string Regdate { get; set; }
        string Blockdate { get; set; }
        string Remarkdate { get; set; }
        string Ownarea { get; set; }
        string Bddays { get; set; }
        string Mmdays { get; set; }
        string Prevwwd { get; set; }
        string LocGroup { get; set; }
        string OwnCountry { get; set; }
        string Pool { get; set; }
        string Colour { get; set; }
        string Serial { get; set; }
        int Overdue { get; set; }
        string Lstoorc { get; set; }
        int OnRent { get; set; }
        string Lsttype { get; set; }
        string CarVan { get; set; }
    }
}
