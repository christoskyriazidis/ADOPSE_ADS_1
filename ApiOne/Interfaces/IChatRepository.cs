using ApiOne.Models.Chats;
using ApiOne.Models.Chats.ReturnObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Interfaces
{
    public interface IChatRepository
    {
        InsertMessageReturn InsertMessage(PostChatMessage ChatMessage,int CustomerId);

        IEnumerable<ChatMessage> GetChatMessages(ChatMessagePagination chatMessagePagination);
        IEnumerable<ActiveChat> GetActiveChats(int cId);
        IEnumerable<ChatRequest> GetChatRequests(int CustomerId);

        bool RequestChatByAdId(int AdId,int BuyerId);
        int AcceptChatRequest(int Rid);
        bool DeclineChatRequest(int Rid);

        

    }
}
