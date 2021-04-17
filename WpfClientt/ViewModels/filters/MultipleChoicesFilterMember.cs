using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClientt.model;
using WpfClientt.services;

namespace WpfClientt.viewModels.filters {
    public class MultipleChoicesFilterMember : FilterMember {
        public string Title { get; private set; }
        public ObservableCollection<Checkbox> Choices { get; private set; } = new ObservableCollection<Checkbox>();
        private Action<long> finishAction;

        private MultipleChoicesFilterMember(ISet<AdDetailComponent> filters, string title, Action<long> finishAction) {
            this.Title = title;
            filters.Select(component => new Checkbox(component.Title, component.Id))
                .ToList().ForEach(choice => Choices.Add(choice));
            this.finishAction = finishAction;
        }

        public void Finish() {
            Choices
                .Where(choice => choice.Selected)
                .Select(choice => choice.Code)
                .ToList()
                .ForEach(finishAction);
        }

        public void Reset() {
            foreach (Checkbox choice in Choices) {
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
