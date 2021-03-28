using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Models
{
    public class WishListNotification
    {
        public int AdId { get; set; }
        public string Username { get; set; }
        public string Img { get; set; }
        public string Title { get; set; }
        public string LastUpdate { get; set; }
        public bool Clicked { get; set; }
        public int CustomerId { get; set; }
        public int Price { get; set; }


    }
}
