using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClientt.viewModels {
    /// <summary>
    /// MinMax price filter that allows user to choose values in range of long values.
    /// </summary>
    public class MinMaxPriceFilterMember : MinMaxFilterMember<int> {

        public MinMaxPriceFilterMember(Action<int> minAction,string title, Action<int> maxAction) 
            : base(minAction,title, maxAction) {
            
        }

    }
}
