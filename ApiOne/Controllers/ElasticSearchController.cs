using ApiOne.Models.Ads;
using ApiOne.Models.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ApiOne.Controllers
{
    public class ElasticSearchController : Controller
    {
        [HttpGet]
        [Route("/ad")]
        public IActionResult SearchWithFilters([FromQuery] AdFiltersFromParam paramTypeFilter, Pagination pagination)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return BadRequest(allErrors);
            }
            AdFiltersFromParamClient adFiltersFromParamClient = new AdFiltersFromParamClient(pagination);
            foreach (var prop in paramTypeFilter.GetType().GetProperties())
            {
                var value = prop.GetValue(paramTypeFilter, null);
                if (value != null && prop.Name != "Title" && prop.Name != "SortBy" && prop.Name != "Description" && prop.Name != "MaxPrice" && prop.Name != "MinPrice")
                {
                    var filterArray = value.ToString().Split("_");
                    var filterIntArray = filterArray.Select(Int32.Parse).ToList();
                    adFiltersFromParamClient.GetType().GetProperty(prop.Name).SetValue(adFiltersFromParamClient, filterIntArray);
                }
            }
            var settings = new ConnectionSettings(new Uri("http://localhost:9200/")).DefaultIndex("ads");
            var client = new ElasticClient(settings);

            var elasticResponse = client.Search<CompleteAd>(s => s
            //pagination
            .From((pagination.PageNumber - 1) * pagination.PageSize)
            .Size(pagination.PageSize)
            //elasticSearch query
                .Query(q =>
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
                        .Field(f => f.Condition)
                        .Terms(adFiltersFromParamClient.Condition)
                        )
                    )
                 &&
                 (
                    q.Regexp(r => r
                        .Field(f => f.Title)
                        .Value(paramTypeFilter.Title)
                        )
                ||
                    q.Regexp(r => r
                        .Field(f => f.Description)
                        .Value(paramTypeFilter.Title)
                        )
                    )
                ).Sort(ss => ss
        .Field(f =>
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
                    f.Field(f => f.Id);
                    f.Descending();
                    break;
            }
            return f;
        })
            ));

            var elasticResponseCount = client.Count<CompleteAd>(s => s
            .Query(q =>
                    q.Range(r=>r
                        .Field(f=>f.Price)
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
                        .Field(f =>f.Manufacturer)
                        .Terms(adFiltersFromParamClient.Manufacturer)
                        )
                &&
                    q.Terms(t => t
                        .Field(f => f.Condition)
                        .Terms(adFiltersFromParamClient.Condition)
                        )
                    )
                 &&
                 (
                    q.Regexp(r => r
                        .Field(f => f.Title)
                        .Value(paramTypeFilter.Title)
                        )
                ||
                    q.Regexp(r => r
                        .Field(f => f.Description)
                        .Value(paramTypeFilter.Title)
                        )
                    )
                )   
            );

            var queryDocsCount = elasticResponseCount.Count;

            var urlFilters = "";
            //loop through se ka8e property gia na gemise to front object
            foreach (var prop in paramTypeFilter.GetType().GetProperties())
            {
                var value = prop.GetValue(paramTypeFilter, null);
                if (value != null && prop.Name != "Title" && prop.Name != "MaxPrice" && prop.Name != "MinPrice" && prop.Name != "SortBy")
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


            AdsWithPagination adsWithPagination = new AdsWithPagination();
            adsWithPagination.PageSize = pagination.PageSize;
            adsWithPagination.CurrentPage = pagination.PageNumber;
            adsWithPagination.TotalAds = queryDocsCount;
            long lastPageNumber = (queryDocsCount % pagination.PageSize == 0) ? queryDocsCount / pagination.PageSize : queryDocsCount / pagination.PageSize + 1;
            adsWithPagination.TotalPages = lastPageNumber;
            adsWithPagination.LastPageUrl = $"https://localhost:44374/ad?{urlFilters}PageNumber={lastPageNumber}&PageSize={pagination.PageSize}";
            long nextPageNumber = (pagination.PageNumber == lastPageNumber) ? lastPageNumber : pagination.PageNumber + 1;
            long previousPageNumber = (pagination.PageNumber < 2) ? 1 : pagination.PageNumber - 1;
            adsWithPagination.PreviousPageUrl= $"https://localhost:44374/ad?{urlFilters}PageNumber={previousPageNumber}&PageSize={pagination.PageSize}";
            adsWithPagination.NextPageUrl= $"https://localhost:44374/ad?{urlFilters}PageNumber={nextPageNumber}&PageSize={pagination.PageSize}";
            adsWithPagination.Result = elasticResponse.Documents;

            return Ok(adsWithPagination);
        }
    }
}
