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
        /// Adds a listener for new messages that arrive.
        /// </summary>
        /// <param name="listener"></param>
        void AddMessageListener(Action<Message> listener);

        /// <summary>
        /// Removes message listener.
        /// </summary>
        /// <param name="listener"></param>
        void RemoveMessageListener(Action<Message> listener);

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
        /// Returns all the messages of the chat that has the given id.
        /// </summary>
        /// <param name="chatId"></param>
        /// <returns></returns>
        IScroller<Message> Messages(int chatId);

    }
}
