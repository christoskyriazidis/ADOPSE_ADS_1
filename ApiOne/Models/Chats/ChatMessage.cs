using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Models.Chats
{
    public class ChatMessage
    {
        public string Message { get; set; }
        public int CustomerId { get; set; }
        public int ChatId { get; set; }

        
    }
}
