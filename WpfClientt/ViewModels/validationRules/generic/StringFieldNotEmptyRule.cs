using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfClientt.viewModels.validationRules.generic {
    public abstract class StringFieldNotEmptyRule : FieldNotNullRule {


        public StringFieldNotEmptyRule(string fieldName):base(fieldName) {
        }


        public override ValidationResult Validate(object value, CultureInfo cultureInfo) {
            ValidationResult parentValidation = base.Validate(value, cultureInfo);
            if (!parentValidation.IsValid) {
                return parentValidation;
            }
            if(!(value is string) || ((string)value).Trim().Length== 0) {
                return new ValidationResult(false, $"Field {fieldName} cannot be empty.");
            }
            return new ValidationResult(true, null);
        }
    }
}
