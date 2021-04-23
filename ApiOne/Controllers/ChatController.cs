using ApiOne.Hubs;
using ApiOne.Interfaces;
using ApiOne.Models.Chats;
using ApiOne.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ApiOne.Controllers
{
    public class ChatController :Controller
    {

        private readonly IChatRepository _chatRepository = new ChatRepository();
        private readonly IHubContext<ChatHub> _chatHub;

        public ChatController(IHubContext<ChatHub> chatHub)
        {
            _chatHub = chatHub;
        }

        [HttpGet]
        [Route("/message")]
        public IActionResult GetMessagesById(ChatMessagePagination chatMessagePagination)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return BadRequest(allErrors);
            }
            var messages = _chatRepository.GetChatMessages(chatMessagePagination);
            if (messages != null)
            {
                return Json(messages);
            }
            return BadRequest(new { message = "something went wrong with chat messages" });
        }

        [HttpPost]
        [Route("/message")]
        public async Task<IActionResult> PostMessage([FromBody] ChatMessage chatMessage)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return BadRequest(allErrors);
            }
            chatMessage.CustomerId = 3;
            chatMessage.Message = HttpUtility.HtmlEncode(chatMessage.Message);
            chatMessage.Message = chatMessage.Message.Replace(" ", "");
            if (_chatRepository.InsertMessage(chatMessage))
            {
                var connections = ChatHub._connections;
                var sockets = connections.GetKeyValuePairs()["admin"];

                foreach(var i in connections.GetKeyValuePairs()["admin"])
                {
                    await _chatHub.Clients.Client(i).SendAsync("ReceiveMessage",chatMessage.Message);
                }
                return Ok(new { success = "message sent!" });
            }
            return BadRequest(new { error = "something went wrong when trying to send message" });
        }

        //[HttpGet]
        //[Route("/chat")]
        //public IActionResult GetChatById(int chatId)
        //{
        //    //int userId = 3;

        //    //if (result == null)
        //    //{
        //    //    return BadRequest(new { error = "customer Out of range" });
        //    //}
        //    //return Json(result);
        //}

        [HttpGet]
        [Route("/profile/chat")]
        public IActionResult GetMyChats()
        {
            int cid = 6;
            var chatRooms = _chatRepository.GetChatRooms(cid);
            if (chatRooms != null)
            {
                return Json(chatRooms);
            }
            return BadRequest(new { error= "Something went wrong with chat"});
        } 

    }
}
