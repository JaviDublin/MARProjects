using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.BLL.VehiclesAbroad.Abstract;
using App.Entities.VehiclesAbroad;


namespace App.BLL.VehiclesAbroad.Concrete {

    public class SaveCSVReservationDetails : ICSVDownload<ReservationMatchEntity> {

        ICSVGenerator _csvGen;
        //ILog _log = log4net.LogManager.GetLogger("VehiclesAbroad");

        public IEnumerable<ReservationMatchEntity> csvList { get; set; }
        public string FileName { get; set; }

        public SaveCSVReservationDetails(ICSVGenerator icsvGen) {

            _csvGen = icsvGen;
        }
        public bool downloadFile(string path, string fileName) {

            if (!string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(fileName)) {

                _csvGen.write2Cell("Location,");
                _csvGen.write2Cell("Group,");
                _csvGen.write2Cell("Checkout_Date,");
                _csvGen.write2Cell("Checkin_Location,");
                _csvGen.write2Cell("ResId,");
                _csvGen.write2Cell("No_Days_Until_Checkout,");
                _csvGen.write2Cell("No_Days_Reserved,");
                _csvGen.write2Cell("Driver_Name");
                _csvGen.newLine();

                // get the data from the screen
                if (csvList != null) {

                    foreach (var row in csvList) {

                        _csvGen.write2Cell(row.ResLocation + ",");
                        _csvGen.write2Cell(row.ResGroup + ",");
                        _csvGen.write2Cell(row.ResCheckoutDate + ",");
                        _csvGen.write2Cell(row.ResCheckinLoc + ",");
                        _csvGen.write2Cell(row.ResId + ",");
                        _csvGen.write2Cell(row.ResNoDaysUntilCheckout + ",");
                        _csvGen.write2Cell(row.ResNoDaysReserved + ",");
                        _csvGen.write2Cell(row.ResDriverName);
                        _csvGen.newLine();
                    }
                }
                try {
                    FileName = _csvGen.saveAs(fileName, path);
                    return true;
                }
                catch {
                    //_log.Error("Exception in SaveCSVReservationDetails, downloadFile method, fileName = " + fileName);
                    return false;
                }
            }
            return false;
        }
    }
}