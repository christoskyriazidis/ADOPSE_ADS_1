using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public int Reviews { get; set; }
        public string Username { get; set; }

        public Customer(int id, int reviews, string username)
        {
            Id = id;
            Reviews = reviews;
            Username = username;
        }

        public Customer()
        {
        }
    }
}
