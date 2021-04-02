using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClientt.viewModels.filters {
    public class RadioButtonViewModel : BaseViewModel {
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
        public string GroupName { get; private set; }

        public RadioButtonViewModel(string title, long code,string groupName) {
            Title = title;
            Code = code;
            GroupName = groupName;
        }
    }
}
