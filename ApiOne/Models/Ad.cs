using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Models
{
    public class Ad
    {
        [Required(ErrorMessage = "Id is required")]
        [Range(1, 5000000)]
        public int Id { get; set; }

        //[StringLength(15, ErrorMessage = "{0} length must be between {15} and {100}.", MinimumLength = 5)]
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [StringLength(100, ErrorMessage = "{0} length must be between {15} and {100}.", MinimumLength = 15)]
        [Required(ErrorMessage = "Description is required (min 15 charactes)")]
        public string Description { get; set; }

        [Range(1, 4)]
        [Required(ErrorMessage = "State is required (min 15 charactes)")]
        public int State { get; set; }

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

        [Required]
        public int SubCategoryId { get; set; }

        [Range(1, 10000)]
        [Required(ErrorMessage = "Price is not null")]
        public int Price { get; set; }

        public string LastUpdate { get; set; }
        public int Reports { get; set; }
        public int Views { get; set; }
        public string CreateDate { get; set; }
        public string Img { get; set; }
        public int Customer { get; set; }
        public Ad()
        {
        }

        public Ad(int id, string title, string description, int state, int type, int category, int condition, int manufacturer, int subCategoryId, int price, string lastUpdate, int reports, int views, string createDate, string img, int customer)
        {
            Id = id;
            Title = title;
            Description = description;
            State = state;
            Type = type;
            Category = category;
            Condition = condition;
            Manufacturer = manufacturer;
            SubCategoryId = subCategoryId;
            Price = price;
            LastUpdate = lastUpdate;
            Reports = reports;
            Views = views;
            CreateDate = createDate;
            Img = img;
            Customer = customer;
        }
    }



}
