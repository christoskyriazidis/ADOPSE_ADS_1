using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfClientt.viewModels.validationRules.generic {
    public class IntFieldRangeRule : FieldNotNullRule {

        private int minimum;
        private int maximum;

        public IntFieldRangeRule(string fieldName,int minimum,int maximum) : base(fieldName) {
            this.minimum = minimum;
            this.maximum = maximum;
            if (minimum > maximum) {
                throw new ArgumentException($"Minimum({minimum}) should not be greater than maximum({maximum})");
            }
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo) {
            ValidationResult parentValidation = base.Validate(value, cultureInfo);
            if (!parentValidation.IsValid) {
                return parentValidation;
            }

            int intValue = (int)value;
            if(intValue < minimum || intValue > maximum) {
                return new ValidationResult(false, $"The field {fieldName} must be between {minimum}-{maximum}");
            }

            return new ValidationResult(true, null);
        }
    }
}
