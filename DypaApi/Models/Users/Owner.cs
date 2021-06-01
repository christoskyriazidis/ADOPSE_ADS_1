using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DypaApi.Models.Users
{
    public class Owner
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Address { get; set; }
        public string coords { get; set; }
        public string SubId { get; set; }
        public string MobilePhone { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public int Role { get; set; }
        public string RoleTitle { get; set; }
        public string Img { get; set; }
    }
}
