using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DypaApi.Models.Worker
{
    public class Worker
    {
        public float Hourly { get; set; }
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string MobilePhone { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Img { get; set; }
    }
}
