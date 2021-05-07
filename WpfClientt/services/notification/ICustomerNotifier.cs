using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClientt.services {
    public interface ICustomerNotifier {

        void Information(string info);

        void Success(string success);

        void Warning(string warning);

        void Error(string Error);

    }
}
