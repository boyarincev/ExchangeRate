using ExchangeRate.Web.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRate.Web.AppServices
{
    public class ExchangeRateService
    {
        /// <summary>
        /// Получить список доступных валют
        /// </summary>
        /// <returns></returns>
        public async Task<List<Currency>> GetCurrenciesAsync()
        {
            List<Currency> currencies;

            using (var client = new CbrDailyInfo.DailyInfoSoapClient())
            {
                DataSet currenciesDataSet;

                try
                {
                    currenciesDataSet = await client.EnumValutesAsync(false);
                }
                catch (Exception)
                {
                    //Добавить логгирование
                    return new List<Currency>();
                }

                currencies = GetCurrenciesFrom(currenciesDataSet);
            }

            return currencies;
        }

        private List<Currency> GetCurrenciesFrom(DataSet currenciesDataSet)
        {
            try
            {
                return currenciesDataSet
                    .Tables[0]
                    .AsEnumerable()
                    .Select(dr => new Currency
                    {
                        Code = dr.Field<string>("Vcode"),
                        Name = dr.Field<string>("Vname")
                    })
                    .ToList();
            }
            catch (Exception)
            {
                //TODO добавить логгирование
                return new List<Currency>();
            }
        }

        /// <summary>
        /// Получить список котировок валюты для месяца
        /// </summary>
        /// <param name="month">Номер месяца</param>
        /// <param name="currencyCode">Код валюты</param>
        /// <returns></returns>
        public async Task<List<ExchangeRateItem>> GetExchangeRatesOrderedByDateAsync(int month, string currencyCode)
        {
            if (month < 1 || month > 12)
            {
                throw new ArgumentOutOfRangeException("month", "Месяц должен быть больше 0 и меньше или равен 12");
            }

            using (var client = new CbrDailyInfo.DailyInfoSoapClient())
            {
                var currentYear = DateTime.Now.Year;
                int daysInMonts = DateTime.DaysInMonth(currentYear, month);
                DateTime startDate = new DateTime(currentYear, month, 1);
                DateTime endDate = new DateTime(currentYear, month, daysInMonts);
                DataSet cursForDatesDataSet;

                try
                {
                    cursForDatesDataSet = await client.GetCursDynamicAsync(startDate, endDate, currencyCode);
                }
                catch (Exception)
                {
                    //TODO добавить логгирование
                    return new List<ExchangeRateItem>();
                }

                var exchangeRateValues = GetExchangeRateItemsFrom(cursForDatesDataSet);

                return exchangeRateValues;
            }
        }

        private List<ExchangeRateItem> GetExchangeRateItemsFrom(DataSet exchangeRatesDataSet)
        {
            try
            {
                return exchangeRatesDataSet.Tables[0].AsEnumerable().Select(dr => new ExchangeRateItem
                {
                    DayNumber = dr.Field<DateTime>("CursDate").Day,
                    Value = dr.Field<decimal>("Vcurs") / dr.Field<decimal>("Vnom")
                }).OrderBy(cv => cv.DayNumber).ToList();
            }
            catch (Exception)
            {
                //TODO добавить логгирование
                return new List<ExchangeRateItem>();
            }
        }
    }
}