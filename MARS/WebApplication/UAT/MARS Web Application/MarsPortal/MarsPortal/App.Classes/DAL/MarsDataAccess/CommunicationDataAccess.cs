using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.Windsor.Installer;
using Mars.App.Classes.DAL.MarsDBContext;

namespace Mars.App.Classes.DAL.MarsDataAccess
{
    public static class CommunicationDataAccess
    {
        public static List<Document> GetAllDocuments()
        {
            List<Document> returned;
            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                var documents = from doc in dataContext.Documents
                    where doc.Active
                    orderby doc.OrderNumber
                    select doc;
                returned = documents.ToList();
            }
            return returned;
        }

        public static IEnumerable<Communication> GetAllActiveNewsItems()
        {
            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                var com =
                    from cm in dataContext.Communications
                    where cm.IsActive == true
                    orderby cm.Priority descending, cm.CommDate descending
                    select cm;

                return com.ToList();

            }
        }

        public static IEnumerable<Communication> GetAllNewsItems()
        {
            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                var com =
                    from cm in dataContext.Communications
                    
                    orderby cm.Priority descending, cm.CommDate descending
                    select cm;

                return com.ToList();

            }
        }

        public static void SaveNews(string userId, string heading, string details, bool Active, bool Priority)
        {
            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                var spa = new Communication
                {
                    UpdatedBy = userId,
                    CommDate = DateTime.Now,
                    Heading = heading,
                    Details = details,
                    IsActive = Active,
                    Priority = Priority
                };
                dataContext.Communications.InsertOnSubmit(spa);
                dataContext.SubmitChanges();
            }
        }

        public static void UpdateNews(int commsID, string userId, string heading, string details, bool Active, bool Priority)
        {

            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                var commEntry = dataContext.Communications.FirstOrDefault(d => d.CommunicationsID == commsID);
                if (commEntry == null)
                {
                    throw new Exception("Invalid primary key passed to Update News");
                }

                commEntry.CommunicationsID = commsID;
                commEntry.CommDate = DateTime.Now;
                commEntry.UpdatedBy = userId;
                commEntry.Heading = heading;
                commEntry.Details = details;
                commEntry.IsActive = Active;
                commEntry.Priority = Priority;

                dataContext.SubmitChanges();
            }
        }

        public static Communication SelectNewsItem(int commsID)
        {

            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {
                var commEntry = dataContext.Communications.FirstOrDefault(d => d.CommunicationsID == commsID);
                if (commEntry == null)
                {

                    throw new Exception("Invalid primary key passed to Select News");
                }

                return commEntry;

            }
        }

        public static void DeleteNewsItem(int commsID)
        {

            using (var dataContext = new MarsDBDataContext(MarsConnection.ConnectionString))
            {

                var deleteDetails =
                 from details in dataContext.Communications
                 where details.CommunicationsID == commsID
                 select details;

                foreach (var detail in deleteDetails)
                {
                    dataContext.Communications.DeleteOnSubmit(detail);
                }

                try
                {
                    dataContext.SubmitChanges();
                }
                catch
                {

                    throw new Exception("Invalid primary key passed to Delete News");
                }

            }
        }

            

    



    }

}