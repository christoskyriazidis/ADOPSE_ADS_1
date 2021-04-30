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
        bool InsertMessage(ChatMessage ChatMessage,int CustomerId);
        IEnumerable<ChatMessage> GetChatMessages(ChatMessagePagination chatMessagePagination);
        IEnumerable<ActiveChat> GetActiveChats(int cId);


        IEnumerable<ChatRequest> GetChatRequests(int CustomerId);
        bool RequestChatByAdId(int AdId,int BuyerId);

        bool AcceptChatRequest(int Rid);
        

    }
}
