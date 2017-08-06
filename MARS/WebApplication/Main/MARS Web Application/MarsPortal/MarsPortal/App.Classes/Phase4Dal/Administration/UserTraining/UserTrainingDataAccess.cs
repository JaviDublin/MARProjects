using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;
using Mars.App.Classes.Phase4Dal.Administration.UserTraining.Entities;

namespace Mars.App.Classes.Phase4Dal.Administration.UserTraining
{
    public class UserTrainingDataAccess : BaseDataAccess
    {
        public List<UserTrainingEntity> GetActiveEntries()
        {
            var entities = from ut in DataContext.UserTrainings
                where ut.IsActive
                select new UserTrainingEntity
                       {
                           Description = ut.Description,
                           Url = ut.Url,
                       };

            var returned = entities.ToList();
            return returned;
        }
    }
}