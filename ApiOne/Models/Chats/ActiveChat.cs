using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Models.Chats
{
    public class ActiveChat
    {
        public int Id { get; set; }
        public int customerId { get; set; }
        public int AdId { get; set; }
        public bool Sold { get; set; }
        public string ProfileImg { get; set; }
        public string Username { get; set; }
        public string Type { get; set; }
        public string LatestMessage { get; set; }

    }
}
