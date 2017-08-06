using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Mars.App.Classes.Phase4Dal.NonRev.Entities.FileUploadEntities
{
    public class UploadSummary
    {
        public UploadSummary()
        {
            DataToBeUploaded = new List<UploadRow>();
            ErrorList = new StringBuilder();
        }

        public List<UploadRow> DataToBeUploaded;
        public StringBuilder ErrorList;

        
        public int InvalidDates;
        public int InvalidReasons;
        public int InvalidVins;
        public int NonUniqueVinAndOwningCountry;
    }
}