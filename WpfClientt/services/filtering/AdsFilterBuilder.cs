using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClientt.services.filtering {
    public sealed class AdsFilterBuilder {

        private string mainUrl = ApiInfo.FilterMainUrl();

        private ISet<long> conditions = new HashSet<long>();
        private ISet<long> states = new HashSet<long>();
        private ISet<long> types = new HashSet<long>();
        private ISet<long> manufacturers = new HashSet<long>();

        public void AddConditionFilter(long conditionCode) {
            conditions.Add(conditionCode);
        }

        public void AddStateFilter(long stateCode) {
            states.Add(stateCode);
        }

        public void AddTypeFilter(long typeCode) {
            types.Add(typeCode);
        }

        public void AddManufacturerFilter(long manufacturerCode) {
            manufacturers.Add(manufacturerCode);
        }

        public string build() {
            StringBuilder filterUrl = new StringBuilder(mainUrl);
            string conditionsFilter = PrefixIfNotEmpty("condition=",string.Join("_", conditions.Select(LongToString).ToArray()));
            string statesFilter = PrefixIfNotEmpty("state=",string.Join("_", states.Select(LongToString).ToArray()));
            string typesFilter = PrefixIfNotEmpty("type=",string.Join("_", types.Select(LongToString).ToArray()));
            string manufacturersFilter = PrefixIfNotEmpty("manufacturer=",string.Join("_", manufacturers.Select(LongToString).ToArray()));

            return filterUrl.ToString() + string.Join("&",
                new string[] {conditionsFilter, statesFilter, typesFilter, manufacturersFilter }.Where(str => str.Length > 0)
                );
        }

        public void ClearFilters() {
            conditions.Clear();
            states.Clear();
            types.Clear();
            manufacturers.Clear();
        }

        private string LongToString(long l) => l.ToString();

        private string PrefixIfNotEmpty(string prefix, string str) {
            string result = string.Empty;
            if (str.Length != 0) {
                result = prefix + str;
            }
            return result;
        }

    }
}
