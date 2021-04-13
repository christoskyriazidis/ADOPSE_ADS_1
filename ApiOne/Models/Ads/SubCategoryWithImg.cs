using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Models.Ads
{
    public class SubCategoryWithImg
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
    }
}
