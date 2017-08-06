using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Entities.VehiclesAbroad {
    public interface ICarFilterEntity {
        string Unit { get; set; }
        string License { get; set; }
        string Model { get; set; }
        string ModelDesc { get; set; }
        string Vin { get; set; }
        string Name { get; set; }
        string Colour { get; set; }
        string Mileage { get; set; }
        int NoRecords { get; set; }
        int NoPerPage { get; set; }
        int CurrentPage { get; set; }
        int NoOfPages { get; set; }
    }
}
