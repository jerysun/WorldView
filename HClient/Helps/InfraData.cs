using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace HClient.Helps
{
    public static class InfraData
    {
        public const string APPID = "&appid=6fad1179b35c3658343b2ec07587e0f8";
        public const string CityWeather = "https://api.openweathermap.org/data/2.5/weather";
        public const string AllCountires = "https://restcountries.eu/rest/v2/all";

        public static readonly HashSet<string> CountriesNames = new HashSet<string>();
        public static readonly Dictionary<string, Codes> CountriesCodes = new Dictionary<string, Codes>();

        static InfraData() {
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

        /*
        public const string Amsterdam = "https://api.openweathermap.org/data/2.5/weather?q=Amsterdam&appid=6fad1179b35c3658343b2ec07587e0f8";
        public const string AmsterdamUs = "https://api.openweathermap.org/data/2.5/weather?q=Amsterdam,us&appid=6fad1179b35c3658343b2ec07587e0f8";
         */
    }
}
