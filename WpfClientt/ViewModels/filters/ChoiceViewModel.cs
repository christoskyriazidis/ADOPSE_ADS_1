using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClientt.viewModels.filters {
    public sealed class ChoiceViewModel : BaseViewModel {
        private bool selected = false;

        public string Title { get; private set; }
        public long Code { get; private set; }
        public bool Selected {
            get {
                return selected;
            }
            set {
                selected = value;
                OnPropertyChanged("Selected");
            } 
        }

        public ChoiceViewModel(string title, long code) {
            Title = title;
            Code = code;
        }
    }
}
