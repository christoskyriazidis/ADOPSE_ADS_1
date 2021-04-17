using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfClientt.viewModels.validationRules {
    public abstract class StringNotEmptyRule : ValidationRule {

        private string field;

        public StringNotEmptyRule(string field) {
            this.field = field;
        }


        public sealed override ValidationResult Validate(object value, CultureInfo cultureInfo) {
            if(value == null || !(value is string) || ((string)value).Length == 0) {
                return new ValidationResult(false, $"Provide value for the {field}");
            }
            return new ValidationResult(true, null);
        }
    }
}
