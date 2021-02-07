using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Cities.Helps
{
    public static class InfraData
    {
        public const string APPID = "&appid=6fad1179b35c3658343b2ec07587e0f8";
        public const string CityWeather = "https://api.openweathermap.org/data/2.5/weather";
        public const string AllCountires = "https://restcountries.eu/rest/v2/all";

        public static readonly HashSet<string> CountriesNames = new HashSet<string>();
        public static readonly Dictionary<string, Codes> CountriesCodes = new Dictionary<string, Codes>();

        static InfraData()
        {
            GetCountriesNamesCodes();
        }

        public static void GetCountriesNamesCodes()
        {
            UriBuilder uriBuilder = new UriBuilder(AllCountires);
            HttpClient client = new HttpClient();

            var result = client.GetAsync(uriBuilder.Uri).Result;

            using (StreamReader sr = new StreamReader(result.Content.ReadAsStreamAsync().Result))
            {
                string countries = sr.ReadToEnd();
                dynamic data = JsonConvert.DeserializeObject(countries);

                foreach (dynamic country in data)
                {
                    CountriesNames.Add((string)country.name);

                    Codes codes = new Codes(
                        (string)country.alpha2Code,
                        (string)country.alpha3Code,
                        (string)country.currencies[0].code);

                    CountriesCodes.Add((string)country.name, codes);
                }
            }
        }

        public static string GetCityWeather(string cityName, string countryCode = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("q=");
            sb.Append(cityName);
            if (!string.IsNullOrEmpty(countryCode))
            {
                sb.Append(",");
                sb.Append(countryCode);
            }
            sb.Append(APPID);

            UriBuilder uriBuilder = new UriBuilder(CityWeather);
            uriBuilder.Query = sb.ToString();

            HttpClient client = new HttpClient();
            var result = client.GetAsync(uriBuilder.Uri).Result;
            string weather = string.Empty;

            using (StreamReader sr = new StreamReader(result.Content.ReadAsStreamAsync().Result))
            {

                string weatherInfo = sr.ReadToEnd();
                dynamic data = JObject.Parse(weatherInfo);
                weather = data.weather[0].description;
            }
            return weather;
        }
    }
}
