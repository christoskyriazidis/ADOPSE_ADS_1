using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Models.Chats
{
    public class ChatRoom
    {
        public int Id { get; set; }
        public int Bid { get; set; }
        public int Sid { get; set; }
        public string Buyer { get; set; }
        public string Seller { get; set; }
    }
}
