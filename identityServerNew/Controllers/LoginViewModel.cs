using Microsoft.AspNetCore.Authentication;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace identityServerNew.Controllers
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }

        //[DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }

        public IEnumerable<AuthenticationScheme>  ExternalProviders { get; set; }
    }
}