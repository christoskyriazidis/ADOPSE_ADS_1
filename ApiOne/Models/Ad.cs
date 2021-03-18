using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Models
{
    public class Ad
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
        public int State { get; set; }
        public string Img { get; set; }
        public int Views { get; set; }
        public int Type { get; set; }
        public int Category { get; set; }
        public int Condition { get; set; }
        public int Customer { get; set; }
        public int Manufacturer { get; set; }
        public int Reports { get; set; }

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
