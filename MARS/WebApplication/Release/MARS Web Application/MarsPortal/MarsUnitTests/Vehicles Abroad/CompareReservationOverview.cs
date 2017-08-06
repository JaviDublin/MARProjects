using System;
using App.BLL.VehiclesAbroad;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
// added for datatable class
using Mars.App.Classes.DAL.MarsDBContext; // added
using App.Entities.VehiclesAbroad; // added
using App.BLL.VehiclesAbroad.Abstract;






namespace MarsUnitTests.Vehicles_Abroad
{
    [TestClass]
    public class CompareReservationOverview
    {
        [TestMethod]
        public void CompareResOVerview()
        {
            FilterEntity fe = new FilterEntity("", 0, "", "", "", "", "", "");
            fe.ReservationStartDate= new DateTime(2014, 07, 1);
            fe.ReservationEndDate = new DateTime(2014, 07, 3);

            var newData = getNewList(fe);



            var oldData = getOldList(fe);


            //var q2 = oldData.Select(t => t.)
            //              .Except(newQuery.Select(e => e.ResId))
            //              .ToList();

        }






        private IList<IDataTableEntity> getOldList(FilterEntity filters)
        {

            using (MarsDBDataContext db = new MarsDBDataContext())
            {

                IList<IDataTableEntity> l = new List<IDataTableEntity>();

                try
                {  // this is volatile code


                    var q = (from p in db.RESERVATIONS_EUROPE_ACTUALs
                             join tPool in db.CMS_POOLs on p.CMS_POOL equals tPool.cms_pool_id.ToString()
                             join tLoc in db.CMS_LOCATION_GROUPs on p.CMS_LOC_GRP equals tLoc.cms_location_group_id.ToString()
                             join tc1 in db.COUNTRies on p.COUNTRY equals tc1.country1
                             join tc2 in db.COUNTRies on p.RTRN_LOC.Substring(0, 2) equals tc2.country1
                             // ====== the joins for the destination(return) using rtrn_loc
                             join returnLocations in db.LOCATIONs on p.RTRN_LOC equals returnLocations.location1
                             join returnLocationGroups in db.CMS_LOCATION_GROUPs on returnLocations.cms_location_group_id equals returnLocationGroups.cms_location_group_id
                             join returnPools in db.CMS_POOLs on returnLocationGroups.cms_pool_id equals returnPools.cms_pool_id
                             join carSegment in db.CAR_SEGMENTs on p.CAR_SEGMENT equals carSegment.car_segment_id.ToString()
                             join carClass in db.CAR_CLASSes on p.CAR_CLASS equals carClass.car_class_id.ToString()
                             //// ======
                             where
                                       (p.RS_ARRIVAL_DATE >= filters.ReservationStartDate && p.RS_ARRIVAL_DATE <= filters.ReservationEndDate) // reservation start dates
                  &&
                                (p.COUNTRY != returnPools.country) // used the country from the join
                             && (filters.OwnCountry == p.COUNTRY || filters.OwnCountry == "" || filters.OwnCountry == null)
                             && (tPool.cms_pool1 == filters.Pool || filters.Pool == "" || filters.Pool == null)
                             && (tLoc.cms_location_group1 == filters.Location || filters.Location == "" || filters.Location == null)
                             && (p.RES_VEH_CLASS == filters.CarGroup || filters.CarGroup == "" || filters.CarGroup == null)
                             && (tc1.active) // only corporate countries
                             && (filters.CarSegment == carSegment.car_segment1 || string.IsNullOrEmpty(filters.CarSegment))
                             && (filters.CarClass == carClass.car_class1 || string.IsNullOrEmpty(filters.CarClass))
                             && (filters.CarGroup == p.GR_INCL_GOLDUPGR || string.IsNullOrEmpty(filters.CarGroup))
                                 // ====== filtering for the destination(start) location
                             && (returnPools.country.Equals(filters.DueCountry) || string.IsNullOrEmpty(filters.DueCountry))
                             && (returnPools.cms_pool1.Equals(filters.DuePool) || string.IsNullOrEmpty(filters.DuePool))
                             && (returnLocationGroups.cms_location_group1.Equals(filters.DueLocationGroup) || string.IsNullOrEmpty(filters.DueLocationGroup))
                             // ======
                             select new { p.RES_ID_NBR, tc1, tc2 }).Distinct(); // selecting RES_ID_NUMBER is to ensure all entries are unique

                    var grp = from p in q
                              group p by new { o = p.tc1.country_description, d = p.tc2.country_description } into g
                              select new { ownwwd = g.Key.o, duewwd = g.Key.d, sum = g.Key.o.Count() };

                    //var grp = from p in q
                    //          group p by new { o = p.tc1.country_description, d = p.tc2.country_description } into g
                    //          select new { ownwwd = g.Key.o, duewwd = g.Key.d, sum = g.Key.o.Count() };



                         
                    foreach (var item in grp)
                    {
                        l.Add(new DataTableEntity { header = item.ownwwd, rowDefinition = item.duewwd, theValue = item.sum.ToString() });
                    }
                }
                catch
                {
                    // do nothing
                }
                return l;
            }
        }



        private IList<IDataTableEntity> getNewList(FilterEntity filters)
        {

            using (MarsDBDataContext db = new MarsDBDataContext())
            {

                IList<IDataTableEntity> l = new List<IDataTableEntity>();

                try
                {  // this is volatile code




                    var q = (from p in db.Reservations
                             join startloc in db.LOCATIONs on p.RENT_LOC equals startloc.dim_Location_id
                             join startCmsLoc in db.CMS_LOCATION_GROUPs on startloc.cms_location_group_id equals
                                 startCmsLoc.cms_location_group_id
                             join startCmsP in db.CMS_POOLs on startCmsLoc.cms_pool_id equals startCmsP.cms_pool_id
                             join startCtry in db.COUNTRies on startCmsP.country equals startCtry.country1
                             // Return Location
                             join returnloc in db.LOCATIONs on p.RTRN_LOC equals returnloc.dim_Location_id
                             join returnCmsLoc in db.CMS_LOCATION_GROUPs on returnloc.cms_location_group_id equals
                                 returnCmsLoc.cms_location_group_id
                             join returnCmsP in db.CMS_POOLs on returnCmsLoc.cms_pool_id equals returnCmsP.cms_pool_id
                             join returnCtry in db.COUNTRies on returnCmsP.country equals returnCtry.country1
                             // Car details
                             join carGp in db.CAR_GROUPs on p.GR_INCL_GOLDUPGR equals carGp.car_group_id
                             join carCs in db.CAR_CLASSes on carGp.car_class_id equals carCs.car_class_id
                             join carS in db.CAR_SEGMENTs on carCs.car_segment_id equals carS.car_segment_id
                             // ======
                             where
                                 (p.RS_ARRIVAL_DATE >= filters.ReservationStartDate &&
                                  p.RS_ARRIVAL_DATE <= filters.ReservationEndDate) // reservation start dates
                                  &&
                                 (p.COUNTRY != returnCmsP.country) // used the country from the join
                                 &&
                                 (filters.OwnCountry == p.COUNTRY || filters.OwnCountry == "" || filters.OwnCountry == null)
                                 && (startCmsP.cms_pool1 == filters.Pool || filters.Pool == "" || filters.Pool == null)
                                 &&
                                 (startCmsLoc.cms_location_group1 == filters.Location || filters.Location == "" ||
                                  filters.Location == null)
                                 &&
                                 (p.ReservedCarGroup == filters.CarGroup || filters.CarGroup == "" ||
                                  filters.CarGroup == null)
                                 && (startCtry.active) // only corporate countries
                                 && (filters.CarSegment == carS.car_segment1 || string.IsNullOrEmpty(filters.CarSegment))
                                 && (filters.CarClass == carCs.car_class1 || string.IsNullOrEmpty(filters.CarClass))
                                 && (filters.CarGroup == carGp.car_group1 || string.IsNullOrEmpty(filters.CarGroup))

                                 // ====== filtering for the destination(start) location
                                 &&
                                 (returnCmsP.country.Equals(filters.DueCountry) || string.IsNullOrEmpty(filters.DueCountry))
                                 && (returnCmsP.cms_pool1.Equals(filters.DuePool) || string.IsNullOrEmpty(filters.DuePool))
                                 &&
                                 (returnCmsLoc.cms_location_group1.Equals(filters.DueLocationGroup) ||
                                  string.IsNullOrEmpty(filters.DueLocationGroup))
                             // ======
                             select new { p.RES_ID_NBR, startCtry, returnCtry }).Distinct(); // selecting RES_ID_NUMBER is to ensure all entries are unique

                    var grp = from p in q
                              group p by new { o = p.startCtry.country_description, d = p.returnCtry.country_description } into g
                              select new { ownwwd = g.Key.o, duewwd = g.Key.d, sum = g.Key.o.Count() };

                    foreach (var item in grp)
                    {
                        l.Add(new DataTableEntity { header = item.ownwwd, rowDefinition = item.duewwd, theValue = item.sum.ToString() });
                    }
                }
                catch
                {
                    // do nothing
                }
                return l;
            }
        }


    }
}
