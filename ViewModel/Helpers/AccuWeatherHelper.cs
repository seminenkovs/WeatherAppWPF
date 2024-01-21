using System.Net.Http;
using System.Security.Policy;
using Newtonsoft.Json;
using WeatherAppWPF.Model;

namespace WeatherAppWPF.ViewModel.Helpers;

public class AccuWeatherHelper
{
    public const string BASE_URL = "http://dataservice.accuweather.com/";

    public const string AUTOCOMPLETE_ENDPOINT =
        "locations/v1/cities/autocomplete?apikey={0}&q={1}";

    public const string CURRENT_CONDITIONS_ENDPOINT =
        "currentconditions/v1/{0}?apikey={1}";

    public const string API_KEY = "IWOJCX5THXcbLMviauwsGpCb8xa5zFdy";

    public static async Task<List<City>> GetCities(string query)
    {
        List<City> cities = new List<City>();

        string url = BASE_URL + string.Format(AUTOCOMPLETE_ENDPOINT, API_KEY, query);

        using (HttpClient client = new HttpClient())
        {
            var response = await client.GetAsync(url);
            string json = await response.Content.ReadAsStringAsync();
            cities =  JsonConvert.DeserializeObject<List<City>>(json);
        }

        return cities;
    }

    public static async Task<CurrentConditions> GetCurrentConditions(string cityKey)
    {
        CurrentConditions conditions = new CurrentConditions();

        string url = BASE_URL + string.Format(CURRENT_CONDITIONS_ENDPOINT, cityKey,API_KEY);

        using (HttpClient client = new HttpClient())
        {
            var response = await client.GetAsync(url);
            string json = await response.Content.ReadAsStringAsync();
            conditions = (JsonConvert.DeserializeObject<List<CurrentConditions>>(json)).FirstOrDefault();
        }

        return conditions;
    }
}