using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mars.App.Classes.BLL.Pooling.AdditionDeletion
{
    public class AdditionDeletionGridViewHolder
    {
        public int Identifier { get; set; }
        public string LocationWwd { get; set; }
        public int LocationWwdId { get; set; }
        public string CarGroup { get; set; }
        public int CarGroupId { get; set; }
        public DateTime RepDate { get; set; }
        public int Value { get; set; }
        public bool Addition { get; set; }

        public bool MarkedForDeletion { get; set; }
    }
}