using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Models.Notification
{
    public class WishSubNotification
    {
        public int Id { get; set; }
        public string Timestamp { get; set; }
        public string Username { get; set; }
        public int Clicked { get; set; }
        public int AdId { get; set; }
        public int Price { get; set; }
        public int CustomerId { get; set; }
        public int Sold { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }

        public string Img { get; set; }

    }
}
