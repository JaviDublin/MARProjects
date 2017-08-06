using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mars.FleetAllocation.DataAccess.CurrencyConversion.Entities;
using Mars.FleetAllocation.DataContext;

namespace Mars.FleetAllocation.DataAccess.CurrencyConversion
{
    public class CurrencyConversionDataAccess : BaseDataAccess
    {
        public List<UsdEuroConversionRow> GetConversionRates(int historyYears, int futureYears)
        {
            var currentYear = DateTime.Now.Year;

            var conversionRates = from cr in DataContext.CurrencyUsdRates
                where cr.Year >= (currentYear - historyYears)
                      && cr.Year <= (currentYear + futureYears)
                    select new UsdEuroConversionRow
                            {
                                Year = cr.Year,
                                EuroRate = cr.Eur,
                                GbpRate = cr.Gbp,
                                EditYearParameter = cr.Year
                            };

            var localConversionRates = conversionRates.ToList();

            for (int i = currentYear + futureYears; i >= currentYear - historyYears - 1; i--)
            {
                if (localConversionRates.All(d => d.Year != i))
                {
                    localConversionRates.Add(new UsdEuroConversionRow
                                             {
                                                 Year = i,
                                                 EuroRate = 1,
                                                 GbpRate = 1,
                                                 EditYearParameter = i
                                             });
                }
            }

            return localConversionRates;
        }

        public UsdEuroConversionRow GetRateByYear(int year)
        {
            var rates = DataContext.CurrencyUsdRates.SingleOrDefault(d => d.Year == year);
            UsdEuroConversionRow returned;
            if (rates == null)
            {
                returned = new UsdEuroConversionRow
                           {
                               Year = year,
                               EuroRate = 1,
                               GbpRate = 1,
                               EditYearParameter = 0,
                           };
            }
            else
            {
                returned = new UsdEuroConversionRow
                {
                    Year = rates.Year,
                    EuroRate = rates.Eur,
                    GbpRate = rates.Gbp,
                    EditYearParameter = rates.Year,
                };    
            }

            
            return returned;
        }

        public void UpdateRateForYear(int year, double euroRate, double gbpRate)
        {
            var rateEntry = DataContext.CurrencyUsdRates.FirstOrDefault(d => d.Year == year);

            if (rateEntry == null)
            {
                var newDbEntry = new CurrencyUsdRate
                                 {
                                     Year = (short)year,
                                     Eur = euroRate,
                                     Gbp = gbpRate
                                 };
                DataContext.CurrencyUsdRates.InsertOnSubmit(newDbEntry);
            }
            else
            {
                rateEntry.Eur = euroRate;
                rateEntry.Gbp = gbpRate;
            }
            DataContext.SubmitChanges();
        }
    }
}