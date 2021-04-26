using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClientt.viewModels.filters {
    public class SearchFilterMember : FilterMember {

        public string SearchQuery { get; set; } = string.Empty;
        public string Title { get; private set; }
        private Action<string> finishAction;

        public SearchFilterMember(Action<string> finishAction,string title) {
            this.finishAction = finishAction;
            this.Title = title;
        }

        public void Finish() {
            if(SearchQuery.Length > 0) {
                finishAction.Invoke(SearchQuery);
            }
        }

        public void Reset() {
            SearchQuery = string.Empty;
        }
    }
}
