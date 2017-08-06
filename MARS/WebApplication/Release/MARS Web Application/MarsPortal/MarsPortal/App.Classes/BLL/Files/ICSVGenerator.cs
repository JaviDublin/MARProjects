
namespace App.BLL {

    public interface ICSVGenerator {

        string saveAs(string fileName, string filePath);
        void write2Cell(string cell);
        void newLine();
    }
}
