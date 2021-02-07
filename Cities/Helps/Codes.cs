using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cities.Helps
{
    public class Codes
    {
        public string Alpha2Code { get; set; }
        public string Alpha3Code { get; set; }
        public string CurrenciesCode { get; set; }

        public Codes(string a2, string a3, string cc)
        {
            Alpha2Code = a2;
            Alpha3Code = a3;
            CurrenciesCode = cc;
        }
    }
}
