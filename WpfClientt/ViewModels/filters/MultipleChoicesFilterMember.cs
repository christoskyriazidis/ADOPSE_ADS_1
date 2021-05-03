using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClientt.model;
using WpfClientt.services;

namespace WpfClientt.viewModels {
    /// <summary>
    /// Filter that allows users to select multiple values.
    /// </summary>
    public class MultipleChoicesFilterMember : FilterMember {
        /// <summary>
        /// The title to display to the user.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Available choices.
        /// </summary>
        public ObservableCollection<CheckBoxViewModel> Choices { get; private set; } = new ObservableCollection<CheckBoxViewModel>();

        private Action<long> finishAction;

        private MultipleChoicesFilterMember(ISet<AdDetailComponent> filters, string title, Action<long> finishAction) {
            this.Title = title;
            filters.Select(component => new CheckBoxViewModel(component.Title, component.Id))
                .ToList().ForEach(choice => Choices.Add(choice));
            this.finishAction = finishAction;
        }

        /// <summary>
        /// For every selected choice calls the finish action with its long value.
        /// </summary>
        public void Finish() {
            Choices
                .Where(choice => choice.Selected)
                .Select(choice => choice.Code)
                .ToList()
                .ForEach(finishAction);
        }

        /// <summary>
        /// Resets the filter by deselecting all the choices.
        /// </summary>
        public void Reset() {
            foreach (CheckBoxViewModel choice in Choices) {
                choice.Selected = false;
            }
        }

        public static MultipleChoicesFilterMember getInstance<T>(ISet<T> set, string title, Action<long> finishAction) where T : AdDetailComponent {
            ISet<AdDetailComponent> adDetailComponents = new HashSet<AdDetailComponent>();
            foreach(T t in set) {
                adDetailComponents.Add(t);
            }
            return new MultipleChoicesFilterMember(adDetailComponents, title,finishAction);
        }


    }
}
