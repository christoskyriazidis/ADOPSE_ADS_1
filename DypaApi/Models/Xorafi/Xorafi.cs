using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DypaApi.Models.Xorafi
{
    public class Xorafi
    {
        public int Id { get; set; }

        public string Title { get; set; }
        
        public string LocationTitle { get; set; }

        [Required]
        public float Latitude { get; set; }

        [Required]
        public float Longitude { get; set; }

        public int Owner { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Acres { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int PlantRoots { get; set; }
        
        [Required]
        [Range(1, int.MaxValue)]
        public int WaterSupply { get; set; }

        public Xorafi(int id, string title, float latitude, float longitude, int owner, int acres, int plantRoots, int waterSupply)
        {
            Id = id;
            Title = title;
            Latitude = latitude;
            Longitude = longitude;
            Owner = owner;
            Acres = acres;
            PlantRoots = plantRoots;
            WaterSupply = waterSupply;
        }

        public Xorafi()
        {

        }
    }
}
