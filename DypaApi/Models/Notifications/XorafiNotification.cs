using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DypaApi.Models.Notifications
{
    public class XorafiNotification
    {
        public int Id { get; set; }
        public int XorafiId { get; set; }
        public string Timestamp { get; set; }
        public string Title { get; set; }
        public string LocationTitle { get; set; }
        public int CurrentSoilHum { get; set; }
        public int LowestSoilHum { get; set; }
    }
}
