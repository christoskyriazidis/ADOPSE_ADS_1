using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfClientt.model;

namespace WpfClientt.viewModels {
    public abstract class SubcategoryViewModel {

        public Subcategory Subcategory { get; set; }
        public ICommand Command { get; set; }

        public SubcategoryViewModel(Subcategory subcategory,ICommand command) {
            this.Subcategory = subcategory;
            this.Command = command;
        }

    }
}
