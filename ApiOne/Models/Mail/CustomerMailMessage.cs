using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Models.Mail
{
    public class CustomerMailMessage
    {
        [StringLength(500, MinimumLength = 10)]
        [Required]
        public string Message { get; set; }

        [Range(1, int.MaxValue)]
        [Required]
        public int AdId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int SellerId { get; set; }
    }
}
