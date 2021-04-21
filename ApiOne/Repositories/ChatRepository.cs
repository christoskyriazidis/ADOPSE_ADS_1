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

        public IEnumerable<ChatRoom> GetChatRooms(int cId)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "select  (select username from customer where id = c.seller) as seller," +
                    "(select username from customer where id = c.buyer) as buyer , c.seller as sid ," +
                    " c.buyer as bid, c.id as id from chat c where c.seller =@Seller or buyer = @Buyer";
                var chatRooms = conn.Query<ChatRoom>(sql, new { Seller = cId ,Buyer = cId }).ToList();
                return chatRooms;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return null;
            }
        }

        public bool InsertMessage(ChatMessage ChatMessage)
        {
            try
            {
                using SqlConnection conn = ConnectionManager.GetSqlConnection();
                string sql = "INSERT INTO message (message,customer,chat) values (@Message,@CustomerId,@ChatId)";
                var result = conn.Query<int>(sql, ChatMessage).FirstOrDefault();
                return true;
            }
            catch (SqlException sqlEx)
            {
                Debug.WriteLine(sqlEx);
                return false;
            }
        }

        public bool MakeChat(int buyerId, int SellerId)
        {
            throw new NotImplementedException();
        }
    }
}
