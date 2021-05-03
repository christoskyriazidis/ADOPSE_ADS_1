using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClientt.viewModels {
    /// <summary>
    /// Represents a radio button viewmodel.
    /// </summary>
    public class RadioButtonViewModel : BaseViewModel {
        private bool selected = false;

        /// <summary>
        /// The user-friendly title of this radio button.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// The server-specific value of this radio button.
        /// </summary>
        public long Code { get; private set; }

        /// <summary>
        /// Indicates whether this radio button is selected.
        /// </summary>
        public bool Selected {
            get {
                return selected;
            }
            set {
                selected = !selected;
                OnPropertyChanged(nameof(Selected));
            }
        }

        /// <summary>
        /// The name of the group to which this radio button belongs.In any time,only one 
        /// radio button should be allowed to be selected out of the set of radio buttons belonging to a group.
        /// </summary>
        public string GroupName { get; private set; }

        public RadioButtonViewModel(string title, long code,string groupName) {
            Title = title;
            Code = code;
            GroupName = groupName;
        }
    }
}
