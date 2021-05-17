using GeoCoordinatePortable;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.IO;
using ApiOne.Models.Ads;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using ApiOne.Models.Location;
using Newtonsoft.Json;

namespace ApiOne.Controllers
{
    public class TestoController : Controller
    {

        //[HttpGet]
        //[Route("/")]
        //public async Task<IActionResult> TestbenchAsync()
        //{
        //    var sCoord = new GeoCoordinate(40.6587, 22.91014);
        //    var eCoord = new GeoCoordinate(40.669, 22.93295);

        //    var a= eCoord.GetDistanceTo(sCoord);

        //    var coords = await GetCoordsFromAddress("euosmos", "56122");
        //    if (coords != null)
        //    {
        //        return Json(coords); ;
        //    }
        //    return Json(new { response= a }); ;

        //}

        public async Task<LocationModel> GetCoordsFromAddress(string address,string postCode)
        {
            var url = $"https://api.tomtom.com/search/2/geocode/{address}+{postCode}.json?key=nXKMDmz767kiY4dtmo6dMWEz88TYJAVK";

            HttpClient httpClient = new HttpClient();
            using var httpResponse = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            httpResponse.EnsureSuccessStatusCode();
            var jsonResult = await httpResponse.Content.ReadAsStringAsync();
            dynamic stuff2 = JsonConvert.DeserializeObject(jsonResult);
            var addres2s = stuff2.results[0];
            var addres2ssss = stuff2.results[0].address;
            var summary = stuff2.summary;
            var results= summary.numResults;
            LocationModel locationModel=null;
            if (results > 0)
            {
                locationModel = new LocationModel { 
                    Latitude = addres2s.position.lat,
                    Longitude = addres2s.position.lon,
                    Address=addres2s.address.streetName};
            }
            return locationModel;
        }
    }
}

