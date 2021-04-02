using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClientt.services {
    public interface IMapper {

        Task<IDictionary<long,string>> Categories();

        Task<IDictionary<long, string>> Conditions();

        Task<IDictionary<long, string>> Types();

        Task<IDictionary<long, string>> Manufacturers();

        Task<IDictionary<long, string>> States();

    }
}
