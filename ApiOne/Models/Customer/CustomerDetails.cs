using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Models.Customer
{
    public class CustomerDetails
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string ProfileImg { get; set; }
        public string Reviews { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
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
