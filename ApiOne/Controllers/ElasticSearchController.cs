using ApiOne.Models.Ads;
using ApiOne.Models.Queries;
using Microsoft.AspNetCore.Mvc;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiOne.Controllers
{
    public class ElasticSearchController : Controller
    {
       
        //[Route("/search")]
        //public IActionResult ReturnSearchResult([FromQuery] string name) {
        //    var settings = new ConnectionSettings(new Uri("http://localhost:9200/"))
        //        .DefaultIndex("people");

        //    var client = new ElasticClient(settings);
        //    var searchResponse = client.Search<Person>(s => s
        //   .From(0)
        //   .Size(10)
        //   .Query(q => q
        //        .Match(m => m
        //           .Field(f => f.FirstName)
        //           .Query(name)
        //        )
        //       )
        //    );

        //    var searchId = client.Search<Person>(s => s
        //    .Query(q => q.Range(m => m.Field(f => f.Id).GreaterThan(5)
        //    ))
        //    );

        //    var stringSearch = client.Search<Person>(s => s
        //    .Query(q => q
        //    .QueryString(qs => qs
        //    .Query("mar")))
        //    );



        //    var people = searchResponse.Documents;
        //    var secondS = searchId.Documents;
        //    var stringSearchres = stringSearch.Documents;
        //    var quequeque = myQuery.Documents;
        //    return Ok(quequeque);
        //}

        [Route("/search")]
        public IActionResult ReturnSearchResult([FromQuery] string title, [FromQuery] Pagination pager) {
            var settings = new ConnectionSettings(new Uri("http://localhost:9200/"))
                .DefaultIndex("ads");
            string[] array = { "1"  };
            var client = new ElasticClient(settings);
            var myQuery = client.Search<CompleteAd>(s => s
                .From((pager.PageNumber - 1) * pager.PageSize)
                .Size(pager.PageSize)
                
                );

            //    .Query(q => q
            //    .Bool(b => b
            //        .Must(mu => mu
            //            .Match(m => m
            //                .Field(f => f.State)
            //                .Query(q=>q.)
            //            )
            //            )
            //         .Must(mu => mu
            //            .Match(m => m
            //                .Field(f => f.State)
            //                .Query("1")
            //            )
            //        )
            //)));

            //.Query(q2 => q2
            //    .Wildcard(qs => qs
            //        .Field(f => f.Title)
            //        .Value("*" + title + "*")
            //    )
            //)

            //)


            //.Query(b => b.Terms(t => t.Field(f => f.Category).Terms(array)))
            return Ok(myQuery.Documents);
        }
    }
}
