using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfClientt.model;

namespace WpfClientt.viewModels {
    public abstract class CategoryViewModel {

        public ICommand Command { get; set; }
        public Category Category { get; set; }

        public CategoryViewModel(Category category,ICommand command) {
            this.Category = category;
            this.Command = command;
        }
    }
}
