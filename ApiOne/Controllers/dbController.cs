using ApiOne.Databases;
using ApiOne.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Controllers
{
    public class dbController : Controller
    {


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
            if (error.Error != null)
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
        public IActionResult DeleteAd([FromRoute] long id)
        {
            //if error
            //else ok
            return Json(new { message = $"byebye {id}" });
        }

    }
}
