namespace App.BLL.EventArgs
{
    public class ExportToExcelEventArgs : System.EventArgs
    {
        public bool isTextExport;
        public bool isAddition;
        public int scenarioType;
    }
}