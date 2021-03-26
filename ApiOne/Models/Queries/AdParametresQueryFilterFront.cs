using ApiOne.Models.Ads;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Models.Queries
{
    public class AdParametresQueryFilterFront
    {
        public AdPageSizeNumberParameters Params { get; set; }
        public IEnumerable Type { get; set; }
        public IEnumerable Category { get; set; }
        public IEnumerable Manufacturer { get; set; }
        public IEnumerable State { get; set; }
        public IEnumerable Condition { get; set; }
        public int TotalAds { get; set; }
        public int TotalPages { get; set; }
        public string NextPageUrl { get; set; }
        public string PreviousPageUrl { get; set; }
        public string LastPageUrl { get; set; }
        public string CurrentPageUrl { get; set; }
        public List<Ad> Ads { get; set; }

        public AdParametresQueryFilterFront()
        {

        }

        
    }
}
