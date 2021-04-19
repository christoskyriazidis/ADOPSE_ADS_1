using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfClientt.viewModels.validationRules.generic {
    public class StringFieldLengthRule : StringFieldNotEmptyRule {

        private int minimum, maximum;

        public StringFieldLengthRule(string fieldName, int minimum, int maximum) : base(fieldName) {
            this.minimum = minimum;
            this.maximum = maximum;
            if(minimum > maximum) {
                throw new ArgumentException($"Minimum({minimum}) should not be greater than maximum({maximum})");
            }
        }

        public sealed override ValidationResult Validate(object value, CultureInfo cultureInfo) {
            ValidationResult parentValidation = base.Validate(value, cultureInfo);

            if (!parentValidation.IsValid) { return parentValidation; }

            string strValue = (string)value;

            if(strValue.Length < minimum || strValue.Length > maximum) {
                return new ValidationResult(false, $"The value of the {fieldName} should be between {minimum} - {maximum}");
            }

            return new ValidationResult(true, null);
        }
    }
}
