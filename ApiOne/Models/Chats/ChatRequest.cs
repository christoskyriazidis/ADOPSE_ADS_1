using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Models.Chats
{
    public class ChatRequest
    {
        public int Id { get; set; }
        public int BuyerId { get; set; }
        public int AdId { get; set; }
        public string Username { get; set; }
        public string ProfileImg { get; set; }
        public string Timestamp { get; set; }


    }
}
