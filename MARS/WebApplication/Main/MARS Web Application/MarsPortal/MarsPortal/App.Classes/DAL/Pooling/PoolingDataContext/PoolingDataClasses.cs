

namespace Mars.App.Classes.DAL.Pooling.PoolingDataContext
{
    partial class PoolingDataClassesDataContext
    {
    }

    public partial class Reservation
    {
        [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "LOCATION_Reservation1", Storage = "_LOCATION1", ThisKey = "RENT_LOC", OtherKey = "dim_Location_id", IsForeignKey = true)]
        public LOCATION RentalLocation { get {return this.LOCATION1; }}

        [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "LOCATION_Reservation", Storage = "_LOCATION", ThisKey = "RTRN_LOC", OtherKey = "dim_Location_id", IsForeignKey = true)]
        public LOCATION ReturnLocation { get {return this.LOCATION;  }}
    }
}
