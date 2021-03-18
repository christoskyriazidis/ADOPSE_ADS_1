using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Models
{
    public class Customer
    {
        public int id { get; set; }
        public int reviews { get; set; }
        public string username { get; set; }

        public Customer(int id, int reviews, string username)
        {
            this.id = id;
            this.reviews = reviews;
            this.username = username;
        }

        public Customer()
        {
        }
    }
}
