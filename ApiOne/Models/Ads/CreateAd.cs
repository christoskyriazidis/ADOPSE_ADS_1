using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Models.Ads
{
    public class CreateAd
    {
        public IFormFile Img { set; get; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        //[StringLength(100, ErrorMessage = "{0} length must be between {15} and {100}.", MinimumLength = 15)]
        [Required(ErrorMessage = "Description is required (min 15 charactes)")]
        public string Description { get; set; }

        [Range(1, 4)]
        [Required(ErrorMessage = "Type is required")]
        public int Type { get; set; }

        [Range(1, 5)]
        [Required(ErrorMessage = "Category is required")]
        public int Category { get; set; }

        [Range(1, 3)]
        [Required(ErrorMessage = "Condition is required")]
        public int Condition { get; set; }

        [Range(1, 100)]
        [Required(ErrorMessage = "Manufacturer is required")]
        public int Manufacturer { get; set; }

        [Range(1, 10000)]
        [Required(ErrorMessage = "Price is not null")]

        public int Price { get; set; }
        public int State { get; set; }
        public string LastUpdate { get; set; }
        public string CreateDate { get; set; }
        public int Customer { get; set; }
    }
}
