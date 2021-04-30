using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Models.Ads
{
    public class PaginationBSA
    {
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public long TotalAds { get; set; }
        public long TotalPages { get; set; }
        public string NextPageUrl { get; set; }
        public string PreviousPageUrl { get; set; }
        public string LastPageUrl { get; set; }
        public IEnumerable<BoughSoldAds> Result { get; set; }

        public PaginationBSA(int pageSize, int currentPage, long totalAds, long totalPages, string nextPageUrl, string previousPageUrl, string lastPageUrl, IEnumerable<BoughSoldAds> result)
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
        public PaginationBSA()
        {

        }
    }
}
