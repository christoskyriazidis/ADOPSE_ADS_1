using ApiOne.Models.ElasticSearch;
using DypaApi.Models;
using Microsoft.AspNetCore.Mvc;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DypaApi.Controllers
{
    public class ElasticSearchController : Controller
    {
        [HttpGet]
        public IActionResult GetDataElasticData(Pagination pagination)
        {
            ConnectionSettings settings = new ConnectionSettings(new Uri("http://localhost:9200/")).DefaultIndex("sensorlogs");
            ElasticClient client = new ElasticClient(settings);

            var elasticResponse = client.Search<ElasticXorafi>(s =>s
            .From((pagination.PageNumber - 1) * pagination.PageSize)
            .Size(pagination.PageSize)

            );

            return Json(new { });

        }
    }
}
