using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClientt.model {
    public class SubcategoryChangedNotification {

        private Ad ad;
        private string timestamp;

        public Ad Ad { get => ad; set => ad = value; }
        public string Timestamp { get => timestamp; set => timestamp = value; }

        public SubcategoryChangedNotification(Ad ad, string timestamp) {
            this.ad = ad;
            this.timestamp = timestamp;
        }
    }
}
