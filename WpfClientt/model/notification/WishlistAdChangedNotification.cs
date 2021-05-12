using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClientt.model {
    public class WishlistAdChangedNotification : AdNotification {

        private Ad ad;
        private string timestamp;

        public Ad Ad { get => ad; set => ad = value; }
        public string Timestamp { get => timestamp; set => timestamp = value; }

        public WishlistAdChangedNotification(Ad ad, string timestamp) {
            this.ad = ad;
            this.timestamp = timestamp;
        }

    }
}
