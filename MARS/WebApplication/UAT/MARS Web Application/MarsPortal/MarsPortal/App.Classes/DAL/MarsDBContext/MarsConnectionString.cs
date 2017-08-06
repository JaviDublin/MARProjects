using System.Configuration;

namespace Mars.App.Classes.DAL.MarsDBContext
{
    internal static class MarsConnection
    {
        internal static string ConnectionString =
            ConfigurationManager.ConnectionStrings["RAD.Properties.Settings.ApplicationDataBase"].ConnectionString;
    }
}