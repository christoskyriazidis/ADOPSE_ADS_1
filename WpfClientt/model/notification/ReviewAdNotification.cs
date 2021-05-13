using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClientt.model {
    public class ReviewAdNotification : AdNotification {

        public Ad Ad { get; set; }
        public Customer AdOwner { get; set; }
        public string Timestamp { get; set; }

        public ReviewAdNotification(Ad ad,string timestamp,Customer adOwner) {
            Ad = ad;
            Timestamp = timestamp;
            AdOwner = adOwner;
        }

    }
}
