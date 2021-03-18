using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Models
{
    public class Category
    {
        public int id { get; set; }
        public string title { get; set; }

        public Category(int id, string title)
        {
            this.id = id;
            this.title = title;
        }

        public Category()
        {
        }
    }
}
