using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Entities.VehiclesAbroad;

using App.BLL.VehiclesAbroad.Abstract;

namespace App.BLL.VehiclesAbroad.Concrete {

    public class SaveCSVFileVehicleDetails : ICSVDownload<CarSearchDataEntity> {

        ICSVGenerator _csvGen;
        //ILog _log = log4net.LogManager.GetLogger("VehiclesAbroad");

        public IEnumerable<CarSearchDataEntity> csvList { get; set; }
        public string FileName { get; set; } // the filename returned by the ICSVGenerator implementation

        // Note the CSVGenerator is dependency injected
        public SaveCSVFileVehicleDetails(ICSVGenerator csvGen) {

            _csvGen = csvGen;
        }
        public bool downloadFile(string path, string fileName) {

            if (!string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(fileName)) {

                _csvGen.write2Cell("Lstwwd,");
                _csvGen.write2Cell("Lstdate,");
                _csvGen.write2Cell("Vc,");
                _csvGen.write2Cell("Unit,");
                _csvGen.write2Cell("License,");
                _csvGen.write2Cell("Model,");
                _csvGen.write2Cell("Moddesc,");
                _csvGen.write2Cell("Duewwd,");
                _csvGen.write2Cell("Duedate,");
                _csvGen.write2Cell("Duetime,");
                _csvGen.write2Cell("Op,");
                _csvGen.write2Cell("Mt,");
                _csvGen.write2Cell("Nr,");
                _csvGen.write2Cell("Driver,");
                _csvGen.write2Cell("Doc,");
                _csvGen.write2Cell("Lstmlg");
                _csvGen.newLine();

                // get the data from the screen
                if (csvList != null) {

                    foreach (var row in csvList) {

                        _csvGen.write2Cell(row.Lstwwd + ",");
                        _csvGen.write2Cell(row.Lstdate + ",");
                        _csvGen.write2Cell(row.Vc + ",");
                        _csvGen.write2Cell(row.Unit + ",");
                        _csvGen.write2Cell(row.License + ",");
                        _csvGen.write2Cell(row.Model + ",");
                        _csvGen.write2Cell(row.Moddesc + ",");
                        _csvGen.write2Cell(row.Duewwd + ",");
                        _csvGen.write2Cell(row.Duedate + ",");
                        _csvGen.write2Cell(row.Duetime + ",");
                        _csvGen.write2Cell(row.Op + ",");
                        _csvGen.write2Cell(row.Mt + ",");
                        _csvGen.write2Cell(row.Nr + ",");
                        _csvGen.write2Cell(row.Driver + ",");
                        _csvGen.write2Cell(row.Doc + ",");
                        _csvGen.write2Cell(row.Lstmlg.ToString());
                        _csvGen.newLine();
                    }
                }
                try {

                    FileName = _csvGen.saveAs(fileName, path);
                    return true;
                }
                catch {
                    //_log.Error("Exception in SaveCSVFileVehicleDetails, downloadFile method, fileName = " + fileName);
                    return false;
                }
            }
            return false;
        }
    }
}