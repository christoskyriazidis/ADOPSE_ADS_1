using ApiOne.Hubs;
using ApiOne.Interfaces;
using ApiOne.Models.Chats;
using ApiOne.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ApiOne.Controllers
{
    public class ChatController :Controller
    {

        private readonly IChatRepository _chatRepository = new ChatRepository();
        private readonly ICustomerRepository _customerRepo = new CustomerRepository();
        private readonly IHubContext<NotificationHub> _notificationHub;

        private readonly IHubContext<ChatHub> _chatHub;

        public ChatController(IHubContext<ChatHub> chatHub, IHubContext<NotificationHub> hubContext)
        {
            _chatHub = chatHub;
            _notificationHub = hubContext;
        }


        [HttpGet]
        [Route("/message")]
        public IActionResult GetChatMessagesByChatId([FromQuery] ChatMessagePagination chatMessagePagination)
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

        [Authorize]
        [HttpPost]
        [Route("/message")]
        public async Task<IActionResult> SendChatMessage([FromBody] PostChatMessage chatMessage)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return BadRequest(allErrors);
            }
            var claims = User.Claims.ToList();
            var subId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var username = claims.FirstOrDefault(c => c.Type =="username")?.Value;
            var intId = _customerRepo.GetCustomerIdFromSub(subId);
            //html injection prevent
            chatMessage.MessageText = HttpUtility.HtmlEncode(chatMessage.MessageText);
            //insert message if(true)>>> push message with signalR 
            var insertResponse = _chatRepository.InsertMessage(chatMessage, intId);
            if (string.IsNullOrEmpty(insertResponse.Error))
            {
                foreach (var connectionId in ChatHub._connections.GetConnections(insertResponse.Username))
                {
                    await _chatHub.Clients.Client(connectionId).SendAsync("ReceiveMessage", chatMessage.ActiveChat);
                }
                return Ok(new { success = $"message sent! to {insertResponse.Username}" });
            }
            return BadRequest(new { error = $"{insertResponse.Error}" });
        }


        [Authorize]
        [HttpGet]
        [Route("/chat/chatrequest")]
        public IActionResult GetChatRequests()
        {
            var claims = User.Claims.ToList();
            var subId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var intId = _customerRepo.GetCustomerIdFromSub(subId);
            var chatRequests = _chatRepository.GetChatRequests(intId);
            if (chatRequests != null)
            {
                return Json(chatRequests);
            }
            return BadRequest(new { message = "You do not have any chat requests yet" });
        }


        [Authorize]
        [HttpPost]
        [Route("/chat/chatrequest/{AdId}")]
        public async Task<IActionResult> RequestChat(int AdId)
        {
            var claims = User.Claims.ToList();
            var subId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var intId = _customerRepo.GetCustomerIdFromSub(subId);
            if (_chatRepository.RequestChatByAdId(AdId, intId))
            {
                await _chatHub.Clients.All.SendAsync("ReceiveChatRequest",subId);
                return Json(new {response="Chat request submitted"});
            }
            await _notificationHub.Clients.All.SendAsync("ChatRequestNotification");
            return BadRequest(new { message= "Chat request ALREADY submitted " });
        }
        
        [Authorize]
        [HttpPost]
        [Route("/chat/chatrequest/confirm/{ChatId}")]
        public async Task<IActionResult> ConfirmChatRequest(int ChatId)
        {
            int activeChatId= _chatRepository.AcceptChatRequest(ChatId);
            var claims = User.Claims.ToList();
            var subId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (activeChatId != -1)
            {
                await _chatHub.Clients.All.SendAsync("ReceiveActiveChat", subId);
                return Json(new { response = $"{ChatId} accepted" });
            }
            return BadRequest(new { message="kati pige lathos"});
        } 
        
        //[Authorize]
        //[HttpPost]
        //[Route("/chat/chatrequest/decline/{ChatId}")]
        //public async Task<IActionResult> DeclineChatRequest(int ChatId)
        //{
        //    if (_chatRepository.AcceptChatRequest(ChatId))
        //    {
        //        await _chatHub.Clients.All.SendAsync("ReceiveActiveChat", $" ActiveChatId:{ChatId}");
        //        return Json(new { response = $"{ChatId} accepted" });
        //    }
        //    return BadRequest(new { message="kati pige lathos"});
        //}
        
        [Authorize]
        [HttpGet]
        [Route("/activechat")]
        public IActionResult GetActiveChats()
        {
            var claims = User.Claims.ToList();
            var subId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var intId = _customerRepo.GetCustomerIdFromSub(subId);
            var activeChats = _chatRepository.GetActiveChats(intId);
            if (activeChats!=null)
            {
                return Json(activeChats);
            }
            return BadRequest(new { message="kati pige lathos me to active chat"});
        }


        [HttpGet]
        [Route("/profile/Achat")]
        public IActionResult GetMyChats()
        {
            //int cid = 6;
            //var chatRooms = _chatRepository.GetChatRooms(cid);
            //if (chatRooms != null)
            //{
            //    return Json(chatRooms);
            //}
            return BadRequest(new { error= "Something went wrong with chat"});
        } 



    }
}
