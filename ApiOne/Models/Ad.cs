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
        public int Id { get; set; }

        [StringLength(15)]
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [StringLength(100, ErrorMessage = "{0} length must be between {15} and {100}.", MinimumLength = 15)]
        [Required(ErrorMessage = "Description is required (min 15 charactes)")]
        public string Description { get; set; }

        [Range(1, 4)]
        [Required(ErrorMessage = "State is required (min 15 charactes)")]
        public int State { get; set; }

        [Required(ErrorMessage = "Img is required")]
        public string Img { get; set; }

        [Range(1, 4)]
        [Required(ErrorMessage = "Type is required")]
        public int Type { get; set; }

        [Range(1, 5)]
        [Required(ErrorMessage = "Category is required")]
        public int Category { get; set; }

        [Range(1, 3)]
        [Required(ErrorMessage = "Condition is required")]
        public int Condition { get; set; }

        [Range(1, 10000)]
        [Required(ErrorMessage = "Customer is required")]
        public int Customer { get; set; }

        [Range(1, 100)]
        [Required(ErrorMessage = "Manufacturer is required")]
        public int Manufacturer { get; set; }

        public string LastUpdate { get; set; }
        public int Reports { get; set; }
        public int Views { get; set; }
        public string Date { get; set; }

        public Ad()
        {
        }

        public Ad(int id, string title, string description, string date, int state, string img, int views, int type, int category, int condition, int customer, int manufacturer, int reports)
        {
            Id = id;
            Title = title;
            Description = description;
            Date = date;
            State = state;
            Img = img;
            Views = views;
            Type = type;
            Category = category;
            Condition = condition;
            Customer = customer;
            Manufacturer = manufacturer;
            Reports = reports;
        }
    }



}
