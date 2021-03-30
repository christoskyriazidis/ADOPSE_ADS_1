using ApiOne.Interfaces;
using ApiOne.Models.Queries;
using ApiOne.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nest;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly ICustomerRepository _customerRepo = new CustomerRepository();


        public CustomerController(IWebHostEnvironment webHostEnvironment )
        {
            _env = webHostEnvironment;
        }

        [HttpPost]
        [Route("/customer/profileImage")]
        public IActionResult SingleFileUpload(IFormFile file)
        {
            int adId = 5873458;
            if (file.Length > 3145728)
            {
                return BadRequest(new { error = "File is too big (max 3mb)" });
            }
            if (file.ContentType != "image/png" && file.ContentType != "image/jpeg" && file.ContentType != "image/jpg")
            {
                return BadRequest(new { error = "Wrong file type" });
            }
            var dir = _env.ContentRootPath;
            var smallSizeAdPath = Path.Combine(dir, "Images", "serverA", "small", $"{adId}.png");
            using var image = Image.Load(file.OpenReadStream());
            image.Mutate(x => x.Resize(200, 200));
            image.Save(smallSizeAdPath);
            var FullSizeAdPath = Path.Combine(dir, "Images", "serverA", "full", $"{adId}.png");
            using (var fileStream = new FileStream(FullSizeAdPath, FileMode.Create, FileAccess.Write))
            {
                file.CopyTo(fileStream);
            }
            return Ok();
        }


        [HttpGet]
        [Route("/customer")]
        public IActionResult GetCustomers([FromQuery] Pagination pagination)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { error = "Ads Out of range" });
            }
            var customers = _customerRepo.GetCustomers(pagination);
            if (customers==null)
            {
                return BadRequest(new { error = "customer Out of range" });
            }
            return Json(customers);
        }
        
        [HttpGet]
        [Route("/customer/{id}")]
        public IActionResult GetCustomer(int id)
        {
            var customer = _customerRepo.GetCustomer(id);
            if (customer==null)
            {
                return BadRequest(new { error = "wrong customer id" });
            }
            return Json(customer);
        }



        [HttpGet]
        [Route("/secret")]
        [Authorize(Policy = "Admin")]
        public IActionResult Secret()
        {
            var claims = User.Claims.ToList();
            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.DateOfBirth)?.Value;
            var role = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var id = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var username = claims.FirstOrDefault(c => c.Type == "username")?.Value;
            var usernamee = claims.FirstOrDefault(c => c.Type == "usernameeee")?.Value;

            return Json(new { secret = "very secret" });
        }
        [Route("/search")]
        public IActionResult yesNo([FromQuery] string name)
        {
            var settings = new ConnectionSettings(new Uri("http://localhost:9100/"))
                .DefaultIndex("people");

            var client = new ElasticClient(settings);
            var searchResponse = client.Search<Person>(s => s
           .From(0)
           .Size(10)
           .Query(q => q
                .Match(m => m
                   .Field(f => f.FirstName)
                   .Query(name)
                )
               )
            );

            var searchId = client.Search<Person>(s => s
            .Query(q => q.Range(m => m.Field(f => f.Id).GreaterThan(5)
            ))
            );

            var stringSearch = client.Search<Person>(s=>s
            .Query(q=>q
            .QueryString(qs=>qs
            .Query("mar")))
            );

            var people = searchResponse.Documents;
            var secondS = searchId.Documents;
            var stringSearchres = stringSearch.Documents;
            return Ok(people);
        }


        [Route("/yes")]
        public async Task<IActionResult> yes()
        {
            var settings = new ConnectionSettings(new Uri("http://localhost:9100/"))
                .DefaultIndex("people");

            var client = new ElasticClient(settings);
            List<Person> persons = new List<Person>();
             for(int i = 0; i < 10; i++)
            {
                persons.Add(new Person(i,$"mart",$"lastname{i}"));
           
            }
             foreach(var i in persons)
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
                    .Query("Mart1")
                 )
                )
           );

            var people = searchResponse.Documents;

            return Ok(people);
        }
    }
}
