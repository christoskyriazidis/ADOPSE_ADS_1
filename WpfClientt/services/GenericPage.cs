using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WpfClientt.services {
    class GenericPage<T> : IPage<T> {

        [JsonPropertyName("result")]
        public IList<T> Entities { get; set; } = new List<T>();

        [JsonPropertyName("currentPage")]
        public int PageNumber { get; set; }

        [JsonPropertyName("totalPages")]
        public int NumOfPages { get; set; }

        [JsonPropertyName("nextPageUrl")]
        public string NextPageUrl { get; set; }

        [JsonPropertyName("previousPageUrl")]
        public string PreviousPageUrl { get; set; }

        public int Number() {
            return PageNumber;
        }

        public IList<T> Objects() {
            return Entities;
        }

        public override string ToString() {
            StringBuilder stringBuilder = new StringBuilder("Entities:\n");
            foreach (T t in Entities) {
                stringBuilder.Append(t.ToString());
                stringBuilder.AppendLine();
            }
            stringBuilder.AppendLine();
            stringBuilder.Append($"Page Number={PageNumber},Next Page Url={NextPageUrl} , Previous Page Url={PreviousPageUrl},Number of pages={NumOfPages}");
            return stringBuilder.ToString();
        }
    }
}
