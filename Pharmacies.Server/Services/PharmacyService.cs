using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pharmacies.Server.Interfaces;
using Pharmacies.Server.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacies.Server.Services
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public class PharmacyService : IPharmacyService
    {
        public static HttpClient Client = new HttpClient();

        private readonly IConfiguration _configuration;

        private readonly string _apiKey;

        public PharmacyService(IConfiguration configuration)
        {
            _configuration = configuration;
            _apiKey = _configuration.GetSection("Api").GetSection("ApiKey").Value;
        }



        public async Task<Rootobject> SearchPharmacies(double lat, double lng, int range, string type = "pharmacy")
        {
            var baseUrl = _configuration.GetSection("Api").GetSection("PlacesApi_nearbysearch");
            var apiKey = _configuration.GetSection("Api").GetSection("ApiKey");
            var parameters = $"location={lat},{lng}&radius={range}&type={type}&key={apiKey.Value}";


            var url = $"{baseUrl.Value}{parameters}";

            using var response = await Client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Rootobject>(responseString);
            }
            else
                throw new Exception(response.ReasonPhrase);

        }


        public async Task<Location> GetSearchedLocation(string input)
        {
            var encodedInput = input.Replace(" ", "+");
            var baseUrl = _configuration.GetSection("Api").GetSection("GeocodingApi_geocode").Value;
            var apiKey = _configuration.GetSection("Api").GetSection("ApiKey").Value;
            var parameters = $"address={encodedInput}&key={apiKey}";

            var url = $"{baseUrl}{parameters}";

            using var response = await Client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Rootobject>(responseString).results[0].geometry.location;
            }
            else
                throw new Exception(response.ReasonPhrase);
        }



        public async Task<Rootobject> GetPlaceDetails(string id)
        {
            var baseUrl = _configuration.GetSection("Api").GetSection("details").Value;
            var parameters = $"place_id={id}&key={_apiKey}&fields=vicinity,name,photo,opening_hours";

            var url = $"{baseUrl}{parameters}";

            using var response = await Client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Rootobject>(responseString);
            }
            else
                throw new Exception(response.ReasonPhrase);

        }


        public string GetPhoto(string photoRef, int width, int height)
        {
            var baseUrl = _configuration.GetSection("Api").GetSection("photo").Value;
            var parameters = $"maxwidth={width}&maxheight={height}&photoreference={photoRef}&key={_apiKey}";

            var url = $"{baseUrl}{parameters}";

            return url;
        }
    }
}
