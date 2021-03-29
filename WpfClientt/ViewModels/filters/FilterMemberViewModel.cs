using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClientt.viewModels.filters {
    public class FilterMemberViewModel {

        public string Title { get; private set; }
        public ObservableCollection<Choice> Choices { get; private set; } = new ObservableCollection<Choice>();

        public FilterMemberViewModel(IDictionary<long, string> filters, string title) {
            this.Title = title;
            filters.Select(pair => new Choice(pair.Value, pair.Key))
                .ToList().ForEach(choice => Choices.Add(choice));
        }

    }
}
