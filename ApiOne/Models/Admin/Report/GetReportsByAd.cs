using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Models.Admin.Report
{
    public class GetReportsByAd
    {
        public string Timestamp { get; set; }
        public string ReportText { get; set; }
        public int CustomerId { get; set; }
        public string Title { get; set; }
        public string Username { get; set; }

    }
}
