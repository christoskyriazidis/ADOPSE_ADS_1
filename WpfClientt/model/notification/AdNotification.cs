using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClientt.model {
    public interface AdNotification {

        Ad Ad { get; set; }

        string Timestamp { get; set; }

    }
}
