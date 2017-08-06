using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.App.Classes.Phase4Dal.Pooling.Entities
{
    public class ActualDataEntity
    {
        public DateTime Day { set; private get; }
        public int Hour { set; private get; }

        public DateTime Date
        {
            get { return Day.AddHours(Hour); }
        }

        public int Number { get; set; }
    }
}