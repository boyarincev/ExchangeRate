using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data;
using ExchangeRate.Web.ViewModel;
using ExchangeRate.Web.AppServices;
using System.Globalization;
using ExchangeRate.Web.Model;

namespace ExchangeRate.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ExchangeRateService _exchangeRateService;

        public HomeController()
        {
            //TODO добавить внедрение зависимости, для целей тестирования контроллера
            _exchangeRateService = new ExchangeRateService();
        }

        public async Task<ActionResult> Index()
        {
            var currencies = await _exchangeRateService.GetCurrenciesAsync();

            var model = new HomeIndexViewModel
            {
                Currencies = currencies.Select(c => new SelectListItem { Text = c.Name, Value = c.Code }).ToList(),
                Months = GetMonthsSelectList()
            };

            return View(model);
        }

        private static List<SelectListItem> GetMonthsSelectList()
        {
            var currentMonth = DateTime.Now.Month;
            var monthNumbersList = Enumerable.Range(1, 12);
            var months = monthNumbersList.Select(cmn => new SelectListItem
            {
                Text = CultureInfo.CurrentUICulture.DateTimeFormat.GetMonthName(cmn),
                Value = cmn.ToString(),
                Disabled = cmn > currentMonth
            })
            .ToList();

            return months;
        }
    }
}