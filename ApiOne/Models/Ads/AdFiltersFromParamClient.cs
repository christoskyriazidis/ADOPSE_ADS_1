using ApiOne.Models.Queries;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Models.Ads
{
    public class AdFiltersFromParamClient
    {
        public Pagination Params { get; set; }
        public IEnumerable<int> Type { get; set; }
        public IEnumerable<int> Category { get; set; }
        public IEnumerable<int> SubCategory { get; set; }
        public IEnumerable<int> Manufacturer { get; set; }
        public IEnumerable<int> State { get; set; }
        public IEnumerable<int> Condition { get; set; }
        public int TotalAds { get; set; }
        public int TotalPages { get; set; }
        public string NextPageUrl { get; set; }
        public string PreviousPageUrl { get; set; }
        public string LastPageUrl { get; set; }
        public string CurrentPageUrl { get; set; }
        public List<CompleteAd> Ads { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }

        public AdFiltersFromParamClient()
        {
        }

        public AdFiltersFromParamClient(Pagination @params)
        {
            Params = @params;
        }
    }
}
