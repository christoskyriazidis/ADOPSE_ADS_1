using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using WpfClientt.model;

namespace WpfClientt.services {
    class ChatPage : IPage<Message> {

        private int pageNumber;
        private IList<Message> messages;

        public ChatPage(int pageNumber,IList<Message> messages) {
            this.pageNumber = pageNumber;
            this.messages = messages;
            foreach (Message message in messages) {
                message.Body = HttpUtility.HtmlDecode(message.Body);
            }
        }

        public int Number() {
            return pageNumber;
        }

        public IList<Message> Objects() {
            return messages;
        }
    }
}
