using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Models
{
    public class Ad
    {
        public string title { get; set; }
        public int quantity { get; set; }
        public int price { get; set; }
        public int size { get; set; }
        public string description { get; set; }

        public Ad(string title, int quantity, int price, int size, string description)
        {
            this.title = title;
            this.quantity = quantity;
            this.price = price;
            this.size = size;
            this.description = description;
        }

        public Ad()
        {

        }
       
    }
}
