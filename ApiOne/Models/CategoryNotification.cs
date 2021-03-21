using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Models
{
    public class CategoryNotification
    {
        public int Id { get; }
        public int CategoryId { get; set; }
        public int CustomerId { get; set; }
        public bool Clicked { get; set; }
        public int AdId { get; set; }
    }
}
