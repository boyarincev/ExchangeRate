using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExchangeRate.Web.Models
{
    public class ExchangeRatesQuery
    {
        [Required]
        [Range(1, 12)]
        public int Month { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string CurrencyCode { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string CurrencyName { get; set; }
    }
}