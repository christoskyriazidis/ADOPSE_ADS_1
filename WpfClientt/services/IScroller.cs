using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClientt.services {
    /// <summary>
    /// Scroller above T instance.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IScroller<T>  {

        /// <summary>
        /// Initializes the scroller.Must be called before use!
        /// Must not call any other method of this class until Init has completed.
        /// </summary>
        Task Init();

        /// <summary>
        /// Returns the current page.
        /// </summary>
        /// <returns></returns>
        IPage<T> CurrentPage();

        /// <summary>
        /// Moves to the next page,if possible.
        /// After this method is completed,to retrieve the new page,call CurrentPage again.
        /// </summary>
        /// <returns>Returns true if next page retrieved,false otherwise.</returns>
        Task<bool> MoveNext();

        /// <summary>
        /// Moves to the previous page,if possible.
        /// After this method is completed,to retrieve the new page,call CurrentPage again.
        /// </summary>
        /// <returns>Returns true if previous page retrieved,false otherwise.</returns>
        Task<bool> MoveBack();

        int NumberOfPages();

    }
}
