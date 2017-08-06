namespace Mars.App.Classes.DAL.MarsDBContext
{
    partial class MarsDBDataContext
    {
        partial void OnCreated()
        {
            CommandTimeout = 120;
        }
    }

    public partial class Reservation
    {
        [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "LOCATION_Reservation", Storage = "_LOCATION", ThisKey = "PickupLocationId", OtherKey = "dim_Location_id", IsForeignKey = true)]
        public LOCATION PickupLocation { get { return this.LOCATION; } }

        [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "LOCATION_Reservation1", Storage = "_LOCATION1", ThisKey = "ReturnLocationId", OtherKey = "dim_Location_id", IsForeignKey = true)]
        public LOCATION ReturnLocation { get { return this.LOCATION1; } }

        [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "CAR_GROUP_Reservation", Storage = "_CAR_GROUP", ThisKey = "ReservedCarGroupId", OtherKey = "car_group_id", IsForeignKey = true)]
        public CAR_GROUP ReservedCarGroup { get { return this.CAR_GROUP; } }

        [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "CAR_GROUP_Reservation", Storage = "_CAR_GROUP1", ThisKey = "UpgradedCarGroupId", OtherKey = "car_group_id", IsForeignKey = true)]
        public CAR_GROUP UpgradedCarGroup { get { return this.CAR_GROUP1; } }


    }

    //public partial class VehicleSublease
    //{
    //    [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "COUNTRy_VehicleSublease", Storage = "_COUNTRy", ThisKey = "OwningCountryId", OtherKey = "CountryId", IsForeignKey = true)]

    //    public COUNTRy OwningCountry { get { return this.COUNTRy; } }

    //    [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "COUNTRy_VehicleSublease1", Storage = "_COUNTRy1", ThisKey = "RentingCountryId", OtherKey = "CountryId", IsForeignKey = true)]

    //    public COUNTRy RentingCountry { get { return this.COUNTRy1; } }
    //}


}
