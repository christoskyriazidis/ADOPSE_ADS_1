using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Models.Review
{
    public class PostReview
    {
        [Required]
        public int RatingNumb { get; set; }

        [Required]
        public string ReviewTxt { get; set; }

        [Required]
        public int SoldAd { get; set; }

        [Required]
        public int BuyerId { get; set; }

    }
}
