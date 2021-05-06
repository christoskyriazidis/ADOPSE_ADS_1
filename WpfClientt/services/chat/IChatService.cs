using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClientt.model;

namespace WpfClientt.services {
    /// <summary>
    /// Service for chatting.
    /// </summary>
    public interface IChatService {

        /// <summary>
        /// Sends new message.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task SendMessage(Message message);

        /// <summary>
        /// Adds a listener that is being notified when a new message arrives.
        /// </summary>
        /// <param name="listener"></param>
        void AddMessageListener(Func<Task> listener);

        /// <summary>
        /// Removes listener that was registreted in AddMessageListener.
        /// </summary>
        /// <param name="listener"></param>
        void RemoveMessageListener(Func<Task> listener);

        /// <summary>
        /// Adds a listener that receives notifications upon the arriaval of new active chat.
        /// The listener get the instance of the new active chat.
        /// </summary>
        /// <param name="listener"></param>
        void AddActiveChatListener(Func<Chat, Task> listener);

        /// <summary>
        /// Removes a listener that was added in AddActiveChatListener
        /// </summary>
        /// <param name="listener"></param>
        void RemoveActiveChatListener(Func<Chat, Task> listener);

        /// <summary>
        /// Adds a listener that receives notifications upon the arriaval of new chat requests.
        /// </summary>
        /// <param name="listener"></param>
        void AddChatRequestListener(Func<Task> listener);

        /// <summary>
        /// Removes chat request listener registered through AddChatRequestListener
        /// </summary>
        /// <param name="listener"></param>
        void RemoveChatRequestListener(Func<Task> listener);

        /// <summary>
        /// Sends an event that the user is typing.
        /// </summary>
        /// <returns></returns>
        Task Typing();

        /// <summary>
        /// Returns all the chats of the logged in customer.
        /// </summary>
        /// <returns></returns>
        Task<ISet<Chat>> Chats();

        /// <summary>
        /// Returns all the messages of the chat.
        /// </summary>
        /// <param name="chat"></param>
        /// <returns></returns>
        IScroller<Message> Messages(Chat chat);

        /// <summary>
        /// Returns the requested chats by other customers to the logged in customer.
        /// </summary>
        /// <returns></returns>
        Task<ISet<ChatRequest>> ChatRequests();

        /// <summary>
        /// Sends a new chat request to the customer that created the given ad.
        /// </summary>
        /// <param name="ad">The ad for wich the chat is created.</param>
        /// <returns></returns>
        Task SendChatRequest(Ad ad);

        /// <summary>
        /// Confirms the chat request.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task AcceptChatRequest(ChatRequest request);

        /// <summary>
        /// Declines the chat reuqest.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task DeclineChatRequest(ChatRequest request);

    }
}
