using ApiOne.Models.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Models.Ads
{
    public class AdPagination
    {
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalAds { get; set; }
        public int TotalPages { get; set; }
        public string NextPageUrl { get; set; }
        public string PreviousPageUrl { get; set; }
        public string LastPageUrl { get; set; }
        public List<Ad> Ads { get; set; }

        public AdPagination(int pageSize, int currentPage, int totalAds, int totalPages, string nextPageUrl, string previousPageUrl, string lastPageUrl, List<Ad> ads)
        {
            PageSize = pageSize;
            CurrentPage = currentPage;
            TotalAds = totalAds;
            TotalPages = totalPages;
            NextPageUrl = nextPageUrl;
            PreviousPageUrl = previousPageUrl;
            LastPageUrl = lastPageUrl;
            Ads = ads;
        }

        public AdPagination()
        {

        }
    }
}
