using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq.Mapping;
using System.Data.Linq;

namespace App.Entities.VehiclesAbroad {

    public class UtilisationHistoryEntity {

        public DateTime Rep_Date { get; set; }
        public string OwnCountry { get; set; }
        public decimal Local { get; set; }
        public decimal OutOfCountry { get; set; }
        public int TotalFleet { get; set; }
    }
}