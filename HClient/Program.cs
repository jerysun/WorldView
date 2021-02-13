using HClient.Helps;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace HClient
{
    class Program
    {
        static void Main(string[] args)
        {
            /*string weather = GetCityWeather("Amsterdam", "us");
            Console.WriteLine($"weather: {weather}");*/

            foreach(var cname in InfraData.CountriesNames) Console.WriteLine(cname);

            foreach(var ccode in InfraData.CountriesCodes)
            {
                Console.WriteLine($"{ccode.Key}: {ccode.Value.Alpha2Code}, {ccode.Value.Alpha3Code}, {ccode.Value.CurrenciesCode}");
            }
        }

        static string GetCityWeather(string cityName, string countryCode = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("q=");
            sb.Append(cityName);
            if (!string.IsNullOrEmpty(countryCode))
            {
                sb.Append(",");
                sb.Append(countryCode);
            }
            sb.Append(InfraData.APPID);

            UriBuilder uriBuilder = new UriBuilder(InfraData.CityWeather);
            uriBuilder.Query = sb.ToString();

            HttpClient client = new HttpClient();
            var result = client.GetAsync(uriBuilder.Uri).Result;
            StringBuilder sbWeather = new StringBuilder();

            using (StreamReader sr = new StreamReader(result.Content.ReadAsStreamAsync().Result))
            {
                
                string weatherInfo = sr.ReadToEnd();
                dynamic data = JObject.Parse(weatherInfo);

                sbWeather.Append(data.weather[0].description);

                string temp = K2C(Convert.ToDouble(data.main.temp));
                sbWeather.Append($", {temp}");
            }
            return sbWeather.ToString();
        }

        // kelvin to celsius
        static string K2C(double k)
        {
            double tDbl = k - 273.15 + 0.5;
            int tInt = (int)tDbl;
            return tInt.ToString() + "C";
        }



    }
}
