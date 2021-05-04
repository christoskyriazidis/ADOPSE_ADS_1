using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClientt.services {
    /// <summary>
    /// Represents a page of T instances of the IScroller<T>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPage<T> {

        /// <summary>
        /// Returns the objects this page has.
        /// </summary>
        /// <returns></returns>
        IList<T> Objects();

        /// <summary>
        /// Returns the Page number of this page.
        /// </summary>
        /// <returns></returns>
        int Number();

    }
}
