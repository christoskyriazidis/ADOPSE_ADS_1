using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClientt.viewModels.filters {
    public class MinMaxPriceFilterMember : MinMaxFilterMember<long> {

        public MinMaxPriceFilterMember(string title, Action<long, long> onFinish) : base(title,onFinish) {
            
        }

    }
}
