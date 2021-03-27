using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Models
{
    public class CategoryNotification
    {
        public int AdId { get; set; }
        public string Username { get; set; }
        public string Img { get; set; }
        public string Title { get; set; }
        public bool Clicked { get; set; }
        public int CategoryId { get; set; }
    }
}
