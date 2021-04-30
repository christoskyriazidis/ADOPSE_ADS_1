using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Models.Chats
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public int  Customer { get; set; }
        public int ActiveChat { get; set; }
        public string Timestamp { get; set; }
        public string Username { get; set; }
        public string ProfileImg { get; set; }
        public string SubId { get; set; }
        
    }
}
