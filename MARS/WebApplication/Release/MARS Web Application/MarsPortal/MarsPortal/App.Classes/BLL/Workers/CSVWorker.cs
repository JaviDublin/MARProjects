using System.Collections.Generic;
// --Added--


namespace App.BLL.Workers
{
    // created: 10/4/12
    // Generate csv sheets handler 
    // Explicitly coupled to MarsV1 Availability Car Search tool

    public class CSVWorker {

        public delegate void CSVWorkerEventHandler(object o, CSVWorker.CSVWorkerEventArgs e); // Event handler delegate

        private CSVGenerator _csv;
        private List<AvailabilityCarSearch.CarSearchDetails> _list;

        public string FileName { get; private set; }

        #region constructor
        
        public CSVWorker(string fn, string fp, List<AvailabilityCarSearch.CarSearchDetails> l) 
        {
            _list = l;
            _csv = new CSVGenerator();
            worker(); // runs the main thread
            FileName = _csv.saveAs(fn, fp);
        }
        
        #endregion

        private void worker() { // the main running thread

            _csv.write2Cell("LSTWWD,");
            _csv.write2Cell("LSTDate,");
            _csv.write2Cell("LSTTIME,");
            _csv.write2Cell("DUEWWD,");
            _csv.write2Cell("DUEDATE,");
            _csv.write2Cell("DUETIME,");
            _csv.write2Cell("VC,");
            _csv.write2Cell("RC,");// rc
            _csv.write2Cell("MODEL,");// modeldesc
            _csv.write2Cell("MODDESC,");
            _csv.write2Cell("Unit,");
            _csv.write2Cell("License,");
            _csv.write2Cell("SERIAL,");
            _csv.write2Cell("OPERSTAT,");
            _csv.write2Cell("MOVETYPE,");
            _csv.write2Cell("DAYSREV,");
            _csv.write2Cell("LSTNO,");
            _csv.write2Cell("DRVNAME,");
            _csv.write2Cell("LSTMLG,");
            _csv.write2Cell("IDATE,");
            _csv.write2Cell("MSODATE,");
            _csv.write2Cell("CAPDATE,");
            _csv.write2Cell("OWNDATE,");
            _csv.write2Cell("CARHOLD1,");
            _csv.write2Cell("BDDAYS,");
            _csv.write2Cell("MMDAYS,");
            _csv.write2Cell("REMARK,");
            _csv.write2Cell("RESOLUTION");
            _csv.newLine();
            foreach (var f in _list) {
                _csv.write2Cell(f.LSTWWD + ",");
                _csv.write2Cell(f.LSTDate + ",");
                _csv.write2Cell(f.LstTime + ",");
                _csv.write2Cell(f.DUEWWD + ",");
                _csv.write2Cell(f.DUEDate + ",");
                _csv.write2Cell(f.DUETime + ",");
                _csv.write2Cell(f.VC + ",");
                _csv.write2Cell(f.Rc + ",");
                _csv.write2Cell(f.Model + ",");
                _csv.write2Cell(f.Modeldesc + ",");
                _csv.write2Cell(f.Unit + ",");
                _csv.write2Cell(f.License + ",");
                _csv.write2Cell(f.SERIAL + ",");
                _csv.write2Cell(f.OP + ",");
                _csv.write2Cell(f.MT + ",");
                _csv.write2Cell(f.NR + ",");
                _csv.write2Cell(f.DOC + ",");
                _csv.write2Cell(f.Driver + ",");
                _csv.write2Cell(f.LSTMLG + ",");
                _csv.write2Cell(f.iDate + ",");
                _csv.write2Cell(f.msoDate + ",");
                _csv.write2Cell(f.capDate + ",");
                _csv.write2Cell(f.ownArea + ",");
                _csv.write2Cell(f.carHold1 + ",");
                _csv.write2Cell(f.bdDays + ",");
                _csv.write2Cell(f.mmDays + ",");
                _csv.write2Cell(f.Remark + ",");
                _csv.write2Cell(f.Resolution + "");
                _csv.newLine();
            }
        }
        
        public event CSVWorker.CSVWorkerEventHandler eventMessage;
        
        private void onEventMessage(CSVWorkerEventArgs e) 
        {
            // Just sends messages of actions
            if (eventMessage != null)
                eventMessage(this, e);
        }
        
        public class CSVWorkerEventArgs : System.EventArgs 
        {
            // Class to handle events
            public string Message { get; set; }
            public CSVWorkerEventArgs(string message) {
                Message = message;
            }
        }
    }
}
