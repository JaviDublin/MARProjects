﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.Core.Internal;
using Mars.FleetAllocation.DataAccess;
using Mars.FleetAllocation.DataAccess.AdditionsLimits.Entities;
using Mars.FleetAllocation.DataAccess.Entities;
using Mars.FleetAllocation.DataAccess.Entities.Output;


namespace Mars.FleetAllocation.BusinessLogic
{
    public static class DemandGapCalculations
    {

        public static void CalculateGap(List<DemandGapOneRow> demandGapData)
        {
            if (demandGapData.Count == 0) return;
            var minWeek = demandGapData.Min(d => d.WeekNumber);
            foreach (var dg in demandGapData.OrderBy(d=> d.WeekNumber))
            {
                

                var currentCarGroup = dg.GetCarGroupId();
                var currentLocationId = dg.GetLocationId();
                var currentWeekNumber = dg.WeekNumber;
                int i = 1;
                DemandGapOneRow previousDg = null;
                while (previousDg == null && dg.WeekNumber - i > minWeek)
                {
                    previousDg = demandGapData.FirstOrDefault(d => d.GetCarGroupId() == currentCarGroup
                        && d.GetLocationId() == currentLocationId
                        && d.WeekNumber == currentWeekNumber - i);
                    i++;
                }
                if (previousDg == null)
                {
                    dg.CumulativeAddition = dg.AdditionDeletionSum;
                }
                else
                {
                    dg.CumulativeAddition = previousDg.CumulativeAddition + dg.AdditionDeletionSum;
                }
                    
                
                
            }
        }

        public static List<WeeklyAddition> AssignGroupOne(List<DemandGapOneRow> demandGapData,
                                List<MonthlyLimitRow> monthlyLimitByCarGroup,
                                List<WeeklyLimitRow> weeklyLimitBySegment)
        {
            var calculatedAdditions = new List<WeeklyAddition>();

            foreach (var weeklyLimit in weeklyLimitBySegment.OrderBy(d=> d.Week))
            {
                
                var weekNumber = weeklyLimit.Week;
                var year = weeklyLimit.Year;
                var carSegmentId = weeklyLimit.GetCarSegmentId();

                var monthNumber = weeklyLimit.GetMonth();
                var yearNumber = weeklyLimit.Year;
                var gapsForWeek = demandGapData.Where(d => d.WeekNumber == weekNumber 
                                            && d.GetCarSegmentId() == carSegmentId
                                            );
                
                //Loop through all Car Groups that need filling
                foreach (var dg in gapsForWeek)
                {
                    if (dg.MissingVehicles >= 0) continue;

                    var groupToAdd = dg.GetCarGroupId();
                    var locationToAddTo = dg.GetLocationId();

                    //Do we have a corresponding CarGroup entry in the MonthlyLimit File
                    var monthlyLimit = monthlyLimitByCarGroup.FirstOrDefault(d => d.GetCarGroupId() == groupToAdd
                                                                            && d.GetMonth() == monthNumber);

                    //Is there an Addition in the Monthly file
                    if (monthlyLimit == null) continue;

                    int amountToAdd = Math.Abs(dg.MissingVehicles);

                    if (monthlyLimit.AdditionsLimit == monthlyLimit.Assigned)
                    {
                        dg.ReasonForGap = "M";
                        continue;
                    }

                    if (monthlyLimit.AdditionsLimit < monthlyLimit.Assigned + amountToAdd)
                    {
                        amountToAdd = monthlyLimit.AdditionsLimit - monthlyLimit.Assigned;
                        dg.ReasonForGap = "M";
                    }
                    

                    if (weeklyLimit.AdditionsLimit < weeklyLimit.Assigned + amountToAdd)
                    {
                        dg.ReasonForGap += "W";
                        if ((weeklyLimit.AdditionsLimit - weeklyLimit.Assigned) < amountToAdd)
                        {
                            amountToAdd = weeklyLimit.AdditionsLimit - weeklyLimit.Assigned;    
                        }
                        
                    }
                    if (amountToAdd == 0) continue;

                    
                    demandGapData.Where(d => (d.WeekNumber >= weekNumber || d.Year > yearNumber)
                                            && d.GetCarGroupId() == groupToAdd).ForEach(d => d.AddVehicles(amountToAdd));

                    //Add to Car Segment Limit
                    weeklyLimit.Assigned += amountToAdd;
                    //Add to Car Group Limit
                    monthlyLimit.Assigned += amountToAdd;

                    calculatedAdditions.Add(new WeeklyAddition
                    {
                        CarGroupId = groupToAdd,
                        LocationId = locationToAddTo,
                        IsoWeek = weekNumber,
                        Amount = amountToAdd,
                        Year = year
                    });
                }

            }

            return calculatedAdditions;
        }

        public static void FillGapsFromMonthlyLimits(List<DemandGapOneRow> gaps,
                                List<MonthlyLimitRow> monthlyLimitByCarGroup,
                                List<WeeklyLimitRow> weeklyLimitBySegment,
                                List<WeeklyAddition> generatedAdditions)
        {
            var gapsFromMonthlyLimits = gaps.Where(d => d.ReasonForGap.StartsWith("M")).ToList();

            if (!gapsFromMonthlyLimits.Any()) return;

            using (var lookup = new LookupDataAccess())
            {
                foreach (var dgor in gapsFromMonthlyLimits)
                {
                    
                    var carGroupLimited = dgor.GetCarGroupId();
                    var carSegmentLimited = dgor.GetCarSegmentId();
                    var weekLimitedOn = dgor.WeekNumber;
                    var monthLimitedOn = dgor.GetMonth();
                    var yearLimitedOn = dgor.Year;
                    var locationId = dgor.GetLocationId();
                    var totalDesired = Math.Abs(dgor.MissingVehicles);

                    var limitForThisWeek =
                        weeklyLimitBySegment.FirstOrDefault(d => d.GetCarSegmentId() == carSegmentLimited 
                                                    && d.Week == weekLimitedOn
                                                    && d.Year == yearLimitedOn);

                    if (limitForThisWeek == null) continue;
                    //If the limit for this week has already been reached
                    if (limitForThisWeek.AdditionsLimit == limitForThisWeek.Assigned)
                    {
                        continue;
                    }
                    //Reduce the amount needed to match the weekly max
                    if (limitForThisWeek.Assigned + totalDesired > limitForThisWeek.AdditionsLimit)
                    {
                        totalDesired = limitForThisWeek.AdditionsLimit - limitForThisWeek.Assigned;
                    }

                    var replacementCarGroups = lookup.FindCarGroupsInSameCarClass(carGroupLimited);

                    //Total amount tracks the amount to give in case it's spread across several car groups
                    var totalAmountToGive = totalDesired;

                    foreach (var rcg in replacementCarGroups)
                    {
                        if (totalAmountToGive == 0) break;
                        int whatCanBeGiven;
                        var carGroupId = rcg;
                        var limitForMonth = monthlyLimitByCarGroup.FirstOrDefault(d => 
                                                                d.GetMonth() == monthLimitedOn
                                                                && d.Year == yearLimitedOn
                                                                && d.GetCarGroupId() == carGroupId);
                        if (limitForMonth == null) continue;

                        //Limit already reached this month for this Car Group
                        if(limitForMonth.AdditionsLimit == limitForMonth.Assigned) continue;

                        if (limitForMonth.Assigned + totalAmountToGive > limitForMonth.AdditionsLimit)
                        {
                            whatCanBeGiven = limitForMonth.AdditionsLimit - limitForMonth.Assigned;
                            totalAmountToGive -= whatCanBeGiven;
                        }
                        else
                        {
                            whatCanBeGiven = totalAmountToGive;
                            totalAmountToGive = 0;
                        }

                        //Increment Weeky, monthly and Output list
                        limitForMonth.Assigned += whatCanBeGiven;
                        limitForThisWeek.Assigned += whatCanBeGiven;
                        dgor.AddVehicles(whatCanBeGiven);

                        if (generatedAdditions.Any(d => d.CarGroupId == carGroupId
                                                        && d.LocationId == locationId
                                                        && d.IsoWeek == weekLimitedOn
                                                        && d.Year == yearLimitedOn))
                        {
                            var entityToUpdate = generatedAdditions.First(d => d.CarGroupId == carGroupId
                                                                               && d.LocationId == locationId
                                                                               && d.IsoWeek == weekLimitedOn
                                                                               && d.Year == yearLimitedOn);
                            entityToUpdate.Amount += whatCanBeGiven;
                        }
                        else
                        {
                            generatedAdditions.Add(new WeeklyAddition
                            {
                                LocationId = locationId,
                                CarGroupId = carGroupId,
                                IsoWeek = weekLimitedOn,
                                Year = yearLimitedOn,
                                Amount = whatCanBeGiven
                            });    
                        }

                        

                    }


                }    

            }
            

        }
    }
}