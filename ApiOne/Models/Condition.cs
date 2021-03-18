using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Models
{
    public class Condition
    {
        public int id { get; set; }
        public string title { get; set; }

        public Condition(int id, string title)
        {
            this.id = id;
            this.title = title;
        }

        public Condition()
        {
        }
    }
}
