using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.App.Classes.DAL.MarsDBContext;

namespace Mars.App.Classes.BLL.Pooling.AdditionDeletion
{
    public class BufferFileSummary
    {
        public List<ResBuffer> Buffers { get; set; }
        public string FilePaseMessage;
        public int RowsSkipped;
        public int ValidRows;
    }
}