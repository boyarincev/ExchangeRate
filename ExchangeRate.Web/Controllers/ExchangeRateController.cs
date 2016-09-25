using ExchangeRate.Web.AppServices;
using ExchangeRate.Web.Model;
using ExchangeRate.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ExchangeRate.Web.Controllers
{
    public class ExchangeRateController : Controller
    {
        private readonly ExchangeRateService _exchangeRateService;

        public ExchangeRateController()
        {
            //TODO добавить внедрение зависимости, для целей тестирования контроллера
            _exchangeRateService = new ExchangeRateService();
        }

        public async Task<ActionResult> GetExhangeRates(ExchangeRatesQuery query)
        {
            if (!ModelState.IsValid)
            {
                //TODO добавить логгирование
                return Json(new ExchangeRatesContainer());
            }

            var exchangeRateValues = await _exchangeRateService.GetExchangeRatesOrderedByDateAsync(query.Month, query.CurrencyCode);
            var exchangeRatesContainer = new ExchangeRatesContainer { CurrencyName = query.CurrencyName, OrderedByDateExchangeRates = exchangeRateValues };

            return Json(exchangeRatesContainer);
        }
    }
}