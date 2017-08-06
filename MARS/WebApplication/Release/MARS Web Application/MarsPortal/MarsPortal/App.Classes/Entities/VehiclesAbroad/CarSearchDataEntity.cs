using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Entities.VehiclesAbroad;

namespace App.Entities.VehiclesAbroad {

    /// <summary>
    /// Entity class to hold the vehicle details
    /// </summary>
    public class CarSearchDataEntity : ICarSearchDataEntity {
        public string Lstwwd { get; set; }
        public DateTime? Lstdate { get; set; }
        public string Lsttime { get; set; }
        public string Vc { get; set; }
        public string Unit { get; set; }
        public string License { get; set; }
        public string Model { get; set; }
        public string Moddesc { get; set; }
        public string Duewwd { get; set; }
        public DateTime? Duedate { get; set; }
        public string Duetime { get; set; }
        public string Op { get; set; }
        public string Mt { get; set; }
        public string Hold { get; set; }
        public string Nr { get; set; }
        public string Driver { get; set; }
        public string Doc { get; set; }
        public int Lstmlg { get; set; }
        public string Remarks { get; set; }
        public string Charged { get; set; }
        int? _nonRev;
        public int Nonrev {
            get { return _nonRev ?? 0; }
            set { _nonRev = value; }
        }
        public string Regdate { get; set; }
        public string Blockdate { get; set; }
        public string Remarkdate { get; set; }
        public string Ownarea { get; set; }
        public string Bddays { get; set; }
        public string Mmdays { get; set; }
        public string Prevwwd { get; set; }
        public string LocGroup { get; set; }
        public string OwnCountry { get; set; }
        public string Pool { get; set; }
        public string Colour { get; set; }
        public string Serial { get; set; }
        public int Overdue { get; set; }
        public string Lstoorc { get; set; }
        public int OnRent { get; set; }
        public string Lsttype { get; set; }
        public string CarVan { get; set; }
    }
}