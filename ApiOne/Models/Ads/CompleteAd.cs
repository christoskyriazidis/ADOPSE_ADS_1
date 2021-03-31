using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiOne.Models.Ads
{
    public class CompleteAd
    {
        public int Id { get; set; }
        public int Views { get; set; }
        public int Type { get; set; }
        public int Category { get; set; }
        public int Condition { get; set; }
        public int Customer { get; set; }
        public int Manufacturer{ get; set; }
        public int Reports{ get; set; }
        public int Price { get; set; }
        public string Title { get; set; }
        public string Lastupdate { get; set; }
        public string Createdate { get; set; }
        public string Description { get; set; }
        public int State { get; set; }
        public string Img { get; set; }
        public CompleteAd()
        {
                
        }

        public CompleteAd(int id, int views, int type, int category, int condition, int customer, int manufacturer, int reports, int price, string title, string lastupdate, string createdate, string description, int state, string img)
        {
            Id = id;
            Views = views;
            Type = type;
            Category = category;
            Condition = condition;
            Customer = customer;
            Manufacturer = manufacturer;
            Reports = reports;
            Price = price;
            Title = title;
            Lastupdate = lastupdate;
            Createdate = createdate;
            Description = description;
            State = state;
            Img = img;
        }
    }
}
