using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClientt.viewModels {
    public class DisplayTextViewModel : BaseViewModel, IViewModel {
        private static DisplayTextViewModel instance;
        private string text;

        public string ShownText {
            get {
                return text;
            }
            private set {
                text = value;
                OnPropertyChanged("ShownText");
            }
        }

        private DisplayTextViewModel() {
        }

        public static DisplayTextViewModel GetInstance(string text) {
            if (instance == null) { 
                instance = new DisplayTextViewModel();
            }
            instance.ShownText = text;
            return instance;
        }

    }
}
