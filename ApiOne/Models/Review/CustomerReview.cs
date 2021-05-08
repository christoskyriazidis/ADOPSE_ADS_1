using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Models.Review
{
    public class CustomerReview
    {
        public int Id { get; set; }
        public string ReviewText { get; set; }
        public int Rating { get; set; }
        public int SellerId { get; set; }
        public int BuyerId { get; set; }
        public int SoldAd { get; set; }
        public string ReviewDate { get; set; }
        public string Username { get; set; }
        public string ProfileImg { get; set; }
    }
}
