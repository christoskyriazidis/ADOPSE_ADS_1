using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Models.Queries
{
    public class ParamTypesFilter
    {
        [RegularExpression(@"^[0-9]([0-9]|_)*$", ErrorMessage = "You can use only 1_2_3 pattern")]
        public string Type { get; set; }

        [RegularExpression(@"^[0-9]([0-9]|_)*$", ErrorMessage = "You can use only 1_2_3 pattern")]
        public string Category { get; set; }

        [RegularExpression(@"^[0-9]([0-9]|_)*$", ErrorMessage = "You can use only 1_2_3 pattern")]
        public string Manufacturer { get; set; }

        [RegularExpression(@"^[0-9]([0-9]|_)*$", ErrorMessage = "You can use only 1_2_3 pattern")]
        public string State { get; set; }

        [RegularExpression(@"^[0-9]([0-9]|_)*$", ErrorMessage = "You can use only 1_2_3 pattern")]
        public string Condition { get; set; }

    }
}
