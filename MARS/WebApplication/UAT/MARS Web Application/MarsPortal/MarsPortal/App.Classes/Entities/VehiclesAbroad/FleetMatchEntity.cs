using System;
using App.Classes.Entities.VehiclesAbroad.Abstract;

namespace App.Entities.VehiclesAbroad {
    public class FleetMatchEntity : IFleetMatchEntity {
        public string OwnCountry { get; set; }
        public string Unit { get; set; }
        public string License { get; set; }
        public string ModelDesc { get; set; }
        public string Vc { get; set; }
        public string Operstat { get; set; }
        public string Location { get; set; }
        public string Daysrev { get; set; }
        public string CarVan { get; set; } // "C" for car and "V" for van
        public string Matches { get; set; }
    }
}