using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using App.BLL.ExtensionMethods;
using App.Classes.Entities.Pooling.Abstract;
using Mars.App.Classes.BLL.ExtensionMethods;
using Mars.App.Classes.DAL.MarsDBContext;
using Mars.App.Classes.DAL.Pooling.PoolingDataContext;
using Mars.Entities.Pooling;

namespace Mars.App.Classes.DAL.Pooling.SharedDataAccess
{
    public static class AlertsDataAccess
    {

        public static List<DayActualEntity> GetLocationCarGroupsWithNegativeBalance(IMainFilterEntity mfe, DateTime toDate)
        {
            var allgroups = GetLocationCarGroups(mfe, toDate);
            allgroups.RemoveAll(d => d.Balance >= 0);

            var onlyLowestGroups = new List<DayActualEntity>();
            onlyLowestGroups.AddRange(allgroups.Where(d=> d.Tme == 0));

            var next4Hours = allgroups.Where(d => d.Tme > 0 && d.Tme < 5).ToList();
            var labels = next4Hours.Select(d => d.Label).Distinct();
            onlyLowestGroups.AddRange(labels.Select(lbl => next4Hours.Where(d => d.Label == lbl).MinBy(d => d.Balance)));

            var now = DateTime.Now.GetDateAndHourOnlyByCountry(mfe.Country);

            //entityToAdd.Tme >= 5 && now.AddHours(entityToAdd.Tme).Day == now.Day
            var restOfDay = allgroups.Where(d => d.Tme >= 5 && now.AddHours(d.Tme).Day == now.Day).ToList();
            labels = restOfDay.Select(d => d.Label).Distinct();
            onlyLowestGroups.AddRange(labels.Select(lbl => restOfDay.Where(d => d.Label == lbl).MinBy(d => d.Balance)));

            var futureDays = allgroups.Where(d => now.AddHours(d.Tme).Day != now.Day).ToList();
            labels = futureDays.Select(d => d.Label).Distinct();
            onlyLowestGroups.AddRange(labels.Select(lbl => futureDays.Where(d => d.Label == lbl).MinBy(d => d.Balance)));

            return onlyLowestGroups;
        }

        public static List<DayActualEntity> GetLocationCarGroups(IMainFilterEntity mfe, DateTime toDate)
        {
            List<DayActualEntity> feaData, adjustments, buffers, reservationsCheckIn, reservationsCheckOut, emptyHolders;
            //var sw = new Stopwatch();
            //sw.Start();


            using (var db = new MarsDBDataContext())
            {
                emptyHolders = DayActualEntityRetrievers.GetPoolingEmptyAlertsHolders(db, mfe);
                buffers = DayActualEntityRetrievers.GetBuffersForAlerts(mfe, db);
                adjustments = DayActualEntityRetrievers.GetCurrentAdditionDeletion(mfe, true, db, true);
                feaData = DayActualEntityRetrievers.GetFeaFullForAlerts(mfe, db).ToList();
            }

            using (var db = new PoolingDataClassesDataContext())
            {
                
                reservationsCheckIn = DayActualEntityRetrievers.GetPoolingCheckInDataForAlerts(db, mfe);
                reservationsCheckOut = DayActualEntityRetrievers.GetPoolingCheckOutDataForAlerts(db, mfe);
                
            }

            var alertsList = new List<DayActualEntity>();
            var now = DateTime.Now.GetDateAndHourOnlyByCountry(mfe.Country);
            var hourUntilEndOfPeriod = (toDate - now).TotalHours;


            for (int i = 0; i < hourUntilEndOfPeriod; i++)
            {
                int timeSlot = i;
                var feaD = feaData.AsParallel().Where(d => d.Tme == timeSlot).OrderBy(d => d.Label).ToList();
                var bufferD = buffers.AsParallel().OrderBy(d => d.Label).ToList();
                var adjustmentD = adjustments.AsParallel().Where(d => d.Tme == timeSlot).OrderBy(d => d.Label).ToList();
                var resInD = reservationsCheckIn.AsParallel().Where(d => d.Tme == timeSlot).OrderBy(d => d.Label).ToList();
                var resOutD = reservationsCheckOut.AsParallel().Where(d => d.Tme == timeSlot).OrderBy(d => d.Label).ToList();

                foreach (var d in emptyHolders)
                {

                    //var feaEntity = d;
                    var feaEntity = feaD.FirstOrDefault(f => f.Label == d.Label);
                    var bufferEntity = bufferD.FirstOrDefault(f => f.Label == d.Label);
                    var adjustmentEntity = adjustmentD.FirstOrDefault(f => f.Label == d.Label);
                    var resCheckInsEntity = resInD.FirstOrDefault(f => f.Label == d.Label);
                    var resCheckOutsEntity = resOutD.FirstOrDefault(f => f.Label == d.Label);

                    DayActualEntity entityToAdd;

                    var lbl = d.Label;
                    var previousBalance = alertsList.Where(a => a.Label == lbl).OrderByDescending(a => a.Tme).FirstOrDefault();
                    var previousBalanceInt = previousBalance == null ? 0 : previousBalance.Balance;

                    if (feaEntity == null && bufferEntity == null
                        && adjustmentEntity == null && resCheckInsEntity == null && resCheckOutsEntity == null)
                    {
                        if (previousBalanceInt != 0)
                        {
                            entityToAdd = new DayActualEntity
                            {
                                Tme = i,
                                Label = d.Label,
                                Balance = previousBalanceInt
                            };

                            alertsList.Add(entityToAdd);
                        }
                        continue;
                    }



                    var feaAvailable = feaEntity == null ? 0 : feaEntity.Available;
                    var feaOffset = feaEntity == null ? 0 : feaEntity.Offset;
                    var adjustment = adjustmentEntity == null ? 0 : adjustmentEntity.AddditionDeletion;
                    var buffer = bufferEntity == null ? 0 : bufferEntity.Buffer;
                    var reservations = resCheckOutsEntity == null ? 0 : resCheckOutsEntity.Reservations;
                    var resCheckInOffset = resCheckInsEntity == null ? 0 : resCheckInsEntity.Offset;

                    


                    var available = i == 0
                        ? feaAvailable + adjustment
                        : previousBalanceInt + buffer;

                    var balance = available
                                  - reservations + (feaOffset + resCheckInOffset)
                                  - buffer + adjustment;

                    entityToAdd = new DayActualEntity
                                      {
                                          Tme = i,
                                          Label = d.Label,
                                          Balance = balance
                                      };

                    alertsList.Add(entityToAdd);

                    //if (balance < 0)
                    //{
                    //    if (i == 0)
                    //    {
                    //        alertsList.Add(entityToAdd);
                    //    }
                    //    else
                    //    {
                    //        OnlyAddNewOrHigherEntities(alertsList, entityToAdd, now);
                    //    }
                    //}

                }
                
            }

            
            return alertsList;
        }

        private static void OnlyAddNewOrHigherEntities(List<DayActualEntity> fullList, DayActualEntity entityToAdd, DateTime now)
        {
            if (entityToAdd.Tme > 0 && entityToAdd.Tme < 5)
            {
                if (fullList.Any(d => d.Label == entityToAdd.Label && d.Tme > 0 && d.Tme < 5))
                {
                    var holder = fullList.FirstOrDefault(d => d.Label == entityToAdd.Label
                                                              && d.Tme > 0 && d.Tme < 5
                                                              && entityToAdd.Balance < d.Balance);
                    if (holder != null)
                    {
                        holder.Balance = entityToAdd.Balance;
                        holder.Tme = entityToAdd.Tme;
                    }
                }
                else
                {
                    fullList.Add(entityToAdd);
                }
                return;
            }

            if (entityToAdd.Tme >= 5 && now.AddHours(entityToAdd.Tme).Day == now.Day)
            {
                if (fullList.Any(d => d.Label == entityToAdd.Label && d.Tme >= 5 && now.AddHours(d.Tme).Day == now.Day))
                {
                    var holder = fullList.FirstOrDefault(d => d.Label == entityToAdd.Label && entityToAdd.Tme >= 5
                                                              && now.AddHours(entityToAdd.Tme).Day == now.Day
                                                              && entityToAdd.Balance < d.Balance);
                    if (holder != null)
                    {
                        holder.Balance = entityToAdd.Balance;
                        holder.Tme = entityToAdd.Tme;
                    }
                }
                else
                {
                    fullList.Add(entityToAdd);
                }
                return;
            }

            if (fullList.Any(d => d.Label == entityToAdd.Label) && now.AddHours(entityToAdd.Tme).Day != now.Day)
            {
                var holder = fullList.FirstOrDefault(d => d.Label == entityToAdd.Label
                                               && now.AddHours(d.Tme).Day != now.Day
                                               && entityToAdd.Balance < d.Balance);
                if (holder != null)
                {
                    holder.Balance = entityToAdd.Balance;
                    holder.Tme = entityToAdd.Tme;
                }
            }
            else
            {
                fullList.Add(entityToAdd);
            }
            
        }
    }
}