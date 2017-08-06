using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Entities.VehiclesAbroad;

namespace App.Entities.VehiclesAbroad {

    [Serializable]
    public class CarFilterEntity : ICarFilterEntity {
        // Entity to hold the CarFilter details
        public CarFilterEntity() {
            Unit = "";
            License = "";
            Model = "";
            ModelDesc = "";
            Vin = "";
            Name = "";
            Colour = "";
            Mileage = "";
            NoRecords = 0;
            NoPerPage = 0;
            CurrentPage = 0;
            NoOfPages = 0;
        }
        public string Unit { get; set; }
        public string License { get; set; }
        public string Model { get; set; }
        public string ModelDesc { get; set; }
        public string Vin { get; set; }
        public string Name { get; set; }
        public string Colour { get; set; }
        public string Mileage { get; set; }
        public int NoRecords { get; set; }
        public int NoPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int NoOfPages { get; set; }
    }
}