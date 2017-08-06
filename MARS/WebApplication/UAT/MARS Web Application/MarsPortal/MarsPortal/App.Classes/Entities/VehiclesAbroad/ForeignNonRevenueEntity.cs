
namespace App.Entities.VehiclesAbroad {

    public class ForeignNonRevenueEntity {

        private string _OWNING_COUNTRY;
        private string _IN_COUNTRY;
        private int _RENTABLE;
        private int _SHOP;
        private int _OTHER;

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_OWNING_COUNTRY", DbType = "VarChar(10)")]
        public string OWNING_COUNTRY {
            get {
                return this._OWNING_COUNTRY;
            }
            set {
                if ((this._OWNING_COUNTRY != value)) {
                    this._OWNING_COUNTRY = value;
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_IN_COUNTRY", DbType = "VarChar(10)")]
        public string IN_COUNTRY {
            get {
                return this._IN_COUNTRY;
            }
            set {
                if ((this._IN_COUNTRY != value)) {
                    this._IN_COUNTRY = value;
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_RENTABLE", DbType = "Int NOT NULL")]
        public int RENTABLE {
            get {
                return this._RENTABLE;
            }
            set {
                if ((this._RENTABLE != value)) {
                    this._RENTABLE = value;
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SHOP", DbType = "Int NOT NULL")]
        public int SHOP {
            get {
                return this._SHOP;
            }
            set {
                if ((this._SHOP != value)) {
                    this._SHOP = value;
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_OTHER", DbType = "Int NOT NULL")]
        public int OTHER {
            get {
                return this._OTHER;
            }
            set {
                if ((this._OTHER != value)) {
                    this._OTHER = value;
                }
            }
        }
    }
}