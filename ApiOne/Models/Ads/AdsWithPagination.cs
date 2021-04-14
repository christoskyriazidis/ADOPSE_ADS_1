using ApiOne.Models.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Models.Ads
{
    public class AdsWithPagination
    {
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public long TotalAds { get; set; }
        public long TotalPages { get; set; }
        public string NextPageUrl { get; set; }
        public string PreviousPageUrl { get; set; }
        public string LastPageUrl { get; set; }
        public IEnumerable<CompleteAd> Result { get; set; }

        public AdsWithPagination(int pageSize, int currentPage, long totalAds, int totalPages, string nextPageUrl, string previousPageUrl, string lastPageUrl, List<CompleteAd> result)
        {
            PageSize = pageSize;
            CurrentPage = currentPage;
            TotalAds = totalAds;
            TotalPages = totalPages;
            NextPageUrl = nextPageUrl;
            PreviousPageUrl = previousPageUrl;
            LastPageUrl = lastPageUrl;
            Result = result;
        }

        public AdsWithPagination()
        {

        }
    }
}
