using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClientt.model {
    public class Mapping {
    
        public long id { get; set; }

        public string title { get; set; }

        public override string ToString() {
            return $"id = {id},title = {title}";
        }
    }
}
