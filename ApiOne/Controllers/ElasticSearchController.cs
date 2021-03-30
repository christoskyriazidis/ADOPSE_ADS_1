using ApiOne.Models.Ads;
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
        [Route("/elastic")]
        public async Task<IActionResult> yes()
        {
            var settings = new ConnectionSettings(new Uri("http://localhost:9200/"))
                .DefaultIndex("people");

            var client = new ElasticClient(settings);
            List<Person> persons = new List<Person>();
            for (int i = 0; i < 10; i++)
            {
                persons.Add(new Person(i, $"mart", $"lastname{i}"));
                persons.Add(new Person(i+10, $"vaggel", $"lastname{i}"));
            }
            foreach (var i in persons)
            {
                var indexResponse = client.IndexDocument(i);
                var asyncIndexResponse = await client.IndexDocumentAsync(i);
            }


            var searchResponse = client.Search<Person>(s => s
            .From(0)
            .Size(10)
            .Query(q => q
                 .Match(m => m
                    .Field(f => f.FirstName)
                    .Query("mart")
                 )
                )
           );

            var people = searchResponse.Documents;

            return Ok(people);
        }
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

        //    var myQuery = client.Search<Person>(s => s
        //        .Query(q=>q
        //            .Wildcard(qs=>qs
        //                .Field(f=>f.FirstName)
        //                .Value("v*")
        //            )
        //        )
        //    );

        //    var people = searchResponse.Documents;
        //    var secondS = searchId.Documents;
        //    var stringSearchres = stringSearch.Documents;
        //    var quequeque = myQuery.Documents;
        //    return Ok(quequeque);
        //}

        [Route("/search")]
        public IActionResult ReturnSearchResult([FromQuery] string title) {
            var settings = new ConnectionSettings(new Uri("http://localhost:9200/"))
                .DefaultIndex("ads");
            var client = new ElasticClient(settings);
            var myQuery = client.Search<CompleteAd>(s => s
                .Query(q => q
                  .Bool(b => b
                    .Should(m => m
                      .Wildcard(c => c
                        .Field("title").Value("*" + title.ToLower() + "*")
                      )
                    )
                  )
                )
            );

            return Ok(myQuery.Documents);
        }
    }
}
