using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClientt.model.chat;

namespace WpfClientt.services {
    public interface IChatService {

        Task SendMessage(Message message);

        void AddMessageListener(Action<Message> listener);

        void RemoveMessageListener(Action<Message> listener);

        Task Typing();

        Task<ISet<Chat>> Chats();

        IScroller<Message> Messages(int chatId);

    }
}
