using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClientt.services;

namespace WpfClientt.viewModels.filters {
    public class FilterMemberViewModel{
        public string Title { get; private set; }
        public ObservableCollection<ChoiceViewModel> Choices { get; private set; } = new ObservableCollection<ChoiceViewModel>();
        private Action<long> finishAction;

        public FilterMemberViewModel(IDictionary<long, string> filters, string title, Action<long> finishAction) {
            this.Title = title;
            filters.Select(pair => new ChoiceViewModel(pair.Value, pair.Key))
                .ToList().ForEach(choice => Choices.Add(choice));
            this.finishAction = finishAction;
        }

        internal void Finish() {
            Choices
                .Where(choice => choice.Selected)
                .Select(choice => choice.Code)
                .ToList()
                .ForEach(finishAction);
        }

        internal void Reset() {
            foreach (ChoiceViewModel choice in Choices) {
                choice.Selected = false;
            }
        }

    }
}
