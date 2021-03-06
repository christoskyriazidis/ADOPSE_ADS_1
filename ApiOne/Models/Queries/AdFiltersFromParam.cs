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

        [RegularExpression(@"(^([0-9]_)+[0-9]{1,2}$)|(^[0-9]{1,2}$)")]
        public string Category { get; set; }

        [Range(1, int.MaxValue)]
        public int SubCategoryId { get; set; }

        [RegularExpression(@"(^([0-9]_)+[0-9]{1,2}$)|(^[0-9]{1,2}$)")]
        public string Manufacturer { get; set; }

        [RegularExpression(@"(^([0-9]_)+[0-9]{1}$)|(^[0-9]$)")]
        public string State { get; set; }

        [RegularExpression(@"(^([0-9]_)+[0-9]{1}$)|(^[0-9]$)")]
        public string Condition { get; set; }

        public string Title { get; set; }

        [RegularExpression(@"^[0-9]+$")]
        public int MaxPrice { get; set; } = 1000000;

        [RegularExpression(@"^[0-9]+$")]
        public int MinPrice { get; set; } = 1;

        public string SortBy { get; set; } = "idH";
        public string Distance { get; set; } = "10000";
    }
}
