using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClientt.viewModels{

    /// <summary>
    /// Represents a ftiler member(a searchbox,a multiple choice filter,single choice filter,etc).
    /// </summary>
    public interface FilterMember {

        /// <summary>
        /// This method is called when the user has finished with the filter.
        /// Usually,this method adds the selected values to AdsFilterBuilder.
        /// </summary>
        void Finish();

        /// <summary>
        /// Resets the filter to the default settings.
        /// </summary>
        void Reset();
    }
}
