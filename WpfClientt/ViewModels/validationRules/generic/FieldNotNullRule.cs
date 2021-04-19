using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfClientt.viewModels.validationRules.generic {
    public class FieldNotNullRule : ValidationRule {

        protected string fieldName;

        public FieldNotNullRule(string fieldName) {
            base.ValidationStep = ValidationStep.ConvertedProposedValue;//runs the validation after converstion
            this.fieldName = fieldName;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo) {
            if(value == null) {
                return new ValidationResult(false, $"The field {fieldName} is not specified.");
            }
            throw new NotImplementedException();
        }
    }
}
