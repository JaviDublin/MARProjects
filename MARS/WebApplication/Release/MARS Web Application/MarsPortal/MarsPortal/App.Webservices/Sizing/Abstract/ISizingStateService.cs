using System;
using Mars.DAL.Sizing.Abstract;
namespace Mars.Webservices.Sizing.Abstract {
    public interface ISizingStateService {
        String GetUpdate();
        String RunStoredProcedure();
    }
}
