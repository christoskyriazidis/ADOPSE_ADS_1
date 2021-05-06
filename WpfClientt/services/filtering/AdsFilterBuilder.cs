using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WpfClientt.model;

namespace WpfClientt.services.filtering {
    /// <summary>
    /// Builder class for filter url.
    /// </summary>
    public sealed class AdsFilterBuilder {

        private Subcategory subcategory;

        private string mainUrl = $"{ApiInfo.AdMainUrl()}?";
        private int min = 0;
        private int max = 0;

        private Regex NotCharacters = new Regex(@"[^a-z0-9 ]");
        private Regex SpaceCharacters = new Regex(@"\s+");
        private Regex LastPlusCharacter = new Regex(@"/[+]+$/");

        private ISet<long> conditions = new HashSet<long>();
        private ISet<long> states = new HashSet<long>();
        private ISet<long> types = new HashSet<long>();
        private ISet<long> manufacturers = new HashSet<long>();
        private string titleQuery = string.Empty;

        /// <summary>
        /// Creates new filter builder that will filter in the given subcategory.
        /// </summary>
        /// <param name="subcategory"></param>
        public AdsFilterBuilder(Subcategory subcategory) {
            mainUrl = $"{mainUrl}";
            this.subcategory = subcategory;
        }

        /// <summary>
        /// Adds the title search query - text.
        /// </summary>
        /// <param name="titleQuery"></param>
        public void SetTitleQuery(string titleQuery) {
            this.titleQuery = LastPlusCharacter.Replace(SpaceCharacters.Replace(NotCharacters.Replace(titleQuery.ToLower(), ""), "+"), "");
        }

        public void SetMinPrice(int min) {
            this.min = min;
        }

        public void SetMaxPrice(int max) {
            this.max = max;
        }

        /// <summary>
        /// Adds the given condition code into the filter.
        /// </summary>
        /// <param name="conditionCode"></param>
        public void AddConditionFilter(long conditionCode) {
            conditions.Add(conditionCode);
        }

        /// <summary>
        /// Adds the given state code into the filter.
        /// </summary>
        /// <param name="stateCode"></param>
        public void AddStateFilter(long stateCode) {
            states.Add(stateCode);
        }

        /// <summary>
        /// Adds the given type code into the filter.
        /// </summary>
        /// <param name="typeCode"></param>
        public void AddTypeFilter(long typeCode) {
            types.Add(typeCode);
        }

        /// <summary>
        /// Adds the given manufacturer code into the filter.
        /// </summary>
        /// <param name="manufacturerCode"></param>
        public void AddManufacturerFilter(long manufacturerCode) {
            manufacturers.Add(manufacturerCode);
        }

        /// <summary>
        /// Builds and returns the filter url that should be used to retrieve adds that 
        /// satisfy the filter.
        /// </summary>
        /// <returns></returns>
        public string Build() {
            StringBuilder filterUrl = new StringBuilder(mainUrl);
            string conditionsFilter = PrefixIfNotEmpty("Condition=",string.Join("_", conditions.Select(LongToString).ToArray()));
            string statesFilter = PrefixIfNotEmpty("State=",string.Join("_", states.Select(LongToString).ToArray()));
            string typesFilter = PrefixIfNotEmpty("Type=",string.Join("_", types.Select(LongToString).ToArray()));
            string manufacturersFilter = PrefixIfNotEmpty("Manufacturer=",string.Join("_", manufacturers.Select(LongToString).ToArray()));
            string titleFilter = PrefixIfNotEmpty("Title=", titleQuery);
            string subcategoryFilter = $"SubcategoryId={subcategory.Id}";
            string minFilter = string.Empty;
            string maxFilter = string.Empty;
            if(min != 0 && max != 0 && min.CompareTo(max) < 0) {
                minFilter = PrefixIfNotEmpty("MinPrice=", min.ToString());
                maxFilter = PrefixIfNotEmpty("MaxPrice=", max.ToString());
            }else if(min != 0) {
                minFilter = PrefixIfNotEmpty("MinPrice=", min.ToString());
            }else if(max != 0) {
                minFilter = PrefixIfNotEmpty("MinPrice=", min.ToString());
            }

            return filterUrl.ToString() + string.Join("&",
                new string[] {subcategoryFilter,conditionsFilter, statesFilter, typesFilter, manufacturersFilter,titleFilter,minFilter,maxFilter }
                .Where(str => str.Length > 0)
                );
        }

        /// <summary>
        /// Clears the filters.
        /// </summary>
        public void ClearFilters() {
            conditions.Clear();
            states.Clear();
            types.Clear();
            manufacturers.Clear();
            titleQuery = string.Empty;
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
