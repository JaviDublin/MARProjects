using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using App.Classes.DAL.Pooling.Abstract;
using App.Classes.Entities.Pooling.Abstract;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.DAL.Pooling.PoolingDataContext;
using Mars.DAL.Pooling.Queryables;
using Mars.DAL.Reservations.Queryables;
using Mars.Entities.Pooling;

namespace Mars.App.Classes.DAL.Pooling.SharedDataAccess
{
    public static class ReservationsDataAccess
    {
        public static string LookupGoldLevel(string resIdNumber)
        {
            string returned;
            using (var db = new PoolingDataClassesDataContext())
            {
                var data = from r in db.Reservations
                    join rlp in db.ResLoyaltyPrograms on r.N1TYPE equals rlp.N1Type
                    where r.RES_ID_NBR == resIdNumber
                    select rlp.ResGoldLevel.GoldLevelName;
                returned = data.FirstOrDefault();
            }
            return returned;
        }

        public static List<DayActualEntity> CalculateTopics(bool hourlyTimeSlots, int timeSlots, IMainFilterEntity mfe
                                                    , bool withLabels, bool siteQ = false)
        {
            var returned = new List<DayActualEntity>();
            List<DayActualEntity> feaData, adjustments, checkInData, checkOutData;
            var bufferData = new List<DayActualEntity>();
            int buffer = 0;
            using (var db = new MarsDBDataContext())
            {
                db.Log = new DebugTextWriter();
                
                feaData = withLabels
                            ? DayActualEntityRetrievers.GetFeaDataWithLabel(mfe, hourlyTimeSlots, db, siteQ)
                            : DayActualEntityRetrievers.GetFeaData(mfe, hourlyTimeSlots, db);


                adjustments = withLabels
                            ? DayActualEntityRetrievers.GetAdditionDeletionDataWithLabels(mfe, hourlyTimeSlots, db, siteQ)
                            : DayActualEntityRetrievers.GetCurrentAdditionDeletion(mfe, hourlyTimeSlots, db);
                if (withLabels)
                {
                    bufferData = DayActualEntityRetrievers.GetBuffersWithLabels(mfe, db, siteQ);
                }
                else
                {
                    buffer = DayActualEntityRetrievers.GetBuffers(mfe, db);
                }
                
            }
            using (var db = new PoolingDataClassesDataContext())
            {
                checkInData = withLabels
                            ? DayActualEntityRetrievers.GetPoolingCheckInDataWithLabels(mfe, hourlyTimeSlots, db, siteQ)
                            : DayActualEntityRetrievers.GetPoolingCheckInData(mfe, hourlyTimeSlots, db);
                checkOutData = withLabels
                            ? DayActualEntityRetrievers.GetPoolingCheckOutDataWithLabels(mfe, hourlyTimeSlots, db, siteQ)
                            : DayActualEntityRetrievers.GetPoolingCheckOutData(mfe, hourlyTimeSlots, db);

            }

            var emptyHolder = new DayActualEntity();


            var checkInLocations = checkInData.Select(d => d.Label).Distinct().ToList();
            var checkOutLocations = checkInData.Select(d => d.Label).Distinct().ToList();
            var labels = withLabels ? GetLabels(mfe, siteQ) : new List<string> { null };
            if (withLabels)
            {
                labels.AddRange(checkInLocations);
                labels.AddRange(checkOutLocations);
                labels = labels.Distinct().ToList();
            }
            
            for (var i = 0; i < timeSlots; i++)
            {

                foreach (var lbl in labels)
                {

                    var fea = feaData.FirstOrDefault(d => d.Tme == i && (d.Label == lbl)) ?? emptyHolder;
                    var adj = adjustments.FirstOrDefault(d => d.Tme == i && (d.Label == lbl)) ?? emptyHolder;
                    
                    var ci = checkInData.FirstOrDefault(d => d.Tme == i && (d.Label == lbl)) ?? emptyHolder;
                    var co = checkOutData.FirstOrDefault(d => d.Tme == i && (d.Label == lbl)) ?? emptyHolder;
                    var bufferHolder = bufferData.FirstOrDefault(d => d.Label == lbl) ?? emptyHolder;
                    if (withLabels)
                    {
                        buffer = bufferHolder.Buffer;
                    }
                    var previousBalance = returned.FirstOrDefault(d => d.Tme == i - 1 && (d.Label == lbl));
                    var previousBalanceInt = previousBalance == null ? 0 : previousBalance.Balance;


                    var available = i == 0
                        ? fea.Available + adj.AddditionDeletion
                        : previousBalanceInt + buffer;



                    var balance = available
                                  //+ fea.Opentrips
                                  - co.Reservations + (fea.Offset + ci.Offset)
                                  - buffer + adj.AddditionDeletion;

                    var dae = new DayActualEntity
                              {
                                  Tme = i,
                                  Label = lbl,
                                  Available = available,
                                  Opentrips = fea.Opentrips,
                                  Reservations = co.Reservations,
                                  OnewayRes = co.OnewayRes,
                                  GoldServiceReservations = co.GoldServiceReservations,
                                  PrepaidReservations = 0,// co.PrepaidReservations,
                                  Checkin = fea.Checkin + ci.Checkin,
                                  OnewayCheckin = fea.OnewayCheckin+ ci.OnewayCheckin,
                                  Offset = fea.Offset + ci.Offset,
                                  LocalCheckIn = fea.LocalCheckIn,// + ci.LocalCheckIn,
                                  Balance = balance,
                                  Buffer = buffer,
                                  AddditionDeletion = adj.AddditionDeletion,
                                  JustAdditions = adj.JustAdditions,
                                  JustDeletions = adj.JustDeletions,

                              };
                    returned.Add(dae);
                    
                    if (string.IsNullOrEmpty(lbl)) break;
                }

            }


            return returned;


        }



        private static List<string> GetLabels(IMainFilterEntity mfe, bool siteQ = false)
        {
            using (var db = new MarsDBDataContext())
            {
                if (string.IsNullOrEmpty(mfe.Country))
                {

                    var countries = from c in db.COUNTRies
                                    where c.active
                                    select c.country_description;
                    return countries.ToList();


                }

                if (siteQ)
                {
                    if (string.IsNullOrEmpty(mfe.PoolRegion))
                    {
                        if (mfe.CmsLogic)
                        {
                            var pools = from p in db.CMS_POOLs
                                where p.COUNTRy1.country_description == mfe.Country
                                select p.cms_pool1;
                            return pools.ToList();
                        }

                        var regions = from r in db.OPS_REGIONs
                            where r.COUNTRy1.country_description == mfe.Country
                            select r.ops_region1;
                        return regions.ToList();
                    }

                    if (string.IsNullOrEmpty(mfe.LocationGrpArea))
                    {
                        if (mfe.CmsLogic)
                        {
                            var lg = from l in db.CMS_LOCATION_GROUPs
                                where l.CMS_POOL.cms_pool1 == mfe.PoolRegion
                                      && l.CMS_POOL.COUNTRy1.country_description == mfe.Country
                                select l.cms_location_group1;
                            return lg.ToList();
                        }

                        var area = from a in db.OPS_AREAs
                            where a.OPS_REGION.ops_region1 == mfe.PoolRegion
                                  && a.OPS_REGION.COUNTRy1.country_description == mfe.Country
                            select a.ops_area1;
                        return area.ToList();
                    }

                    if (string.IsNullOrEmpty(mfe.Branch))
                    {
                        if (mfe.CmsLogic)
                        {
                            var branches = from b in db.LOCATIONs
                                where b.CMS_LOCATION_GROUP.cms_location_group1 == mfe.LocationGrpArea
                                      && b.CMS_LOCATION_GROUP.CMS_POOL.cms_pool1 == mfe.PoolRegion
                                      && b.COUNTRy1.country_description == mfe.Country
                                      && b.active
                                select b.location1;
                            return branches.ToList();
                        }
                        var branchesOps = from b in db.LOCATIONs
                                       where b.OPS_AREA.ops_area1 == mfe.LocationGrpArea
                                             && b.OPS_AREA.OPS_REGION.ops_region1 == mfe.PoolRegion
                                             && b.COUNTRy1.country_description == mfe.Country
                                             && b.active
                                       select b.location1;
                        return branchesOps.ToList();
                        
                    }
                }
                if (string.IsNullOrEmpty(mfe.CarSegment))
                {
                    var segment = from cs in db.CAR_SEGMENTs
                        where cs.COUNTRy1.country_description == mfe.Country
                        select cs.car_segment1;
                    return segment.ToList();

                }
                if (string.IsNullOrEmpty(mfe.CarClass))
                {
                    var carClasses = from cc in db.CAR_CLASSes
                        where cc.CAR_SEGMENT.car_segment1 == mfe.CarSegment
                              && cc.CAR_SEGMENT.COUNTRy1.country_description == mfe.Country
                        orderby cc.sort_car_class
                        select cc.car_class1;
                        
                    return carClasses.ToList();
                }

                if (string.IsNullOrEmpty(mfe.CarGroup))
                {
                    var carGroups = from cg in db.CAR_GROUPs
                        where cg.CAR_CLASS.car_class1 == mfe.CarClass
                              && cg.CAR_CLASS.CAR_SEGMENT.car_segment1 == mfe.CarSegment
                              && cg.CAR_CLASS.CAR_SEGMENT.COUNTRy1.country_description == mfe.Country
                              orderby cg.sort_car_group
                        select cg.car_group1;
                    
                    return carGroups.ToList();
                }
            }
            return null;
        }


    }
}