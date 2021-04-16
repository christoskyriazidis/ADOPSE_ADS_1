using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Models.Customer
{
    public class CustomerDetails
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int Id { get; set; }

        [StringLength(25, MinimumLength = 4)]
        [Required]
        public string Name { get; set; }

        [StringLength(25, MinimumLength = 4)]
        [Required]
        public string LastName { get; set; }

        //[StringLength(200, MinimumLength = 15)]
        [Required]
        public string Address { get; set; }

        public string Username { get; set; }
        public string Email { get; set; }
        public string ProfileImg { get; set; }
        public string Reviews { get; set; }
        public int Rating { get; set; }


        public CustomerDetails(int id, string username, string profileImg, string reviews, string name, string lastName, string address, int rating)
        {
            Id = id;
            Username = username;
            ProfileImg = profileImg;
            Reviews = reviews;
            Name = name;
            LastName = lastName;
            Address = address;
            Rating = rating;
        }
        public CustomerDetails()
        {
        }
    }
}
