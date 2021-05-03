using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClientt.viewModels {
    /// <summary>
    /// Represents a checkbox viewmodel class.
    /// </summary>
    public sealed class CheckBoxViewModel : BaseViewModel {
        private bool selected = false;

        /// <summary>
        /// Represents a user-friendly title of the checbox vlaue.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Represents a server-specific value that is sent to the server.
        /// </summary>
        public long Code { get; private set; }

        /// <summary>
        /// Indicates whether this checkbox is slected
        /// </summary>
        public bool Selected {
            get {
                return selected;
            }
            set {
                selected = value;
                OnPropertyChanged(nameof(Selected));
            } 
        }

        public CheckBoxViewModel(string title, long code) {
            Title = title;
            Code = code;
        }
    }
}
