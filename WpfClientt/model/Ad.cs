using AggeliesProject.domain.converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AggeliesProject.domain {
    public sealed class Ad {

        public long Id { get; set; }

        public long StateId { get; set; }

        public long TypeId { get; set; }

        public long ManufacturerId { get; set; }

        public long ConditionId { get; set; }

        public long CategoryId { get; set; }

        public string Title { get; set; }
        
        public string Description { get; set; }

        [JsonPropertyName("Img")]
        public string Image { get; set; }

        public int Views { get; set; } = 0;
        
        public int Reports { get; set; } = 0;

        public long CustomerId { get; set; }

        public Ad() { 
        
        }

        public Ad(long id, long stateId, long typeId, long manufacturerId, long conditionId, 
            long categoryId, string title, string description, string imageUrl, int views, int reports) {
            this.Id = id;
            this.StateId = stateId;
            this.TypeId = typeId;
            this.ManufacturerId = manufacturerId;
            this.ConditionId = conditionId;
            this.CategoryId = categoryId;
            this.Title = title;
            this.Description = description;
            this.Image = imageUrl;
            this.Views = views;
            this.Reports = reports;
        }

        public void Report() {
            Reports++;
        }

        public int GetReports() {
            return Reports;
        }

        public void Viewed() {
            Views++;
        }

        public int GetViews() {
            return Views;
        }

        public override string ToString() {
            return $"Id={Id},StateId={StateId},TypeId={TypeId},ManufacturerId={ManufacturerId},ConditionId={ConditionId}," +
                $"CategoryId={CategoryId},Title={Title},Description={Description},Img={Image},Reports={Reports},Views={Views}";
        }
    }
}
