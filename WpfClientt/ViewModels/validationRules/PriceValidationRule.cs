using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClientt.viewModels.validationRules.generic;

namespace WpfClientt.viewModels.validationRules {
    public class PriceValidationRule : IntFieldRangeRule {
        public PriceValidationRule() : base("Price", 1, 100000) {
        }
    }
}
