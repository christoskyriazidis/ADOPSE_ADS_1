using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClientt.model;

namespace WpfClientt.services {
    class DeleteFromCategorySub {

        public int[] CatIds { get; set; }

        public DeleteFromCategorySub(Subcategory[] subcategories) {
            CatIds = subcategories.Select(subcategory => subcategory.Id).ToArray();
        }

    }
}
