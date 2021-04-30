using ApiOne.Helpers;
using ApiOne.Interfaces;
using ApiOne.Models.Chats;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Repositories
{
    public class ChatRepository : IChatRepository
    {
        public IEnumerable<ChatMessage> GetChatMessages(ChatMessagePagination chatMessagePagination)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "EXEC getMessageFromChat @PageNumber,@ChatId";
                var chatMessages = conn.Query<ChatMessage>(sql, chatMessagePagination).ToList();
                return chatMessages;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return null;
            }
        }

        public IEnumerable<ActiveChat> GetActiveChats(int cId)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "EXEC get_active_chats @CustomerId";
                var chatRooms = conn.Query<ActiveChat>(sql, new { CustomerId=cId }).ToList();
                return chatRooms;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return null;
            }
        }

        public bool InsertMessage(ChatMessage ChatMessage,int CustomerId)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();

                string sql = "INSERT INTO message (message,customerid,chatid) values (@Message,@CustomerId,@ChatId)";

                var result = conn.Query<int>(sql, ChatMessage).FirstOrDefault();
                return true;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return false;
            }
        }

        public bool RequestChatByAdId(int AdId, int BuyerId)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "exec request_chat_byAdID @AdId,@BuyerId";
                var chatMessages = conn.Query<ChatMessage>(sql, new { AdId,BuyerId}).ToList();
                return true;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return false;
            }
        }

        public bool AcceptChatRequest(int Rid)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "update ChatRequest set confirmed=1 where id=@Rid";
                var chatMessages = conn.Query<int>(sql, new { Rid}).FirstOrDefault();
                return true;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return false;
            }
        }

        public IEnumerable<ChatRequest> GetChatRequests(int CustomerId)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "select ch.id,ch.BuyerId,ch.adId,cu.username,cu.profileImg,a.title,ch.Timestamp from ChatRequest ch " +
                    "join customer cu on (ch.BuyerId=cu.id) " +
                    "join ad a on (a.id=ch.adId) where ch.sellerId=@CustomerId and ch.confirmed=0";
                var chatRequests = conn.Query<ChatRequest>(sql, new { CustomerId }).ToList();
                return chatRequests;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return null;
            }
        }
    }
}
