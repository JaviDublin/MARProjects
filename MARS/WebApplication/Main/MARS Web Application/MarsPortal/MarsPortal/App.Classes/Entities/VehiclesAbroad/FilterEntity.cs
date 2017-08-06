using System;
using System.Linq;


namespace App.Entities.VehiclesAbroad {

    [Serializable] // for transferring using session
    public class FilterEntity : IFilterEntity {

        private string _ownCountry;
        private string _dueCountry;

        public FilterEntity() {
            // constructor for blank entity
            OwnCountry = "";
            DueCountry = "";
            Pool = "";
            Location = "";
            CarSegment = "";
            CarClass = "";
            CarGroup = "";
            VehiclePredicament = 0;
            ReservationStartDate = new DateTime();
            ReservationEndDate = new DateTime();
            DuePool = "";
            DueLocationGroup = "";
            MoveType = "";
            Operstat = "";
        }
        public FilterEntity(string destinationCountry, int vehiclePredicament, string owningCountry, string pool, string locationGroup,
                                            string carSegment, string carClass, string carGroup) {
            OwnCountry = owningCountry;
            DueCountry = destinationCountry;
            Pool = pool;
            Location = locationGroup;
            CarSegment = carSegment;
            CarClass = carClass;
            CarGroup = carGroup;
            VehiclePredicament = 0;
        }
        public string OwnCountry {
            get { // get the two letter code from the country table
                using (Mars.App.Classes.DAL.MarsDBContext.MarsDBDataContext db = new Mars.App.Classes.DAL.MarsDBContext.MarsDBDataContext()) {
                    var q = from p in db.COUNTRies
                            where _ownCountry == p.country_description
                            select p.country1;
                    return q.Count() > 0 ? q.First() : ""; // should only be one
                }
            }
            set { // set the full country description and store in the field
                _ownCountry = value;
            }
        }
        public string DueCountry {
            get { // get the two letter code from the country table
                using (Mars.App.Classes.DAL.MarsDBContext.MarsDBDataContext db = new Mars.App.Classes.DAL.MarsDBContext.MarsDBDataContext()) {
                    var q = from p in db.COUNTRies
                            where _dueCountry == p.country_description
                            select p.country1;
                    return q.Count() > 0 ? q.First() : ""; // should only be one
                }
            }
            set { // set the full country description and store in the field
                _dueCountry = value;
            }
        }

        public string Pool { get; set; }
        public string Location { get; set; }
        public string CarSegment { get; set; }
        public string CarGroup { get; set; }
        public string CarClass { get; set; }
        public int VehiclePredicament { get; set; }

        // start date and end date required for the reservation 
        public DateTime ReservationStartDate { get; set; }
        public DateTime ReservationEndDate { get; set; }

        // the reservation return filter entities, for DueCountry
        public string DuePool { get; set; }
        public string DueLocationGroup { get; set; }

        public string Operstat { get; set; }
        public string MoveType { get; set; }

        int? _nonRev;
        public int nonRev {
            get { return _nonRev ?? 0; }
            set { _nonRev = value; }
        }
    }
}
