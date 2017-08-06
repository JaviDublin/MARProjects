using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Classes.Entities.VehiclesAbroad.Abstract;

namespace App.Classes.Entities.VehiclesAbroad {

    public class UtilisationHistoryStoredProcEntity {

        public UtilisationHistoryStoredProcEntity() { }

        private System.Nullable<System.DateTime> _REPDATE;
        private string _ownCountry;
        private System.Nullable<double> _UTILIZATION_OUT_OF_COUNTRY;
        private System.Nullable<double> _UTILIZATION_IN_COUNTRY;

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_REPDATE", DbType = "DateTime")]
        public System.Nullable<System.DateTime> REPDATE {
            get {
                return this._REPDATE;
            }
            set {
                if ((this._REPDATE != value)) {
                    this._REPDATE = value;
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ownCountry", DbType = "VarChar(10)")]
        public string ownCountry {
            get {
                return this._ownCountry;
            }
            set {
                if ((this._ownCountry != value)) {
                    this._ownCountry = value;
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_UTILIZATION_OUT_OF_COUNTRY", DbType = "Float")]
        public System.Nullable<double> UTILIZATION_OUT_OF_COUNTRY {
            get {
                return this._UTILIZATION_OUT_OF_COUNTRY;
            }
            set {
                if ((this._UTILIZATION_OUT_OF_COUNTRY != value)) {
                    this._UTILIZATION_OUT_OF_COUNTRY = value;
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_UTILIZATION_IN_COUNTRY", DbType = "Float")]
        public System.Nullable<double> UTILIZATION_IN_COUNTRY {
            get {
                return this._UTILIZATION_IN_COUNTRY;
            }
            set {
                if ((this._UTILIZATION_IN_COUNTRY != value)) {
                    this._UTILIZATION_IN_COUNTRY = value;
                }
            }
        }
    }
}