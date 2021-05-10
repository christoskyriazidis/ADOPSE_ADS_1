using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Models.Admin
{
    public class InsertCategory
    {
        [Required]
        public IFormFile Img { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string Title { get; set; }

        
    }
}
