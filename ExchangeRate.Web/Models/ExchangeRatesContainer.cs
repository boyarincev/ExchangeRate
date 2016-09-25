using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExchangeRate.Web.Model
{
    public class ExchangeRatesContainer
    {
        public ExchangeRatesContainer()
        {
            OrderedByDateExchangeRates = new List<ExchangeRateItem>();
        }

        public string CurrencyName { get; set; }
        public List<ExchangeRateItem> OrderedByDateExchangeRates { get; set; }
    }
}