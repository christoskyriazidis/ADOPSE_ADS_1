using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace identityServerNew.Model
{
    public class ResetPassword
    {
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }

        [Required]
        public string token { get; set; }
        
        [Required]
        public string userId { get; set; }

    }
}
