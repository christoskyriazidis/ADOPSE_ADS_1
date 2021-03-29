using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Models
{
    public class CustomerPreview
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string ProfileImg { get; set; }
        public int Rating { get; set; }

        public CustomerPreview(int id, string username, string profileImg, int rating)
        {
            Id = id;
            Username = username;
            ProfileImg = profileImg;
            Rating = rating;
        }

        public CustomerPreview()
        {
        }
    }
}
