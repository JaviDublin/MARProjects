﻿using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Mars.FleetAllocation.BulkInserts
{
    public static class DataTableHelper
    {
        // Helper function for ADO.Net Bulkcopy to transfer a IEnumerable list to a datatable
        // Adapted from: http://msdn.microsoft.com/en-us/library/bb396189.aspx
        public static DataTable CopyToDataTable<T>(this IEnumerable<T> source)
        {
            return new DataTableCreator<T>().CreateDataTable(source, null, null);
        }

        public static DataTable CopyToDataTable<T>(this IEnumerable<T> source, DataTable table, LoadOption? options)
        {
            return new DataTableCreator<T>().CreateDataTable(source, table, options);
        }


        public static void BulkCopyToDatabase<T>(this IEnumerable<T> source
                    , string tableName, System.Data.Linq.DataContext databaseContext
                    , string schema = "dbo"
                    ) where T : class
        {
            using (var dataTable = CopyToDataTable(source))
            {
                using (var bulkCopy = new SqlBulkCopy(ConfigurationManager.ConnectionStrings["RAD.Properties.Settings.ApplicationDataBase"].ConnectionString))
                {
                    foreach (DataColumn dc in dataTable.Columns)
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(dc.ColumnName, dc.ColumnName));

                    //  We could use "dataTable.TableName" in the following line, but this does sometimes have problems, as 
                    //  LINQ-to-SQL will drop trailing "s" off table names, so try to insert into [Product], rather than [Products]
                    bulkCopy.DestinationTableName = string.Format("{0}.[{1}]", schema, tableName);  
                    bulkCopy.WriteToServer(dataTable);
                }
            }
        }
    }
}
