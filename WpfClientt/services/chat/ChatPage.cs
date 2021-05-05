using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClientt.model;

namespace WpfClientt.services {
    class ChatPage : IPage<Message> {

        private int pageNumber;
        private IList<Message> messages;

        public ChatPage(int pageNumber,IList<Message> messages) {
            this.pageNumber = pageNumber;
            this.messages = messages;
        }

        public int Number() {
            return pageNumber;
        }

        public IList<Message> Objects() {
            return messages;
        }
    }
}
