using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Models.Customer
{
    public class LoginLogs
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Timestamp { get; set; }
        public string IpAddress { get; set; }
    }
}
