using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiOne.Models.Queries
{
    public class AdFiltersFromParam
    {
        [RegularExpression(@"(^([0-9]_)+[0-9]{1}$)|(^[0-9]$)", ErrorMessage = "You can use only 1_2_3 pattern")]
        public string Type { get; set; }

        [RegularExpression(@"(^([0-9]_)+[0-9]{1}$)|(^[0-9]$)", ErrorMessage = "You can use only 1_2_3 pattern")]
        public string Category { get; set; }

        [RegularExpression(@"(^([0-9]_)+[0-9]{1}$)|(^[0-9]$)", ErrorMessage = "You can use only 1_2_3 pattern")]
        public string Manufacturer { get; set; }

        [RegularExpression(@"(^([0-9]_)+[0-9]{1}$)|(^[0-9]$)", ErrorMessage = "You can use only 1_2_3 pattern")]
        public string State { get; set; }

        [RegularExpression(@"(^([0-9]_)+[0-9]{1}$)|(^[0-9]$)", ErrorMessage = "You can use only 1_2_3 pattern")]
        public string Condition { get; set; }

        [RegularExpression(@"^[a-zA-Z|]+$", ErrorMessage = "You can use only a-z| pattern")]
        public string Description { get; set; }

        [RegularExpression(@"^[0-9a-zA-Z|]+$", ErrorMessage = "You can use only a-z| pattern")]
        public string Title { get; set; }


    }
}
