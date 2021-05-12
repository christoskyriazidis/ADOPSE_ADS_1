using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClientt.model {
    public class Review {

        [Required(ErrorMessage = "The rating is not specified.")]
        [Range(1, 10,ErrorMessage = "The rating must be between 1-10.")]
        public int RatingNumb { get; set; }

        [Required(ErrorMessage = "Review text is not specified.")]
        public string ReviewTxt { get; set; }

        public int SoldAd { get; set; }


    }
}
