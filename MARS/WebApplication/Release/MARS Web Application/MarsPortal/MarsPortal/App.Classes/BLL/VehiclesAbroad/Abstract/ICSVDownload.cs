using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.BLL.VehiclesAbroad.Abstract {

    interface ICSVDownload<T> {

        /// <summary>
        /// Implement a method to download a CSV file
        /// </summary>
        /// <param name="fileName">the name of the file</param>
        /// <param name="path">the path to save the file</param>
        /// <returns>true if successful</returns>
        bool downloadFile(string path, string fileName);

        /// <summary>
        /// the filename created (can include the path)
        /// </summary>
        string FileName { get; }

        /// <summary>
        /// the list to be rendered to the the file
        /// the list will need to be cast eg (IEnumerable<MyType>)csvList
        /// </summary>
        IEnumerable<T> csvList { get; set; }
    }
}
