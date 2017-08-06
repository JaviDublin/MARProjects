using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Entities.VehiclesAbroad.Abstract;

namespace App.Classes.Entities.VehiclesAbroad {
    public class ReservationBalanceEntity : IReservationBalanceEntity {

        private string _owning;
        private string _destination;
        private int _result;

        public ReservationBalanceEntity() { }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_owning", DbType = "VarChar(1) NOT NULL", CanBeNull = false)]
        public string owning {
            get {
                return this._owning;
            }
            set {
                if ((this._owning != value)) {
                    this._owning = value;
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_destination", DbType = "VarChar(1) NOT NULL", CanBeNull = false)]
        public string destination {
            get {
                return this._destination;
            }
            set {
                if ((this._destination != value)) {
                    this._destination = value;
                }
            }
        }
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_result", DbType = "Int NOT NULL")]
        public int result {
            get {
                return this._result;
            }
            set {
                if ((this._result != value)) {
                    this._result = value;
                }
            }
        }
    }
}