using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Models.Ads
{
    public class BoughSoldAds
    {
        public int Id { get; set; }
        public string Img{ get; set; }
        public string Username{ get; set; }
        public string ProfileImg{ get; set; }
        public string TransactionDate{ get; set; }
        public string Title{ get; set; }
        public int CustomerId{ get; set; }
        public int AdId{ get; set; }
        public int Price{ get; set; }
        public int reviews{ get; set; }
        public int rating{ get; set; }

    }
}
