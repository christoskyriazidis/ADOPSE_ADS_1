using ApiOne.Models.Chats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Interfaces
{
    public interface IChatRepository
    {
        bool InsertMessage(ChatMessage ChatMessage);
        bool MakeChat(int buyerId,int SellerId);
        IEnumerable<ChatMessage> GetChatMessages(ChatMessagePagination chatMessagePagination);
        IEnumerable<ChatRoom> GetChatRooms(int cId);

    }
}
