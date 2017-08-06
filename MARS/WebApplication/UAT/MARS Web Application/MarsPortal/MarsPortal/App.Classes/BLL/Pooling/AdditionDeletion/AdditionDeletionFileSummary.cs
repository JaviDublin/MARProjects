using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.MarsDBContext;

namespace Mars.App.Classes.BLL.Pooling.AdditionDeletion
{
    public class AdditionDeletionFileSummary
    {
        public List<ResAddition> Additions { get; set; }
        public List<ResDeletion> Deletions { get; set; }
        public string FilePaseMessage;
        public int RowsSkipped;
        public int ValidRows;

    }
}