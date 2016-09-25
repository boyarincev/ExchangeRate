using ExchangeRate.Web.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExchangeRate.Web.ViewModel
{
    public class HomeIndexViewModel
    {
        public List<SelectListItem> Currencies { get; set; }
        public List<SelectListItem> Months { get; set; }
    }
}