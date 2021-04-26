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
        [RegularExpression(@"([0-9]|[A-Za-z]|_)+", ErrorMessage = "You can use abc 123")]
        public string Username { get; set; }
        public string ReturnUrl { get; set; }
    }
}
 