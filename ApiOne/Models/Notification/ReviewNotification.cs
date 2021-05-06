using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Models.Notification
{
    public class ReviewNotification
    {
            public int AdId { get; set; }
            public int CustomerId { get; set; }
            public string Username { get; set; }
            public string Timestamp { get; set; }
    }
}
