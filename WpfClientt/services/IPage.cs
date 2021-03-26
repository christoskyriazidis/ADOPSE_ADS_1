using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClientt.services {
    public interface IPage<T> {

        IList<T> Objects();

        int Number();

    }
}
