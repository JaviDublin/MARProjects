using System;
using System.Text;
namespace Mars.DAL.Sizing.Management.Abstract {
    public interface IFleetSizeExportRepository {
        StringBuilder GetData(string country,int scenarioID,int locationGroupID,int carClassGroupID,string fromDate,string toDate,bool isAddition);
        String GetCmd(String s);
        void SetCmd(String descriptor,String command);
        Boolean IsUserAdmin(String UserId);
    }
}
