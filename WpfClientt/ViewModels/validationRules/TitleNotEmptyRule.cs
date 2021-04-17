using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClientt.viewModels.validationRules {
    public class TitleNotEmptyRule : StringNotEmptyRule {

        public TitleNotEmptyRule() : base("title") { 
        
        }

    }
}
