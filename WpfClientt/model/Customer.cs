using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WpfClientt.model {
    public sealed class Customer {

        public long Id { get; set; }
        public string Username { get; set; }

        public long LocationId { get; set; }

        public int RatedTimes { get; set; } = 0;

        public int SumRating { get; set; } = 0;

        public float Rating {
            get {
                if (RatedTimes == 0) return 0;
                return SumRating / (float) RatedTimes;
            }
            
            set {
                RatedTimes++;
                SumRating += (int)value;
            } 
        
        }

        public Customer() { 
        
        }

        public Customer(long id, string username, long locationId,int sumRating = 0,int ratedTimes = 0) {
            Id = id;
            Username = username;
            LocationId = locationId;
            this.RatedTimes = ratedTimes;
            this.SumRating = sumRating;
        }

        public override string ToString() {
            return $"Id = {Id},Username={Username},LocationId={LocationId},RatedTimes={RatedTimes},SumRating={SumRating},Rating={Rating}";
        }
    }
}
