using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfClientt.viewModels.validationRules.generic {
    public class RegularExpressionRule : FieldNotNullRule {

        private Regex regex;
        private string errorMessage;

        public RegularExpressionRule(string fieldName,Regex regex,string errorMessage) : base(fieldName) {
            this.regex = regex;
            this.errorMessage = errorMessage;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo) {
            ValidationResult parentValidation = base.Validate(value, cultureInfo);
            if (!parentValidation.IsValid) {
                return parentValidation;
            }
            string strValue = (string)value;

            if (!regex.IsMatch(strValue)) {
                return new ValidationResult(false, errorMessage);
            }

            return new ValidationResult(true, null);

        }
    }
}
