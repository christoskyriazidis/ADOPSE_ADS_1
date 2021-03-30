using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClientt.viewModels.filters {
    public sealed class Choice {
        public string Title { get; private set; }

        public long Code { get; private set; }

        public bool Selected { 
            get; 
            set; 
        } = false;

        public Choice(string title, long code) {
            Title = title;
            Code = code;
        }
    }
}
