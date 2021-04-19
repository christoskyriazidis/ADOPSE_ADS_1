using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClientt.viewModels.validationRules.generic;

namespace WpfClientt.viewModels.validationRules {
    public class TitleValidationRule : StringFieldLengthRule {

        public TitleValidationRule() : base("Title",10,72) { 
        
        }

    }
}
