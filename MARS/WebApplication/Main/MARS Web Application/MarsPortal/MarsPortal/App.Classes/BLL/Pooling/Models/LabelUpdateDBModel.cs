using System;
using App.Classes.DAL.Reservations.Abstract;
using Mars.Pooling.Models.Abstract;

namespace Mars.Pooling.Models
{
    public class LabelUpdateDBModel : LabelModel
    {

        IReservationLogRepository _repository;
        public LabelUpdateDBModel(IReservationLogRepository r) { _repository = r; }

        public override void Update()
        {
            if (_repository == null) throw new ArgumentNullException("If using the Update method of the class LabelUpdateDBModel use the one argument constructor, with the argument IReservationLogRepository");
            Text = _repository.getItem().FleetMessage;
            ErrorText = _repository.getItem().TeraDataMessage;
        }
    }
}