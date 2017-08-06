using System;
namespace App.Classes.Entities.VehiclesAbroad.Abstract {
    interface IFleetMatchEntity {
        string CarVan { get; set; }
        string Daysrev { get; set; }
        string License { get; set; }
        string Location { get; set; }
        string ModelDesc { get; set; }
        string Operstat { get; set; }
        string OwnCountry { get; set; }
        string Unit { get; set; }
        string Vc { get; set; }
        string Matches { get; set; }
    }
}
