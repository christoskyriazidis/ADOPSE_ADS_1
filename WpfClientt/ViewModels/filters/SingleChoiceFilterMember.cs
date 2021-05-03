using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClientt.model;

namespace WpfClientt.viewModels {
    /// <summary>
    /// Represents a filter that allows a single value to be selected.
    /// </summary>
    public class SingleChoiceFilterMember : FilterMember {

        /// <summary>
        /// The title shown to the user.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// The available choices from which the user can select one.
        /// </summary>
        public ObservableCollection<RadioButtonViewModel> Choices { get; private set; } = new ObservableCollection<RadioButtonViewModel>();
        
        private Action<long> finishAction;

        public SingleChoiceFilterMember(ISet<AdDetailComponent> filters, string title, Action<long> finishAction,string groupName) {
            this.Title = title;
            filters.Select(component => new RadioButtonViewModel(component.Title, component.Id,groupName))
                .ToList().ForEach(choice => Choices.Add(choice));
            this.finishAction = finishAction;
        }

        /// <summary>
        /// Finds the selected value and invokes the provided action with its value.
        /// </summary>
        public void Finish() {
            RadioButtonViewModel selected = Choices
                .FirstOrDefault(choice => choice.Selected);
            if (selected != null) {
                finishAction.Invoke(selected.Code);
            }
        }

        /// <summary>
        /// Resets the filter by deselecting the selected value.
        /// </summary>
        public void Reset() {
            foreach (RadioButtonViewModel choice in Choices) {
                choice.Selected = false;
            }
        }

    }
}
