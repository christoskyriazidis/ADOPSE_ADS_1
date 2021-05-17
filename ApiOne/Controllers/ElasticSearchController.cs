using ApiOne.Interfaces;
using ApiOne.Models.Ads;
using ApiOne.Models.Queries;
using ApiOne.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ApiOne.Controllers
{
    public class ElasticSearchController : Controller
    {
        private readonly ICustomerRepository _customerRepo = new CustomerRepository();


        [HttpGet]
        [Route("/ad")]
        public IActionResult SearchWithFilters([FromQuery] AdFiltersFromParam paramTypeFilter, Pagination pagination)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return BadRequest(allErrors);
            }
            bool isAuthenticated = User.Identity.IsAuthenticated;
            double latitude=1;
            double longitude=1;
            if (!isAuthenticated && (paramTypeFilter.SortBy.Equals("coordsL") || paramTypeFilter.SortBy.Equals("coordsH")))
            {
                return StatusCode(401);
            }
            else if( isAuthenticated && (paramTypeFilter.SortBy.Equals("coordsL") || paramTypeFilter.SortBy.Equals("coordsH")))
            {
                var claims = User.Claims.ToList();
                var subId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                var stringCoords = _customerRepo.GetCoordsBySubId(subId);
                var  longg = stringCoords.Substring(stringCoords.IndexOf(",")+1);
                var lat = stringCoords.Substring(0, stringCoords.IndexOf(","));
                latitude = double.Parse(lat);
                longitude = double.Parse(longg);
            }
            AdFiltersFromParamClient adFiltersFromParamClient = new AdFiltersFromParamClient(pagination);
            foreach (var prop in paramTypeFilter.GetType().GetProperties())
            {
                var value = prop.GetValue(paramTypeFilter, null);
                if (value != null && prop.Name != "Title" && prop.Name != "SortBy" && prop.Name != "Description" && prop.Name != "MaxPrice" && prop.Name != "MinPrice" && prop.Name != "SubCategoryId"&& prop.Name != "Distance")
                {
                    var filterArray = value.ToString().Split("_");
                    var filterIntArray = filterArray.Select(Int32.Parse).ToList();
                    adFiltersFromParamClient.GetType().GetProperty(prop.Name).SetValue(adFiltersFromParamClient, filterIntArray);
                }
            }
            ConnectionSettings settings = new ConnectionSettings(new Uri("http://localhost:9200/")).DefaultIndex("locationtest");
            ElasticClient client = new ElasticClient(settings);


            var elasticResponse = client.Search<CompleteAd>(s => s
            //pagination
            .From((pagination.PageNumber - 1) * pagination.PageSize)
            .Size(pagination.PageSize)
            //elasticSearch query
                .Query(q =>
                    q.Match(m => m
                        .Field(f => f.subcategoryid)
                            .Query(paramTypeFilter.SubCategoryId.ToString()
                            )
                    )
                 &&
                    q.Range(r => r
                        .Field(f => f.Price)
                            .GreaterThanOrEquals(paramTypeFilter.MinPrice)
                            .LessThanOrEquals(paramTypeFilter.MaxPrice)
                    )
                &&
                (
                    q.Terms(t => t
                        .Field(f => f.Type)
                        .Terms(adFiltersFromParamClient.Type)
                        )
                &&
                    q.Terms(t => t
                        .Field(f => f.Manufacturer)
                        .Terms(adFiltersFromParamClient.Manufacturer)
                        )
                &&
                    q.Terms(t => t
                        .Field(f => f.State)
                        .Terms(1)
                        )
                &&
                    q.Terms(t => t
                        .Field(f => f.Condition)
                        .Terms(adFiltersFromParamClient.Condition)
                        )
                    )
                &&
                q.Bool(b => {
                    if (paramTypeFilter.SortBy.Equals("coordsL") || paramTypeFilter.SortBy.Equals("coordsH"))
                    {
                        b.Filter(filter => filter
                            .GeoDistance(geo => geo
                                .Field(f => f.coords)
                                .Distance(paramTypeFilter.Distance+"km").Location(latitude, longitude)
                                .DistanceType(GeoDistanceType.Arc)
                                
                                ));
                        return b;
                    }
                    return b;
                    })
                 &&
                    q.Match(m=>m
                        .Query(paramTypeFilter.Title)
                            .Operator(Nest.Operator.And)
                            .Field(f=>f.Title)
                            .Fuzziness(Fuzziness.EditDistance(2)
                          ))
                )
                .Sort(ss =>
                {
                    if (paramTypeFilter.SortBy.Equals("coordsL"))
                    {
                        ss.GeoDistance(g => g
                         .Field(f => f.coords)
                         .DistanceType(GeoDistanceType.Arc)
                         .Order(SortOrder.Ascending)
                         .Unit(DistanceUnit.Kilometers)
                         .Points(new GeoLocation(latitude, longitude))
                         .Mode(SortMode.Min)
                         .IgnoreUnmapped(true));
                    }else if (paramTypeFilter.SortBy.Equals("coordsH"))
                    {
                        ss.GeoDistance(g => g
                         .Field(f => f.coords)
                         .DistanceType(GeoDistanceType.Arc)
                         .Order(SortOrder.Descending)
                         .Points(new GeoLocation(latitude, longitude))
                         .Mode(SortMode.Min)
                         .Unit(DistanceUnit.Kilometers)
                         .IgnoreUnmapped(true));

                    }
                    else
                    {
                        ss.Field(f =>
                        {
                            //f.Order(Nest.SortOrder.Ascending);
                            switch (paramTypeFilter.SortBy)
                            {
                                case "idH":
                                    f.Field(f => f.Id);
                                    f.Descending();
                                    break;
                                case "idL":
                                    f.Field(f => f.Id);
                                    f.Ascending();
                                    break;
                                case "priceH":
                                    f.Field(ff => ff.Price);
                                    f.Descending();
                                    break;
                                case "priceL":
                                    f.Field(ff => ff.Price);
                                    f.Ascending();
                                    break;
                                default:
                                    //f.Field(f => f.Id);
                                    //f.Descending();
                                    break;
                            }
                            paramTypeFilter.Distance = null;
                            return f;
                        });
                    }
                    return ss;
                }        
            ));

            var urlFilters = "";
            //loop through se ka8e property gia na gemise to front object
            foreach (var prop in paramTypeFilter.GetType().GetProperties())
            {
                var value = prop.GetValue(paramTypeFilter, null);
                if (value != null && prop.Name != "Title" && prop.Name != "MaxPrice" && prop.Name != "MinPrice" && prop.Name != "SortBy" && prop.Name != "SubCategoryId" && prop.Name != "Distance")
                {
                    //pattern  type=1_2_3&category=1_2_3
                    urlFilters += $"{prop.Name}={value}&";
                    var stringFilter = value.ToString().Length > 2 ? value.ToString().Split("_") : value.ToString().Split("_");
                    var intArrayFilter = stringFilter.Select(Int32.Parse).ToList();
                }
                else if(value!=null)
                {
                    var encoded = WebUtility.UrlEncode(value.ToString());
                    urlFilters += $"{prop.Name}={encoded}&";
                }
            }
            var queryDocsCount = GetCountFromElastic(paramTypeFilter, adFiltersFromParamClient, client, latitude, longitude); ;

            AdsWithPagination adsWithPagination = new AdsWithPagination();
            adsWithPagination.PageSize = pagination.PageSize;
            adsWithPagination.CurrentPage = pagination.PageNumber;
            adsWithPagination.TotalAds = queryDocsCount;
            long lastPageNumber = (queryDocsCount % pagination.PageSize == 0) ? queryDocsCount / pagination.PageSize : queryDocsCount / pagination.PageSize + 1;
            adsWithPagination.TotalPages = lastPageNumber;
            adsWithPagination.LastPageUrl = $"https://localhost:44374/ad?{urlFilters}PageNumber={lastPageNumber}&PageSize={pagination.PageSize}";
            long nextPageNumber = (pagination.PageNumber == lastPageNumber) ? lastPageNumber : pagination.PageNumber + 1;
            long previousPageNumber = (pagination.PageNumber < 2) ? 1 : pagination.PageNumber - 1;
            adsWithPagination.PreviousPageUrl = $"https://localhost:44374/ad?{urlFilters}PageNumber={previousPageNumber}&PageSize={pagination.PageSize}";
            adsWithPagination.NextPageUrl = $"https://localhost:44374/ad?{urlFilters}PageNumber={nextPageNumber}&PageSize={pagination.PageSize}";

            if (paramTypeFilter.SortBy.Equals("coordsL") || paramTypeFilter.SortBy.Equals("coordsH"))
            {
                var sortResult = elasticResponse.Hits.ToList();
                var adsWithDistance = elasticResponse.Documents.ToList();
                //an epileksh coordsL/H gemizei to paidio distance me tn apostasi
                for (var i = 0; i < sortResult.Count; i++)
                {
                    string dist = sortResult[i].Sorts.FirstOrDefault().ToString();
                    double distance = double.Parse(dist);
                    adsWithDistance[i].distance = Math.Round(distance, 2);
                }
                adsWithPagination.Result = adsWithDistance;
            }
            else
            {
                adsWithPagination.Result = elasticResponse.Documents;
            }
            //return Json(elasticResponse);
            return Ok(adsWithPagination);
        }
       
        public long GetCountFromElastic(AdFiltersFromParam paramTypeFilter, AdFiltersFromParamClient adFiltersFromParamClient, ElasticClient client, double latitude,double longtitude)
        {
            var elasticResponseCount = client.Count<CompleteAd>(s => s
           .Query(q =>
                    q.Match(m => m
                        .Field(f => f.subcategoryid)
                            .Query(paramTypeFilter.SubCategoryId.ToString()
                            )
                    )
                 &&
                    q.Range(r => r
                        .Field(f => f.Price)
                            .GreaterThanOrEquals(paramTypeFilter.MinPrice)
                            .LessThanOrEquals(paramTypeFilter.MaxPrice)
                    )
                &&
                (
                    q.Terms(t => t
                        .Field(f => f.Type)
                        .Terms(adFiltersFromParamClient.Type)
                        )
                &&
                    q.Terms(t => t
                        .Field(f => f.Manufacturer)
                        .Terms(adFiltersFromParamClient.Manufacturer)
                        )
                &&
                    q.Terms(t => t
                        .Field(f => f.State)
                        .Terms(1)
                        )
                &&
                    q.Terms(t => t
                        .Field(f => f.Condition)
                        .Terms(adFiltersFromParamClient.Condition)
                        )
                    )
                &&
                q.Bool(b => {
                    if (paramTypeFilter.SortBy.Equals("coordsL") || paramTypeFilter.SortBy.Equals("coordsH"))
                    {
                        b.Filter(filter => filter
                            .GeoDistance(geo => geo
                                .Field(f => f.coords)
                                .Distance(paramTypeFilter.Distance + "km").Location(latitude, longtitude)
                                .DistanceType(GeoDistanceType.Arc)
                                ));
                        return b;
                    }
                    return b;
                })
                 &&
                    q.Match(m => m
                        .Query(paramTypeFilter.Title)
                            .Operator(Nest.Operator.And)
                            .Field(f => f.Title)
                            .Fuzziness(Fuzziness.EditDistance(2)
                          ))
                ));
            return elasticResponseCount.Count;
        }
    }
}
