using System;
using System.Text;
using System.IO;

namespace App.BLL {

    public class CSVGenerator : ICSVGenerator {
        // Created: 10/4/12

        private readonly StringBuilder _sb;

        public CSVGenerator() {

            _sb = new StringBuilder();
        }

        /// <summary>
        /// Save the document with the date in the filePath folder and returns the file name for the download
        /// Also closes down the doc and release the resources into the wild
        /// </summary>
        /// <param name="name"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string saveAs(string name, string filePath) {

            string _time = DateTime.Now.ToLongDateString().Replace(' ', '_') + "_" + DateTime.Now.ToLongTimeString().Replace(':', '_');
            if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath); // Create site directory if it doesn't exist
            string fileName = name + "_" + _time + ".csv"; // Slightly different for Web
            using (StreamWriter outfile = new StreamWriter(filePath + fileName)) {
                try {
                    outfile.Write(_sb.ToString());
                }
                catch {
                    fileName = "FileError";
                }
            }
            return fileName;
        }
        public void write2Cell(string cell) {
            _sb.Append(cell);
        }
        public void newLine() {
            _sb.AppendLine();
        }
    }
}
