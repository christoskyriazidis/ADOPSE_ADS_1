using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClientt.model {
    public class ChatRequest {

        public int Id { get; set; }

        public Customer Buyer { get; set; }

        public Ad Ad { get; set; }

        public string Timestamp { get; set; }

    }
}
