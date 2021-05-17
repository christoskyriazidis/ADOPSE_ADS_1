using identityServerNew.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace identityServerNew.Helpers
{
    public class MapsHelpers
    {
        public static async Task<LocationModel> GetCoordsFromAddress(string address, string postCode)
        {

            var mapsApiKey = Startup._config.GetValue<string>("MapsApi:TomTom");
            var url = $"https://api.tomtom.com/search/2/geocode/{address}+{postCode}.json?key={mapsApiKey}";
            HttpClient httpClient = new HttpClient();
            using var httpResponse = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            LocationModel locationModel = null;
            try
            {
                var jsonResult = await httpResponse.Content.ReadAsStringAsync();
                dynamic mapsResponse = JsonConvert.DeserializeObject(jsonResult);
                if (mapsResponse != null)
                {
                    var addres2s = mapsResponse.results[0];
                    var addres2ssss = mapsResponse.results[0].address;
                    var summary = mapsResponse.summary;
                    var results = summary.numResults;
                    if (results > 0)
                    {
                        locationModel = new LocationModel
                        {
                            Latitude = addres2s.position.lat,
                            Longitude = addres2s.position.lon,
                            Address = addres2s.address.streetName
                        };
                    }
                }
                    return locationModel;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.ToString());
                return locationModel;
            }
        }
    }
}
