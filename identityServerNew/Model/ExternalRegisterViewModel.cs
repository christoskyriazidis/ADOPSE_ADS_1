using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace identityServerNew.Model
{
    public class ExternalRegisterViewModel
    {
        [Required]
        public string Username { get; set; }
        public string ReturnUrl { get; set; }
    }
}
 