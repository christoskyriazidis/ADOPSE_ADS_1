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

        public string username { get; set; }
        public string profileimg { get; set; }
        public string subid { get; set; }
        public string coords { get; set; }
        public int rating { get; set; }
        public int reviews { get; set; }
        public double distance{ get; set; }


        public int subcategoryid { get; set; }


        public CompleteAd()
        {
                
        }

        public CompleteAd(int id, int views, int type, int category, int condition, int customer, int manufacturer, int reports, int price, string title, string lastupdate, string createdate, string description, int state, string img, string username, string profileimg, int rating, int reviews, int subcategoryid)
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
            this.username = username;
            this.profileimg = profileimg;
            this.rating = rating;
            this.reviews = reviews;
            this.subcategoryid = subcategoryid;
        }
    }
}
