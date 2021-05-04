using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClientt.viewModels {
    /// <summary>
    /// Represents a text-search filter.
    /// </summary>
    public class SearchFilterMember : FilterMember {

        /// <summary>
        /// The search query input provided by the user.
        /// </summary>
        public string SearchQuery { get; set; } = string.Empty;

        /// <summary>
        /// The title of the search query presented to the user.
        /// </summary>
        public string Title { get; private set; }

        private Action<string> finishAction;

        public SearchFilterMember(Action<string> finishAction,string title) {
            this.finishAction = finishAction;
            this.Title = title;
        }

        /// <summary>
        /// If search query is not empty,invokes the provided action with the query value.
        /// </summary>
        public void Finish() {
            if(SearchQuery.Length > 0) {
                finishAction.Invoke(SearchQuery);
            }
        }

        /// <summary>
        /// Clears the search query.
        /// </summary>
        public void Reset() {
            SearchQuery = string.Empty;
        }
    }
}
