using ApiOne.Models.Ads;
using ApiOne.Models.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiOne.Controllers
{
    public class ElasticSearchController : Controller
    {
        [HttpGet]
        [Route("/ad/search")]
        public IActionResult SearchWithFilters([FromQuery] AdFiltersFromParam paramTypeFilter, Pagination pagination)
        {
            //if (string.IsNullOrEmpty(paramTypeFilter.State) && string.IsNullOrEmpty(paramTypeFilter.Manufacturer) && string.IsNullOrEmpty(paramTypeFilter.Type) && string.IsNullOrEmpty(paramTypeFilter.Condition) && string.IsNullOrEmpty(paramTypeFilter.Category) && string.IsNullOrEmpty(paramTypeFilter.Title) && string.IsNullOrEmpty(paramTypeFilter.Description))
            //{
            //    return BadRequest(new { error = "you should use at least one filter" });
            //}
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return BadRequest(allErrors);
            }
            AdFiltersFromParamClient adFiltersFromParamClient = new AdFiltersFromParamClient(pagination);
            foreach (var prop in paramTypeFilter.GetType().GetProperties())
            {
                var value = prop.GetValue(paramTypeFilter, null);
                if (value != null && prop.Name != "Title" && prop.Name != "Description" && prop.Name != "MaxPrice" && prop.Name != "MinPrice")
                {
                    var filterArray = value.ToString().Split("_");
                    var filterIntArray = filterArray.Select(Int32.Parse).ToList();
                    adFiltersFromParamClient.GetType().GetProperty(prop.Name).SetValue(adFiltersFromParamClient, filterIntArray);
                }
            }
            var settings = new ConnectionSettings(new Uri("http://localhost:9200/")).DefaultIndex("ads");
            var client = new ElasticClient(settings);

            var elasticResponse = client.Search<CompleteAd>(s => s
            .From((pagination.PageNumber - 1) * pagination.PageSize)
            .Size(pagination.PageSize)
                .Query(q =>
                    q.Range(r => r
                        .Field(f => f.Price)
                            .GreaterThanOrEquals(paramTypeFilter.MinPrice)
                            .LessThanOrEquals(paramTypeFilter.MaxPrice)
                    )
                &&
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
                 &&
                    q.Regexp(r => r
                        .Field(f => f.Title)
                        .Value(paramTypeFilter.Title)
                        )
                ||
                    q.Regexp(r => r
                        .Field(f => f.Description)
                        .Value(paramTypeFilter.Description)
                        )
                )
            );

            var elasticResponseCount = client.Count<CompleteAd>(s => s
            .Query(q =>
                    q.Range(r=>r
                        .Field(f=>f.Price)
                            .GreaterThanOrEquals(paramTypeFilter.MinPrice)
                            .LessThanOrEquals(paramTypeFilter.MaxPrice)
                    )
                &&
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
                 &&
                    q.Regexp(r => r
                        .Field(f => f.Title)
                        .Value(paramTypeFilter.Title)
                        )
                ||
                    q.Regexp(r => r
                        .Field(f => f.Description)
                        .Value(paramTypeFilter.Description)
                        )
                )    
            );

            var queryDocsCount = elasticResponseCount.Count;

            var response = new {
                size= queryDocsCount,
                firstPage=1,
                lastPage= (queryDocsCount % pagination.PageSize==0)? queryDocsCount/pagination.PageSize: queryDocsCount/pagination.PageSize+1,
                ads=elasticResponse.Documents};
            return Ok(response);
        }
    }
}
