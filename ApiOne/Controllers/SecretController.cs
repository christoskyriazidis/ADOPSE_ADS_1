using ApiOne.Databases;
using ApiOne.Hubs;
using ApiOne.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.EventArgs;

namespace ApiOne.Controllers
{

    public class SecretController : Controller
    {
        private readonly IHubContext<ChatHub> _myHub;

        public SecretController(IHubContext<ChatHub> hubContext)
        {
            _myHub = hubContext;
        }

        public MyErrorObject CheckAdForErrors(Product product)
        {
            foreach (PropertyInfo pi in product.GetType().GetProperties())
            {
                if (pi.PropertyType == typeof(string))
                {
                    string value = (string)pi.GetValue(product);
                    if (string.IsNullOrEmpty(value))
                    {
                        return new MyErrorObject { Error = $"Cannot be null or empty {pi}" };
                    }
                }
                else if (pi.PropertyType == typeof(int))
                {
                    int value = (int)pi.GetValue(product);
                    if (value <= 0)
                    {
                         return new MyErrorObject { Error = $"Cannot be null/empty or  <= 0 {pi}" };
                    }
                }
            }
            return new MyErrorObject { Error = null };
        }

        [HttpGet]
        [Route("/ad")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAds()
        {
            List<Product> ads = new List<Product>();
            ads.Add(new Product("title", 10, 10, 10, "lalalaalalalaalala"));
            ads.Add(new Product("title", 10, 10, 10, "lalalaalalalaalala"));
            ads.Add(new Product("title", 10, 10, 10, "lalalaalalalaalala"));
            ads.Add(new Product("title", 10, 10, 10, "lalalaalalalaalala"));
            ads.Add(new Product("title", 10, 10, 10, "lalalaalalalaalala"));

            return Json(ads);
        }

        //[Authorize]
        [Route("/ad")]
        [HttpPost]
        [Consumes("application/json")]
        public IActionResult AddAd([FromBody] Product product)
        {
            MyErrorObject error = CheckAdForErrors(product);
            if (error.Error!=null)
            {
                return BadRequest(error);
            }
            //database stuff add product
            return Json(product);
        }

       
        [HttpPut]
        [Route("/ad")]
        [Produces("application/json")]
        [Consumes("application/json")]
        public IActionResult UpdateAd([FromBody] Product product)
        {
            MyErrorObject error = CheckAdForErrors(product);
            if (error.Error != null)
            {
                return BadRequest(error);
            }
            return Json(product);
        }

        [HttpDelete]
        [Route("/ad/{id}")]
        //[Route("/ad")]
        public IActionResult DeleteAd([FromRoute] long id)
        {
            //if error
            //else ok
            return Json(new { message= $"byebye {id}" });
        }

        [Route("/myget")]
        //[Authorize]
        [HttpGet]
        [Produces("application/json")]
        public Product GetT()
        {
            Product product = new Product("products",1,20,40,"blablabla"); 

            return product;
        }
        


        [Route("/secret")]
        [Authorize]
        public IActionResult Secret()
        {
            var claims = User.Claims.ToList();
            var email = claims.FirstOrDefault(c=>c.Type == ClaimTypes.DateOfBirth)?.Value;
            var role = claims.FirstOrDefault(c=>c.Type == ClaimTypes.Role)?.Value;
            var id = claims.FirstOrDefault(c=>c.Type == ClaimTypes.NameIdentifier)?.Value;
            var username = claims.FirstOrDefault(c=>c.Type == "username")?.Value;
            var usernamee = claims.FirstOrDefault(c=>c.Type == "usernameeee")?.Value;

            return Json(new { secret ="very secret" });
        }



    }
}
