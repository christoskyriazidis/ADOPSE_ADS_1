using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Models.Ads
{
    public class DeleteAdsFromWishList
    {
        [Required]
        public int[] AdIds { get; set; }

    }
}
