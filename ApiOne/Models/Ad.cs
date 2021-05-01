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
        [Required]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [StringLength(72, MinimumLength = 10)]
        [Required]
        public string Title { get; set; }

        [StringLength(1024, MinimumLength = 20)]
        [Required]
        public string Description { get; set; }

        [Range(1, 4)]
        [Required]
        public int State { get; set; }

        [Range(1, 4)]
        [Required]
        public int Type { get; set; }

        [Range(1, 5)]
        [Required]
        public int Category { get; set; }

        [Range(1, 3)]
        [Required]
        public int Condition { get; set; }

        [Range(1, 100)]
        [Required]
        public int Manufacturer { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int SubCategoryId { get; set; }

        [Range(1, 10000)]
        [Required]
        public int Price { get; set; }

        public string LastUpdate { get; set; }
        public int Reports { get; set; }
        public int Views { get; set; }
        public string CreateDate { get; set; }
        public string Img { get; set; }
        public string ProfileImg { get; set; }
        public string Username { get; set; }
        public int Rating { get; set; }
        public int Reviews { get; set; }
        public string Address { get; set; }

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
